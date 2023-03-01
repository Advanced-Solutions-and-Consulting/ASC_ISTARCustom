using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PX.Common;
using PX.Data;
using PX.Objects.GL;
using PX.Objects.CM;
using PX.Objects.CS;
using PX.Objects.CR;
using PX.Objects.TX;
using PX.Objects.IN;
using PX.Objects.EP;
using PX.Objects.AP;
using PX.Objects.AR;
using PX.Objects.SO;
using SOOrder = PX.Objects.SO.SOOrder;
using SOLine = PX.Objects.SO.SOLine;
using PX.Data.DependencyInjection;
using PX.Data.ReferentialIntegrity.Attributes;

using PX.Objects.PM;
using CRLocation = PX.Objects.CR.Standalone.Location;
using PX.Objects.AP.MigrationMode;
using PX.Objects.Common;
using PX.Objects.Common.Discount;

using PX.Data.BQL;
using PX.Objects.Common.Bql;
using PX.Objects.Extensions.CostAccrual;
using PX.Objects.DR;
using PX.Data.WorkflowAPI;
using PX.Objects.Extensions;
using PX.Objects.Common.DAC;
using PX.Objects.Common.Scopes;
using PX.Objects.IN.Services;
using PX.Objects;
using PX.Objects.PO;
//using InfoSmartSearch;

namespace ASCISTARCustom
{
    public class ASCIStarPOOrderEntryExt : PXGraphExtension<POOrderEntry>
    {
        PXSelect<INKitSpecHdr, Where<INKitSpecHdr.kitInventoryID, Equal<Required<INKitSpecHdr.kitInventoryID>>>> CostItem;

        PXSelect<POVendorInventory,
            Where<POVendorInventory.vendorID, Equal<Current<POOrder.vendorID>>,
                And<POVendorInventory.inventoryID, Equal<Current<POLine.inventoryID>>>>> vendorItemSelect;

        PXSelect<APVendorPrice,
           Where<APVendorPrice.vendorID, Equal<Required<APVendorPrice.vendorID>>,
               And<APVendorPrice.inventoryID, Equal<Required<APVendorPrice.inventoryID>>,
                   And<APVendorPrice.effectiveDate, Equal<Required<APVendorPrice.effectiveDate>>>>>> vendorPrice;

        PXSelect<InventoryItemCurySettings,
            Where<InventoryItemCurySettings.inventoryID, Equal<Required<InventoryItemCurySettings.inventoryID>>>> itemCurySettings;

        PXSelect<InventoryItem,
            Where<InventoryItem.inventoryID, Equal<Required<InventoryItemCurySettings.inventoryID>>>> inventoryItem;

        #region Static Method
        public static bool IsActive()
        {
            return true;
        }
        #endregion

        #region Private methods

        private void Update(int? vendorID, int? inventoryID, decimal salesPrice, DateTime effectiveDate)
        {
            POOrder poOrder = Base?.Document?.Current;

            InventoryItemMaint itemGraph = PXGraph.CreateInstance<InventoryItemMaint>();
            itemGraph.Item.Current = itemGraph.Item.Search<InventoryItem.inventoryID>(inventoryID);
            itemGraph.itemstats.Current.LastCost = salesPrice;
            itemGraph.Save.Press();

        }

        #endregion

