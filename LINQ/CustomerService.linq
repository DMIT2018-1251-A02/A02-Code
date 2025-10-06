<Query Kind="Program">
  <Connection>
    <ID>37a64ce9-5c5f-4d4d-afc7-7324799c8fda</ID>
    <NamingServiceVersion>2</NamingServiceVersion>
    <Persist>true</Persist>
    <Driver Assembly="(internal)" PublicKeyToken="no-strong-name">LINQPad.Drivers.EFCore.DynamicDriver</Driver>
    <AllowDateOnlyTimeOnly>true</AllowDateOnlyTimeOnly>
    <Server>.</Server>
    <Database>OLTP-DMIT2018</Database>
    <DisplayName>OLTP-DMIT2018-ENtity</DisplayName>
    <DriverData>
      <EncryptSqlTraffic>True</EncryptSqlTraffic>
      <PreserveNumeric1>True</PreserveNumeric1>
      <EFProvider>Microsoft.EntityFrameworkCore.SqlServer</EFProvider>
    </DriverData>
  </Connection>
  <NuGetReference>BYSResults</NuGetReference>
</Query>

// 	Lightweight result types for explicit success/failure 
//	 handling in .NET applications.
using BYSResults;

// —————— PART 1: Main → UI ——————
//	Driver is responsible for orchestrating the flow by calling 
//	various methods and classes that contain the actual business logic 
//	or data processing operations.
void Main()
{
	CodeBehind codeBehind = new CodeBehind(this); // “this” is LINQPad’s auto Context

	#region GetCustomer
	//	Fail
	//	Rule:  customer ID must be greater than zero
	codeBehind.GetCustomer(0);
	codeBehind.ErrorDetails.Dump("Customer ID must be greater than zero");

	// Rule:  customer ID must valid 
	codeBehind.GetCustomer(1000000);
	codeBehind.ErrorDetails.Dump("Customer was not found for ID 1000000");

	// Pass:  valid customer ID
	codeBehind.GetCustomer(1);
	codeBehind.Customer.Dump("Pass - Valid customer ID");
	#endregion

}

// ———— PART 2: Code Behind → Code Behind Method ————
// This region contains methods used to test the functionality
// of the application's business logic and ensure correctness.
// NOTE: This class functions as the code-behind for your Blazor pages
#region Code Behind Methods
public class CodeBehind(TypedDataContext context)
{
	#region Supporting Members (Do not modify)
	// exposes the collected error details
	public List<string> ErrorDetails => errorDetails;

	// Mock injection of the service into our code-behind.
	// You will need to refactor this for proper dependency injection.
	// NOTE: The TypedDataContext must be passed in.
	private readonly Library YourService = new Library(context);
	#endregion

	#region Fields from Blazor Page Code-Behind
	// feedback message to display to the user.
	private string feedbackMessage = string.Empty;
	// collected error details.
	private List<string> errorDetails = new();
	// general error message.
	private string errorMessage = string.Empty;
	#endregion
	//  customer view returned by the service
	//	using both the GetCustomer() & AddEditCustomer	
	public CustomerEditView Customer = default!;

	public void GetCustomer(int customerID)
	{
		// clear previous error details and messages
		errorDetails.Clear();
		errorMessage = string.Empty;
		feedbackMessage = String.Empty;

		// wrap the service call in a try/catch to handle unexpected exceptions
		try
		{
			var result = YourService.GetCustomer(customerID);
			if (result.IsSuccess)
			{
				Customer = result.Value;
			}
			else
			{
				errorDetails = GetErrorMessages(result.Errors.ToList());
			}
		}
		catch (Exception ex)
		{
			// capture any exception message for display
			errorMessage = ex.Message;
		}
	}

	public void AddEditCustomer(CustomerEditView editCustomer)
	{
		// clear previous error details and messages
		errorDetails.Clear();
		errorMessage = string.Empty;
		feedbackMessage = String.Empty;

		// wrap the service call in a try/catch to handle unexpected exceptions
		try
		{
			var result = YourService.AddEditCustomer(editCustomer);
			if (result.IsSuccess)
			{
				Customer = result.Value;
			}
			else
			{
				errorDetails = GetErrorMessages(result.Errors.ToList());
			}
		}
		catch (Exception ex)
		{
			// capture any exception message for display
			errorMessage = ex.Message;
		}
	}

}
#endregion

// ———— PART 3: Database Interaction Method → Service Library Method ————
//	This region contains support methods for testing
#region Methods
public class Library
{
	#region Data Context Setup
	// The LINQPad auto-generated TypedDataContext instance used to query and manipulate data.
	private readonly TypedDataContext _hogWildContext;

	// The TypedDataContext provided by LINQPad for database access.
	// Store the injected context for use in library methods
	// NOTE:  This constructor is simular to the constuctor in your service
	public Library(TypedDataContext context)
	{
		_hogWildContext = context
					?? throw new ArgumentNullException(nameof(context));
	}
	#endregion

	public Result<CustomerEditView> GetCustomer(int customerID)
	{
		// Create a Result container that will hold either a
		//	CustomerEditView objects on success or any accumulated errors on failure
		var result = new Result<CustomerEditView>();

		#region Business Rules
		//	These are processing rules that need to be satisfied for valid data		
		//		rule:	customerID must be valid (cannot be equal to zero) 
		// 		rule:	RemoveFromViewFlag must be false (soft delete)

		if (customerID == 0)
		{
			result.AddError(new Error("Missing Information",
				"Please provide a valid customer ID"));
			//  need to exit because we have no customer record
			return result;
		}
		#endregion

		var customer = _hogWildContext.Customers
					.Where(c => c.CustomerID == customerID
								 && !c.RemoveFromViewFlag)
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

		//  if no customer were found with the customer ID
		if (customer == null)
		{
			result.AddError(new Error("No Customer", "No customer were found"));
			//  need to exit because we did not find any customer
			return result;
		}

		//  return the result
		return result.WithValue(customer);

	}

