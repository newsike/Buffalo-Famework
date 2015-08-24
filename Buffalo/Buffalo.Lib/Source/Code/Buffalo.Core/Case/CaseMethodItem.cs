using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Buffalo.Core.Case
{
    public class CaseMethodItem
    {
      
        public ElementActions.MethodItem ActiveMethod
        {
            set;
            get;
        }

        public int Index
        {
            set;
            get;
        }

        public bool isLoaded
        {
            set;
            get;
        }

    }
}
