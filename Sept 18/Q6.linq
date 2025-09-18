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

Orders
	.GroupBy(o => o.Employee.EmployeeID)
	.Select(g => new
	{
		//  navigational property
		Sales1 = g.Select(e => e.Employee.FirstName + " " 
						+ e.Employee.LastName).FirstOrDefault(),
		//  table lookup using key
		Sales2 = Employees.Where(e => e.EmployeeID == g.Key)
				.Select(e => e.FirstName + " " + e.LastName)
				.FirstOrDefault(),
		Orders = g.Select(o => new
		{
			OrderId = o.OrderID,
			OrderDate = o.OrderDate,
			Customer = o.Customer.CompanyName
		})
		.Take(5)

	})