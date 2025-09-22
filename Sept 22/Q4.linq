<Query Kind="Statements">
  <Connection>
    <ID>87b589ca-6377-4501-bf9b-ca701d8a544d</ID>
    <NamingServiceVersion>2</NamingServiceVersion>
    <Persist>true</Persist>
    <Server>.</Server>
    <AllowDateOnlyTimeOnly>true</AllowDateOnlyTimeOnly>
    <DeferDatabasePopulation>true</DeferDatabasePopulation>
    <Database>WestWind-2024</Database>
    <DriverData>
      <LegacyMFA>false</LegacyMFA>
    </DriverData>
  </Connection>
</Query>

OrderDetails
	.GroupBy(od => new {od.Order.Customer.City, 
				Category = od.Product.Category.CategoryName})
	.Select(g => new
	{
		City = g.Key.City,
		Category = g.Key.Category,
		Count = g.Count()
	})
	.OrderBy(g => g.City)
	.ThenBy(g => g.Category)
	.ToList()
	.Dump();