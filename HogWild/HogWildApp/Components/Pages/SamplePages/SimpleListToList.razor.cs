using HogWildSystem.BLL;
using HogWildSystem.ViewModels;
using Microsoft.AspNetCore.Components;
using System.IO;

namespace HogWildApp.Components.Pages.SamplePages
{
    public partial class SimpleListToList
    {
        #region Fields
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
        private List<PartView> inventory { get; set; } = new List<PartView>();
        private List<InvoiceLineView> shoppingCart { get; set; } = new List<InvoiceLineView>();

        #endregion

        #region Properties

        [Inject] protected PartService PartService { get; set; } = default!;

        #endregion

        #region Methods

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            //	clear previous error details and message
            errorDetails.Clear();
            errorMessage = string.Empty;
            feedbackMessage = string.Empty;

            //wrap the service cal in a try/catch to handle unexpected exceptions
            try
            {
                var result = PartService.GetParts(22, string.Empty, new List<int>());
                if (result.IsSuccess)
                {
                    inventory = result.Value;
                }
                else
                {
                    errorDetails = HogWildHelperClass.GetErrorMessages(result.Errors.ToList());
                }
            }
            catch (Exception ex)
            {
                // capture any exceptions message for display
                errorMessage = ex.Message;
            }

            await InvokeAsync(StateHasChanged);
        }

        private async Task AddPartToCart(int partID)
        {
            var part = inventory.FirstOrDefault(x => x.PartID == partID);
            if (part != null)
            {
                var invoiceLine = new InvoiceLineView();
                invoiceLine.PartID = partID;
                invoiceLine.Description = part.Description;
                invoiceLine.Price = part.Price;
                invoiceLine.Quantity = 1;
                invoiceLine.Taxable = part.Taxable;
                shoppingCart.Add(invoiceLine);
                inventory.Remove(part);
            }

            await InvokeAsync(StateHasChanged);
        }

        private async Task RemovePartFromCart(int partID)
        {
            // clear previous error details and messages
            errorDetails.Clear();
            errorMessage = string.Empty;
            feedbackMessage = String.Empty;

            // wrap the service call in a try/catch to handle unexpected exceptions
            try
            {
                var result = PartService.GetPart(partID);
                if (result.IsSuccess)
                {
                    var part = result.Value;
                    if (part != null)
                    {
                        inventory.Add(part);
                        inventory = inventory.OrderBy(x => x.Description).ToList();

                        var invoiceLine = shoppingCart.FirstOrDefault(x => x.PartID == partID);
                        if (invoiceLine != null)
                        {
                            shoppingCart.Remove(invoiceLine);
                        }

                    }
                }
                else
                {
                    errorDetails = HogWildHelperClass.GetErrorMessages(result.Errors.ToList());
                }
            }
            catch (Exception ex)
            {
                // capture any exception message for display
                errorMessage = ex.Message;
            }

            await InvokeAsync(StateHasChanged);
        }

        #endregion
    }
}
