using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buffalo.Core.Container
{
    public class GlobalObjsPoolContainer
    {
        public static MessageContainer GlobalObject_MessageContainer = new MessageContainer();

        public virtual void Start()
        {

        }

    }
}
