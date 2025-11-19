using HogWildSystem.BLL;
using HogWildSystem.ViewModels;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace HogWildApp.Components.Pages.SamplePages.PassingObjects
{
    /// <summary>
    /// CartEdit page:
    /// - Allows editing of quantities in the current shopping cart
    /// - Enforces Qty >= 1 using MudBlazor validation
    /// - Prevents navigation to Checkout when the cart is invalid
    /// </summary>
    public partial class CartEdit
    {
        #region Fields

        /// <summary>
        /// Reference to the MudForm that wraps the cart grid.
        /// This is set via @ref in the .razor file (do NOT new it up here).
        /// </summary>
        private MudForm? cartEditForm;

        /// <summary>
        /// True if all cart lines have a quantity greater than zero.
        /// Used to enable/disable the "Goto Cart Check Out" button.
        /// </summary>
        private bool isCartValid => shoppingCart.All(c => c.Quantity > 0);

        /// <summary>
        /// The list of items currently being edited in the cart.
        /// This is loaded from CartState on initialization.
        /// </summary>
        private List<InvoiceLineView> shoppingCart { get; set; } = new List<InvoiceLineView>();

        #endregion

        #region Properties

        /// <summary>
        /// Scoped cart state shared between pages.
        /// </summary>
        [Inject]
        protected CartState CartState { get; set; } = default!;

        /// <summary>
        /// Used for navigating between pages in the application.
        /// </summary>
        [Inject]
        protected NavigationManager NavigationManager { get; set; } = default!;

        #endregion

        #region Methods

        /// <summary>
        /// Loads the current cart from CartState into the local shoppingCart list.
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            if (CartState.Cart != null)
            {
                shoppingCart = CartState.Cart.ToList();
            }

            await InvokeAsync(StateHasChanged);
        }

        /// <summary>
        /// After the component has rendered for the first time, force validation
        /// to run so that any invalid quantities (e.g., 0) are immediately marked
        /// as invalid and display their error messages.
        /// </summary>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            // Important: check that the form reference is available,
            // not the CartState (CartState is injected earlier).
            if (firstRender && cartEditForm is not null)
            {
                // Force all validators (including ValidateQuantity) to run on initial load
                await cartEditForm.Validate();
            }
        }

        /// <summary>
        /// Validation method for the Quantity field.
        /// Returns an error message if the quantity is less than 1;
        /// otherwise returns null (which means the value is valid).
        /// </summary>
        /// <param name="value">The quantity value entered by the user.</param>
        /// <returns>Error message if invalid; otherwise null.</returns>
        private string? ValidateQuantity(int value)
        {
            if (value < 1)
            {
                return "Quantity cannot be less than 1";
            }

            return null; // valid
        }

        /// <summary>
        /// Saves the current cart back into CartState and navigates
        /// to the CartLanding page.
        /// </summary>
        private void GotoCartLanding()
        {
            CartState.Cart = shoppingCart;
            NavigationManager.NavigateTo("/SamplePages/PassingObjects/CartLanding");
        }

        /// <summary>
        /// Saves the current cart back into CartState and navigates
        /// to the CartCheckout page. The button that calls this method
        /// should be disabled if isCartValid is false.
        /// </summary>
        private void GotoCartCheckOut()
        {
            CartState.Cart = shoppingCart;
            NavigationManager.NavigateTo("/SamplePages/PassingObjects/CartCheckout");
        }

        #endregion
    }
}
