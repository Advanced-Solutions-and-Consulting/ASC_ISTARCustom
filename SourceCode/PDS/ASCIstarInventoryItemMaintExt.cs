using System;
using System.Collections;
using PX.Data;
using PX.Data.BQL.Fluent;
using PX.Objects.AP;
using PX.Objects.PO;
using PX.Objects.IN;
using ASCISTARCustom.Inventory.DAC;
using ASCISTARCustom.PDS.CacheExt;

namespace ASCISTARCustom
{
    public class ASCIstarInventoryItemMaintExt : PXGraphExtension<InventoryItemMaint>
    {
        public static bool IsActive() => true;

        #region Selects

        public PXSelect<INJewelryItemData, Where<INJewelryItemData.inventoryID, Equal<Current<InventoryItem.inventoryID>>>> JewelryItemDataSelect;


        public PXSelect<INKitSpecHdr, Where<INKitSpecHdr.kitInventoryID, Equal<Current<InventoryItem.inventoryID>>>> KitCostRollup;

        public PXSelect<POVendorInventory,
            Where<POVendorInventory.inventoryID, Equal<Current<InventoryItem.inventoryID>>,
            And<POVendorInventory.vendorID, Equal<Current<InventoryItemCurySettings.preferredVendorID>>,
            And<POVendorInventory.vendorID, Equal<Current<InventoryItemCurySettings.preferredVendorLocationID>>>>>> DefaultVendorItem;

        //public PXSelect<ASCIStarItemCostRollup, Where<ASCIStarItemCostRollup.inventoryID, Equal<Current<InventoryItem.inventoryID>>, And<ASCIStarItemCostRollup.bAccountID, NotEqual<CompanyBAccount.bAccountID>>>> VendorCostRollup;

        #endregion Selects

        #region CacheAttached

        //[PXMergeAttributes(Method = MergeMethod.Append)]
        //[PXFormula(typeof(Switch<Case<Where<Current<APVendorPrice.uOM>, NotEqual<TOZ>>, Null>, Div<APVendorPrice.salesPrice, TOZ2GRAM>>))]
        //protected void APVendorPrice_UsrCommodityPerGram_CacheAttached(PXCache sender) { }

        //[PXMergeAttributes(Method = MergeMethod.Append)]
        //[PXFormula(typeof(Switch<Case<Where<Current<APVendorPrice.uOM>, NotEqual<TOZ>>, Null>, Div<APVendorPrice.salesPrice, ASCIStarAPVendorPriceExt.usrCommodityPrice>>))]
        //protected void APVendorPrice_UsrIncrement_CacheAttached(PXCache sender) { }

        //[PXMergeAttributes(Method = MergeMethod.Append)]
        //[PXFormula(typeof(Switch<Case<Where<Current<APVendorPrice.uOM>, NotEqual<TOZ>>, Null>, Div<Div<APVendorPrice.salesPrice, ASCIStarAPVendorPriceExt.usrCommodityPrice>, TOZ2GRAM>>))]
        //protected void APVendorPrice_UsrIncrementPerGram_CacheAttached(PXCache sender) { }

        [PXMergeAttributes(Method = MergeMethod.Append)]
        [PXRestrictor(typeof(Where<ASCIStarINUnitExt.usrCommodity, IsNotNull>), "Market Cost requires that a conversion is selected", typeof(INUnit.fromUnit))]
        protected void InventoryItem_UsrPriceToUnit_CacheAttached(PXCache sender) { }


        /* MATT CHECK WHICH COSTS ARE ASSIGNED IN THE UNIT AND PURCHASE COSTS AGAIN*/
        [PXMergeAttributes(Method = MergeMethod.Merge)]
        [PXFormula(typeof(Add<Add<Add<Add<Add<Add<Add<Add<
                             ASCIStarINInventoryItemExt.usrCommodityCost
                           , ASCIStarINInventoryItemExt.usrOtherMaterialCost>
                           , ASCIStarINInventoryItemExt.usrFabricationCost>
                           , ASCIStarINInventoryItemExt.usrLaborCost>
                           , ASCIStarINInventoryItemExt.usrHandlingCost>
                           , ASCIStarINInventoryItemExt.usrFreightCost>
                           , ASCIStarINInventoryItemExt.usrDutyCost>
                           , ASCIStarINInventoryItemExt.usrPackagingCost>
                           , ASCIStarINInventoryItemExt.usrOtherCost>
                           ))]
        protected void InventoryItem_UsrUnitCost_CacheAttached(PXCache sender) { }

        [PXMergeAttributes(Method = MergeMethod.Merge)]
        [PXFormula(typeof(Add<Add<Add<Add<Add<
                     ASCIStarINInventoryItemExt.usrCommodityCost
                   , ASCIStarINInventoryItemExt.usrOtherMaterialCost>
                   , ASCIStarINInventoryItemExt.usrFabricationCost>
                   /*, ASCIStarINInventoryItemExt.usrLaborCost>*/
                   , ASCIStarINInventoryItemExt.usrHandlingCost>
                   , ASCIStarINInventoryItemExt.usrPackagingCost>
                   , ASCIStarINInventoryItemExt.usrOtherCost>
                   ))]
        protected void InventoryItem_UsrContractCost_CacheAttached(PXCache sender) { }

        //SWITCH TO ACTUAL LOOKUP?

        #endregion CacheAttached


        #region Event Handlers


        #region InventoryItem Events

