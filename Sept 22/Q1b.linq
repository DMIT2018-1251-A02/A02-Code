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

Orders
	.GroupBy(o => o.OrderDate.Value.Year)
	.Select(g => new
	{
	Year = g.Key,
	Revenue = OrderDetails.Where(x => x.Order.OrderDate.Value.Year 
								== g.Key)
	.Select(x => x.UnitPrice * x.Quantity).Sum()
	})
	.OrderBy(x => x.Year)
	.Dump();
