using HogWildSystem.ViewModels;

namespace HogWildApp.Components
{
    /// <summary>
    /// Holds shared shopping cart information for the application.
    /// This class is registered as a scoped service so that multiple
    /// Razor pages can access and update the same cart data.
    /// </summary>
    public class CartState
    {
        /// <summary>
        /// Represents the list of items currently in the cart.
        /// Each item is an InvoiceLineView, which includes details
        /// such as PartID, Description, Quantity, and Price.
        /// </summary>
        public List<InvoiceLineView> Cart { get; set; }
    }

}
