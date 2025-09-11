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
	.Select(x => new
	{
		Title = x.Title,
		Artist = x.Artist.Name,
		Year = x.ReleaseYear,
		Decade = x.ReleaseYear < 1970 ? "Oldies"
					: x.ReleaseYear < 1980 ? "70's"
					: x.ReleaseYear < 1990 ? "80's"
					: x.ReleaseYear < 2000 ? "90's"
					: "Modern"
	})
	.OrderBy(x => x.Year)
	//.OrderBy(x => x.Decade)
	.Dump();