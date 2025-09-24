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
	.Where(a => a.AlbumId > 0)
	.FirstOrDefault().Dump("With Where");

Albums
	.OrderByDescending(a => a.AlbumId)
	.FirstOrDefault(a => a.AlbumId > 0).Dump("With FirstOrDefault()");


Albums
.Where(a => a.AlbumId == 1000)
.FirstOrDefault().Dump("Return Null");

Albums
	.Where(a => a.AlbumId == 1000)
	.Select(a => a.Title)
	.FirstOrDefault()
	.Dump();
