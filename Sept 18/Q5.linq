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

Customers
.GroupBy(c => c.Region)
.Select(g => new
{
	Region = g.Key == null ? "Unknown" : g.Key,
	OrderCount = g.Sum(c => c.Orders.Count())
	
}).ToList()
.OrderBy(x => x.OrderCount)