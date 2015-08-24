using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buffalo.Basic.Base
{
    public class ExtendedExcptions:ApplicationException
    {
        public ExtendedExcptions()
        {            
            List<string> msgExceptions=new List<string>();
            msgExceptions.Add(this.Message);
            msgExceptions.Add(this.Source);
            msgExceptions.Add(this.StackTrace);
            LogServices.WriteLog("Exception", msgExceptions);
        }
    }
}