        protected void InventoryItem_RowUpdated(PXCache cache, PXRowUpdatedEventArgs e, PXRowUpdated InvokeBaseHandler)
        {
            //if (InvokeBaseHandler != null)
            InvokeBaseHandler(cache, e);

            var row = (InventoryItem)e.Row;
            if (row != null)
            {

                ASCIStarINInventoryItemExt itemext = row.GetExtension<ASCIStarINInventoryItemExt>();
                POVendorInventory vendorItem = DefaultVendorItem.Select();
                foreach (POVendorInventory vitem in Base.VendorItems.Select())
                {
                    //PXTrace.WriteInformation($"{vitem.VendorID}:{vitem.IsDefault}");

                    if (vitem.IsDefault == true)
                        vendorItem = vitem;
                }
                ASCIStarPOVendorInventoryExt vendorItemExt = vendorItem.GetExtension<ASCIStarPOVendorInventoryExt>();
                if (vendorItem == null || vendorItemExt == null)
                {

                    itemext.UsrCommodityCost = 0.00m;
                }
                else
                {

                    if (itemext.UsrPricingGRAMGold > 0 || itemext.UsrPricingGRAMSilver > 0)
                    {

                        ASCIStarMarketCostHelper.JewelryCost costHelper
                            = new ASCIStarMarketCostHelper.JewelryCost(cache.Graph
                                                , row
                                                , 0.000000m
                                                , 0.000000m
                                                , vendorItem.VendorID
                                                , vendorItemExt.UsrMarketID
                                                , DateTime.Today
                                                , row.BaseUnit);

                        itemext.UsrCommodityCost = costHelper.CostRollupTotal[CostRollupType.Commodity];

                        //costHelper.CostBasis.GoldBasis.BuildFinePrices();
                        //itemext.UsrCommodityCost = 0;
                        //if (itemext.UsrPricingGRAMGold > 0 || itemext.UsrPricingGRAMSilver > 0)
                        //    itemext.UsrCommodityCost += (itemext.UsrPricingGRAMGold / 31.10348m * GetCommodityPrice(vendorItem, vendorItemExt, "24K")) +
                        //        (itemext.UsrPricingGRAMSilver / 31.10348m * GetCommodityPrice(vendorItem, vendorItemExt, "SSS"))
                        //        * (1 + itemext.UsrContractLossPct / 100.0000m)
                        //        * (1 + itemext.UsrContractSurcharge / 100.0000m);
                        cache.SetValue<ASCIStarINInventoryItemExt.usrCommodityCost>(cache.Current, itemext.UsrCommodityCost);

                    }
                    //if (itemext.UsrPricingGRAMGold > 0 && itemext.UsrPricingGRAMSilver == 0
                    //    || itemext.UsrPricingGRAMGold == 0 && itemext.UsrPricingGRAMSilver > 0)
                    //{
                    //    itemext.UsrCommodityCost = (itemext.UsrPricingGRAMSilver ?? 0.00m + itemext.UsrPricingGRAMGold ?? 0.00m) / 31.10348m * vendorItemExt.UsrCommodityPrice;
                    //    cache.SetValueExt<ASCIStarINInventoryItemExt.usrCommodityCost>(cache.Current, itemext.UsrCommodityCost);

                    //}
                }
                //string msg = "";

                if (itemext.UsrPricingGRAMGold > 0 || itemext.UsrPricingGRAMSilver > 0)
                {
                    itemext.UsrCommodityCost = (itemext.UsrPricingGRAMGold / 31.10348m * GetCommodityPrice(vendorItem, vendorItemExt, "24K")) +
                    (itemext.UsrPricingGRAMSilver / 31.10348m * GetCommodityPrice(vendorItem, vendorItemExt, "SSS"))
                    * (1 + itemext.UsrContractLossPct / 100.0000m)
                    * (1 + itemext.UsrContractSurcharge / 100.0000m);
                    cache.SetValue<ASCIStarINInventoryItemExt.usrCommodityCost>(cache.Current, itemext.UsrCommodityCost);


                    itemext.UsrContractCost = itemext.UsrCommodityCost
                        + itemext.UsrOtherMaterialCost
                        + itemext.UsrFabricationCost
                        + itemext.UsrLaborCost
                        + itemext.UsrPackagingCost
                        + itemext.UsrOtherCost;
                    cache.SetValue<ASCIStarINInventoryItemExt.usrContractCost>(cache.Current, itemext.UsrContractCost);

                    itemext.UsrUnitCost = itemext.UsrContractCost
                        + itemext.UsrHandlingCost
                        + itemext.UsrFreightCost
                        + itemext.UsrDutyCost;
                    cache.SetValue<ASCIStarINInventoryItemExt.usrUnitCost>(cache.Current, itemext.UsrUnitCost);
                }
            }
        }

