using PX.Common;
using PX.Data.BQL;
using PX.Data.ReferentialIntegrity.Attributes;
using PX.Data;
using PX.Objects.AP;
using PX.Objects.CM;
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
using PX.Objects.SO;
using PX.Objects.TX;
using PX.Objects;
using System.Collections.Generic;
using System;

namespace ASCISTARCustom
{
    public class ASCIStarSOLineExt : PXCacheExtension<PX.Objects.SO.SOLine>
    {
        #region Static Method
        public static bool IsActive()
        {
            return true;
        }
        #endregion

        #region UsrEndLocation
        [PXDBInt]
        [PXUIField(DisplayName = "End Location")]
        [PXSelector(typeof(Search<Location.locationID, Where<Location.bAccountID, Equal<Current<SOOrder.customerID>>>>), SubstituteKey = typeof(Location.locationCD))]
        public virtual int? UsrEndLocation { get; set; }
        public abstract class usrEndLocation : PX.Data.BQL.BqlInt.Field<usrEndLocation> { }
        #endregion

    }
}