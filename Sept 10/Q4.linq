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
	.Select(a => new
	{
		Title = a.Title,
		Label = a.ReleaseLabel == null || a.ReleaseLabel == "" 	? "Unknown"
																: a.ReleaseLabel,
		Artist = a.Artist.Name,
		Year = a.ReleaseYear
												
	})
	.ToList()
	.Dump();