        protected void InventoryItem_RowSelected(PXCache cache, PXRowSelectedEventArgs e, PXRowSelected InvokeBaseHandler)
        {
            if (InvokeBaseHandler != null)
                InvokeBaseHandler(cache, e);
            var row = (InventoryItem)e.Row;

            #region InfoSource Removed
            //PXUIFieldAttribute.SetVisible<InfoInventoryItemAttributeExtNV.usrLaborCost>(cache, row, false);
            //PXUIFieldAttribute.SetVisible<InfoInventoryItemAttributeExtNV.usrFinishedItemWt>(cache, row, false);
            //PXUIFieldAttribute.SetVisible<InfoInventoryItemAttributeExtNV.usrCastingLaborCost>(cache, row, false);
            //PXUIFieldAttribute.SetVisible<InfoInventoryItemAttributeExtNV.usrGoldCostPerOunce>(cache, row, false);
            //PXUIFieldAttribute.SetVisible<InfoInventoryItemAttributeExtNV.usrPlatinumCostPerOunce>(cache, row, false);
            //PXUIFieldAttribute.SetVisible<InfoInventoryItemAttributeExtNV.usrSilverCostPerOunce>(cache, row, false);

            //PXUIFieldAttribute.SetVisible<InfoInventoryItemAttributeExtNV.usrDiamondTotalCaratWt>(cache, row, false);
            //PXUIFieldAttribute.SetVisible<InfoInventoryItemAttributeExtNV.usrDiamondCost>(cache, row, false);
            //PXUIFieldAttribute.SetVisible<InfoInventoryItemAttributeExtNV.usrColorStoneCaratsWeightTW>(cache, row, false);
            //PXUIFieldAttribute.SetVisible<InfoInventoryItemAttributeExtNV.usrSideStoneCaratWt>(cache, row, false);
            //PXUIFieldAttribute.SetVisible<InfoInventoryItemAttributeExtNV.usrGemStoneCost>(cache, row, false);
            //PXUIFieldAttribute.SetVisible<InfoInventoryItemAttributeExtNV.usrCenterCost>(cache, row, false);

            //PXUIFieldAttribute.SetVisible<InfoInventoryItemAttributeExtNV.usrWeight10k>(cache, row, false);
            //PXUIFieldAttribute.SetVisible<InfoInventoryItemAttributeExtNV.usrWeight14k>(cache, row, false);
            //PXUIFieldAttribute.SetVisible<InfoInventoryItemAttributeExtNV.usrWeight18k>(cache, row, false);
            //PXUIFieldAttribute.SetVisible<InfoInventoryItemAttributeExtNV.usrWeightPlat>(cache, row, false);
            //PXUIFieldAttribute.SetVisible<InfoInventoryItemAttributeExtNV.usrWeightSS>(cache, row, false);

            //PXUIFieldAttribute.SetVisible<InfoInventoryItemAttributeExtNV.usr10KTGram>(cache, row, false);
            //PXUIFieldAttribute.SetVisible<InfoInventoryItemAttributeExtNV.usr18KTGram>(cache, row, false);
            //PXUIFieldAttribute.SetVisible<InfoInventoryItemAttributeExtNV.usr14KTGram>(cache, row, false);
            //PXUIFieldAttribute.SetVisible<InfoInventoryItemAttributeExtNV.usrPlatinumGram>(cache, row, false);
            //PXUIFieldAttribute.SetVisible<InfoInventoryItemAttributeExtNV.usrSterlingGram>(cache, row, false);

            //PXUIFieldAttribute.SetVisible<InfoInventoryItemAttributeExtNV.usr10KTCost>(cache, row, false);
            //PXUIFieldAttribute.SetVisible<InfoInventoryItemAttributeExtNV.usr18KTCost>(cache, row, false);
            //PXUIFieldAttribute.SetVisible<InfoInventoryItemAttributeExtNV.usr14KTCost>(cache, row, false);
            //PXUIFieldAttribute.SetVisible<InfoInventoryItemAttributeExtNV.usrPlatinumCost>(cache, row, false);
            //PXUIFieldAttribute.SetVisible<InfoInventoryItemAttributeExtNV.usrSterlingCost>(cache, row, false);
            #endregion InfoSource Removed


            INItemClass itemClass = INItemClass.PK.Find(cache.Graph, row.ItemClassID);
            if (itemClass == null)
                return;
            //PXTrace.WriteInformation($"row.ItemClassID {row.ItemClassID}");
            //PXTrace.WriteInformation($"itemClass.ItemClassCD '{itemClass.ItemClassCD.Trim()}'");
            bool UseMarketConfig = (itemClass.ItemClassCD.Trim() == "COMMODITY");

            PXUIFieldAttribute.SetVisible<ASCIStarINInventoryItemExt.usrPriceAsID>(cache, row, UseMarketConfig);
            PXUIFieldAttribute.SetVisible<ASCIStarINInventoryItemExt.usrPriceToUnit>(cache, row, UseMarketConfig);

            PXUIFieldAttribute.SetVisible<ASCIStarINInventoryItemExt.usrContractCost>(cache, row, !UseMarketConfig);
            PXUIFieldAttribute.SetVisible<ASCIStarINInventoryItemExt.usrUnitCost>(cache, row, !UseMarketConfig);
            PXUIFieldAttribute.SetVisible<ASCIStarINInventoryItemExt.usrFabricationCost>(cache, row, !UseMarketConfig);
            PXUIFieldAttribute.SetVisible<ASCIStarINInventoryItemExt.usrCommodityCost>(cache, row, !UseMarketConfig);
            PXUIFieldAttribute.SetVisible<ASCIStarINInventoryItemExt.usrFreightCost>(cache, row, !UseMarketConfig);
            PXUIFieldAttribute.SetVisible<ASCIStarINInventoryItemExt.usrLaborCost>(cache, row, !UseMarketConfig);
            PXUIFieldAttribute.SetVisible<ASCIStarINInventoryItemExt.usrDutyCost>(cache, row, !UseMarketConfig);
            PXUIFieldAttribute.SetVisible<ASCIStarINInventoryItemExt.usrDutyCostPct>(cache, row, !UseMarketConfig);
            PXUIFieldAttribute.SetVisible<ASCIStarINInventoryItemExt.usrHandlingCost>(cache, row, !UseMarketConfig);
            PXUIFieldAttribute.SetVisible<ASCIStarINInventoryItemExt.usrPackagingCost>(cache, row, !UseMarketConfig);
            PXUIFieldAttribute.SetVisible<ASCIStarINInventoryItemExt.usrOtherCost>(cache, row, !UseMarketConfig);

            // PXUIFieldAttribute.SetVisible<ASCIStarINInventoryItemExt.usrContractIncrement>(cache, row, !UseMarketConfig);
            // PXUIFieldAttribute.SetVisible<ASCIStarINInventoryItemExt.usrContractLossPct>(cache, row, !UseMarketConfig);
            // PXUIFieldAttribute.SetVisible<ASCIStarINInventoryItemExt.usrContractSurcharge>(cache, row, !UseMarketConfig);
            // PXUIFieldAttribute.SetVisible<ASCIStarINInventoryItemExt.usrContractSurchargeType>(cache, row, !UseMarketConfig);
            // PXUIFieldAttribute.SetVisible<ASCIStarINInventoryItemExt.usrContractIncrement>(cache, row, !UseMarketConfig);

            PXUIFieldAttribute.SetVisible<ASCIStarINInventoryItemExt.usrPricingGRAMGold>(cache, row, !UseMarketConfig);
            PXUIFieldAttribute.SetVisible<ASCIStarINInventoryItemExt.usrPricingGRAMSilver>(cache, row, !UseMarketConfig);
            PXUIFieldAttribute.SetVisible<ASCIStarINInventoryItemExt.usrPricingGRAMPlatinum>(cache, row, !UseMarketConfig);
            PXUIFieldAttribute.SetVisible<ASCIStarINInventoryItemExt.usrActualGRAMGold>(cache, row, !UseMarketConfig);
            PXUIFieldAttribute.SetVisible<ASCIStarINInventoryItemExt.usrActualGRAMSilver>(cache, row, !UseMarketConfig);
            PXUIFieldAttribute.SetVisible<ASCIStarINInventoryItemExt.usrActualGRAMPlatinum>(cache, row, !UseMarketConfig);

        }

        protected virtual void InventoryItem_UsrCostingType_FieldDefaulting(PXCache cache, PXFieldDefaultingEventArgs e, PXFieldDefaulting InvokeBaseHandler)
        {
            if (InvokeBaseHandler != null)
                InvokeBaseHandler(cache, e);

            InventoryItem row = e.Row as InventoryItem;
            if (row != null)
            {
                INItemClass itemClass = INItemClass.PK.Find(cache.Graph, row.ItemClassID);
                if (itemClass == null)
                    return;
                ASCIStarINItemClassExt classExt = itemClass.GetExtension<ASCIStarINItemClassExt>();
                e.NewValue = classExt.UsrCostingType ?? CostingType.StandardCost;

                PXUIFieldAttribute.SetEnabled<ASCIStarINInventoryItemExt.usrCostingType>(cache, row, true);
            }
        }

