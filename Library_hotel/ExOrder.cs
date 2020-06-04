using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library_hotel
{
    public class ExOrder: Order
    {
        public ExOrder(string name, string phone, int days, int persons, RoomType type, DateTime now, DateTime ind, int room, int wish) : base(name, phone, days, persons, type, now, ind, room, wish)
        {

        }
        protected internal override event OrderStateHandler IsDeclain; // when order is declained
        protected internal override event OrderStateHandler HowCost;  //when cost of order is determined

        //method for call event
        private void CallEvent(OrderEvents e, OrderStateHandler handler)
        {
            if (e != null)
                handler?.Invoke(this, e);
        }
        // method for show message of event when cost is determined
        protected override void OnCost(OrderEvents e)
        {
            CallEvent(e, HowCost);
        }
        public override void Declain()
        {
            IsDeclain(this, new OrderEvents("Time is over"));
        }
        // method to print information of order
        public override void Print()
        {
            Console.WriteLine($"Ex Order:  ");
            Console.WriteLine($"Room: {Type}");
            Console.WriteLine($"Name {Name}, phone number {Phone} ");
            Console.WriteLine($"Order for {Persons} persons and for {Days} days");
            OnCost(new OrderEvents($"sum of order:{Persons * Days * Convert.ToDecimal(Type)}"));

        }

    }
}
