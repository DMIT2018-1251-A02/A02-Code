using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HogWildSystem.ViewModels
{
    public class CustomerSearchView
    {
        //  Customer ID
        public int CustomerID { get; set; }
        //  First name
        public string FirstName { get; set; }
        //  last name
        public string LastName { get; set; }
        //  city
        public string City { get; set; }
        //  contact phone number
        public string Phone { get; set; }
        //  email address
        public string Email { get; set; }
        //  status ID.  Status value will use a dropdown and the Lookup View Model
        public int StatusID { get; set; }
        //  Invoice.SubTotal +  Invoice.Tax	
        public decimal TotalSales { get; set; }
    }
}
