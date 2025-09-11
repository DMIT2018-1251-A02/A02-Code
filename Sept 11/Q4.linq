<Query Kind="Statements">
  <Connection>
    <ID>e911c88b-78d8-48b8-9b71-c4628c7c6afb</ID>
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

Products
	.Where(p => p.ProductSubcategory.ProductCategory.ProductCategoryName
				== "Music, Movies and Audio Books")
	.OrderBy(p => p.StyleName)
	.Select(p => new
	{
		Name = p.ProductName,
		Color = p.ColorName,
		ColorProcessNeeded = p.ColorName == "White" ||
								p.ColorName == "Black"
								? "No"
								: "Yes"
	})
	.ToList()
	.Dump();