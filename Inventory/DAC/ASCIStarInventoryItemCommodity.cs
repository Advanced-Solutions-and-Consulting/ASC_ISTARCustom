//using System;
//using PX.Data;
//using PX.Data.ReferentialIntegrity.Attributes;
//using PX.Objects;
//using PX.Objects.AR;
//using PX.Objects.IN;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace ASCISTARCustom
//{

//    [Serializable]
//    [PXCacheName("Inventory Item Commodity")]
//    public class ASCIStarInventoryItemCommodity : IBqlTable
//    {
//        #region Keys
//        public class PK : PrimaryKeyOf<ASCIStarInventoryItemCommodity>.By<inventoryID, commodityID >
//        {
//            public static ASCIStarInventoryItemCommodity Find(PXGraph graph, int inventoryID, int commodityID) => FindBy(graph, inventoryID, commodityID);
//        }
//        public static class FK
//        {
//            public class InventoryItem : PX.Objects.IN.InventoryItem.PK.ForeignKeyOf<ASCIStarInventoryItemCommodity>.By<inventoryID> { }
//            public class CommodityItem : PX.Objects.IN.InventoryItem.PK.ForeignKeyOf<ASCIStarInventoryItemCommodity>.By<commodityID> { }
//        }
//        #endregion

//        #region InventoryID
//        [PXDBInt(IsKey = true)]
//        [PXUIField(DisplayName = "Inventory ID")]
//        //[PXDBDefault(typeof(INKitSpecHdr.kitInventoryID))]
//        public virtual int? InventoryID { get; set; }
//        public abstract class inventoryID : PX.Data.BQL.BqlInt.Field<inventoryID> { }
//        #endregion

//        protected Int32? _CommodityID;
//        [ARTranInventoryItem(Filterable = true, IsKey = true)]
//        [PXUIField(DisplayName = "Metal ID")]
//        [PXSelector(
//        typeof(Search2<InventoryItem.inventoryID, LeftJoin<INItemClass, On<InventoryItem.itemClassID, Equal<INItemClass.itemClassID>>>,
//            Where<INItemClass.itemClassCD, Equal<CommodityClass>>>),
//            typeof(InventoryItem.inventoryCD),
//            typeof(InventoryItem.descr)

//            , SubstituteKey = typeof(InventoryItem.inventoryCD)
//            , DescriptionField = typeof(InventoryItem.descr))]
//        [PXForeignReference(typeof(FK.InventoryItem))]
//        public virtual Int32? CommodityID
//        {
//            get
//            {
//                return this._CommodityID;
//            }
//            set
//            {
//                this._CommodityID = value;
//            }
//        }
//        public abstract class commodityID : PX.Data.BQL.BqlInt.Field<commodityID> { }

//        #region WtInOunce
//        [PXDBDecimal()]
//        [PXUIField(DisplayName = "Wt In Ounce")]
//        public virtual Decimal? WtInOunce { get; set; }
//        public abstract class wtInOunce : PX.Data.BQL.BqlDecimal.Field<wtInOunce> { }
//        #endregion

//        #region WtInGram
//        [PXDBDecimal()]
//        [PXUIField(DisplayName = "Wt In Gram")]
//        public virtual Decimal? WtInGram { get; set; }
//        public abstract class wtInGram : PX.Data.BQL.BqlDecimal.Field<wtInGram> { }
//        #endregion

//        #region Wtindwt
//        [PXDBDecimal()]
//        [PXUIField(DisplayName = "Wt in DWT")]
//        public virtual Decimal? WtinDWT { get; set; }
//        public abstract class wtinDWT : PX.Data.BQL.BqlDecimal.Field<wtinDWT> { }
//        #endregion

//        #region RateInOunce
//        [PXDBDecimal()]
//        [PXUIField(DisplayName = "Rate In Ounce")]
//        public virtual Decimal? RateInOunce { get; set; }
//        public abstract class rateInOunce : PX.Data.BQL.BqlDecimal.Field<rateInOunce> { }
//        #endregion

//        #region RateInGram
//        [PXDBDecimal()]
//        [PXUIField(DisplayName = "Rate In Gram")]
//        public virtual Decimal? RateInGram { get; set; }
//        public abstract class rateInGram : PX.Data.BQL.BqlDecimal.Field<rateInGram> { }
//        #endregion

//        #region Rateindwt
//        [PXDBDecimal()]
//        [PXUIField(DisplayName = "Rateindwt")]
//        public virtual Decimal? RateInDWT { get; set; }
//        public abstract class rateInDWT : PX.Data.BQL.BqlDecimal.Field<rateInDWT> { }
//        #endregion
//    }
//}
