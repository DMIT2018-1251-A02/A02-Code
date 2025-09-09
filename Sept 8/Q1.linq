<Query Kind="Program">
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

void Main()
{
	//  method syntax
	//GetAllAlbum(2000).Dump();
	GetArtistLessThanArtistID(50).Dump();
}

List<Album> GetAllAlbum(int paramYear)
{	
	return Albums
				.Where(x => x.ReleaseYear == paramYear)
				.ToList();
}

List<Artist> GetArtistLessThanArtistID(int id)
{
	return Artists
				.Where(x => x.ArtistId < id)
				.ToList();
}








