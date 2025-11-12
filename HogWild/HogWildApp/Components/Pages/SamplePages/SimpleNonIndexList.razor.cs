using HogWildSystem.ViewModels;

namespace HogWildApp.Components.Pages.SamplePages
{
    public partial class SimpleNonIndexList
    {

        #region Fields

        private string customerName { get; set; } = string.Empty;
        #endregion
        #region Properties
        protected List<CustomerEditView> Customers { get; set; } = new List<CustomerEditView>();
        #endregion

        #region Methods

        private async Task AddCustomerToList()
        {
            if (!string.IsNullOrWhiteSpace(customerName))
            {
                // do not use row count to create an ID
                //int maxID = Customers.Count() + 1;

                int maxID = Customers.Any() ? Customers.Max(x => x.CustomerID + 1) : 1;
                Customers.Add(new CustomerEditView()
                {
                    CustomerID = maxID,
                    FirstName = customerName
                }
                    );
                await InvokeAsync(StateHasChanged);
            }
        }

        private void RemoveCustomer(int customerID)
        {
            var selectedItem = Customers.FirstOrDefault(x => x.CustomerID == customerID);
            if (selectedItem != null)
            {
                Customers.Remove(selectedItem);
            }
        }


        #endregion
    }
}
