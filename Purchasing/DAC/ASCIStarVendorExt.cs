using PX.Common;
using PX.Data.BQL;
using PX.Data.ReferentialIntegrity.Attributes;
using PX.Data;
using PX.Objects.AP;
using PX.Objects.CA;
using PX.SM;
using PX.Objects.Common.Bql;
using PX.Objects.Common.Discount.Attributes;
using PX.Objects.Common.Discount;
using PX.Objects.Common;
using PX.Objects.CR;
using PX.Objects.CS;
using PX.Objects.GL;
using PX.Objects.IN.Matrix.Interfaces;
using PX.Objects.IN;
using PX.Objects.PM;
using PX.Objects.PO;
using PX.Objects.TX;
using PX.Objects;
using System.Collections.Generic;
using System;

namespace ASCISTARCustom
{
    public class ASCIStarVendorExt : PXCacheExtension<PX.Objects.AP.Vendor>
    {

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


        #region UsrSetupID
        public abstract class usrSetupID : PX.Data.BQL.BqlGuid.Field<usrSetupID> { }
        [PXDBGuid()]
        [PXSelector(typeof(Search<NotificationSetup.setupID,
            Where<NotificationSetup.sourceCD, Equal<APNotificationSource.vendor>,
                And<NotificationSetup.module, Like<PXModule.po_>>>>),
            DescriptionField = typeof(NotificationSetup.notificationCD),
            SelectorMode = PXSelectorMode.DisplayModeText | PXSelectorMode.NoAutocomplete)]
        [PXDefault(PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = "Mailing ID")]
        public virtual Guid? UsrSetupID
        {
            get;
            set;
        }
        #endregion



    }
}