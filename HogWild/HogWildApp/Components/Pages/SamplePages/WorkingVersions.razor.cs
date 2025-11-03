using HogWildSystem.BLL;
using HogWildSystem.ViewModels;
using Microsoft.AspNetCore.Components;

namespace HogWildApp.Components.Pages.SamplePages
{
    public partial class WorkingVersions
    {
        #region Fields
        // this field holds a reference to the WorkingVersionsView instance
        private WorkingVersionsView workingVersionsView = default!;

        //  fields for holding any feedback message
        private string feedback = string.Empty;
        #endregion

        #region Properties
        // This attribute marks the propertu for dependency injection
        [Inject]
        //  this property provides access to the 'WorkingVersionService' service
        protected WorkingVersionsService WorkingVersionsService { get; set; } = default!;
        #endregion

        #region Methods
        private void GetWorkingVersion()
        {
            try
            {
                workingVersionsView = WorkingVersionsService.GetWorkingVersion();
            }
            catch (Exception ex)
            {
                //  capture any exception message for display
                feedback = ex.Message;

            }
        }

        #endregion
    }
}