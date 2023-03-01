using System;
using PX.Data;
using PX.Data.ReferentialIntegrity.Attributes;
using PX.Objects.IN;

namespace ASCISTARCustom
{
    [Serializable]
    [PXCacheName("ASCIStarINKitSpecHdrAttribute")]
    public class ASCIStarINKitSpecHdrAttribute : IBqlTable
    {
        #region Keys
        public class PK : PrimaryKeyOf<ASCIStarINKitSpecHdrAttribute>.By<kitInventoryID, revisionID>
        {
            
            public static ASCIStarINKitSpecHdrAttribute Find(PXGraph graph, int? kitInventoryID, string revisionID) => FindBy(graph, kitInventoryID, revisionID);
        }
        #endregion
        #region KitInventoryID
        [PXDBInt(IsKey = true)]
        [PXUIField(DisplayName = "Kit Inventory ID")]
        [PXDBDefault(typeof(INKitSpecHdr.kitInventoryID))]
        public virtual int? KitInventoryID { get; set; }
        public abstract class kitInventoryID : PX.Data.BQL.BqlInt.Field<kitInventoryID> { }
        #endregion

        #region RevisionID
        [PXDBString(10, IsKey = true, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Revision ID")]
        [PXDBDefault(typeof(INKitSpecHdr.revisionID))]
        [PXParent(typeof(Select<INKitSpecHdr,
          Where<INKitSpecHdr.kitInventoryID, Equal<Current<ASCIStarINKitSpecHdrAttribute.kitInventoryID>>,And<INKitSpecHdr.revisionID, Equal<Current<ASCIStarINKitSpecHdrAttribute.revisionID>>>>>))]
        public virtual string RevisionID { get; set; }
        public abstract class revisionID : PX.Data.BQL.BqlString.Field<revisionID> { }
        #endregion

        #region SilverMarket
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Silver Market")]
        public virtual Decimal? SilverMarket { get; set; }
        public abstract class silverMarket : PX.Data.BQL.BqlDecimal.Field<silverMarket> { }
        #endregion

        #region TargetFirstCost
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Target First Cost")]
        public virtual Decimal? TargetFirstCost { get; set; }
        public abstract class targetFirstCost : PX.Data.BQL.BqlDecimal.Field<targetFirstCost> { }
        #endregion

        #region TargetMargin
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Target Margin")]
        public virtual Decimal? TargetMargin { get; set; }
        public abstract class targetMargin : PX.Data.BQL.BqlDecimal.Field<targetMargin> { }
        #endregion

        #region TargetListPrice
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Target List Price")]
        public virtual Decimal? TargetListPrice { get; set; }
        public abstract class targetListPrice : PX.Data.BQL.BqlDecimal.Field<targetListPrice> { }
        #endregion

        #region TargetRetail
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Target Retail")]
        public virtual Decimal? TargetRetail { get; set; }
        public abstract class targetRetail : PX.Data.BQL.BqlDecimal.Field<targetRetail> { }
        #endregion

        #region TotalSilverWt
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Total Silver Wt")]
        public virtual Decimal? TotalSilverWt { get; set; }
        public abstract class totalSilverWt : PX.Data.BQL.BqlDecimal.Field<totalSilverWt> { }
        #endregion
        #region ProductWt
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Product Wt")]
        public virtual Decimal? ProductWt { get; set; }
        public abstract class productWt : PX.Data.BQL.BqlDecimal.Field<productWt> { }
        #endregion

        #region FinishedWt
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Finished Wt")]
        public virtual Decimal? FinishedWt { get; set; }
        public abstract class finishedWt : PX.Data.BQL.BqlDecimal.Field<finishedWt> { }
        #endregion

        #region ActualFirstCost
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Actual First Cost")]
        public virtual Decimal? ActualFirstCost { get; set; }
        public abstract class actualFirstCost : PX.Data.BQL.BqlDecimal.Field<actualFirstCost> { }
        #endregion

        #region ActualMargin
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Actual Margin")]
        public virtual Decimal? ActualMargin { get; set; }
        public abstract class actualMargin : PX.Data.BQL.BqlDecimal.Field<actualMargin> { }
        #endregion

        #region ActualListPrice
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Actual List Price")]
        public virtual Decimal? ActualListPrice { get; set; }
        public abstract class actualListPrice : PX.Data.BQL.BqlDecimal.Field<actualListPrice> { }
        #endregion
        #region FreightType
        [PXDBString(1,IsFixed =true)]
        [PXDefault("1")]
        [PXUIField(DisplayName = "Freight")]
        public virtual string FreightType { get; set; }
        public abstract class freightType : PX.Data.BQL.BqlString.Field<freightType> { }
        #endregion

        #region FreightCost
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Freight Cost")]
        public virtual Decimal? FreightCost { get; set; }
        public abstract class freightCost : PX.Data.BQL.BqlDecimal.Field<freightCost> { }
        #endregion

        #region FreightPercent
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Freight Percent")]
        public virtual Decimal? FreightPercent { get; set; }
        public abstract class freightPercent : PX.Data.BQL.BqlDecimal.Field<freightPercent> { }
        #endregion

        #region TotalFreightCost
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Freight Cost")]
        public virtual Decimal? TotalFreightCost { get; set; }
        public abstract class totalFreightCost : PX.Data.BQL.BqlDecimal.Field<totalFreightCost> { }
        #endregion

        #region DutyPercent
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Duty Percent")]
        public virtual Decimal? DutyPercent { get; set; }
        public abstract class dutyPercent : PX.Data.BQL.BqlDecimal.Field<dutyPercent> { }
        #endregion

        #region DutyCost
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Duty Cost")]
        public virtual Decimal? DutyCost { get; set; }
        public abstract class dutyCost : PX.Data.BQL.BqlDecimal.Field<dutyCost> { }
        #endregion

        #region ComponentCost
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Component Cost")]
        public virtual Decimal? ComponentCost { get; set; }
        public abstract class componentCost : PX.Data.BQL.BqlDecimal.Field<componentCost> { }
        #endregion

        #region LaborCost
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Labor Cost")]
        public virtual Decimal? LaborCost { get; set; }
        public abstract class laborCost : PX.Data.BQL.BqlDecimal.Field<laborCost> { }
        #endregion

        #region SupportCost
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Support Cost")]
        public virtual Decimal? SupportCost { get; set; }
        public abstract class supportCost : PX.Data.BQL.BqlDecimal.Field<supportCost> { }
        #endregion

        #region MiscCost
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Misc. Cost")]
        public virtual Decimal? MiscCost { get; set; }
        public abstract class miscCost : PX.Data.BQL.BqlDecimal.Field<miscCost> { }
        #endregion
    }

    public class Numbers
    {
        public const string One = "1";
        public const string Two = "2";

        public class one : PX.Data.BQL.BqlString.Constant<one>
        {
            public one() : base(One) {; }
        }

        public class two : PX.Data.BQL.BqlString.Constant<two>
        {
            public two() : base(Two) {; }
        }
    }
}