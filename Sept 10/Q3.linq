<Query Kind="Statements">
  <Connection>
    <ID>2a0ff694-e86d-4442-833a-0e37711c8746</ID>
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

Customers
	.OrderByDescending(c => c.Fax)
	.Select(c => new
	{
		Name = c.CompanyName,
		Country = c.Country,
		Fax = c.Fax == null || c.Fax == "" ? "Unknown" : c.Fax,		
		Fax2 = c.Fax
		//  the following will not work in LINQ to SQL
		//	Fax3 = string.IsNullOrWhiteSpace(c.Fax) ? "Unknown" : c.Fax
	})
	.ToList()
	.Dump();