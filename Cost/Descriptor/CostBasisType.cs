
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRLocation = PX.Objects.CR.Standalone.Location;
using PX.Common;
using PX.Data.ReferentialIntegrity.Attributes;
using PX.Data;
using PX.Objects.AP;
using PX.Objects.CM;
using PX.Objects.Common.Bql;
using PX.Objects.Common;
using PX.Objects.CR;
using PX.Objects.CS;
using PX.Objects.EP;
using PX.Objects.IN;
using PX.Objects.PM;
using PX.Objects.PO;
using PX.Objects;
using PX.SM;
using PX.TM;


namespace ASCISTARCustom
{
    public class CostBasisType
    {
        public class ListAttribute : PXStringListAttribute
        {
            public ListAttribute() : base(
                new[] { Market.Substring(0, 1), "X", Item.Substring(0, 1), Vendor.Substring(0, 1) },
                new[] { Market, Matrix, Item, Vendor }
                )
            { }
        }

        public const string Market = "Market";
        public const string Matrix = "Matrix";
        public const string Item = "Item"; // GRAMS * Market Price from Matrix * (1 + Surcharge) * (1 + Loss Percentage)
        public const string Vendor = "Vendor"; // Get Fixed Price from Assembly


        public class market : PX.Data.BQL.BqlString.Constant<market>
        {
            public market() : base(Market) { }

        }

        public class matrix : PX.Data.BQL.BqlString.Constant<matrix>
        {
            public matrix() : base("X") { }

        }
        public class item : PX.Data.BQL.BqlString.Constant<item>
        {
            public item() : base(Item) { }

        }
        public class vendor : PX.Data.BQL.BqlString.Constant<vendor>
        {
            public vendor() : base(Vendor) { }

        }


    }
}

