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

ProductSubcategories
	.Select(x => new
	{
		Category = x.ProductCategory.ProductCategoryName,
		SubCategory = x.ProductSubcategoryName,
		LowestCost = x.Products.Min(x => x.UnitCost),
		LowestPrice = x.Products.Min(x => x.UnitPrice)
	})
	.Where(x => x.LowestCost != null && x.LowestPrice != null)
	.OrderBy(x => x.Category)
	.ThenBy(x => x.SubCategory)
	.Dump();