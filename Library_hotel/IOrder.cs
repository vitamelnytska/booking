using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library_hotel
{
    public interface IOrder //interface for orders
    {
        void MakeOrder(); //method for make order
        void IsFinished(); // method if time of order(reserve) is finished
        void Bill(); // method for determine cost of order
        void Confirm(); // method for show message when order is confirmed
        void Declain(); // method for show message when order is declained
        void Print(); // method to print information of order


    }
}
