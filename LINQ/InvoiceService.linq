<Query Kind="Program">
  <Connection>
    <ID>cb92c0e3-4ff0-43ea-9726-2fe3c34b87cd</ID>
    <NamingServiceVersion>2</NamingServiceVersion>
    <Persist>true</Persist>
    <Driver Assembly="(internal)" PublicKeyToken="no-strong-name">LINQPad.Drivers.EFCore.DynamicDriver</Driver>
    <AllowDateOnlyTimeOnly>true</AllowDateOnlyTimeOnly>
    <Server>.</Server>
    <Database>OLTP-DMIT2018</Database>
    <DisplayName>OLTP-DMIT2018-Entity</DisplayName>
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

	#region GetParts
	//		// create a place holder for existing parts ids
	//		List<int> existingPartIDs = new();
	//	
	//		//	Fail
	//		//	Rule:  category ID & description must be provided
	//		codeBehind.GetParts(0, string.Empty, existingPartIDs);
	//		codeBehind.ErrorDetails.Dump("Category ID & description must be provided");
	//	
	//		// Rule:  No parts found 
	//		codeBehind.GetParts(0, "zzz", existingPartIDs);
	//		codeBehind.ErrorDetails.Dump("No parts were found that contain description 'zzz'");
	//
	//		// Pass:  valid part category ID (23 -> "Parts")
	//		codeBehind.GetParts(23, string.Empty, existingPartIDs);
	//		codeBehind.Parts.Dump("Pass - Valid part category ID");
	//
	//		// Pass:  valid partial description ("ra")
	//		codeBehind.GetParts(0, "ra", existingPartIDs);
	//		codeBehind.Parts.Dump("Pass - Valid partial description");
	//
	//		// Pass: Updating existing parts ids
	//		// This will simulate that we have parts on our invoice lines
	//		existingPartIDs.Add(27); // Brake Oil, pint
	//		existingPartIDs.Add(33); // Transmission fuild, quart
	//		codeBehind.GetParts(0, "ra", existingPartIDs);
	//		codeBehind.Parts.Dump("Pass - Valid partial description with existing parts ids");
	#endregion

	#region GetPart
	//	//	Fail
	//	//	Rule:  part ID must be greater than zero
	//	codeBehind.GetPart(0);
	//	codeBehind.ErrorDetails.Dump("Part ID must be greater than zero");
	//
	//	// Rule:  part ID must valid 
	//	codeBehind.GetPart(1000000);
	//	codeBehind.ErrorDetails.Dump("No part was found for ID 1000000");
	//
	//	// Pass:  valid part ID
	//	codeBehind.GetPart(52);
	//	codeBehind.Part.Dump("Pass - Valid part ID");
	#endregion

	#region GetInvoice
		//	Fail
		//	Rule:  Customer IDs must be greater than zero
		codeBehind.GetInvoice(0, 0, 1);
		codeBehind.ErrorDetails.Dump("CustomerID must be greater than zero");
	
		//	Rule:  Employee IDs must be greater than zero
		codeBehind.GetInvoice(0, 1, 0);
		codeBehind.ErrorDetails.Dump("EmployeeID must be greater than zero");
	
		// Pass:  New Invoice
		codeBehind.GetInvoice(0, 1, 1);
		codeBehind.Invoice.Dump("Pass - New Invoice");
	
		// Pass:  Existing Invoice
		codeBehind.GetInvoice(1, 1, 1);
		codeBehind.Invoice.Dump("Pass - Existing Invoice");
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

	//  part view returned by the service
	//	using GetParts()	
	public List<PartView> Parts = default!;

	//	using GetPart()	
	public PartView Part = default!;

	//  invoice view returned by the service
	//	using both the GetInvoice() & AddEditInvoice	
	public InvoiceView Invoice = default!;

	public void GetParts(int partCategoryID, string description, List<int> existingPartIDs)
	{
		// clear previous error details and messages
		errorDetails.Clear();
		errorMessage = string.Empty;
		feedbackMessage = String.Empty;

		// wrap the service call in a try/catch to handle unexpected exceptions
		try
		{
			var result = YourService.GetParts(partCategoryID, description, existingPartIDs);
			if (result.IsSuccess)
			{
				Parts = result.Value;
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

	public void GetPart(int partID)
	{
		// clear previous error details and messages
		errorDetails.Clear();
		errorMessage = string.Empty;
		feedbackMessage = String.Empty;

		// wrap the service call in a try/catch to handle unexpected exceptions
		try
		{
			var result = YourService.GetPart(partID);
			if (result.IsSuccess)
			{
				Part = result.Value;
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

	public void GetInvoice(int invoiceID, int customerID, int employeeID)
	{
		// clear previous error details and messages
		errorDetails.Clear();
		errorMessage = string.Empty;
		feedbackMessage = String.Empty;

		// wrap the service call in a try/catch to handle unexpected exceptions
		try
		{
			var result = YourService.GetInvoice(invoiceID, customerID, employeeID);
			if (result.IsSuccess)
			{
				Invoice = result.Value;
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
	//	Get the parts
	public Result<List<PartView>> GetParts(int partCategoryID, string description, List<int> existingPartIDs)
	{
		// Create a Result container that will hold either a
		//	PartView objects on success or any accumulated errors on failure
		var result = new Result<List<PartView>>();
		#region Business Rules
		//	These are processing rules that need to be satisfied
		//		for valid data
		//		rule:	both part id must be valid and/or description cannot be empty
		//		rule: 	part IDs in existingPartIDs will be ignored 
		//		rule: 	RemoveFromViewFlag must be false

		if (partCategoryID == 0 && string.IsNullOrWhiteSpace(description))
		{
			result.AddError(new Error("Missing Information",
				"Please provide either a category and/or description"));
			//  need to exit because we have no part information
			return result;
		}
		#endregion

		//  need to update description parameters so we are not searching on 
		//	 an empty value. Otherwise, this would return all records
		Guid tempGuild = Guid.NewGuid();
		if (string.IsNullOrWhiteSpace(description))
		{
			description = tempGuild.ToString();
		}

		//	ignore any parts that are in the "existing part ID" list
		//  ensure that we are compairing uppercase values for description
		var parts = _hogWildContext.Parts
						.Where(p => !existingPartIDs.Contains(p.PartID)
							&& (description.Length > 0
							&& description != tempGuild.ToString()
							&& partCategoryID > 0
								? (p.Description.ToUpper().Contains(description.ToUpper())
									&& p.PartCategoryID == partCategoryID)
								: (p.Description.ToUpper().Contains(description.ToUpper())
									|| p.PartCategoryID == partCategoryID)
							&& !p.RemoveFromViewFlag))
					.Select(p => new PartView
					{
						PartID = p.PartID,
						PartCategoryID = p.PartCategoryID,
						CategoryName = p.PartCategory.Name,
						Description = p.Description,
						Cost = p.Cost,
						Price = p.Price,
						ROL = p.ROL,
						QOH = p.QOH,
						Taxable = (bool)p.Taxable,
						RemoveFromViewFlag = p.RemoveFromViewFlag
					})
					.OrderBy(p => p.Description)
					.ToList();

		//  if no parts were found
		if (parts == null || parts.Count == 0)
		{
			return result.AddError(new Error("No parts", "No parts were found"));
		}

		//  return the result
		return result.WithValue(parts);
	}

	//	Get the part
	public Result<PartView> GetPart(int partID)
	{
		// Create a Result container that will hold either a
		//	PartView objects on success or any accumulated errors on failure
		var result = new Result<PartView>();
		#region Business Rules
		//	These are processing rules that need to be satisfied
		//		rule:	partID must be valid
		//		rule: 	RemoveFromViewFlag must be false
		if (partID == 0)
		{
			result.AddError(new Error("Missing Information",
							"Please provide a valid part id"));
			//  need to exit because we have no part information
			return result;
		}
		#endregion

		var part = _hogWildContext.Parts
						.Where(p => (p.PartID == partID
									 && !p.RemoveFromViewFlag))
						.Select(p => new PartView
						{
							PartID = p.PartID,
							PartCategoryID = p.PartCategoryID,
							//  PartCategory is an alias for Lookup
							CategoryName = p.PartCategory.Name,
							Description = p.Description,
							Cost = p.Cost,
							Price = p.Price,
							ROL = p.ROL,
							QOH = p.QOH,
							Taxable = (bool)p.Taxable,
							RemoveFromViewFlag = p.RemoveFromViewFlag
						}).FirstOrDefault();

		//  if no part were found
		if (part == null)
		{
			result.AddError(new Error("No part", "No part were found"));
			//  need to exit because we did not find any part
			return result;
		}

		//  return the result
		return result.WithValue(part);
	}

	//	Get the customer invoice
	public Result<InvoiceView> GetInvoice(int invoiceID, int customerID, int employeeID)
	{
		// Create a Result container that will hold either a
		//	Invoice objects on success or any accumulated errors on failure
		var result = new Result<InvoiceView>();

		#region Business Rules
		//    These are processing rules that need to be satisfied
		//        for valid data
		//        rule:    both the customerID and empplyeeID must be provided 
		if (customerID == 0)
		{
			result.AddError(new Error("Missing Information",
				"Please provide a customer ID"));
		}
		if (employeeID == 0)
		{
			result.AddError(new Error("Missing Information",
				"Please provide a employee ID"));
		}
		if (result.IsFailure)
		{
			//  need to exit because we have no invoice record
			return result;
		}
		#endregion

		//	Handles both new and existing invoice
		//  For a new invoice the following information is needed
		//		Customer & Employee ID
		//  For a existing invoice the following information is needed
		//		Invoice & Employee ID (We maybe updating an invoice at a later date
		//			and we need the current employee who is handling the transaction.

		InvoiceView invoice;
		//  new invoice for customer
		if (invoiceID == 0)
		{
			invoice = new InvoiceView
			{
				CustomerID = customerID,
				EmployeeID = employeeID,
				InvoiceDate = DateOnly.FromDateTime(DateTime.Now)
			};
		}
		else
		{
			invoice = _hogWildContext.Invoices
				.Where(x => x.InvoiceID == invoiceID
				 && !x.RemoveFromViewFlag
				)
				.Select(x => new InvoiceView
				{
					InvoiceID = invoiceID,
					InvoiceDate = x.InvoiceDate,
					CustomerID = x.CustomerID,
					EmployeeID = x.EmployeeID,
					SubTotal = x.SubTotal,
					Tax = x.Tax,
					InvoiceLines = _hogWildContext.InvoiceLines
						.Where(invoiceLine => invoiceLine.InvoiceID == invoiceID)
						.Select(invoiceLine => new InvoiceLineView
						{
							InvoiceLineID = invoiceLine.InvoiceLineID,
							InvoiceID = invoiceLine.InvoiceID,
							PartID = invoiceLine.PartID,
							Quantity = invoiceLine.Quantity,
							Description = invoiceLine.Part.Description,
							Price = invoiceLine.Price,
							Taxable = (bool)invoiceLine.Part.Taxable,
							RemoveFromViewFlag = invoiceLine.RemoveFromViewFlag
						}).ToList()
				}).FirstOrDefault() ?? new InvoiceView();
			customerID = invoice.CustomerID;
		}
		invoice.CustomerName = GetCustomerFullName(customerID);
		invoice.EmployeeName = GetEmployeeFullName(employeeID);

		//  only happens if the invoice was mark as remove
		if (invoice == null)
		{
			result.AddError(new Error("No Invoice", "No invoice were found"));
			//  need to exit because we did not find any invoice
			return result;
		}
		//  return the result
		return result.WithValue(invoice);
	}

	//	Get the customer full name
	public string GetCustomerFullName(int customerID)
	{
		return _hogWildContext.Customers
			.Where(x => x.CustomerID == customerID && !x.RemoveFromViewFlag)
			.Select(x => $"{x.FirstName} {x.LastName}").FirstOrDefault() ?? string.Empty;
	}
	//	Get the employee full name
	public string GetEmployeeFullName(int employeeId)
	{
		return _hogWildContext.Employees
			.Where(x => x.EmployeeID == employeeId && !x.RemoveFromViewFlag)
			.Select(x => $"{x.FirstName} {x.LastName}").FirstOrDefault() ?? string.Empty;
	}
}
#endregion

// ———— PART 4: View Models → Service Library View Model ————
//	This region includes the view models used to 
//	represent and structure data for the UI.
#region View Models
public class PartView
{
	public int PartID { get; set; }
	public int PartCategoryID { get; set; }
	public string CategoryName { get; set; }
	public string Description { get; set; }
	public decimal Cost { get; set; }
	public decimal Price { get; set; }
	public int ROL { get; set; }
	public int QOH { get; set; }
	public bool Taxable { get; set; }
	public bool RemoveFromViewFlag { get; set; }
}

public class InvoiceView
{
	public int InvoiceID { get; set; }
	public DateOnly InvoiceDate { get; set; }
	public int CustomerID { get; set; }
	public string CustomerName { get; set; }
	public int EmployeeID { get; set; }
	public string EmployeeName { get; set; }
	public decimal SubTotal { get; set; }
	public decimal Tax { get; set; }
	public decimal Total => SubTotal + Tax;
	public List<InvoiceLineView> InvoiceLines { get; set; } = new List<InvoiceLineView>();
	public bool RemoveFromViewFlag { get; set; }
}

public class InvoiceLineView
{
	public int InvoiceLineID { get; set; }
	public int InvoiceID { get; set; }
	public int PartID { get; set; }
	public string Description { get; set; }
	public int Quantity { get; set; }
	public decimal Price { get; set; }
	public bool Taxable { get; set; }
	public decimal ExtentPrice => Price * Quantity;
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