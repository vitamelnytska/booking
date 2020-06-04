using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library_hotel
{
    class BookOrder: Order
    {
        public BookOrder(string name, string phone, int days, int persons, RoomType type, DateTime now, DateTime ind, int room, int wish) : base(name, phone, days, persons, type, now, ind,room,wish)
        {

        }
        protected internal override event OrderStateHandler NewOrder; // when is making new order
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

        //method for make order
        public override void MakeOrder()
        {
            DateTime BookNow = Now;
            NewOrder(this, new OrderEvents($"Booking order for {Persons} persons and for {Days} days. "));
            Console.WriteLine($"Time the end of order is in {BookNow.AddDays(Days)}");
            BookInd = BookNow.AddDays(Days);
            GetCost = Persons * Days * Convert.ToDecimal(Type);
            OnCost(new OrderEvents($"sum of order:{Persons * Days * Convert.ToDecimal(Type)}"));
        }
        // method for show message when order is declained
        public override void Declain()
        {
            GetCost = 0;
            
            IsDeclain(this, new OrderEvents("Book order is delete"));
        }
    //     method to print information of order
        public override void Print()
        {
            Console.WriteLine("Order: Booking ");
            Console.WriteLine($"Room: {Type}");
            Console.WriteLine($"Name {Name}, phone number {Phone} ");
            Console.WriteLine($"Order for {Persons} persons and for {Days} days");
            Console.WriteLine($"Time the end of order is in {Now.AddDays(Days)}");
            OnCost(new OrderEvents($"sum of order:{Persons * Days * Convert.ToDecimal(Type)}"));

        }
    }
}