        protected virtual void InventoryItem_UsrContractIncrement_FieldUpdated(PXCache cache, PXFieldUpdatedEventArgs e, PXFieldUpdated InvokeBaseHandler)
        {
            if (InvokeBaseHandler != null)
                InvokeBaseHandler(cache, e);

            InventoryItem row = e.Row as InventoryItem;
            if (row != null)
            {

                ASCIStarINInventoryItemExt ext = row.GetExtension<ASCIStarINInventoryItemExt>();
                if (ext.UsrActualGRAMGold > 0)
                {

                    POVendorInventory vendorItem = DefaultVendorItem.Select();
                    foreach (POVendorInventory vitem in Base.VendorItems.Select())
                    {
                        //PXTrace.WriteInformation($"{vitem.VendorID}:{vitem.IsDefault}");

                        if (vitem.IsDefault == true)
                            vendorItem = vitem;
                    }
                    ASCIStarPOVendorInventoryExt vendorItemExt = vendorItem.GetExtension<ASCIStarPOVendorInventoryExt>();
                    if (vendorItem == null || vendorItemExt == null)
                    {

                        ext.UsrCommodityCost = 0.00m;
                    }
                    else
                    {

                        //ASCIStarMarketCostHelper.JewelryCost costHelper
                        //    = new ASCIStarMarketCostHelper.JewelryCost(cache.Graph
                        //                        , row
                        //                        , 0.000000m
                        //                        , 0.000000m
                        //                        , vendorItem.VendorID
                        //                        , vendorItemExt.UsrMarketID
                        //                        , DateTime.Today
                        //                        , row.BaseUnit);

                        //costHelper.CostBasis.GoldBasis.BuildFinePrices();

                        decimal stdIncrement = (decimal)ext.UsrPricingGRAMGold / (decimal)ext.UsrActualGRAMGold / 31.10348m;
                        decimal SurchargePct = ((decimal)ext.UsrContractIncrement / (decimal)stdIncrement - 1) * 100.0000m;
                        PXTrace.WriteInformation($"InventoryItem_UsrContractIncrement_FieldUpdated: stdIncrement{stdIncrement}: ContractIncrement:{ext.UsrContractIncrement} SurchargePct{SurchargePct}");
                        cache.SetValueExt<ASCIStarINInventoryItemExt.usrContractSurcharge>(cache.Current, SurchargePct);

                        //InventoryItem_Metal_FieldUpdated(cache, e, InvokeBaseHandler);

                        ext.UsrCommodityCost = (ext.UsrPricingGRAMGold / 31.10348m * GetCommodityPrice(vendorItem, vendorItemExt, "24K")) +
                            (ext.UsrPricingGRAMSilver / 31.10348m * GetCommodityPrice(vendorItem, vendorItemExt, "SSS"))
                            * (1 + ext.UsrContractLossPct / 100.0000m)
                            * (1 + ext.UsrContractSurcharge / 100.0000m);
                        cache.SetValue<ASCIStarINInventoryItemExt.usrCommodityCost>(cache.Current, ext.UsrCommodityCost);
                    }


                }
            }
        }

        protected virtual void INJewelryItemData_MetalType_FieldUpdated(PXCache cache, PXFieldUpdatedEventArgs e, PXFieldUpdated InvokeBaseHandler)
        {
            PXTrace.WriteInformation("InventoryItem_MetalType_FieldUpdated");

            InventoryItem_UsrActualGRAMGold_FieldUpdated(cache, e, InvokeBaseHandler);
            InventoryItem_UsrActualGRAMSilver_FieldUpdated(cache, e, InvokeBaseHandler);
        }

        protected virtual void InventoryItem_UsrActualGRAMGold_FieldUpdated(PXCache cache, PXFieldUpdatedEventArgs e, PXFieldUpdated InvokeBaseHandler)
        {
            PXTrace.WriteInformation("InventoryItem_UsrActualGRAMGold_FieldUpdated");

            if (InvokeBaseHandler != null)
                InvokeBaseHandler(cache, e);

            InventoryItem row = e.Row as InventoryItem;
            if (row == null)
                return;

            INJewelryItemData attr = PXSelect<INJewelryItemData, Where<INJewelryItemData.inventoryID, Equal<Required<INJewelryItemData.inventoryID>>>>.Select(Base, row.InventoryID);
            if (attr == null || attr.MetalType == null)
                return;
            //InfoInventoryItemAttributeExtNV attr = row.GetExtension<InfoInventoryItemAttributeExtNV>();
            ASCIStarINInventoryItemExt itemext = row.GetExtension<ASCIStarINInventoryItemExt>();
            if (attr == null || attr.MetalType == null)
                return;

            decimal mult = 1.000000m;
            switch (attr.MetalType.ToUpper())
            {
                case "24K":
                    mult = 24.000000m;
                    break;
                case "22K":
                    mult = 22.000000m;
                    break;
                case "20K":
                    mult = 20.000000m;
                    break;
                case "18K":
                    mult = 18.000000m;
                    break;
                case "16K":
                    mult = 16.000000m;
                    break;
                case "14K":
                    mult = 14.000000m;
                    break;
                case "12K":
                    mult = 12.000000m;
                    break;
                case "10K":
                    mult = 10.000000m;
                    break;
                case "08K":
                    mult = 8.000000m;
                    break;
                case "06K":
                    mult = 6.000000m;
                    break;
                default:
                    mult = 1.000000m;
                    break;
            }

            PXTrace.WriteInformation($"{attr.MetalType}:{mult}");
            cache.SetValueExt<ASCIStarINInventoryItemExt.usrPricingGRAMGold>(cache.Current, itemext.UsrActualGRAMGold * (mult / 24));

        }

