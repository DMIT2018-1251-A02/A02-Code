using BYSResults;
using HogWildSystem.DAL;
using HogWildSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HogWildSystem.BLL
{
    public class CustomerService
    {
        #region Fields
        //  hog wild context
        private readonly HogWildContext _hogWildContext;
        #endregion

        //  constructor for the WorkingVersionsService class.
        internal CustomerService(HogWildContext hogWildContext)
        {
            //  Initialize the _hogWildContext field with the provided HogWildContext instance.
            _hogWildContext = hogWildContext;
        }

        public Result<List<CustomerSearchView>> GetCustomers(string lastName, string phone)
        {
            // Create a Result container that will hold either a
            //	CustomerSearchView objects on success or any accumulated errors on failure

            var result = new Result<List<CustomerSearchView>>();
            #region Business Rules
            //    These are processing rules that need to be satisfied
            //        for valid data

            // 	rule:	Both last name and phone number cannot be empty
            // 	rule:	RemoveFromViewFlag must be false (soft delete)
            if (string.IsNullOrEmpty(lastName) && string.IsNullOrWhiteSpace(phone))
            {
                //  need to exit because we have nothing to search on
                return result.AddError(new Error("Missing Information",
                    "Please provide either a last name and/or phone number"));
            }
            #endregion

            //  update for either empty lastname or phone wiht GUID
            if (string.IsNullOrWhiteSpace(lastName))
            {
                lastName = Guid.NewGuid().ToString();
            }

            if (string.IsNullOrWhiteSpace(phone))
            {
                phone = Guid.NewGuid().ToString();
            }

            //	filter rules
            // 	1) only apply lastName filter if supplied
            // 	2) only apply phone filter if supplied
            // 	3) always exclude removed records

            var customers = _hogWildContext.Customers
                                .Where(c => (string.IsNullOrWhiteSpace(lastName)
                                    || c.LastName.ToUpper().Contains(lastName.ToUpper())) // 1
                                || (string.IsNullOrWhiteSpace(phone)
                                || c.Phone.Contains(phone)) // 2
                                && !c.RemoveFromViewFlag // 3
                                )
                                .Select(c => new CustomerSearchView
                                {
                                    CustomerID = c.CustomerID,
                                    FirstName = c.FirstName,
                                    LastName = c.LastName,
                                    City = c.City,
                                    Phone = c.Phone,
                                    Email = c.Email,
                                    StatusID = c.StatusID,
                                    TotalSales = (decimal?)c.Invoices.Sum(i => i.SubTotal + i.Tax) ?? 0
                                })
                            .OrderBy(c => c.LastName)
                                .ToList();

            //	if no customer were found with either the last name or phone number
            if (customers == null || customers.Count() == 0)
            {
                //	need to exit because we did not find any customers
                return result.AddError(new Error("No Customers", "No customers were found"));
            }
            return result.WithValue(customers);
        }

        public Result<CustomerEditView> GetCustomer(int customerID)
        {
            //	Create a Result container that will hold either a 
            //		CustomerEditView object on success or any accumulated errors on failure
            var result = new Result<CustomerEditView>();

            #region Business Rules
            //	These are processing rules that need to be satisfied for valid data
            //		rule:	customerID must be valid (greater than zero)
            //		rule:	RemoveFromViewFlag muse be false (soft delete)

            if (customerID == 0)
            {
                return result.AddError(new Error("Missing Information",
                    "Please provide a valid customer ID"));
            }
            #endregion

            var customer = _hogWildContext.Customers
                .Where(c => c.CustomerID == customerID && !c.RemoveFromViewFlag)
                .Select(c => new CustomerEditView
                {
                    CustomerID = c.CustomerID,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    Address1 = c.Address1,
                    Address2 = c.Address2,
                    City = c.City,
                    ProvStateID = c.ProvStateID,
                    CountryID = c.CountryID,
                    PostalCode = c.PostalCode,
                    Phone = c.Phone,
                    Email = c.Email,
                    StatusID = c.StatusID,
                    RemoveFromViewFlag = c.RemoveFromViewFlag
                }).FirstOrDefault();
            //	if no customer was found with the customer ID
            if (customer == null)
            {
                return result.AddError(new Error("No Customer",
                    $"No customer for ID {customerID} was found"));
            }

            //	return the result
            return result.WithValue(customer);
        }
    }
}
