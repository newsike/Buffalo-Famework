using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buffalo.Core.Container
{

    public enum ErrLevel
    {
        Below=0,
        Normal=1,
        Above=2
    }

    public enum CodeErrMessage
    {
        InvalidatedKeyWord = 0,
        InvalidatedParam = 1,
        UnselectedElement = 2,
        InvalidatedParamValue=3
    }

    public class MessageItem
    {
        public int SourceLineIndex
        {
            set;
            get;
        }

        public CodeErrMessage ErrorMessage
        {
            set;
            get;
        }

        public ErrLevel ErrorLevel
        {
            set;
            get;
        }

        public bool Interrup
        {
            set;
            get;
        }

    }

    public class MessageContainer
    {
        private List<MessageItem> _MessagePool = new List<MessageItem>();

        public void Action_InsertMessage(int SourceLineIndex,CodeErrMessage errMessage,ErrLevel errorLevel,bool isInterrup)
        {
            MessageItem newItem = new MessageItem();
            newItem.SourceLineIndex = SourceLineIndex;
            newItem.ErrorMessage = errMessage;
            newItem.ErrorLevel = errorLevel;
            newItem.Interrup = isInterrup;
        }

    }
}
