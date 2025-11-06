using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HogWildSystem.ViewModels
{
    public class CustomerEditView
    {
        public int CustomerID { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        //  Prov/State ID. Value will use a dropdown and the Lookup View Model
        public int ProvStateID { get; set; }
        //  Country ID. Value will use a dropdown and the Lookup View Model
        public int CountryID { get; set; }
        public string PostalCode { get; set; }
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        //  status ID.  Status value will use a dropdown and the Lookup View Model
        public int StatusID { get; set; }
        public bool RemoveFromViewFlag { get; set; }
    }
}
