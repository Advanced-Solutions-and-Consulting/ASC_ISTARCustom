﻿using CRLocation = PX.Objects.CR.Standalone.Location;
using PX.Common;
//using PX.Data.BQL.Fluent;
using PX.Data.ReferentialIntegrity.Attributes;
using PX.Data;
using PX.Objects.AP;
using PX.Objects.AR;
using PX.Objects.CM;
using PX.Objects.CR;
using PX.Objects.CS;
using PX.Objects.GL;
using PX.Objects.IN;
using PX.Objects.PO;
using PX.Objects;
using System.Collections.Generic;
using System;

namespace ASCISTARCustom
{
    public class ASCIStarPOVendorInventoryExt : PXCacheExtension<PX.Objects.PO.POVendorInventory>
    {

        #region Fields

        #region MarketID
        public abstract class usrMarketID : PX.Data.BQL.BqlInt.Field<usrMarketID> { }
        protected Int32? _usrMarketID;
        //[APTranInventoryItem(Filterable = true)]
        [PXSelector(
        typeof(Search2<Vendor.bAccountID, InnerJoin<VendorClass, On<Vendor.vendorClassID, Equal<VendorClass.vendorClassID>>>,
            Where<VendorClass.vendorClassID, Equal<MarketClass>>>),
            typeof(Vendor.acctCD),
            typeof(Vendor.acctName)

            , SubstituteKey = typeof(Vendor.acctCD)
            , DescriptionField = typeof(Vendor.acctName))]
        [PXUIField(DisplayName = "Market")]
        [PXDefault(PersistingCheck = PXPersistingCheck.Nothing)]
        [PXDBInt()]
        public virtual Int32? UsrMarketID
        {
            get
            {
                return this._usrMarketID;
            }
            set
            {
                this._usrMarketID = value;
            }
        }
        #endregion

        #region CommodityID
        public abstract class usrCommodityID : PX.Data.BQL.BqlInt.Field<usrCommodityID> { }
        protected Int32? _CommodityID;
        //[APTranInventoryItem(Filterable = true)]
        [PXSelector(
        typeof(Search2<InventoryItem.inventoryID, InnerJoin<INItemClass, On<InventoryItem.itemClassID, Equal<INItemClass.itemClassID>>>,
            Where<INItemClass.itemClassCD, Equal<CommodityClass>>>),
            typeof(InventoryItem.inventoryCD),
            typeof(InventoryItem.descr)

            , SubstituteKey = typeof(InventoryItem.inventoryCD)
            , DescriptionField = typeof(InventoryItem.descr))]
        [PXUIField(DisplayName = "Metal")]
        [PXDBInt()]
        [PXDefault(PersistingCheck = PXPersistingCheck.Nothing)]
        public virtual Int32? UsrCommodityID
        {
            get
            {
                return this._CommodityID;
            }
            set
            {
                this._CommodityID = value;
            }
        }
        #endregion

        #region UsrVendorDefault
        [PXDBBool()]
        [PXUIField(DisplayName = "Override Vendor")]
        [PXDefault(false)]
        public virtual bool? UsrVendorDefault { get; set; }
        public abstract class usrVendorDefault : PX.Data.BQL.BqlBool.Field<usrVendorDefault> { }
        #endregion

        #region UsrCommodityPrice
        [PXDBDecimal(6)]
        [PXUIField(DisplayName = "Market Price")]
        public virtual decimal? UsrCommodityPrice { get; set; }
        public abstract class usrCommodityPrice : PX.Data.BQL.BqlDecimal.Field<usrCommodityPrice> { }
        #endregion

        #region UsrCommodityIncrement
        [PXDBDecimal(6)]
        [PXUIField(DisplayName = "Market Increment")]
        public virtual decimal? UsrCommodityIncrement { get; set; }
        public abstract class usrCommodityIncrement : PX.Data.BQL.BqlDecimal.Field<usrCommodityIncrement> { }
        #endregion

        #region UsrCommodityLossPct
        [PXDBDecimal(4)]
        [PXUIField(DisplayName = "Loss Pct")]
        public virtual decimal? UsrCommodityLossPct { get; set; }
        public abstract class usrCommodityLossPct : PX.Data.BQL.BqlDecimal.Field<usrCommodityLossPct> { }
        #endregion

        #region UsrCommoditySurchargePct
        [PXDBDecimal(4)]
        [PXUIField(DisplayName = "Surcharge Pct")]
        public virtual decimal? UsrCommoditySurchargePct { get; set; }
        public abstract class usrCommoditySurchargePct : PX.Data.BQL.BqlDecimal.Field<usrCommoditySurchargePct> { }
        #endregion

        #region UsrCommodityCost
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Metal Cost")]
        public virtual Decimal? UsrCommodityCost { get; set; }
        public abstract class usrCommodityCost : PX.Data.BQL.BqlDecimal.Field<usrCommodityCost> { }
        #endregion

        #region UsrOtherMaterialCost
        [PXUIField(DisplayName = "Other Materials")]
        [PXDBDecimal(6, MinValue = 0, MaxValue = 1000)]
        [PXDefault(TypeCode.Decimal, "0.000000", PersistingCheck = PXPersistingCheck.Null)]
        public virtual Decimal? UsrOtherMaterialCost { get; set; }
        public abstract class usrOtherMaterialCost : PX.Data.BQL.BqlDecimal.Field<usrOtherMaterialCost> { }
        #endregion

        #region UsrFabricationCost
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Fabrication Cost")]
        public virtual Decimal? UsrFabricationCost { get; set; }
        public abstract class usrFabricationCost : PX.Data.BQL.BqlDecimal.Field<usrFabricationCost> { }
        #endregion

        #region UsrPackagingCost

        [PXUIField(DisplayName = "Packaging Cost")]
        [PXDBDecimal(6, MinValue = 0, MaxValue = 1000)]
        [PXDefault(TypeCode.Decimal, "0.000000", PersistingCheck = PXPersistingCheck.Null)]
        public virtual Decimal? UsrPackagingCost { get; set; }
        public abstract class usrPackagingCost : PX.Data.BQL.BqlDecimal.Field<usrPackagingCost> { }
        #endregion

        #region UsrLaborCost
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Labor Cost")]
        public virtual Decimal? UsrLaborCost { get; set; }
        public abstract class usrLaborCost : PX.Data.BQL.BqlDecimal.Field<usrLaborCost> { }
        #endregion

        #region UsrHandlingCost
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Handling Cost")]
        public virtual Decimal? UsrHandlingCost { get; set; }
        public abstract class usrHandlingCost : PX.Data.BQL.BqlDecimal.Field<usrHandlingCost> { }
        #endregion

        #region UsrFreightCost
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Freight Cost")]
        public virtual Decimal? UsrFreightCost { get; set; }
        public abstract class usrFreightCost : PX.Data.BQL.BqlDecimal.Field<usrFreightCost> { }
        #endregion

        #region UsrDutyCost
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Duty Cost")]
        public virtual Decimal? UsrDutyCost { get; set; }
        public abstract class usrDutyCost : PX.Data.BQL.BqlDecimal.Field<usrDutyCost> { }
        #endregion

        #region UsrOtherCost
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Other Cost")]
        public virtual Decimal? UsrOtherCost { get; set; }
        public abstract class usrOtherCost : PX.Data.BQL.BqlDecimal.Field<usrOtherCost> { }
        #endregion

        #region UnitCost
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Unit Cost")]
        public virtual Decimal? UsrUnitCost { get; set; }
        public abstract class usrUnitCost : PX.Data.BQL.BqlDecimal.Field<usrUnitCost> { }
        #endregion

        #endregion Fields
    }
}