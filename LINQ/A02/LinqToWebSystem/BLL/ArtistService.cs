using BYSResults;
using LinqToWebSystem.DAL;
using LinqToWebSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqToWebSystem.BLL
{
    public class ArtistService
    {
        //  linq to web context
        private readonly LinqToWebContext _linqToWebContext;

        //  Constructor for the ArtistService class.
        internal ArtistService(LinqToWebContext linqToWebContext)
        {
            //  Initialize the _hogWildContext fiekd with the provieded HogWildContext instance.
            _linqToWebContext = linqToWebContext;
        }

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
            var artist = _linqToWebContext.Artists
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
            var artists = _linqToWebContext.Artists
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
}
