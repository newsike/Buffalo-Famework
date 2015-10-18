using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buffalo.Basic.Base
{

    public enum MessageLevel
    {
        Normal=0,
        Interrupt=1,
        Abort=2
    }

    public class MessageItem
    {
        public MessageLevel SelectedMessageLevel;
        public string Message;
        public string CodeIndex;        
    }

    public class MessageServices
    {
        Queue<MessageItem> _MessageQueue = new Queue<MessageItem>();

        public void SetNewMessage(MessageLevel Level,string Message,string CodeIndex)
        {
            MessageItem newItem = new MessageItem();
            newItem.CodeIndex = CodeIndex;
            newItem.Message = Message;
            newItem.SelectedMessageLevel = Level;
        }

    }
}