        #region Event Handlers

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
        protected void POLine_OrderQty_FieldVerifying(PXCache cache, PXFieldVerifyingEventArgs e, PXFieldVerifying InvokeBaseHandler)
        {
            string msg = $"POLine_OrderQty_FieldVerifying{Environment.NewLine}";
            try
            {
                //MOVE STANDARD COSTING TO REMOVE CAUTION SYMBOL!!!

                if (InvokeBaseHandler != null)
                    InvokeBaseHandler(cache, e);
                var row = (POLine)e.Row;
                if (row == null) return;

                decimal? cost = decimal.Zero;
                POOrder po = Base.Document.Current;
                ASCIStarPOOrderExt poExt = po.GetExtension<ASCIStarPOOrderExt>();

                InventoryItem item = InventoryItem.PK.Find(cache.Graph, row.InventoryID);
                msg += $"item.InventoryCD:{item.InventoryCD}{Environment.NewLine}";

                ASCIStarINInventoryItemExt itemExt = item.GetExtension<ASCIStarINInventoryItemExt>();
                //INKitSpecHdr kit = CostItem.Select(cache, item.InventoryID);
                //ASCIStarINKitSpecHdrExt kitExt = kit.GetExtension<ASCIStarINKitSpecHdrExt>();
                //weight = weight + itemExt.UsrActualGRAMSilver + itemExt.UsrActualGRAMGold /* + itemExt.UsrActualGRAMPlatinum*/;


                ASCIStarMarketCostHelper.JewelryCost costHelper =
                    new ASCIStarMarketCostHelper.JewelryCost(cache.Graph
                                                            , item
                                                            , 0.000000m
                                                            , 0.000000m
                                                            , po.VendorID
                                                            , poExt.UsrMarketID
                                                            , po.ExpectedDate
                                                            , row.UOM
                                                            , po.CuryID
                                                            , false);


                //POVendorInventory vendorItem = vendorItemSelect.Select(cache.Graph);
                //ASCIStarPOVendorInventoryExt vendorItemExt = vendorItem.GetExtension<ASCIStarPOVendorInventoryExt>();

                //itemExt.UsrCommodityCost = (itemExt.UsrPricingGRAMGold / 31.10348m * GetCommodityPrice(vendorItem, vendorItemExt, "24K")) +
                //   (itemExt.UsrPricingGRAMSilver / 31.10348m * GetCommodityPrice(vendorItem, vendorItemExt, "SSS"))
                //   * (1 + itemExt.UsrContractLossPct / 100.0000m)
                //   * (1 + itemExt.UsrContractSurcharge / 100.0000m);

                msg += $"MarketCost Cost: {costHelper.MarketCost}{Environment.NewLine}";
                msg += $"Contract Cost: {costHelper.ContractCost}{Environment.NewLine}";
                msg += $"Unit Cost: {costHelper.UnitCost}{Environment.NewLine}";
                PXTrace.WriteInformation(msg);


                row.UnitCost = costHelper.MarketCost;
                row.ExtCost = row.OrderQty * row.UnitCost;
            }
            catch(Exception err)
            {
                PXTrace.WriteInformation(msg);
                throw err;
            }
        }


        protected virtual void POLine_CuryUnitCost_FieldDefaulting(PXCache sender, PXFieldDefaultingEventArgs e, PXFieldDefaulting InvokeBaseHandler)
        {
            string msg = $"POLine_CuryUnitCost_FieldDefaulting{ Environment.NewLine}";

            //if (InvokeBaseHandler != null)
                InvokeBaseHandler(sender, e);
            POLine row = e.Row as POLine;
            POOrder po = Base.Document.Current;
            if (row == null) return;
            if (row.InventoryID == null) return;
            if (row.UOM == null) return;

            ASCIStarPOOrderExt poExt = po.GetExtension<ASCIStarPOOrderExt>();
            InventoryItem item = InventoryItem.PK.Find(sender.Graph, row.InventoryID);
            ASCIStarINInventoryItemExt itemExt = item.GetExtension<ASCIStarINInventoryItemExt>();

            if (itemExt.UsrPricingGRAMGold == 0 && itemExt.UsrPricingGRAMSilver == 0)
                return;

            ASCIStarMarketCostHelper.JewelryCost costHelper =
                new ASCIStarMarketCostHelper.JewelryCost(sender.Graph
                                                        , item
                                                        , 0.000000m
                                                        , 0.000000m
                                                        , po.VendorID
                                                        , poExt.UsrMarketID
                                                        , po.ExpectedDate
                                                        , row.UOM
                                                        , po.CuryID
                                                        , false);



            msg += $"MarketCost Cost: {costHelper.MarketCost}{Environment.NewLine}";
            msg += $"Contract Cost: {costHelper.ContractCost}{Environment.NewLine}";
            msg += $"Unit Cost: {costHelper.UnitCost}{Environment.NewLine}";
            PXTrace.WriteInformation(msg);

            row.UnitCost = costHelper.MarketCost;
            row.ExtCost = row.OrderQty * row.UnitCost;
            e.NewValue = costHelper.MarketCost;

        }

        //protected void POLine_InventoryID_FieldUpdated(PXCache cache, PXFieldUpdatedEventArgs e, PXFieldUpdated InvokeBaseHandler)
        //{
        //    if (InvokeBaseHandler != null)
        //        InvokeBaseHandler(cache, e);
        //    var row = (POLine)e.Row;
        //    if (row == null) return;

        //    decimal? cost = decimal.Zero;

        //    POOrder po = POOrder.PK.Find(cache.Graph, row.OrderType, row.OrderNbr);
        //    ASCIStarPOOrderExt poExt = po.GetExtension<ASCIStarPOOrderExt>();

        //    InventoryItem item = InventoryItem.PK.Find(cache.Graph, row.InventoryID);
        //    PXTrace.WriteInformation(item.InventoryCD);
        //    ASCIStarINInventoryItemExt itemExt = item.GetExtension<ASCIStarINInventoryItemExt>();
        //    PXTrace.WriteInformation($"{itemExt.UsrPricingGRAMGold.ToString() ?? "0.000000"} GRAMS 24k GOLD");
        //    //PXTrace.WriteInformation($"{itemExt.UsrPricingGRAMPlatinum.ToString() ?? "0.000000"} GRAMS PLATINUM");
        //    PXTrace.WriteInformation($"{itemExt.UsrPricingGRAMSilver.ToString() ?? "0.000000"} GRAMS STERLING SILVER");
        //    //INKitSpecHdr kit = CostItem.Select(cache, item.InventoryID);
        //    //ASCIStarINKitSpecHdrExt kitExt = kit.GetExtension<ASCIStarINKitSpecHdrExt>();
        //    //weight = weight + itemExt.UsrActualGRAMSilver + itemExt.UsrActualGRAMGold /* + itemExt.UsrActualGRAMPlatinum*/;