        protected virtual void InventoryItem_UsrPricingGRAMGold_FieldUpdated(PXCache cache, PXFieldUpdatedEventArgs e, PXFieldUpdated InvokeBaseHandler)
        {
            if (InvokeBaseHandler != null)
                InvokeBaseHandler(cache, e);

            InventoryItem row = e.Row as InventoryItem;
            if (row != null)
            {
                ASCIStarINInventoryItemExt itemext = row.GetExtension<ASCIStarINInventoryItemExt>();
                if (itemext == null)
                {
                    PXTrace.WriteInformation("InventoryItem_UsrPricingGRAMGold_FieldUpdated itemext WAS NULL!");
                    return;

                }
                POVendorInventory vendorItem = DefaultVendorItem.Select();
                foreach (POVendorInventory vitem in Base.VendorItems.Select())
                {
                    PXTrace.WriteInformation($"{vitem.VendorID}:{vitem.IsDefault}");

                    if (vitem.IsDefault == true)
                        vendorItem = vitem;
                }
                if (vendorItem == null)
                {
                    PXTrace.WriteInformation("InventoryItem_UsrPricingGRAMGold_FieldUpdated vendorItem WAS NULL!");
                    return;
                }
                ASCIStarPOVendorInventoryExt vendorItemExt = vendorItem.GetExtension<ASCIStarPOVendorInventoryExt>();
                if (vendorItemExt == null)
                {
                    PXTrace.WriteInformation("InventoryItem_UsrPricingGRAMGold_FieldUpdated vendorItemExt WAS NULL!");
                    return;
                }
                if (vendorItemExt.UsrCommodityPrice == null)
                {
                    PXTrace.WriteInformation("InventoryItem_UsrPricingGRAMGold_FieldUpdated vendorItemExt.UsrCommodityPrice WAS NULL!");
                    return;
                }
                if (itemext.UsrPricingGRAMGold > 0 || itemext.UsrPricingGRAMSilver > 0)
                {

                    cache.SetValue<ASCIStarINInventoryItemExt.usrContractIncrement>(cache.Current,
                        (itemext.UsrPricingGRAMGold / itemext.UsrActualGRAMGold / 31.10348m) * (1.000m + (itemext.UsrContractSurcharge / 100.0000m)));

                    itemext.UsrCommodityCost = (itemext.UsrPricingGRAMGold / 31.10348m * GetCommodityPrice(vendorItem, vendorItemExt, "24K")) +
                        (itemext.UsrPricingGRAMSilver / 31.10348m * GetCommodityPrice(vendorItem, vendorItemExt, "SSS"))
                        * (1 + itemext.UsrContractLossPct / 100.0000m)
                        * (1 + itemext.UsrContractSurcharge / 100.0000m);
                    cache.SetValue<ASCIStarINInventoryItemExt.usrCommodityCost>(cache.Current, itemext.UsrCommodityCost);
                }
            }
        }

        protected virtual void InventoryItem_UsrActualGRAMSilver_FieldUpdated(PXCache cache, PXFieldUpdatedEventArgs e, PXFieldUpdated InvokeBaseHandler)
        {
            PXTrace.WriteInformation("InventoryItem_UsrActualGRAMSilver_FieldUpdated");

            if (InvokeBaseHandler != null)
                InvokeBaseHandler(cache, e);

            InventoryItem row = e.Row as InventoryItem;
            if (row == null)
                return;

            INJewelryItemData attr = PXSelect<INJewelryItemData, Where<INJewelryItemData.inventoryID, Equal<Required<INJewelryItemData.inventoryID>>>>.Select(Base, row.InventoryID);

            //InfoInventoryItemAttributeExtNV attr = row.GetExtension<InfoInventoryItemAttributeExtNV>();
            ASCIStarINInventoryItemExt itemext = row.GetExtension<ASCIStarINInventoryItemExt>();
            if (attr == null || attr.MetalType == null)
                return;
            if (attr == null || attr.MetalType == null)
                return;

            decimal mult = 1.000000m;
            switch (attr.MetalType.ToUpper())
            {
                case "FSS":
                    mult = 1.081080m;
                    break;
                default:
                    mult = 1.000000m;
                    break;
            }
            PXTrace.WriteInformation($"{attr.MetalType}:{mult}");
            cache.SetValueExt<ASCIStarINInventoryItemExt.usrPricingGRAMSilver>(cache.Current, itemext.UsrActualGRAMSilver * mult);

        }

        protected virtual void InventoryItem_UsrPricingGRAMSilver_FieldUpdated(PXCache cache, PXFieldUpdatedEventArgs e, PXFieldUpdated InvokeBaseHandler)
        {
            if (InvokeBaseHandler != null)
                InvokeBaseHandler(cache, e);

            InventoryItem row = e.Row as InventoryItem;
            if (row != null)
            {
                ASCIStarINInventoryItemExt itemext = row.GetExtension<ASCIStarINInventoryItemExt>();
                if (itemext == null)
                {
                    PXTrace.WriteInformation("InventoryItem_UsrPricingGRAMSilver_FieldUpdated itemext WAS NULL!");
                    return;

                }
                POVendorInventory vendorItem = DefaultVendorItem.Select();
                foreach (POVendorInventory vitem in Base.VendorItems.Select())
                {
                    PXTrace.WriteInformation($"{vitem.VendorID}:{vitem.IsDefault}");

                    if (vitem.IsDefault == true)
                        vendorItem = vitem;
                }
                if (vendorItem == null)
                {
                    PXTrace.WriteInformation("InventoryItem_UsrPricingGRAMSilver_FieldUpdated vendorItem WAS NULL!");
                    return;
                }
                ASCIStarPOVendorInventoryExt vendorItemExt = vendorItem.GetExtension<ASCIStarPOVendorInventoryExt>();
                if (vendorItemExt == null)
                {
                    PXTrace.WriteInformation("InventoryItem_UsrPricingGRAMSilver_FieldUpdated vendorItemExt WAS NULL!");
                    return;
                }
                if (vendorItemExt.UsrCommodityPrice == null)
                {
                    PXTrace.WriteInformation("InventoryItem_UsrPricingGRAMSilver_FieldUpdated vendorItemExt.UsrCommodityPrice WAS NULL!");
                    return;
                }
                decimal newCost = 0.00m;
                if (itemext.UsrPricingGRAMGold > 0)
                {

                    cache.SetValue<ASCIStarINInventoryItemExt.usrContractIncrement>(cache.Current,
                        (itemext.UsrPricingGRAMGold / itemext.UsrActualGRAMGold / 31.10348m) * (1.000m + (itemext.UsrContractSurcharge / 100.0000m)));

                    newCost += ((decimal)itemext.UsrPricingGRAMGold / 31.10348m * GetCommodityPrice(vendorItem, vendorItemExt, "24K"))
                        * (1 + (decimal)itemext.UsrContractLossPct / 100.0000m)
                        * (1 + (decimal)itemext.UsrContractSurcharge / 100.0000m);
                }
                if (itemext.UsrPricingGRAMSilver > 0)
                {
                    ASCIStarMarketCostHelper.JewelryCost costHelper
                            = new ASCIStarMarketCostHelper.JewelryCost(cache.Graph
                                                , row
                                                , 0.000000m
                                                , 0.000000m
                                                , vendorItem.VendorID
                                                , vendorItemExt.UsrMarketID
                                                , DateTime.Today
                                                , row.BaseUnit);


                    decimal matrixPrice = costHelper.marketCommodityCost;
                    newCost += ((decimal)itemext.UsrPricingGRAMSilver / 31.10348m * matrixPrice /*GetCommodityPrice(vendorItem, vendorItemExt, "SSS")*/ )
                        * (1 + (decimal)itemext.UsrContractLossPct / 100.0000m)
                        * (1 + (decimal)itemext.UsrContractSurcharge / 100.0000m);

                }
                cache.SetValueExt<ASCIStarINInventoryItemExt.usrCommodityCost>(cache.Current, newCost);
            }
        }

