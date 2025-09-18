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

Products
	.GroupBy(p => p.Category.CategoryID)
	.Select(x => new
	{
		Categories = x.Key,
		Products = x.ToList()
	}).ToList()
	.Dump();

Products
.GroupBy(p => p.Category.CategoryID)
.Select(g => new
{
	Categories = g.Key,
	Products = g.Select(p => new
	{
		ProductID = p.ProductID,
		ProductName = p.ProductName
	}).ToList()
}).ToList()
.Dump();