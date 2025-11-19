using HogWildSystem.BLL;
using HogWildSystem.ViewModels;
using Microsoft.AspNetCore.Components;

namespace HogWildApp.Components.Pages.SamplePages.PassingObjects
{
    /// <summary>
    /// Landing page for the cart workflow:
    /// - Displays available inventory to add to the cart
    /// - Displays the current cart contents (via CartState)
    /// - Allows moving items between inventory and cart
    /// - Navigates to the CartEdit page with the current cart
    /// </summary>
    public partial class CartLanding
    {
        #region Fields

        // Feedback message to display to the user (e.g., success info)
        private string feedbackMessage = string.Empty;

        // Error message for unexpected exceptions or general failures
        private string errorMessage = string.Empty;

        // True if there is a feedback message to display
        private bool hasFeedback => !string.IsNullOrWhiteSpace(feedbackMessage);

        // True if there is an error message or error details to display
        private bool hasError => !string.IsNullOrWhiteSpace(errorMessage) || errorDetails.Count() > 0;

        // Collection of detailed error messages from the BLL result
        private List<string> errorDetails = new List<string>();

        // The list of parts available to be added to the cart
        private List<PartView> inventory { get; set; } = new List<PartView>();

        // The list of items currently in the shopping cart
        private List<InvoiceLineView> shoppingCart { get; set; } = new List<InvoiceLineView>();

        #endregion

        #region Properties

        /// <summary>
        /// Business logic service used to retrieve part information.
        /// </summary>
        [Inject]
        protected PartService PartService { get; set; } = default!;

        /// <summary>
        /// Scoped cart state shared between pages.
        /// This allows us to pass the cart contents to other pages
        /// (CartEdit, CartCheckout, etc.) without using query strings.
        /// </summary>
        [Inject]
        protected CartState CartState { get; set; }

        /// <summary>
        /// Used for navigating to other pages in the application.
        /// </summary>
        [Inject]
        protected NavigationManager NavigationManager { get; set; } = default!;

        #endregion

        #region Methods

        /// <summary>
        /// Loads the inventory and restores any existing cart items from CartState.
        /// Filters the inventory so that items already in the cart are not shown.
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            // Clear previous error and feedback messages
            errorDetails.Clear();
            errorMessage = string.Empty;
            feedbackMessage = string.Empty;

            try
            {
                // List of PartIDs that are already in the cart
                List<int> existingCartItems = new List<int>();

                // If we have a cart in the state, copy it into our local list
                if (CartState.Cart != null)
                {
                    shoppingCart = CartState.Cart.ToList();

                    // OPTIONAL: normalize invalid quantities coming back from other pages
                    // This ensures no cart item has a quantity less than 1.
                    //foreach (var line in shoppingCart)
                    //{
                    //    if (line.Quantity < 1)
                    //    {
                    //        line.Quantity = 0; // or 1, depending on your story
                    //    }
                    //}

                    // Build list of PartIDs already in the cart so we don't show them in the inventory
                    existingCartItems = shoppingCart.Select(x => x.PartID).ToList();
                }

                // Retrieve parts for the landing page.
                // 22 could represent a category or page size, depending on your design.
                // existingCartItems is used to exclude items that are already in the cart.
                var result = PartService.GetParts(22, string.Empty, existingCartItems);

                if (result.IsSuccess)
                {
                    // Populate the inventory with parts not currently in the cart
                    inventory = result.Value;
                }
                else
                {
                    // Capture business-rule level errors to be shown to the user
                    errorDetails = HogWildHelperClass.GetErrorMessages(result.Errors.ToList());
                }
            }
            catch (Exception ex)
            {
                // Capture any unexpected exception messages for display
                errorMessage = ex.Message;
            }

            await InvokeAsync(StateHasChanged);
        }

        /// <summary>
        /// Adds the selected part from the inventory to the shopping cart,
        /// and removes it from the inventory list.
        /// </summary>
        /// <param name="partID">The ID of the part to add to the cart.</param>
        private async Task AddPartToCart(int partID)
        {
            // Find the part in the inventory by PartID
            var part = inventory.FirstOrDefault(x => x.PartID == partID);

            if (part != null)
            {
                // Create a new invoice line for the cart
                var invoiceLine = new InvoiceLineView
                {
                    PartID = partID,
                    Description = part.Description,
                    Price = part.Price,
                    Quantity = 1,        // default quantity when first added
                    Taxable = part.Taxable
                };

                // Add to the cart and remove from inventory
                shoppingCart.Add(invoiceLine);
                inventory.Remove(part);
            }

            await InvokeAsync(StateHasChanged);
        }

        /// <summary>
        /// Removes a part from the shopping cart and returns it to the inventory.
        /// </summary>
        /// <param name="partID">The ID of the part to remove from the cart.</param>
        private async Task RemovePartFromCart(int partID)
        {
            // Clear previous error details and messages
            errorDetails.Clear();
            errorMessage = string.Empty;
            feedbackMessage = string.Empty;

            try
            {
                // Retrieve the full part details again from the BLL
                var result = PartService.GetPart(partID);

                if (result.IsSuccess)
                {
                    var part = result.Value;

                    if (part != null)
                    {
                        // Add the part back into the inventory
                        inventory.Add(part);

                        // Keep the inventory sorted for a nicer user experience
                        inventory = inventory.OrderBy(x => x.Description).ToList();

                        // Find the matching invoice line in the cart
                        var invoiceLine = shoppingCart.FirstOrDefault(x => x.PartID == partID);

                        if (invoiceLine != null)
                        {
                            // Remove the line from the cart
                            shoppingCart.Remove(invoiceLine);
                        }
                    }
                }
                else
                {
                    // Capture business-rule level errors from the BLL
                    errorDetails = HogWildHelperClass.GetErrorMessages(result.Errors.ToList());
                }
            }
            catch (Exception ex)
            {
                // Capture any unexpected exception message for display
                errorMessage = ex.Message;
            }

            await InvokeAsync(StateHasChanged);
        }

        /// <summary>
        /// Saves the current cart into CartState and navigates to the CartEdit page,
        /// where quantities and other details can be modified.
        /// </summary>
        private void GotoCartEdit()
        {
            // Persist the current cart to the shared state object
            CartState.Cart = shoppingCart;

            // Navigate to the CartEdit page
            NavigationManager.NavigateTo("/SamplePages/PassingObjects/CartEdit");
        }

        #endregion
    }
}
