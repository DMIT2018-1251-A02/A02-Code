<Query Kind="Statements">
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

InvoiceLines
	.GroupBy(il => new {il.Product.ProductSubcategory.ProductCategory.ProductCategoryName,
						il.Product.ProductSubcategory.ProductSubcategoryName})
	.Select(g => new
	{
		CategoryName = g.Key.ProductCategoryName,
		SubCategoryName = g.Key.ProductSubcategoryName,
		Invoices = g.Select(x => new
		{
			InvoiceID = x.InvoiceID,
			Product = x.Product.ProductName,
			Amount = x.SalesQuantity
		})
		.OrderBy(x => x.Product)
		.ToList()
	})
	.OrderBy(g => g.CategoryName)
	.ThenBy(g => g.SubCategoryName)
	.ToList()
	.Dump();
	