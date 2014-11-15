using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Messaging;

namespace MSMQDemoCS
{
    class Program
    {

        private MessageQueue mq;
        public string myText = "Not Initialized";

        private void GetChannel()
        {
            if (MessageQueue.Exists(@".\Private$\MyQueue1"))
                mq = new System.Messaging.MessageQueue(@".\Private$\MyQueue1");
            else
                mq = MessageQueue.Create(@".\Private$\MyQueue1");
            Console.WriteLine(" Queue Created ");
        }

        private void Populate()
        {
            Message msg = new System.Messaging.Message();
            myText = "Message body text";
            msg.Body = myText;
            msg.Label = "Richard Haley III";
            mq.Send(msg);
            Console.WriteLine(" Posted in MyQueue1 ");
        }

        private string GetResult()
        {
            Message msg;
            string str = "";
            string label = "";
            try
            {
                msg = mq.Receive(new TimeSpan(0, 0, 50));
                msg.Formatter = new XmlMessageFormatter(new String[] { "System.String,mscorlib" });
                str = msg.Body.ToString();
                label = msg.Label;
            }
            catch { str = " Error in GetResult()"; }
            Console.WriteLine(" Received from " + label);
            return str;
        }

        static void Main(string[] args)
        {
            Program p = new Program();
            p.GetChannel();
            p.Populate();
            string result = p.GetResult();
            Console.WriteLine(" send: {0} ", p.myText);
            Console.WriteLine(" receive: {0} ", result);
            Console.ReadLine();
        }
    }
}