	public Result<CustomerEditView> AddEditCustomer(CustomerEditView editCustomer)
	{
		// Create a Result container that will hold either a
		//	CustomerEditView objects on success or any accumulated errors on failure
		var result = new Result<CustomerEditView>();

		#region Business Rules
		//	These are processing rules that need to be satisfied for valid data	
		//    rule:    customer cannot be null
		if (editCustomer == null)
		{
			result.AddError(new Error("Missing Customer",
				"No customer was supply"));
			//  need to exit because we have no customer view model to add/edit
			return result;
		}
		//	rule: first name, last name, phone number 
		//			and email are required (not empty)
		if (string.IsNullOrEmpty(editCustomer.FirstName))
		{
			result.AddError(new Error("Missing Information", "First name is required"));
		}

		if (string.IsNullOrEmpty(editCustomer.LastName))
		{
			result.AddError(new Error("Missing Information", "Last name is required"));
		}

		if (string.IsNullOrEmpty(editCustomer.Phone))
		{
			result.AddError(new Error("Missing Information", "Phone number is required"));
		}

		if (string.IsNullOrEmpty(editCustomer.Email))
		{
			result.AddError(new Error("Missing Information", "Email is required"));
		}
		//		rule: 	first name, last name and phone number cannot be duplicated (found more than once)
		if (editCustomer.CustomerID == 0)
		{
			bool customerExist = _hogWildContext.Customers.Any(x =>
										  x.FirstName.ToUpper() == editCustomer.FirstName.ToUpper() &&
										  x.LastName.ToUpper() == editCustomer.LastName.ToUpper() &&
										  x.Phone.ToUpper() == editCustomer.Phone.ToUpper()
										);

			if (customerExist)
			{
				result.AddError(new Error("Existing Customer Data", "Customer already exist in the " +
																		  "database and cannot be enter again"));
			}
		}

		//  exit if we have any outstanding errors
		if (result.IsFailure)
		{
			return result;
		}
		#endregion

		Customer customer = _hogWildContext.Customers
						.Where(x => x.CustomerID == editCustomer.CustomerID)
						.Select(x => x).FirstOrDefault();

		//  if the customer was not found (CustomerID == 0)
		//		the we are dealing with a new customer
		if (customer == null)
		{
			customer = new Customer();
		}

		//	NOTE:	You do not have to update the primary key "CustomerID".
		//				This is true for all primary keys for any view models.
		//			- If is is a new customer, the CustomerID will be "0"
		//			- If is is na existing customer, there is no need to update it.

		customer.FirstName = editCustomer.FirstName;
		customer.LastName = editCustomer.LastName;
		customer.Address1 = editCustomer.Address1;
		customer.Address2 = editCustomer.Address2;
		customer.City = editCustomer.City;
		customer.ProvStateID = editCustomer.ProvStateID;
		customer.CountryID = editCustomer.CountryID;
		customer.PostalCode = editCustomer.PostalCode;
		customer.Email = editCustomer.Email;
		customer.Phone = editCustomer.Phone;
		customer.StatusID = editCustomer.StatusID;
		customer.RemoveFromViewFlag = editCustomer.RemoveFromViewFlag;

		//  new customer
		if (customer.CustomerID == 0)
			_hogWildContext.Customers.Add(customer);
		else
			//	existing customer
			_hogWildContext.Customers.Update(customer);

		try
		{
			// NOTE:  YOU CAN ONLY HAVE ONE SAVE CHANGES IN A METHOD  
			_hogWildContext.SaveChanges();
		}
		catch (Exception ex)
		{
			// Clear changes to maintain data integrity.
			_hogWildContext.ChangeTracker.Clear();
			// we do not have to throw an exception, just need to log the error message
			result.AddError(new Error(
				"Error Saving Changes", ex.InnerException.Message));
			//  need to return the result
			return result;
		}
		//  need to refresh the customer information
		return GetCustomer(customer.CustomerID);
	}
}
#endregion

// ———— PART 4: View Models → Service Library View Model ————
	//	This region includes the view models used to 
	//	represent and structure data for the UI.
	#region View Models
	public class CustomerEditView
	{
		public int CustomerID { get; set; }
		public string FirstName { get; set; } = string.Empty;
		public string LastName { get; set; } = string.Empty;
		public string Address1 { get; set; }
		public string Address2 { get; set; }
		public string City { get; set; }
		//  Prov/State ID. Value will use a dropdown and the Lookup View Model
		public int ProvStateID { get; set; }
		//  Country ID. Value will use a dropdown and the Lookup View Model
		public int CountryID { get; set; }
		public string PostalCode { get; set; }
		public string Phone { get; set; } = string.Empty;
		public string Email { get; set; } = string.Empty;
		//  status ID.  Status value will use a dropdown and the Lookup View Model
		public int StatusID { get; set; }
		public bool RemoveFromViewFlag { get; set; }
	}
	#endregion

	//	This region includes support methods
	#region Support Method
	// Converts a list of error objects into their string representations.
	public static List<string> GetErrorMessages(List<Error> errorMessage)
	{
		// Initialize a new list to hold the extracted error messages
		List<string> errorList = new();

		// Iterate over each Error object in the incoming list
		foreach (var error in errorMessage)
		{
			// Convert the current Error to its string form and add it to errorList
			errorList.Add(error.ToString());
		}

		// Return the populated list of error message strings
		return errorList;
	}
#endregion