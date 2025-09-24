<Query Kind="Statements">
  <Connection>
    <ID>bf2de8d9-a3de-41ab-8ceb-e145025ece12</ID>
    <NamingServiceVersion>2</NamingServiceVersion>
    <Persist>true</Persist>
    <Server>.</Server>
    <AllowDateOnlyTimeOnly>true</AllowDateOnlyTimeOnly>
    <DeferDatabasePopulation>true</DeferDatabasePopulation>
    <Database>Chinook-2025</Database>
    <DriverData>
      <LegacyMFA>false</LegacyMFA>
    </DriverData>
  </Connection>
</Query>

Albums
	.Where(a => a.ReleaseYear > 1970)
	.OrderBy(a => a.ReleaseYear)
	.ThenBy(a => a.ReleaseLabel)
	.Select(a => new
	{
		Year = a.ReleaseYear,
		Label = a.ReleaseLabel
	}).ToList()
	.Dump();

Albums
	.Where(a => a.ReleaseYear > 1970)
	.OrderBy(a => a.ReleaseYear)
	.ThenBy(a => a.ReleaseLabel)
	.Select(a => new
	{
		Year = a.ReleaseYear,
		Label = a.ReleaseLabel
	}).Distinct()
	.ToList()
	.Dump();