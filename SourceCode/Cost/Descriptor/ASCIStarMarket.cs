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
    public class MarketList
    {
        public class ListAttribute : PXStringListAttribute
        {
            public ListAttribute() : base(
                new[] { NewYork, LondonAM, LondonPM },
                new[] { MessageNewYork, MessageLondonAM, MessageLondonPM }
                )
            { }
        }

        public const string NewYork = "NY"; 
        public const string LondonAM = "LA"; 
        public const string LondonPM = "LP"; 

        public const string MessageNewYork = "NEW YORK"; 
        public const string MessageLondonAM = "LONDON AM"; 
        public const string MessageLondonPM = "LONDON PM"; 


        public class newYork : PX.Data.BQL.BqlString.Constant<newYork>
        {
            public newYork() : base(NewYork) { }
            
        }
        public class londonAM : PX.Data.BQL.BqlString.Constant<londonAM>
        {
            public londonAM() : base(LondonAM) { }
        }

        public class londonPM : PX.Data.BQL.BqlString.Constant<londonPM>
        {
            public londonPM() : base(LondonPM) { }
        }
        public class defaultMarket : PX.Data.BQL.BqlString.Constant<defaultMarket>
        {
            public defaultMarket() : base(MessageLondonPM) { }
        }

    }
    public class MarketClass : PX.Data.BQL.BqlString.Constant<MarketClass>
    {
        public static readonly string value = "MARKET";
        public MarketClass() : base(value) { }
    }
}

