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
	// never ever, ever place your order by before the where clause.
	// .OrderBy(a => a.Artist.Name)  <======  BAD
	.Where(a => a.AlbumId < 10)
	.OrderBy(a => a.Artist.Name)
	.Select(a => new
	{
		Album = a.Title,
		//Artist = a.Artist.Name,
		Year = a.ReleaseYear,
		Label = a.ReleaseLabel,
		TrackCount = a.Tracks.Count(),
		TotalPrice = a.Tracks.Sum(t => t.UnitPrice),
		YearByArtist = a.ReleaseYear + " - " + a.Artist.Name
	}
	)
	.OrderBy(a => a.YearByArtist).Dump()
	.Select(x => new
	{
		James = x.YearByArtist,
		Bob = x.TotalPrice
	})
	.ToList()
	.Dump();