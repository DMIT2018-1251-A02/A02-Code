using HogWildSystem.BLL;
using HogWildSystem.ViewModels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc.Rendering;
using MudBlazor;
using System.Globalization;
using static MudBlazor.Icons;

namespace HogWildApp.Components.Pages.SamplePages
{
    public partial class CustomerEdit
    {
        #region Fields
        private string feedbackMessage = string.Empty;
        private string errorMessage = string.Empty;
        private bool hasFeedback => !string.IsNullOrWhiteSpace(feedbackMessage);
        private bool hasError => !string.IsNullOrWhiteSpace(errorMessage) || errorDetails.Count() > 0;
        //error list
        private List<string> errorDetails = new List<string>();

        // customer
        private CustomerEditView customer = new();
        // mudform control
        private MudForm customerForm = new();
        #endregion

        #region Propertiers
        //  customer service
        [Inject]
        protected CustomerService CustomerService { get; set; } = default!;

        //  Customer ID used to create or edit a customer
        [Parameter]
        public int CustomerID { get; set; } = 0;

        #endregion

        #region Methods

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            //	clear previous error details and messages
            errorDetails.Clear();
            errorMessage = string.Empty;
            feedbackMessage = string.Empty;

            //  check to see if we are navigation using a valid customer CustomerID
            //      or are we going to create a new customer
            if (CustomerID > 0)
            {
                //	wrap the service call in a try/catch to handle unexpected exceptions
                try
                {
                    var result = CustomerService.GetCustomer(CustomerID);
                    if (result.IsSuccess)
                    {
                        customer = result.Value;
                    }
                    else
                    {
                        errorDetails = HogWildHelperClass.GetErrorMessages(result.Errors.ToList());
                    }
                }
                catch (Exception ex)
                {
                    //	capture any exception message for display
                    errorMessage = ex.Message;
                }
            }
            else
            {
                customer = new();
            }

            //  update that data has change
            StateHasChanged();
        }
        #endregion

    }
}
