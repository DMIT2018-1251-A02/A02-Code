using BYSResults;
using LinqToWebSystem.BLL;
using LinqToWebSystem.ViewModels;
using Microsoft.AspNetCore.Components;

namespace LinqToWebApp.Components.Pages.SamplePages
{
    public partial class ArtistPage
    {
        #region Fields
        private int artistID = 0;
        private string artistName = string.Empty;

        //  Tells us if the search has been performed
        private bool noRecords;
        //  The feedback message
        private string feedbackMessage = string.Empty;

        //  The error message
        private string errorMessage = string.Empty;

        //  has feedback
        private bool hasFeedback => !string.IsNullOrWhiteSpace(feedbackMessage);

        //  has error
        private bool hasError => !string.IsNullOrWhiteSpace(errorMessage) || errorDetails.Count > 0;

        // error details
        private List<string> errorDetails = new();
        #endregion

        #region Properties
        //	need to use the new() so that the artist page will load
        //  artist view returned by the service using GetArtist().
        protected ArtistEditView Artist = new();

        //  artist view returned by the service  using GetArtist().
        protected List<ArtistEditView> Artists { get; set; } = new();
        //  Injects the CustomerService dependency.
        [Inject]
        protected ArtistService ArtistService { get; set; } = default!;
        #endregion

        #region Methods
        // errorDetails = LinqToWebHelperClass.GetErrorMessages(result.Errors.ToList());
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
                var result = ArtistService.GetArtist(artistID);
                if (result.IsSuccess)
                {
                    Artist = result.Value;
                }
                else
                {
                    errorDetails = LinqToWebHelperClass.GetErrorMessages(result.Errors.ToList());
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
                var result = ArtistService.GetArtists(artistName);
                if (result.IsSuccess)
                {
                    Artists = result.Value;
                }
                else
                {
                    errorDetails = LinqToWebHelperClass.GetErrorMessages(result.Errors.ToList());
                }
            }
            catch (Exception ex)
            {
                // capture any exception message for display
                errorMessage = ex.Message;
            }
        }

        private void SearchID()
        {
            artistName = string.Empty;
            Artists.Clear();
            GetArtist(artistID);
        }
        private void SearchName()
        {
            artistID= 0;
            GetArtists(artistName);
        }

        private void Clear()
        {
            errorDetails.Clear();
            errorMessage = string.Empty;
            feedbackMessage = String.Empty;
            artistID = 0;
            artistName = string.Empty;
        }
        #endregion
    }
}
