using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library_hotel
{
   public class ReserveOrder: Order
    {
        public ReserveOrder(string name, string phone, int days, int persons, RoomType type, DateTime now, DateTime ind, int room, int wish) :base(name,phone,days,persons,type, now,ind,room,wish)
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

        // method if time of order(reserve) is finished
        public override void IsFinished() { GetCost = Persons * Days * Convert.ToDecimal(Type);  }

        //method for make order
        public override void MakeOrder()
        {
            
            double daysOfReserve = (Ind - Now).TotalDays;
            if (daysOfReserve == 0)
            {
                Console.WriteLine("The date of reserve is finished");
                IsFinished();
            }
            OnCost(new OrderEvents($"The reservation is for {Ind}. To this date is {daysOfReserve} days. After finish time you get money "));
            NewOrder(this, new OrderEvents($"Reservation of order for {Persons} persons and for {Days} days"));
            OnCost(new OrderEvents($"sum of order:{Persons * Days * Convert.ToDecimal(Type)}"));
        }
        // method for show message when order is declained

        public override void Declain()
        {
            GetCost = 0;
            
            IsDeclain(this, new OrderEvents("Reserve order is declained"));

        }
        // method to print information of order
        public override void Print()
        {
            Console.WriteLine("Order: Reservation ");
            Console.WriteLine($"Room: {Type}");
            Console.WriteLine($"Name {Name}, phone number {Phone} ");
            Console.WriteLine($"Order for {Persons} persons and for {Days} days");
            Console.WriteLine($"The reservation is for {Ind}.");
            OnCost(new OrderEvents($"sum of order:{Persons * Days * Convert.ToDecimal(Type)}"));
        }

    }
}
