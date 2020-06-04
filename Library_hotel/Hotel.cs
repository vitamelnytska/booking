using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library_hotel
{
    public enum RoomType //types of rooms of Hotel and their prices
    {
        Lux=1300, Class=700, Econom=500
    }
    public enum UserWish //types of orders
    {
        Reservation, Booking
    }
    public class Hotel<T> where T : Order //class Hotel made on list of orders
    {
        public string NameofHotel { get; private set; } //name of hotel
        private int LuxRooms; // amount of available Lux rooms
        private int ClassRooms;  // amount of available Clas rooms
        private int EconomRooms; // amount of available Econom rooms
        private int BookedLuxRooms=0; //amount of booked Lux rooms
        private int BookedClassRooms=0;  //amount of booked Class rooms
        private int BookedEconomRooms=0; //amount of booked Econom rooms
        private int RLuxRooms=0; //amount of reserved Lux rooms
        private int RClassRooms=0; //amount of reserved Class rooms
        private int REconomRooms=0; //amount of reserved Econom rooms
        private int index_room; //index of types of rooms
        public decimal bill=0; // profit of hotel
        public T[] orders; //new orders
        public Hotel(string name, int l, int c, int e) //constructor of class Hotel
        {
            this.NameofHotel = name; this.LuxRooms = l;this.ClassRooms = c;this.EconomRooms = e;
        }
        public void HotelRooms() // method to show available rooms
        {
            
            Console.WriteLine(" Amonut of rooms:");
            Console.WriteLine($"Lux: {LuxRooms}\t Class: {ClassRooms}\t Econom: {EconomRooms}\n\n");
        }
        //method for decrement available rooms
        public void minusRoom(int i) 
        {
            if (i == 1)
            {
                LuxRooms -= 1; 
            }
            else if (i == 2)
            {
                ClassRooms -= 1; 
            }
            else
            {
                EconomRooms -= 1; 
            }
        }
        //method for decrement reserve or booked rooms and increment available rooms
        public void DeclainRoom(int i,int b)
        {
            if (i == 1)
            {
                LuxRooms += 1;
                
            }
            else if (i == 2)
            {
                ClassRooms += 1;
            }
            else
            {
                EconomRooms += 1;
            }
            if (b == 1)
            {
                if (i == 1)
                {
                    BookedLuxRooms -= 1;
                }
                else if (i == 2)
                {
                    BookedClassRooms -= 1;
                }
                else
                {
                    BookedEconomRooms -= 1;
                }
            }
            if (b == 2)
            {
                if (i == 1)
                {
                    RLuxRooms -= 1;
                }
                else if (i == 2)
                {
                    RClassRooms -= 1;
                }
                else
                {
                    REconomRooms -= 1;
                }
            }
        }
        //method for increment booked rooms
        public void Book(int i)
        {
            if (i == 1)
            {
                 BookedLuxRooms += 1;
            }
            else if (i == 2)
            {
                BookedClassRooms += 1;
            }
            else
            {
                BookedEconomRooms += 1;
            }
        }
        //method for increment reserved rooms
        public void Reserve(int i)
        {
            if (i == 1)
            {
                RLuxRooms += 1;
            }
            else if (i == 2)
            {
                RClassRooms += 1;
            }
            else
            {
                REconomRooms += 1;
            }
        }
        //method to show booked rooms
        public void BookedRooms()
        {
            Console.WriteLine($"Booked rooms:\n Lux: {BookedLuxRooms}\tClass: {BookedClassRooms} \tEconom: {BookedEconomRooms}");
        }
        //method to show reserved rooms
        public void ReservedRooms()
        {
            Console.WriteLine($"Reserved rooms:\n Lux: {RLuxRooms}\tClass: {RClassRooms} \tEconom: {REconomRooms}");
        }
        //method to make order
        public void ToOrder(DateTime inDate,DateTime now, RoomType type, int days, int persons, UserWish wish, string name, string phone,
            OrderStateHandler MakeOrder, OrderStateHandler cost, OrderStateHandler declain, OrderStateHandler confirm )
        {
            
            T newOrder = null; 
            int Wish = 0;
            // for choosing index of type of room
            switch (type)
            {
                case RoomType.Class:
                    index_room = 2;
                    break;
                case RoomType.Econom:
                    index_room = 3;
                    break;
                case RoomType.Lux:
                    index_room = 1;
                    break;
            }
            // choosing type of order 
            switch(wish)
            {
                case UserWish.Booking:
                    Wish = 1;
                    newOrder = new BookOrder(name, phone,days,persons,type,now,inDate,index_room,Wish) as T;
                    Book(index_room);
                    
                    break;
                case UserWish.Reservation:
                    Wish = 2;
                    newOrder = new ReserveOrder(name, phone, days, persons, type,now,inDate,index_room,Wish) as T;
                    Reserve(index_room);
                    break;
            }
            //making list of orders
            if (orders == null)
                orders = new T[] { newOrder };
            else
            {
                T[] tempOrders = new T[orders.Length + 1];
                for (int j = 0; j < orders.Length; j++)
                    tempOrders[j] = orders[j];
                tempOrders[tempOrders.Length - 1] = newOrder;
                orders = tempOrders;
            }
            // making order and using method of class Order
            newOrder.HowCost += cost;
            newOrder.IsDeclain += declain;
            newOrder.IsConfirm += confirm;
            newOrder.NewOrder += MakeOrder;
            
            newOrder.MakeOrder();
            minusRoom(index_room); //decrement available rooms
            
        }
      
        //method to declain order
        public void Declain(string name, string phone, int index_r, int wish)
        {
            int index;
            //finding order
            T order = FindOrder(name, phone,out index);
            if (order == null)
                throw new Exception("Order by name didn't find");
            DeclainRoom(index_r, wish);
            order.Declain();
            if (orders.Length <= 1)
                orders = null;
            else
            {
                T[] temporders = new T[orders.Length - 1];
                for (int i = 0, j = 0; i < orders.Length; i++)
                {
                    if (i != index)
                        temporders[j++] = orders[i];
                }

                orders = temporders;
            }
        }
        //method to delete order
        public void Delete(DateTime now, OrderStateHandler MakeOrder, OrderStateHandler cost, OrderStateHandler declain, OrderStateHandler confirm)
        {

            for (int i = 0; i < orders.Length; i++)
            {
                if (orders.Length != 0)
                {
                    if (orders[i].BookInd == now&& orders[i] is BookOrder )
                    {
                        T newBorder = new ExOrder(Convert.ToString(orders[i].Name), orders[i].Phone, orders[i].Days, orders[i].Persons, orders[i].Type, orders[i].Now, orders[i].BookInd, orders[i].Room, orders[i].Wish) as T;
                        

                        Declain(orders[i].Name, orders[i].Phone, orders[i].Room, 1);
                        if (orders == null)
                            orders = new T[] { newBorder };
                        else
                        {
                            T[] tempOrders = new T[orders.Length + 1];
                            for (int j = 0; j < orders.Length; j++)
                                tempOrders[j] = orders[j];
                            tempOrders[tempOrders.Length - 1] = newBorder;
                            orders = tempOrders;
                        }

                        newBorder.HowCost += cost;
                        newBorder.IsDeclain += declain;
                        newBorder.IsConfirm += confirm;
                        newBorder.NewOrder += MakeOrder;
                        

                    }

                }
            }
        }
        //method to confirm order
        public void Confirm(string name, string phone)
        {
            //finding order
            T order = FindOrder(name, phone);
            if (order == null)
                throw new Exception("Order by name didn't find");
            order.Confirm();
            // add sum of order to profit of hotel
            bill += order.GetCost;
        }
        //method to find order
        public T FindOrder(string name, string phone)
        {
            for (int i = 0; i < orders.Length; i++)
            {
                if (orders[i].Name == name&&orders[i].Phone==phone)
                    return orders[i];
            }
            return null;
        }
        //overloaded version of finding order
        public T FindOrder(string name, string phone, out int index)
        {
            for(int i = 0; i<orders.Length; i++)
            {
                if (orders[i].Name == name && orders[i].Phone == phone)
                {
                    index = i;
                    return orders[i];
                }
            }
            index = -1;
            return null;
        }
        //method to show former orders
        public T FormerOrders()
        {
            if (orders == null)
                Console.WriteLine("There aren't former orders yet");
            else

            {
                for (int i = 0; i < orders.Length; i++)
                {
                    if(orders[i] is ExOrder)
                        orders[i].Print();
                }
            }
            return null;
        }
        // method to show actual orders
        public T ActualOrders()
        {
             if (orders == null)
                Console.WriteLine("There aren't orders yet");
            else

            {
                for (int i = 0; i < orders.Length; i++)
                {
                    if( (orders[i] is ReserveOrder)||orders[i] is BookOrder)
                        orders[i].Print();
                }
            }
            return null;
        }
        // method to show profit of hotel
        public void HotelBill()
        {
            if (orders != null)
            {
                
                Console.WriteLine($"Hotel get {bill} money");
            }
            else
                Console.WriteLine("The bill is empty");
        } 
        //method to make from reserve order booked order
        public void ResInBook(DateTime now,OrderStateHandler MakeOrder, OrderStateHandler cost, OrderStateHandler declain, OrderStateHandler confirm)
        {
             
            for (int i = 0; i < orders.Length; i++)
            {
                if (orders.Length != 0)
                {
                    if (orders[i] is ReserveOrder && orders[i].Ind == now)
                    {
                        T newBorder = new BookOrder(Convert.ToString(orders[i].Name + "Booking"), orders[i].Phone, orders[i].Days, orders[i].Persons, orders[i].Type, orders[i].Ind, orders[i].Ind, orders[i].Room, orders[i].Wish) as T;
                        Book(newBorder.Room);

                        Declain(orders[i].Name, orders[i].Phone, orders[i].Room, orders[i].Wish);
                        if (newBorder == null)
                            throw new Exception("order didn't make");
                        if (orders == null)
                            orders = new T[] { newBorder };
                        else
                        {
                            T[] tempOrders = new T[orders.Length + 1];
                            for (int j = 0; j < orders.Length; j++)
                                tempOrders[j] = orders[j];
                            tempOrders[tempOrders.Length - 1] = newBorder;
                            orders = tempOrders;
                        }

                        newBorder.HowCost += cost;
                        newBorder.IsDeclain += declain;
                        newBorder.IsConfirm += confirm;
                        newBorder.NewOrder += MakeOrder;

                        newBorder.MakeOrder();
                        minusRoom(newBorder.Room);
                        Confirm(newBorder.Name, newBorder.Phone);

                    }

                }
            }
        }
        //return date of reserve order
        public void Date(DateTime now, OrderStateHandler MakeOrder, OrderStateHandler cost, OrderStateHandler declain, OrderStateHandler confirm)
        {
                for (int j = 0; j < orders.Length; j++)
                {
                    
                    if(orders[j] is BookOrder && now == orders[j].BookInd)
                    {
                        Delete(now,MakeOrder, cost, declain, confirm);
                      
                    }
                    if (orders[j] is ReserveOrder && now == orders[j].Ind)
                    {
                        Console.WriteLine("Reserves is finished");
                        ResInBook(now, MakeOrder, cost, declain, confirm);
                    }
            }
        }
    }
}
