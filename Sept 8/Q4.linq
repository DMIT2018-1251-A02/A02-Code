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
	GetArtistLessThanArtistID(50).Dump();
}

// You can define other methods, fields, classes and namespaces here
List<ArtistView> GetArtistLessThanArtistID(int id)
{
	return Artists
			.Where(a => a.ArtistId < id)
			.Select(a => new ArtistView
			{
				ID = a.ArtistId,
				CoolArtistName = a.Name
			})
			.ToList();
}

public class ArtistView
{
	public int ID { get; set; }
	public string CoolArtistName { get; set; }
}