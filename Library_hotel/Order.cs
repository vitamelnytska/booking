using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library_hotel
{
    public abstract class Order: IOrder 
    {
        protected internal virtual event OrderStateHandler NewOrder; // when is making new order 
        protected internal virtual event OrderStateHandler IsDeclain; // when order is declained
       protected internal virtual event OrderStateHandler IsConfirm; //when order is confirmed
        protected internal virtual event OrderStateHandler HowCost; //when cost of order is determined
        public int Days { get; private set; } // value of number days for booking
        public int Persons { get; private set; } // value of amount of persons for booking
        public RoomType Type { get; private set; } // value type of room
        public string Name { get; private set; } //name of client
        public string Phone { get; private set; } //phone number of client
        public decimal GetCost { get; set; } // cost or order
        public DateTime Now { get; private set; } // date now
        public DateTime Ind { get; private set; } // date of reserve
        public DateTime BookInd { get; set; } // date of end of booking
        public int NumberOfOrder { get; set; } // number of order
        public int Room { get; private set; } // index of types of rooms
        public int Wish { get; private set; } // index of type of order
        static int counter = 0; //static counter for index of order

        //constructor of Order
        public Order(string name, string phone, int days, int persons, RoomType type, DateTime now, DateTime ind, int room, int wish)
        {
           
             Name = name;  Phone = phone;  Days = days; Persons = persons;  Type = type;
            NumberOfOrder = ++counter; Now = now; Ind = ind; Room = room;Wish = wish;
        }
        // method if time of order(reserve) is finished
        public virtual void IsFinished() { Bill();  }
        //method for call event
        private void CallEvent(OrderEvents e, OrderStateHandler handler)
        {
            if (e != null)
                handler?.Invoke(this, e);
        }
        // method for show message of event when cost is determined
        protected virtual void OnCost(OrderEvents e)
        {
            CallEvent(e, HowCost);
        }
        
        // method for determine cost of order
        public virtual void Bill()
        {
            GetCost = Persons * Days * Convert.ToDecimal(Type);
        }
        //method for make order
        public virtual void MakeOrder()
        {

            NewOrder(this, new OrderEvents($"Your order for {Persons} persons and for {Days} days"));

            OnCost(new OrderEvents($"You must to pay {Persons * Days * Convert.ToDecimal(Type)}"));
        }
        // method for show message when order is confirmed
        public virtual void Confirm()
        {
            IsConfirm(this, new OrderEvents("Order is made"));
        }
        // method for show message when order is declained
        public virtual void Declain()
        {
            IsDeclain(this, new OrderEvents("Order is declained"));
        }
        // method to print information of order
        public virtual void Print()
        {
            Console.WriteLine("Order ");
            Console.WriteLine($"Room: {Type}");
            Console.WriteLine($"Name {Name}, phone number {Phone} ");
            Console.WriteLine($"Order for {Persons} persons and for {Days} days");
            Console.WriteLine($"Time the end of order is in {BookInd}");
        }
    }
}