        protected virtual void InventoryItem_UsrDutyCostPct_FieldUpdated(PXCache cache, PXFieldUpdatedEventArgs e, PXFieldUpdated InvokeBaseHandler)
        {
            if (InvokeBaseHandler != null)
                InvokeBaseHandler(cache, e);

            InventoryItem row = e.Row as InventoryItem;
            if (row != null)
            {
                ASCIStarINInventoryItemExt ext = row.GetExtension<ASCIStarINInventoryItemExt>();
                cache.SetValueExt<ASCIStarINInventoryItemExt.usrDutyCost>(cache.Current, (ext.UsrCommodityCost
                    + ext.UsrFabricationCost
                    + ext.UsrOtherMaterialCost
                    + ext.UsrLaborCost
                    + ext.UsrHandlingCost
                    + ext.UsrFreightCost
                    + ext.UsrOtherCost
                    + ext.UsrPackagingCost) * (ext.UsrDutyCostPct / 100.0000m));

                INJewelryItemData_MetalType_FieldUpdated(cache, e, InvokeBaseHandler);
            }
        }

        protected virtual void InventoryItem_UsrContractSurcharge_FieldUpdated(PXCache cache, PXFieldUpdatedEventArgs e, PXFieldUpdated InvokeBaseHandler)
        {
            if (InvokeBaseHandler != null)
                InvokeBaseHandler(cache, e);

            InventoryItem row = e.Row as InventoryItem;
            if (row != null)
            {
                ASCIStarINInventoryItemExt ext = row.GetExtension<ASCIStarINInventoryItemExt>();
                if (ext.UsrActualGRAMGold > 0)
                {
                    decimal stdIncrement = (decimal)ext.UsrPricingGRAMGold / (decimal)ext.UsrActualGRAMGold / 31.10348m;
                    decimal ContractIncrement = stdIncrement * (1 + (decimal)ext.UsrContractSurcharge / 100.0000m);
                    PXTrace.WriteInformation($"InventoryItem_UsrContractSurcharge_FieldUpdated: stdIncrement{stdIncrement}: ContractIncrement:{ContractIncrement} SurchargePct{ext.UsrContractSurcharge}");
                    cache.SetValue<ASCIStarINInventoryItemExt.usrContractIncrement>(cache.Current, ContractIncrement);

                    INJewelryItemData_MetalType_FieldUpdated(cache, e, InvokeBaseHandler);


                }
            }
        }

        protected virtual void InventoryItem_UsrCostRollupType_FieldDefaulting(PXCache cache, PXFieldDefaultingEventArgs e, PXFieldDefaulting InvokeBaseHandler)
        {
            if (InvokeBaseHandler != null)
                InvokeBaseHandler(cache, e);

            InventoryItem row = e.Row as InventoryItem;
            if (row != null)
            {
                INItemClass itemClass = INItemClass.PK.Find(cache.Graph, row.ItemClassID);
                if (itemClass == null)
                    return;
                ASCIStarINItemClassExt classExt = itemClass.GetExtension<ASCIStarINItemClassExt>();
                e.NewValue = classExt.UsrCostRollupType ?? CostRollupType.Other;
                PXUIFieldAttribute.SetEnabled<ASCIStarINInventoryItemExt.usrCostRollupType>(cache, row, true);

            }
        }

        #endregion InventoryItem Events

        #region POVendorInventory Events

        //protected virtual void POVendorInventory_UsrMarketID_FieldDefaulting(PXCache cache, PXFieldDefaultingEventArgs e, PXFieldDefaulting InvokeBaseHandler)
        //{
        //    Vendor vendor = SelectFrom<Vendor>
        //    .Where<Vendor.bAccountID.IsEqual<POVendorInventory.vendorID.FromCurrent>>.View.Select(Base);

        //    if (vendor == null)
        //    {
        //        Vendor Market = SelectFrom<Vendor>
        //            .Where<Vendor.acctCD.IsEqual<@P.AsString>>.View.Select(Base, "LONDON PM").FirstOrDefault();

        //        e.NewValue = Market.BAccountID;
        //    }
        //    else
        //    {
        //        ASCIStarVendorExt ext = vendor.GetExtension<ASCIStarVendorExt>();
        //        Vendor Market = SelectFrom<Vendor>
        //            .Where<Vendor.bAccountID.IsEqual<@P.AsInt>>.View.Select(Base, ext.UsrMarketID).FirstOrDefault();
        //        e.NewValue = Market.BAccountID;
        //    }

        //}

        protected virtual void POVendorInventory_UsrCommodityLossPct_FieldUpdated(PXCache cache, PXFieldUpdatedEventArgs e, PXFieldUpdated InvokeBaseHandler)
        {
            if (InvokeBaseHandler != null)
                InvokeBaseHandler(cache, e);
            PXTrace.WriteInformation("POVendorInventory_UsrCommodityLossPct_FieldUpdated");
            POVendorInventory row = e.Row as POVendorInventory;
            ASCIStarPOVendorInventoryExt porow = row.GetExtension<ASCIStarPOVendorInventoryExt>();
            if (porow != null)
            {
                PXTrace.WriteInformation("porow != null");
                InventoryItem item = InventoryItem.PK.Find(cache.Graph, row.InventoryID);
                ASCIStarINInventoryItemExt ext = item.GetExtension<ASCIStarINInventoryItemExt>();
                //ext.UsrContractSurcharge = porow.UsrCommodityLossPct + porow.UsrCommoditySurchargePct;
                cache.SetValueExt<ASCIStarINInventoryItemExt.usrContractSurcharge>(cache.Current, porow.UsrCommodityLossPct + porow.UsrCommoditySurchargePct);



            }
        }

