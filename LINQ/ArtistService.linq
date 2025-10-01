<Query Kind="Program">
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
  <NuGetReference>BYSResults</NuGetReference>
</Query>

// 	Lightweight result types for explicit success/failure 
//	 handling in .NET applications.
using BYSResults;

// —————— PART 1: Main → UI ——————
//	Driver is responsible for orchestrating the flow by calling 
//	various methods and classes that contain the actual business logic 
//	or data processing operations.
void Main()
{
	CodeBehind codeBehind = new CodeBehind(this); // “this” is LINQPad’s auto Context

	#region Get Artist (GetArtist)
	//	Fail
	//	Rule:	artistID must be valid
	codeBehind.GetArtist(0);
	codeBehind.ErrorDetails.Dump("ArtistID must be Valid");

	// Rule:	artistID must be valid (no artist id for 100000
	codeBehind.GetArtist(100000);
	codeBehind.ErrorDetails.Dump("No ArtistID for 100000");

	//	Pass
	codeBehind.GetArtist(1);
	codeBehind.Artist.Dump("Pass - Valid ID");
	#endregion

	#region Get Artists (GetArtists)
	//	Fail
	//	Rule:	artistName must be valid
	codeBehind.GetArtists(string.Empty);
	codeBehind.ErrorDetails.Dump("Artist name is required");

	// Rule:	artist must be valid (no artist name for abc)
	codeBehind.GetArtists("abc");
	codeBehind.ErrorDetails.Dump("No artists found for name 'abc'");

	//	Pass
	codeBehind.GetArtists("ABB");
	codeBehind.Artists.Dump("Pass - Valid Artist Name");
	#endregion

}

// ———— PART 2: Code Behind → Code Behind Method ————
// This region contains methods used to test the functionality
// of the application's business logic and ensure correctness.
// NOTE: This class functions as the code-behind for your Blazor pages
#region Code Behind Methods
public class CodeBehind(TypedDataContext context)
{
	#region Supporting Members (Do not modify)
	// exposes the collected error details
	public List<string> ErrorDetails => errorDetails;

	// Mock injection of the service into our code-behind.
	// You will need to refactor this for proper dependency injection.
	// NOTE: The TypedDataContext must be passed in.
	private readonly Library YourService = new Library(context);
	#endregion

	#region Fields from Blazor Page Code-Behind
	// feedback message to display to the user.
	private string feedbackMessage = string.Empty;
	// collected error details.
	private List<string> errorDetails = new();
	// general error message.
	private string errorMessage = string.Empty;
	#endregion

	//	using default! so we do not get warning
	//  artist view returned by the service using GetArtist().
	public ArtistEditView Artist = default!;

	//  artist view returned by the service  using GetArtist().
	public List<ArtistEditView> Artists = default!;

	//  GetArtist method
	public void GetArtist(int artistID)
	{
		// clear previous error details and messages
		errorDetails.Clear();
		errorMessage = string.Empty;
		feedbackMessage = String.Empty;

		// wrap the service call in a try/catch to handle unexpected exceptions
		try
		{
			var result = YourService.GetArtist(artistID);
			if (result.IsSuccess)
			{
				Artist = result.Value;
			}
			else
			{
				errorDetails = GetErrorMessages(result.Errors.ToList());
			}
		}
		catch (Exception ex)
		{
			// capture any exception message for display
			errorMessage = ex.Message;
		}
	}
	
	//  GetArtists method
	public void GetArtists(string artistName)
	{
		// clear previous error details and messages
		errorDetails.Clear();
		errorMessage = string.Empty;
		feedbackMessage = String.Empty;

		// wrap the service call in a try/catch to handle unexpected exceptions
		try
		{
			var result = YourService.GetArtists(artistName);
			if (result.IsSuccess)
			{
				Artists = result.Value;
			}
			else
			{
				errorDetails = GetErrorMessages(result.Errors.ToList());
			}
		}
		catch (Exception ex)
		{
			// capture any exception message for display
			errorMessage = ex.Message;
		}
	}
}
#endregion

