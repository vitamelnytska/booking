using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library_hotel
{
    public delegate void OrderStateHandler(object sender, OrderEvents e); // delegate for performing events

    public class OrderEvents
    {
        public string Message { get; private set; } // for messages
        public OrderEvents(string mes) //constructor of OrderEvents
        {
            Message = mes; 
        }
    }
}
