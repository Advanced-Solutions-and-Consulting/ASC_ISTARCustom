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
    public class CommodityType
    {

        public class ListAttribute : PXStringListAttribute
        {
            public ListAttribute() : base(
                new[] { Undefined, Gold, Silver, Platinum, Costume, Brass },
                new[] { MessageUndefined, MessageGold, MessageSilver, MessagePlatinum, MessageCostume, MessageBrass }
                )
            { }
        }

        //ADD BRASS HERE
        public const string Undefined = "U";    // (U)ndefined
        public const string Gold = "G";         // (G)mporting
        public const string Silver = "S";       // (S)ilver
        public const string Platinum = "P";     // (P)latinum
        public const string Costume = "C";      // (C)ostume
        public const string Brass = "B";        // (C)ostume

        public const string MessageUndefined = "  ";
        public const string MessageGold = "Gold";
        public const string MessageSilver = "Silver";
        public const string MessagePlatinum = "Platinum";
        public const string MessageCostume = "Costume";
        public const string MessageBrass = "Brass";


        public class undefined : PX.Data.BQL.BqlString.Constant<undefined>
        {
            public undefined() : base(Undefined) { }
        }
        public class gold : PX.Data.BQL.BqlString.Constant<gold>
        {
            public gold() : base(Gold) { }
        }
        public class silver : PX.Data.BQL.BqlString.Constant<silver>
        {
            public silver() : base(Silver) { }
        }
        public class platinum : PX.Data.BQL.BqlString.Constant<platinum>
        {
            public platinum() : base(Platinum) { }
        }

        public class costume : PX.Data.BQL.BqlString.Constant<costume>
        {
            public costume() : base(Costume) { }
        }
        public class brass : PX.Data.BQL.BqlString.Constant<brass>
        {
            public brass() : base(Brass) { }
        }

    }
    public class CommodityClass : PX.Data.BQL.BqlString.Constant<CommodityClass>
    {
        public static readonly string value = "COMMODITY";
        public CommodityClass() : base(value) { }
    }

    public class TOZ : PX.Data.BQL.BqlString.Constant<TOZ>
    {
        public static readonly string value = "TOZ";
        public TOZ() : base(value) { }
    }
    public class TOZ2GRAM : PX.Data.BQL.BqlDecimal.Constant<TOZ2GRAM>
    {
        public static readonly decimal value = 31.10348m;
        public TOZ2GRAM() : base(value) { }
    }
}

