using BYSResults;
using HogWildSystem.DAL;
using HogWildSystem.Entities;
using HogWildSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HogWildSystem.BLL
{
    public class InvoiceService
    {
        #region Fields
        //  hog wild context
        private readonly HogWildContext _hogWildContext;
        #endregion

        //  constructor for the InvoiceService class.
        internal InvoiceService(HogWildContext hogWildContext)
        {
            //  Initialize the _hogWildContext field with the provided HogWildContext instance.
            _hogWildContext = hogWildContext;
        }

        public Result<InvoiceView> GetInvoice(int invoiceID, int customerID, int employeeID)
        {
            //	Create a Result container that will hold either a
            //	  InvoiceView object on success or any accumulated errors on failure
            var result = new Result<InvoiceView>();

            #region Business Rules
            //	These are processing rules that need to be satisfied
            //		for valid data
            //		rule:  cusomerID must be provided if invoiceID == 0
            //		Rule:  employeeID must be provided
            if (customerID == 0 && invoiceID == 0)
            {
                result.AddError(new Error("Missing Information", "Please provide a customer ID"));
            }

            if (employeeID == 0)
            {
                result.AddError(new Error("Missing Information", "Please provide a employee ID"));
            }

            // need to exit because we are missing key data
            if (result.IsFailure)
            {
                return result;
            }
            #endregion
            //	Handles both new and existing invoice
            //	For a new invoice, the following information is needed
            //		Customer & Employee IDs
            //	For a existing invoice, the following information is needed
            //	Invoice & employeeID (We maybe updating an invoice at a later date
            //		and we need the current employee who is handling the transaction

            InvoiceView invoice = null;
            //	new invoice for a customer
            if (invoiceID == 0)
            {
                invoice = new InvoiceView()
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
                                        && !x.RemoveFromViewFlag)
                            .Select(x => new InvoiceView
                            {
                                InvoiceID = x.InvoiceID,
                                InvoiceDate = DateOnly.FromDateTime(x.InvoiceDate),
                                CustomerID = x.CustomerID,
                                EmployeeID = x.EmployeeID,
                                SubTotal = x.SubTotal,
                                Tax = x.Tax,
                                RemoveFromViewFlag = x.RemoveFromViewFlag, //  this will always be false
                                InvoiceLines = x.InvoiceLines
                                                .Select(il => new InvoiceLineView
                                                {
                                                    InvoiceLineID = il.InvoiceLineID,
                                                    InvoiceID = il.InvoiceID,
                                                    PartID = il.PartID,
                                                    Quantity = il.Quantity,
                                                    Description = il.Part.Description,
                                                    Price = il.Part.Price,
                                                    Taxable = (bool)il.Part.Taxable,
                                                    RemoveFromViewFlag = il.RemoveFromViewFlag
                                                }).ToList()
                            }).FirstOrDefault();
                customerID = invoice.CustomerID;
            }
            invoice.CustomerName = GetCustomerFullName(customerID);
            invoice.EmployeeName = GetEmployeeFullName(employeeID);

            //	only happen if the invoice was mark as remove
            if (invoice == null)
            {
                //	need to exit because we did not find any invoice
                return result.AddError(new Error("No Invoice", "No invoice was found"));
            }
            return result.WithValue(invoice);

        }

        //	Get the customer invoices
        public Result<List<InvoiceView>> GetCustomerInvoices(int customerId)
        {
            // Create a Result container that will hold either a
            //	PartView objects on success or any accumulated errors on failure
            var result = new Result<List<InvoiceView>>();
            #region Business Rules
            //	These are processing rules that need to be satisfied
            //		rule:	customerID must be valid
            //		rule: 	RemoveFromViewFlag must be false
            if (customerId == 0)
            {
                result.AddError(new Error("Missing Information",
                                "Please provide a valid customer id"));
                //  need to exit because we have no customer information
                return result;
            }
            #endregion

            var customerInvoices = _hogWildContext.Invoices
                    .Where(x => x.CustomerID == customerId
                                && !x.RemoveFromViewFlag)
                    .Select(x => new InvoiceView
                    {
                        InvoiceID = x.InvoiceID,
                        InvoiceDate = DateOnly.FromDateTime(x.InvoiceDate),
                        CustomerID = x.CustomerID,
                        SubTotal = x.SubTotal,
                        Tax = x.Tax
                    }).ToList();

            //  if no invoices were found
            if (customerInvoices == null || customerInvoices.Count() == 0)
            {
                result.AddError(new Error("No customer invoices", "No invoices were found"));
                //  need to exit because we did not find any invoices
                return result;
            }
            //  return the result
            return result.WithValue(customerInvoices);
        }

        //	add edit invoice
        public Result<InvoiceView> AddEditInvoice(InvoiceView invoiceView)
        {
            //	Create a result container that will hold either a 
            //		invoice view object on sucess or any accumulated errors on failure
            var result = new Result<InvoiceView>();

            #region Business Rule
            //	These are processing rules that need to be satisfied
            //		for valid data
            //	Rule:	invoice view cannot be null
            if (invoiceView == null)
            {
                //	need to exit because we have no invoice object
                return result.AddError(new Error("Missing Invoice", "No invoice was supply"));
            }

            //	rule:	CustomerID must be supply
            if (invoiceView.CustomerID == 0 && invoiceView.InvoiceID == 0)
            {
                result.AddError(new Error("Missing Information", "Please provide a valid customer ID"));
            }

            //	rule:	EmployeeID must be supply
            if (invoiceView.EmployeeID == 0)
            {
                result.AddError(new Error("Missing Information", "Please provide a valid employee ID"));
            }

            //	rule:	there must be invoice lines provides
            //			Make sure that your InvoiceLines have been initialize (xxx = new List<InvoiceLine>();
            if (invoiceView.InvoiceLines.Count() == 0)
            {
                result.AddError(new Error("Missing Information", "Invoice details are required"));
            }

            //	rule:	foreac each invoice line, there must be a part ID
            //	rule:	foreac each invoice line, price cannot be less than zero
            //	rule:	foreac each invoice line, quantity cannot be less than 1
            foreach (var invoiceLIne in invoiceView.InvoiceLines)
            {
                if (invoiceLIne.PartID == 0)
                {
                    //	need to exit because we have no part information to process
                    return result.AddError(new Error("Missing Information", "Missing part ID"));
                }
                if (invoiceLIne.Price < 0)
                {
                    string partName = _hogWildContext.Parts
                                        .Where(p => p.PartID == invoiceLIne.PartID)
                                        .Select(p => p.Description).FirstOrDefault();
                    result.AddError(new Error("Invalid Price", $"Part {partName} has a price less than zero"));
                }
                if (invoiceLIne.Quantity < 1)
                {
                    string partName = _hogWildContext.Parts
                                        .Where(p => p.PartID == invoiceLIne.PartID)
                                        .Select(p => p.Description).FirstOrDefault();
                    result.AddError(new Error("Invalid Quanity", $"Part {partName} has a quantity less than one"));
                }
            }

            // rule:    parts cannot be duplicated on more than one line.
            List<string> duplicatedParts = invoiceView.InvoiceLines
                                            .GroupBy(x => new { x.PartID })
                                            .Where(gb => gb.Count() > 1)
                                            .OrderBy(gb => gb.Key.PartID)
                                            .Select(gb => _hogWildContext.Parts
                                                            .Where(p => p.PartID == gb.Key.PartID)
                                                            .Select(p => p.Description)
                                                            .FirstOrDefault()
                                            ).ToList();
            if (duplicatedParts.Count > 0)
            {
                foreach (var partName in duplicatedParts)
                {
                    result.AddError(new Error("Duplicate Invoice Line Items",
                            $"Part {partName} can only be added to the invoice lines once."));
                }
            }

            //	exit if we have any outstanding errors
            if (result.IsFailure)
            {
                return result;
            }
            #endregion

            //	retrieve the invoice from the database or create a new record/entity if it does not exist
            Invoice invoice = _hogWildContext.Invoices
                                .Where(i => i.InvoiceID == invoiceView.InvoiceID
                                                && !i.RemoveFromViewFlag
                                ).Select(i => i).FirstOrDefault();

            //	if the invoice doesn't exist, initizlize it
            if (invoice == null)
            {
                invoice = new Invoice();
                //	set the current date for the new invoice
                invoice.InvoiceDate = DateTime.Now;
                invoice.CustomerID = invoiceView.CustomerID;
            }
            //	update invoice properties (fields) from the view model
            invoice.EmployeeID = invoiceView.EmployeeID;
            invoice.RemoveFromViewFlag = invoiceView.RemoveFromViewFlag;
            //	reset the subtotal & tax as this will be updated from the invoice lines
            invoice.SubTotal = 0;
            invoice.Tax = 0;

            //	process each line item in the the provided view model
            foreach (InvoiceLineView invoiceLineView in invoiceView.InvoiceLines)
            {
                // record/entiry
                InvoiceLine invoiceLine = _hogWildContext.InvoiceLines
                                            .Where(il => il.InvoiceLineID == invoiceLineView.InvoiceLineID)
                                            .Select(il => il).FirstOrDefault();
                //	if the line item does not exist, initialize it
                if (invoiceLine == null)
                {
                    invoiceLine = new InvoiceLine();  //  record/entity
                    invoiceLine.PartID = invoiceLineView.PartID;
                }

                //	update the invoice line properties/field from the view model
                invoiceLine.Quantity = invoiceLineView.Quantity;
                invoiceLine.Price = invoiceLineView.Price;
                invoiceLine.RemoveFromViewFlag = invoiceLineView.RemoveFromViewFlag;

                //	handle new or existing line items
                if (invoiceLine.InvoiceLineID == 0)
                {
                    //	add new line item to the invoice entity
                    invoice.InvoiceLines.Add(invoiceLine);
                }
                else
                {
                    //	update the database record with the existing line item
                    _hogWildContext.InvoiceLines.Update(invoiceLine);
                }

                //	need to update sub total and tax if the invoice line item 
                //		is not set to be removed from view
                if (!invoiceLine.RemoveFromViewFlag)
                {
                    invoice.SubTotal = invoiceLineView.Price * invoiceLineView.Quantity;
                    bool isTaxable = _hogWildContext.Parts
                                        .Where(p => p.PartID == invoiceLineView.PartID)
                                        .Select(p => p.Taxable).FirstOrDefault();
                    // invoice.Tax = isTaxable ? invoice.Tax  + invoiceLine.Quantity * invoiceLine.Price * 0.05m 
                    //								: invoice.Tax;
                    invoice.Tax += isTaxable ? invoiceLine.Quantity * invoiceLine.Price * 0.05m : 0;
                }
            }
            //	if it is a new invoice, add it to the collection
            if (invoice.InvoiceID == 0)
            {
                //  add the invoice to the invoice table
                _hogWildContext.Invoices.Add(invoice);
            }
            else
            {
                //	update the invoice in the invoice table
                _hogWildContext.Invoices.Update(invoice);
            }

            try
            {
                //	NOTE: YOU CAN ONLY HAVE ONE SAVE CHANGES IN A METHOD
                _hogWildContext.SaveChanges();
            }
            catch (Exception ex)
            {
                //	clear change to maintain data integrity
                _hogWildContext.ChangeTracker.Clear();
                //	we  do not have to throw an exception, just need to log the erro message
                return result.AddError(new Error("Error Saving Changes", ex.InnerException.Message));
            }
            return GetInvoice(invoice.InvoiceID, invoice.CustomerID, invoice.EmployeeID);
        }

        //	get the customer full name
        public string GetCustomerFullName(int customerID)
        {
            return _hogWildContext.Customers
                        .Where(c => c.CustomerID == customerID)
                        .Select(c => $"{c.FirstName} {c.LastName}").FirstOrDefault() ?? string.Empty;
        }

        //	get the employee full name
        public string GetEmployeeFullName(int employeeID)
        {
            return _hogWildContext.Employees
                        .Where(e => e.EmployeeID == employeeID)
                        .Select(e => $"{e.FirstName} {e.LastName}").FirstOrDefault() ?? string.Empty;
        }
    }
}