        //    ASCIStarMarketCostHelper.JewelryCost costHelper =
        //        new ASCIStarMarketCostHelper.JewelryCost(cache.Graph
        //                                                , item
        //                                                , 0.000000m
        //                                                , 0.000000m
        //                                                , po.VendorID
        //                                                , poExt.UsrMarketID
        //                                                , po.ExpectedDate
        //                                                , row.UOM
        //                                                , po.CuryID
        //                                                , false);



        //    cost = costHelper.ContractCost;

        //    PXTrace.WriteInformation($"Contract Cost: {costHelper.ContractCost}");
        //    PXTrace.WriteInformation($"Unit Cost: {costHelper.UnitCost}");

        //    //if(costHelper.ContractCost > 0)
        //    //{
        //    //    this.Update(po.VendorID, item.InventoryID, cost ?? 0.0m, costHelper.pricingDate ?? DateTime.Today.AddDays(-1));
        //    //}
        //}

        //protected void POOrder_VendorID_FieldUpdated(PXCache cache, PXFieldUpdatedEventArgs e, PXFieldUpdated InvokeBaseHandler)
        //{
        //    if (InvokeBaseHandler != null)
        //        InvokeBaseHandler(cache, e);
        //    var row = (POOrder)e.Row;
        //    if (row == null) return;
        //    Vendor vendor = Vendor.PK.Find(cache.Graph, row.VendorID);
        //    ASCIStarVendorExt vendorExt = vendor.GetExtension<ASCIStarVendorExt>();
        //    ASCIStarPOOrderExt extPO = row.GetExtension<ASCIStarPOOrderExt>();
        //    extPO.UsrMarketID = vendorExt.UsrMarketID;
        //    extPO.UsrSetupID = vendorExt.UsrSetupID;


        //}

        public class ASCPOOrderExt : PXGraphExtension<POOrderEntry>
        {
            // Overriden method
            [PXButton(CommitChanges = true)]
            [PXUIField(DisplayName = "Email Purchase Order", MapEnableRights = PXCacheRights.Select, MapViewRights = PXCacheRights.Select)]
            public virtual IEnumerable EmailPurchaseOrder(
                PXAdapter adapter,
                [PXString] string notificationCD = null)
            {
                bool massProcess = adapter.MassProcess;
                PXLongOperation.StartOperation(Base, (PXToggleAsyncDelegate)(() =>
                {
                    bool flag = false;
                    foreach (POOrder currentItem in adapter.Get<POOrder>())
                    {
                        if (massProcess)
                            PXProcessing<POOrder>.SetCurrentItem((object)currentItem);
                        try
                        {
                            Base.Document.Cache.Current = (object)currentItem;
                            Dictionary<string, string> parameters = new Dictionary<string, string>();
                            parameters["POOrder.OrderType"] = currentItem.OrderType;
                            parameters["POOrder.OrderNbr"] = currentItem.OrderNbr;
                            using (PXTransactionScope transactionScope = new PXTransactionScope())
                            {
                                var list = PXNoteAttribute.GetFileNotes(Base.Document.Cache, Base.Document.Current).Select(attachment => (Guid?)attachment).ToList();
                                ASCIStarPOOrderExt currentExt = currentItem.GetExtension<ASCIStarPOOrderExt>();
                                NotificationSetup ns = NotificationSetup.PK.Find(Base, currentExt.UsrSetupID);
                                Base.Activity.SendNotification(
                                    "Vendor",
                                    (ns.NotificationCD ?? "PURCHASE ORDER"),
                                    currentItem.BranchID,
                                    parameters,
                                    list);
                                Base.Save.Press();
                                transactionScope.Complete();
                            }
                            if (massProcess)
                                PXProcessing<POOrder>.SetProcessed();
                        }
                        catch (Exception ex)
                        {
                            if (!massProcess)
                            {
                                throw;
                            }

                            Base.Document.Cache.SetStatus((object)currentItem, PXEntryStatus.Notchanged);
                            Base.Document.Cache.Remove((object)currentItem);
                            PXProcessing<POOrder>.SetError(ex);
                            flag = true;
                        }
                    }
                    if (flag)
                        throw new PXOperationCompletedWithErrorException("At least one item has not been processed.");
                }));
                return adapter.Get<POOrder>();
            }
        }



        #endregion


    }
}