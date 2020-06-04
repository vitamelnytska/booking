using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library_hotel;

namespace hotel
{
    
    class Program
    {
        static void Main(string[] args)
        {
            Hotel<Order> hotel = new Hotel<Order>("Romantik", 10,30,60); //new Hotel object with name and amount of rooms 
            Console.WriteLine(hotel.NameofHotel); 
            hotel.HotelRooms(); //available rooms
            bool alive = true;
            DateTime now = DateTime.Today;
            ConsoleColor color = Console.ForegroundColor;
            Console.ForegroundColor = color;
            while (alive) //for check if date is entered correct
            {

                Console.WriteLine("Enter today Date: ");
                try
                {
                    now = Convert.ToDateTime(Console.ReadLine());
                    break;
                }
                catch (Exception ex)
                {
                    color = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(ex.Message);
                    Console.ForegroundColor = color;
                }
            }
            //checking for correct entering 
            while (alive)
            {
                
                //color = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Yellow;

                Console.WriteLine(" 1. Make an order \t 2. Available rooms \t 3. Booked room");
                Console.WriteLine(" 4. Rooms in reserved \t 5. Actual orders \t 6. Former order \t 7. Profit \t 8. Exit \n 9. Enter today Date ");
                
                Console.WriteLine("Enter number of action: ");
                Console.ForegroundColor = color;
                try
                {
                    int command = Convert.ToInt32(Console.ReadLine());
                    switch (command)
                    {
                        case 1:
                            newOrder(hotel,now); //make order
                            break;
                        case 2:
                            hotel.HotelRooms(); //show available rooms
                            break;
                        case 3:
                            hotel.BookedRooms(); //show booked rooms
                            break;
                        case 4:
                            hotel.ReservedRooms(); //show reserve rooms
                            break;
                        case 5:
                            hotel.ActualOrders(); //show actual orders
                            break;
                        case 6:
                            hotel.FormerOrders(); //show ex orders
                            break;
                        case 7:
                            hotel.HotelBill(); //show profit of hotel
                            break;
                        case 8:
                            alive = false; //to exict from programme
                            continue;
                        case 9:
                            Console.WriteLine("Enter date today: ");
                            now = Convert.ToDateTime(Console.ReadLine());
                            hotel.Date(now, NewOrderHandler, CostHandler, DeclainHandler, ConfirmHandler);
                           // checkReserve(hotel); //check date today with date of reserve orders
                            break;

                    }
                    
                } //catchinf for errors
                catch (Exception ex)
                {
                    color = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(ex.Message);
                    Console.ForegroundColor = color;
                }

            }   
        }
        
        //method to make new order
        private static void newOrder(Hotel<Order> hotel, DateTime now)
        {
            bool alive = true;
            string Name = null;
            string Phone = null;
            ConsoleColor color = Console.ForegroundColor;
            Console.ForegroundColor = color;
             
            while (alive)
            {
                Console.WriteLine("Please, enter name and phone number of client.");
                try
                {
                    string name = Convert.ToString(Console.ReadLine());
                    string phone = Convert.ToString(Console.ReadLine());
                    //checking of enterring name and phone number
                    if (name.Length < 3)
                        throw new Exception();
                    else if (phone.Length < 10 || phone.Length >12)
                        throw new Exception();
                    else
                    {
                        Name = name;
                        Phone = phone;
                        alive = false;
                    }
                    Console.WriteLine("Enter number of room: 1. Lux \t 2. Class \t 3. Econom");
                    RoomType room = 0;

                    int type = Convert.ToInt32(Console.ReadLine());
                    if (type == 1)
                        room = RoomType.Lux;
                    else if (type == 2)
                        room = RoomType.Class;
                    else if (type == 3)
                        room = RoomType.Econom;
                    else
                        throw new Exception();

                    Console.WriteLine("Enter days: ");
                    int days = Convert.ToInt32(Console.ReadLine());
                    Console.WriteLine("Enter persons: ");
                    int persons = Convert.ToInt32(Console.ReadLine());
                    Console.WriteLine("Choose order: \n 1. Booking \n 2. Reservation");
                    int wish = Convert.ToInt32(Console.ReadLine());
                    
                    UserWish order;
                    //choosing type of order
                    
                     if(wish!=1&&wish!=2)
                        throw new Exception();
                    if (wish == 1)
                    {
                        order = UserWish.Booking;

                        hotel.ToOrder(now, now, room, days, persons, order, Name, Phone, NewOrderHandler, CostHandler, DeclainHandler, ConfirmHandler);
                    }
                     if (wish == 2)
                    {
                        Console.WriteLine("Enter date to reserve");
                        DateTime inDate = Convert.ToDateTime(Console.ReadLine());
                        order = UserWish.Reservation;
                        hotel.ToOrder(inDate, now, room, days, persons, order, Name, Phone, NewOrderHandler, CostHandler, DeclainHandler, ConfirmHandler);
                    }
                   
                    Console.WriteLine("1. Confirm \t 2. Declain");
                    int chose = Convert.ToInt32(Console.ReadLine());
                    // choosing declaining or confirming order
                    if (chose == 1)
                    {
                        hotel.Confirm(Name, Phone);
                    }
                    else if (chose == 2)
                    {
                        hotel.Declain(Name, Phone, type, wish);

                    }
                    else
                    {
                        hotel.Declain(Name, Phone, type, wish);
                        throw new Exception();

                    }
                }//catching for errors
                catch (Exception)
                {
                    color = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Something was entered incorrect, \nplease repeat order creation");
                    Console.ForegroundColor = color;

                }
                
            }

            
        }
        // for new order
        private static void NewOrderHandler(object sender, OrderEvents e)
        {
            Console.WriteLine(e.Message);
            
        }
        // for price of order
        private static void CostHandler(object sender, OrderEvents e)
        {
            Console.WriteLine(e.Message);
        }
        //for declaining order
        private static void DeclainHandler(object sender, OrderEvents e)
        {
            Console.WriteLine(e.Message);

        }
        // for confirming order
        private static void ConfirmHandler(object sender, OrderEvents e)
        {
            Console.WriteLine(e.Message);

        }
     
    }
}
