using HogWildSystem.BLL;
using HogWildSystem.ViewModels;
using Microsoft.AspNetCore.Components;
using HogWildApp.Components;

namespace HogWildApp.Components.Pages.SamplePages
{
    public partial class CustomerList
    {
        #region Fields
        //  The last name
        private string lastName = string.Empty;

        //  The phone number
        private string phoneNumber = string.Empty;

        //  Tells us if the search has been performed
        private bool noRecords;

        //  The feedback message
        private string feedbackMessage = string.Empty;

        //  The error message
        private string errorMessage = string.Empty;

        //  has feedback
        private bool hasFeedback => !string.IsNullOrWhiteSpace(feedbackMessage);

        //  has error
        private bool hasError => !string.IsNullOrWhiteSpace(errorMessage) || errorDetails.Count() > 0;
        //error list
        private List<string> errorDetails = new List<string>();

        #endregion

        #region Properties
        //  inject the CustomerService dependency
        [Inject]
        protected CustomerService CustomerService { get; set; } = default!;
        //  inject the NavigationManager dependency
        [Inject]
        protected NavigationManager NavigationManager { get; set; } = default!;

        //  list of customer search views
        protected List<CustomerSearchView> CustomerSearchViews { get; set; } = new();
        #endregion

        #region Methods
        private void Search()
        {
            //	clear the previous error details and messages
            errorDetails.Clear();
            errorMessage = string.Empty;
            feedbackMessage = string.Empty;

            //  clear customer list
            CustomerSearchViews.Clear();

            //	wrap the service call in a try/catch to handle unexpected exceptions
            try
            {
                var result = CustomerService.GetCustomers(lastName, phoneNumber);
                if (result.IsSuccess)
                {
                    CustomerSearchViews = result.Value;
                }
                else
                {
                    if(result.Errors.Any(e => e.Code == "No Customers"))
                    {
                        noRecords=true;
                    }
                    errorDetails = HogWildHelperClass.GetErrorMessages(result.Errors.ToList());
                }

            }
            catch (Exception ex)
            {
                //	capture any exception message for display
                errorMessage = ex.Message;
            }
        }

        //  new customer
        private void New()
        {

        }

        //  edit selected customer
        private void EditCustomer(int customerID)
        {

        }

        //  new invoice for selected customer
        private void NewInvoice(int customerID)
        {

        }
        #endregion
    }
}
