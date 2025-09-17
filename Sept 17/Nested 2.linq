<Query Kind="Program">
  <Connection>
    <ID>63f7df99-61e5-497e-b9e3-230360397903</ID>
    <NamingServiceVersion>2</NamingServiceVersion>
    <Persist>true</Persist>
    <Server>.</Server>
    <AllowDateOnlyTimeOnly>true</AllowDateOnlyTimeOnly>
    <DeferDatabasePopulation>true</DeferDatabasePopulation>
    <Database>Contoso</Database>
    <DriverData>
      <LegacyMFA>false</LegacyMFA>
    </DriverData>
  </Connection>
</Query>

void Main()
{
	GetInvoicesWithDetails("Torres").Dump();
}

public List<InvoiceView> GetInvoicesWithDetails(string name)
{
	return Invoices
			.Where(i => i.Customer.LastName.Contains(name))
			.Select(i => new InvoiceView
			{
				InvoiceNo = i.InvoiceID,
				InvoiceDate = i.DateKey.DateOnly(),
				Customer = i.Customer.FirstName + " " + i.Customer.LastName,
				Amount = i.TotalAmount,
				Details = i.InvoiceLines
							.Select(il => new InvoiceLineView
							{
								LineReference = il.InvoiceLineID,
								ProductName = il.Product.ProductName,
								Qty = il.SalesQuantity,
								Price = il.UnitPrice,
								Discount = il.DiscountAmount,
								ExtPrice = (il.UnitPrice - il.DiscountAmount) * il.SalesQuantity
							}).ToList()
		}).ToList();
}

#region View Models
public class InvoiceView
{
	public int InvoiceNo { get; set; }
	public DateOnly InvoiceDate { get; set; }
	public string Customer { get; set; }
	public decimal Amount { get; set; }
	public List<InvoiceLineView> Details { get; set; }
}

public class InvoiceLineView
{
	public int LineReference { get; set; }
	public string ProductName { get; set; }
	public int Qty { get; set; }
	public decimal? Price { get; set; }
	public decimal? Discount { get; set; }
	public decimal? ExtPrice { get; set; }
}
#endregion