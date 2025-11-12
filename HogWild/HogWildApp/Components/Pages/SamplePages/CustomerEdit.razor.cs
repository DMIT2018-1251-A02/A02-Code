using BYSResults;
using HogWildSystem.BLL;
using HogWildSystem.ViewModels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc.Rendering;
using MudBlazor;
using System.Collections.Generic;
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
        //  provinces
        private List<LookupView> provinces = new();
        //  countries
        private List<LookupView> countries = new();
        //  status
        private List<LookupView> statusLookup = new();
        // mudform control
        private MudForm customerForm = new();
        #endregion

        #region Validation
        //  flag if the form is valid.
        private bool isFormValid = false;
        //  flag if date has change
        private bool hasDataChanged = false;
        //  set text for cancel/close button
        private string closeButtonText => hasDataChanged ? "Cancel" : "Close";
        #endregion

        #region Propertiers
        //  customer service
        [Inject]
        protected CustomerService CustomerService { get; set; } = null!;

        //  category lookup service
        [Inject]
        protected CategoryLookupService CategoryLookupService { get; set; } = null!;

        //  inject the DialogService dependency
        [Inject] protected IDialogService DialogService { get; set; } = null!;
        //  inject the NavigationManager dependency
        [Inject]
        protected NavigationManager NavigationManager { get; set; } = null!;

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

            #region Lookups
            var results = CategoryLookupService.GetLookups("Province");
            if (results.IsSuccess)
            {
                provinces = results.Value;
            }
            else
            {
                errorDetails = HogWildHelperClass.GetErrorMessages(results.Errors.ToList());
            }

            results = CategoryLookupService.GetLookups("Country");
            if (results.IsSuccess)
            {
                countries = results.Value;
            }
            else
            {
                errorDetails.AddRange(HogWildHelperClass.GetErrorMessages(results.Errors.ToList()));
            }

            results = CategoryLookupService.GetLookups("Customer Status");
            if (results.IsSuccess)
            {
                statusLookup = results.Value;
            }
            else
            {
                errorDetails.AddRange(HogWildHelperClass.GetErrorMessages(results.Errors.ToList()));
            }
            #endregion

            //  update that data has changed
            StateHasChanged();
        }

        //  save the customer
        private void Save()
        {
            //	clear previous error details and messages
            errorDetails.Clear();
            errorMessage = string.Empty;
            feedbackMessage = string.Empty;

            //	wrap the service call in a try/catch to handle unexpected exception
            try
            {
                var result = CustomerService.AddEditCustomer(customer);
                if (result.IsSuccess)
                {
                    customer = result.Value;
                    feedbackMessage = "Data was successfully saved!";

                    //  reset change tracking
                    hasDataChanged = false;
                    isFormValid = false;
                    customerForm.ResetTouched();  //  reset the touched
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

        //  Cancel or close this instance
        private async Task Cancel()
        {
            bool? result = await DialogService.ShowMessageBox("Confirm Cancel",
                "Do you wish to close the customer editor?  All unsaved changes will be lost.",
                yesText: "Yes", cancelText: "No");

            //  true mean affirmative action (e.g. "Yes")
            //  null means the user dismissed the dialog (e.g. clicking "No" or closing the dialog)
            if (result == null)
            {
                return;
            }
            NavigationManager.NavigateTo("/SamplePages/CustomerList");
        }
        #endregion
    }
}