// ———— PART 3: Database Interaction Method → Service Library Method ————
//	This region contains support methods for testing
#region Methods
public class Library
{
	#region Data Context Setup
	// The LINQPad auto-generated TypedDataContext instance used to query and manipulate data.
	private readonly TypedDataContext _hogWildContext;

	// The TypedDataContext provided by LINQPad for database access.
	// Store the injected context for use in library methods
	// NOTE:  This constructor is simular to the constuctor in your service
	public Library(TypedDataContext context)
	{
		_hogWildContext = context
					?? throw new ArgumentNullException(nameof(context));
	}
	#endregion

	public Result<ArtistEditView> GetArtist(int artistID)
	{
		//  Create a Result continer that will hold eithe a 
		//  ArtistEditView object or success or any accumlated errors on failure
		var result = new Result<ArtistEditView>();

		#region Business Logic and Parameter Exceptions
		//	Business Rules
		// 	These are processing rules that need to be satisfied
		//		for valid data
		//		Rule:  artistID must be valid

		if (artistID == 0)
		{
			result.AddError(new Error("Missing Information", "Please provide a valid artist ID"));
			//	need to exit because we have nothing to search for
			return result;
		}
		#endregion
		//  artist that meet our criteria
		var artist = _hogWildContext.Artists
					.Where(a => a.ArtistId == artistID)
					.Select(a => new ArtistEditView
					{
						ArtistID = a.ArtistId,
						Name = a.Name
					}).FirstOrDefault();

		// if not artist were found with the artis id provied
		if (artist == null)
		{
			result.AddError(new Error("No Artist", $"No artist was found for with ID: {artistID}"));
			//	need to exit because we will not be able to add a null artist
			//	to the result if there are any errors
			return result;
		}
		//	return the result
		return result.WithValue(artist);
	}

	public Result<List<ArtistEditView>> GetArtists(string artistName)
	{
		//  Create a Result continer that will hold eithe a 
		//  ArtistEditView object or success or any accumlated errors on failure
		var result = new Result<List<ArtistEditView>>();

		#region Business Logic and Parameter Exceptions
		//	Business Rules
		// 	These are processing rules that need to be satisfied
		//		for valid data
		//		Rule:  artist name is required

		if (string.IsNullOrWhiteSpace(artistName))
		{
			result.AddError(new Error("Missing Information", "Artist name is required"));
			//	need to exit because we have nothing to search for
			return result;
		}
		#endregion

		//  artist that meet our criteria
		var artists = _hogWildContext.Artists
					.Where(a => a.Name.ToUpper().Contains(artistName.ToUpper()))
					.Select(a => new ArtistEditView
					{
						ArtistID = a.ArtistId,
						Name = a.Name
					}).ToList();

		// if not artist were found with the artist name provied
		if (artists.Count() == 0)
		{
			result.AddError(new Error("No Artist", $"No artist was found with the name: '{artistName}'"));
			//	need to exit because we will not be able to add a null artist
			//	to the result if there are any errors
			return result;
		}
		//	return the result
		return result.WithValue(artists);
	}
}

#endregion

// ———— PART 4: View Models → Service Library View Model ————
//	This region includes the view models used to 
//	represent and structure data for the UI.
#region View Models
public class ArtistEditView
{
	public int ArtistID { get; set; }
	public string Name { get; set; }
}
#endregion

//	This region includes support methods
#region Support Method
// Converts a list of error objects into their string representations.
public static List<string> GetErrorMessages(List<Error> errorMessage)
{
	// Initialize a new list to hold the extracted error messages
	List<string> errorList = new();

	// Iterate over each Error object in the incoming list
	foreach (var error in errorMessage)
	{
		// Convert the current Error to its string form and add it to errorList
		errorList.Add(error.ToString());
	}

	// Return the populated list of error message strings
	return errorList;
}
#endregion