        protected virtual void POVendorInventory_UsrCommoditySurchargePct_FieldUpdated(PXCache cache, PXFieldUpdatedEventArgs e, PXFieldUpdated InvokeBaseHandler)
        {
            if (InvokeBaseHandler != null)
                InvokeBaseHandler(cache, e);
            PXTrace.WriteInformation("POVendorInventory_UsrCommoditySurchargePct_FieldUpdated");

            POVendorInventory row = e.Row as POVendorInventory;
            ASCIStarPOVendorInventoryExt porow = row.GetExtension<ASCIStarPOVendorInventoryExt>();
            if (porow != null)
            {
                PXTrace.WriteInformation("porow != null");
                InventoryItem item = InventoryItem.PK.Find(cache.Graph, row.InventoryID);
                ASCIStarINInventoryItemExt ext = item.GetExtension<ASCIStarINInventoryItemExt>();
                cache.SetValueExt<ASCIStarINInventoryItemExt.usrContractSurcharge>(cache.Current, porow.UsrCommodityLossPct + porow.UsrCommoditySurchargePct);

            }
        }

        protected virtual void POVendorInventory_UsrCommodityPrice_FieldDefaulting(PXCache sender, PXFieldDefaultingEventArgs e)
        {
            PXTrace.WriteInformation($"Entering POVendorInventory_UsrCommodityPrice_FieldDefaulting");
            POVendorInventory row = e.Row as POVendorInventory;
            if (row == null || row.VendorID == null)
                return;

            ASCIStarPOVendorInventoryExt rowExt = row.GetExtension<ASCIStarPOVendorInventoryExt>();
            if (rowExt == null || rowExt.UsrCommodityID == null)
                return;

            ASCIStarMarketCostHelper.JewelryCost costHelper = new ASCIStarMarketCostHelper.JewelryCost(Base, Base.Item.Current);
            Vendor v = PXSelect<Vendor, Where<Vendor.bAccountID, Equal<Required<Vendor.bAccountID>>>>.Select(Base, row.VendorID);
            costHelper.CostBasis.ItemVendor = v;
            ASCIStarVendorExt vExt = v.GetExtension<ASCIStarVendorExt>();

            if (row != null)
            {
                e.NewValue = costHelper.vendorPrice.UsrCommodityPrice;
            }
        }

        protected virtual void POVendorInventory_UsrCommodityIncrement_FieldDefaulting(PXCache sender, PXFieldDefaultingEventArgs e)
        {
            PXTrace.WriteInformation($"Entering POVendorInventory_UsrCommodityIncrement_FieldDefaulting");
            POVendorInventory row = e.Row as POVendorInventory;
            if (row == null || row.VendorID == null)
                return;

            ASCIStarPOVendorInventoryExt rowExt = row.GetExtension<ASCIStarPOVendorInventoryExt>();
            if (rowExt == null || rowExt.UsrCommodityID == null)
                return;

            ASCIStarMarketCostHelper.JewelryCost costHelper = new ASCIStarMarketCostHelper.JewelryCost(Base, Base.Item.Current);
            Vendor v = PXSelect<Vendor, Where<Vendor.bAccountID, Equal<Required<Vendor.bAccountID>>>>.Select(Base, row.VendorID);
            costHelper.CostBasis.ItemVendor = v;
            ASCIStarVendorExt vExt = v.GetExtension<ASCIStarVendorExt>();

            if (row != null)
            {
                e.NewValue = costHelper.vendorPrice.UsrCommodityIncrement;
            }
        }

        protected virtual void POVendorInventory_UsrCommodityLossPct_FieldDefaulting(PXCache sender, PXFieldDefaultingEventArgs e)
        {
            PXTrace.WriteInformation($"Entering POVendorInventory_UsrCommodityIncrement_FieldDefaulting");
            POVendorInventory row = e.Row as POVendorInventory;
            if (row == null || row.VendorID == null)
                return;

            ASCIStarPOVendorInventoryExt rowExt = row.GetExtension<ASCIStarPOVendorInventoryExt>();
            if (rowExt == null || rowExt.UsrCommodityID == null)
                return;

            ASCIStarMarketCostHelper.JewelryCost costHelper = new ASCIStarMarketCostHelper.JewelryCost(Base, Base.Item.Current);
            Vendor v = PXSelect<Vendor, Where<Vendor.bAccountID, Equal<Required<Vendor.bAccountID>>>>.Select(Base, row.VendorID);
            costHelper.CostBasis.ItemVendor = v;
            ASCIStarVendorExt vExt = v.GetExtension<ASCIStarVendorExt>();

            if (row != null)
            {
                e.NewValue = costHelper.vendorPrice.UsrCommodityLossPct;
            }
        }

        protected virtual void POVendorInventory_UsrCommoditySurchargePct_FieldDefaulting(PXCache sender, PXFieldDefaultingEventArgs e)
        {
            PXTrace.WriteInformation($"Entering POVendorInventory_UsrCommodityIncrement_FieldDefaulting");
            POVendorInventory row = e.Row as POVendorInventory;
            if (row == null || row.VendorID == null)
                return;

            ASCIStarPOVendorInventoryExt rowExt = row.GetExtension<ASCIStarPOVendorInventoryExt>();
            if (rowExt == null || rowExt.UsrCommodityID == null)
                return;

            ASCIStarMarketCostHelper.JewelryCost costHelper = new ASCIStarMarketCostHelper.JewelryCost(Base, Base.Item.Current);
            Vendor v = PXSelect<Vendor, Where<Vendor.bAccountID, Equal<Required<Vendor.bAccountID>>>>.Select(Base, row.VendorID);
            costHelper.CostBasis.ItemVendor = v;
            ASCIStarVendorExt vExt = v.GetExtension<ASCIStarVendorExt>();

            if (row != null)
            {
                e.NewValue = costHelper.vendorPrice.UsrCommoditySurchargePct;
            }
        }
        protected virtual void POVendorInventory_UsrVendorDefault_FieldUpdated(PXCache cache, PXFieldUpdatedEventArgs e, PXFieldUpdated InvokeBaseHandler)
        {
            if (InvokeBaseHandler != null)
                InvokeBaseHandler(cache, e);

            POVendorInventory row = e.Row as POVendorInventory;
            if (row != null)
            {
                ASCIStarPOVendorInventoryExt ext = row.GetExtension<ASCIStarPOVendorInventoryExt>();
                bool UseVendor = (ext.UsrVendorDefault == true);
                if (UseVendor)
                {
                    //ext.UsrCommodityPrice
                    //Replace with Vendor Defaults
                }
                PXUIFieldAttribute.SetEnabled<ASCIStarPOVendorInventoryExt.usrCommodityPrice>(cache, row, !UseVendor);
                PXUIFieldAttribute.SetEnabled<ASCIStarPOVendorInventoryExt.usrCommodityIncrement>(cache, row, !UseVendor);
                PXUIFieldAttribute.SetEnabled<ASCIStarPOVendorInventoryExt.usrCommoditySurchargePct>(cache, row, !UseVendor);
                PXUIFieldAttribute.SetEnabled<ASCIStarPOVendorInventoryExt.usrCommodityLossPct>(cache, row, !UseVendor);

            }
        }

