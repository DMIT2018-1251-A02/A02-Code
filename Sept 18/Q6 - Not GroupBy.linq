<Query Kind="Expression">
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

Employees
	.Select(x => new
	{
		Sales = x.FirstName + " " + x.LastName,
		Orders = x.Orders.Select(o => new
		{
			OrderID = o.OrderID,
			OrderDagte = o.OrderDate.DateOnly(),
			Customer = o.Customer.CompanyName
		}
		).ToList()
		.Take(5)
	})
	.ToList()
	.Dump()