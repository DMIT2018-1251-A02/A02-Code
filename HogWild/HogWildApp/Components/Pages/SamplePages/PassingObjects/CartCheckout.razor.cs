using HogWildSystem.ViewModels;
using Microsoft.AspNetCore.Components;

namespace HogWildApp.Components.Pages.SamplePages.PassingObjects
{
    /// <summary>
    /// CartCheckout page:
    /// - Displays a read-only summary of the cart
    /// - Shows SubTotal, Tax, and Total in the table footer
    /// - Allows navigating back to CartEdit to make changes
    /// </summary>
    public partial class CartCheckout
    {
        #region Fields

        /// <summary>
        /// The list of items currently in the shopping cart.
        /// Loaded from CartState.
        /// </summary>
        private List<InvoiceLineView> shoppingCart { get; set; } = new();

        /// <summary>
        /// Calculated subtotal of all items in the cart (before tax).
        /// </summary>
        private decimal subTotal;

        /// <summary>
        /// Calculated tax amount based on the subtotal.
        /// </summary>
        private decimal tax;

        /// <summary>
        /// Calculated grand total (subtotal + tax).
        /// </summary>
        private decimal total;

        /// <summary>
        /// Fixed tax rate used for the calculation (5%).
        /// If this ever changes, update here.
        /// </summary>
        private const decimal TaxRate = 0.05m;

        #endregion

        #region Properties

        /// <summary>
        /// Scoped cart state shared across the cart pages.
        /// </summary>
        [Inject]
        protected CartState CartState { get; set; } = default!;

        /// <summary>
        /// Used for navigation back to CartEdit.
        /// </summary>
        [Inject]
        protected NavigationManager NavigationManager { get; set; } = default!;

        #endregion

        #region Methods

        /// <summary>
        /// Loads the cart from CartState and calculates subtotal, tax, and total.
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            if (CartState.Cart != null)
            {
                shoppingCart = CartState.Cart.ToList();
            }

            CalculateTotals();

            await InvokeAsync(StateHasChanged);
        }

        /// <summary>
        /// Calculates the subtotal, tax, and total based on the current cart.
        /// </summary>
        private void CalculateTotals()
        {
            // Subtotal is the sum of (Price * Quantity) for each invoice line
            subTotal = shoppingCart.Sum(line => line.Price * line.Quantity);

            // Tax is calculated using the fixed TaxRate
            tax = Math.Round(subTotal * TaxRate, 2);

            // Total is subtotal plus tax
            total = subTotal + tax;
        }

        /// <summary>
        /// Navigates back to the CartEdit page to allow changes.
        /// The current cart is already stored in CartState, so no extra work is needed.
        /// </summary>
        private void GotoCartCartEdit()
        {
            NavigationManager.NavigateTo("/SamplePages/PassingObjects/CartEdit");
        }

        #endregion
    }
}