        #endregion POVendorInventory Events

        #endregion Event Handlers

        #region Helper Methods


        protected decimal GetCommodityPrice(POVendorInventory vendorItem, ASCIStarPOVendorInventoryExt vendorItemExt, string Commodity)
        {


            decimal price = 0.00000m;

            if (Commodity != "24K" && Commodity != "SSS")
            {
                PXTrace.WriteWarning($"{Commodity} must be either 24K or SSS, returning $0 price");
                return 0;
            }


            //Vendor vendor = new PXSelect<Vendor, 
            //    Where<Vendor.bAccountID, 
            //    Equal<Required<Vendor.bAccountID>>>>(Base).SelectSingle(Base, vendorItem.VendorID);
            //ASCIStarVendorExt vendorExt = vendor.GetExtension<ASCIStarVendorExt>();


            //PXTrace.WriteInformation($"NO VENDOR ASSIGNED TO ITEM, DEFAULTING $20 SILVER");
            //APVendorPrice systemDefault =
            //new PXSelect<APVendorPrice,
            //Where<APVendorPrice.vendorID, Equal<Required<APVendorPrice.vendorID>>,
            //    And<APVendorPrice.inventoryID, Equal<Required<APVendorPrice.inventoryID>>,
            //    And<APVendorPrice.uOM, Equal<Required<APVendorPrice.uOM>>,
            //    And<APVendorPrice.effectiveDate, LessEqual<Required<APVendorPrice.effectiveDate>>,
            //    And<APVendorPrice.effectiveDate, LessEqual<Required<APVendorPrice.effectiveDate>>>>>>>,
            //OrderBy<Desc<APVendorPrice.effectiveDate>>>(Base).SelectSingle(
            //    vendorItem.VendorID,
            //    Commodity,
            //    "TOZ",
            //    DateTime.Today,
            //    DateTime.Today);

            //price = (decimal)systemDefault.SalesPrice;
            //PXTrace.WriteInformation($"{vendor.AcctCD}:{Commodity}:{vendorDefPrice.EffectiveDate}:{price}");
            //return 0.00m;
            price = (decimal)vendorItemExt.UsrCommodityPrice;
            Vendor vendor = new PXSelect<Vendor, Where<Vendor.bAccountID, Equal<Required<Vendor.bAccountID>>>>(Base).SelectSingle(vendorItem.VendorID);


            if (vendorItemExt.UsrVendorDefault == false || vendorItemExt.UsrVendorDefault == false)
            {

                InventoryItem commodity = new PXSelect<InventoryItem, Where<InventoryItem.inventoryCD, Equal<Required<InventoryItem.inventoryCD>>>>(Base).SelectSingle(Commodity);

                APVendorPrice vendorDefPrice =
                new PXSelect<APVendorPrice,
                Where<APVendorPrice.vendorID, Equal<Required<APVendorPrice.vendorID>>,
                    And<APVendorPrice.inventoryID, Equal<Required<APVendorPrice.inventoryID>>,
                    And<APVendorPrice.uOM, Equal<Required<APVendorPrice.uOM>>,
                    And<APVendorPrice.effectiveDate, LessEqual<Required<APVendorPrice.effectiveDate>>,
                    And<APVendorPrice.effectiveDate, LessEqual<Required<APVendorPrice.effectiveDate>>>>>>>,
                OrderBy<Desc<APVendorPrice.effectiveDate>>>(Base).SelectSingle(
                    vendorItem.VendorID,
                    commodity.InventoryID,
                    "TOZ",
                    DateTime.Today,
                    DateTime.Today);
                price = (decimal)vendorDefPrice.SalesPrice;
                PXTrace.WriteInformation($"{vendor.AcctCD}:{Commodity}:{vendorDefPrice.EffectiveDate}:{price}");
            }
            PXTrace.WriteInformation($"{vendor.AcctCD}:{Commodity}:{System.DateTime.MinValue}:{price}");

            return price;
            //}

        }

        protected void RecalculateCosts(PXCache cache, InventoryItem item, int? vendorId = null, DateTime? PriceAt = null)
        {
            //decimal silverGrams = 0;



            if (item == null) return;

            ASCIStarINInventoryItemExt itemExt = item.GetExtension<ASCIStarINInventoryItemExt>();


            ASCIStarMarketCostHelper.JewelryCost costHelper = new ASCIStarMarketCostHelper.JewelryCost(cache.Graph, item, 0.000000m);

            cache.SetValue<ASCIStarINInventoryItemExt.usrDutyCost>(item, (itemExt.UsrCommodityCost
                + itemExt.UsrFabricationCost
                + itemExt.UsrLaborCost
                + itemExt.UsrHandlingCost
                + itemExt.UsrFreightCost
                + itemExt.UsrOtherCost
                + itemExt.UsrPackagingCost) * (itemExt.UsrDutyCostPct / 100.0000m));


        }

        protected void RecalculateCommodity(PXCache cache, InventoryItem item, int vendorId)
        {
            POVendorInventory itemVendor = (POVendorInventory)PXSelect<POVendorInventory,
                Where<POVendorInventory.inventoryID, Equal<Required<InventoryItem.inventoryID>>,
                And<POVendorInventory.vendorID, Equal<Required<POVendorInventory.vendorID>>>>>.Select(cache.Graph, item.InventoryID, vendorId);
            ASCIStarPOVendorInventoryExt basis = itemVendor.GetExtension<ASCIStarPOVendorInventoryExt>();


        }


        #endregion Helper Methods


        #region Action
        public PXAction<InventoryItem> recalcMarketCost;
        [PXUIField(DisplayName = "Update Market Cost", MapEnableRights = PXCacheRights.Select, MapViewRights = PXCacheRights.Select)]
        [PXButton]
        public virtual IEnumerable RecalcMarketCost(PXAdapter adapter)
        {


            return adapter.Get();
        }
        #endregion Action


    }
}