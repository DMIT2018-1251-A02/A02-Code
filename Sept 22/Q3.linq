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
	.GroupBy(o => new {o.Customer.Country, o.OrderDate.Value.Year})
	.Select(g => new
	{
		Country = g.Key.Country,
		Year = g.Key.Year,
		Orders = g.Select(o => new
		{
			OrderID = o.OrderID,
			Customer = o.Customer.CompanyName
		})
		.ToList()
		.OrderBy(o => o.OrderID)
	}).OrderBy(g => g.Country)
	.ThenBy(g => g.Year)
	.ToList()
	.Dump();