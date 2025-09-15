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
	List<SongView> result = SongByPartialName("Dance");
	result.Dump();
	result.Select(x => x.SongTitle).ToList().Dump();

}

public List<SongView> SongByPartialName(string name)
{
	return Tracks
			.Where(x => x.Name.Contains(name))
			.Select(x => new SongView
			{
				AlbumTitle = x.Album.Title,
				SongTitle = x.Name,
				Artist = x.Album.Artist.Name
			}).ToList();

}

public class SongView
{
	public string AlbumTitle { get; set; }
	public string SongTitle { get; set; }
	public string Artist { get; set; }
}

