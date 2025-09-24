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
	.Any(x => x.ReleaseYear == 1976)
	.Dump("Return True");

Albums
	.Any(x => x.ReleaseYear == 1400)
	.Dump("Return False");

Albums
	.All(x => x.ReleaseYear >= 1957)
	.Dump("Return True");

Albums
	.All(x => x.ReleaseYear >= 1970)
	.Dump("Return False");