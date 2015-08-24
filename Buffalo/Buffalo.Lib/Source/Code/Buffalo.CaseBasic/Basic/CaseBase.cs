using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Buffalo.CaseBasic.Basic
{
    public class CaseBase
    {



        public string CaseName
        {
            set;
            get;
        }

        public string CaseID
        {
            set;
            get;
        }

        public XmlDocument CaseDoc
        {
            set;
            get;
        }

     }
}
