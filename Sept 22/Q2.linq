<Query Kind="Statements">
  <Connection>
    <ID>e0a87a77-277f-494c-93a7-51c2205344d2</ID>
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
	.GroupBy(a => new {Label = a.ReleaseLabel, a.ReleaseYear})
	.Select(g => new
	{
		Year = g.Key.ReleaseYear,
		Label = g.Key.Label == null ? "Unknown" : g.Key.Label,
		Count = g.Count(),
		Albums = g.Select(a => new
		{
			Title = a.Title,
			Artist = a.Artist.Name,
		})
		.OrderBy(a => a.Title)
		.ToList()
	})
	.Where(g => g.Count > 1)
	.OrderBy(g => g.Year)
	.ThenBy(g => g.Label)
	.ToList()
	.Dump();