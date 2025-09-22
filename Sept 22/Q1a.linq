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
	.GroupBy(o => o.Order.OrderDate.Value.Year)
	.Select(g => new
	{
		Year = g.Key,
		Revenue = g.Sum(x => x.Quantity * x.UnitPrice)
	}).OrderBy(x => x.Year)
	.Dump();