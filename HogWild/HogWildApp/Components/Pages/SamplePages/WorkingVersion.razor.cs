//#nullable disable
using HogWildSystem.BLL;
using HogWildSystem.ViewModels;
using Microsoft.AspNetCore.Components;

namespace HogWildApp.Components.Pages.SamplePages
{
    public partial class WorkingVersion
    {
        #region Fields
        //  feedback message
        private string feedback = string.Empty;
        //  holds a reference to the WorkingVersionView instance.
        private WorkingVersionsView workingVersionsView = new WorkingVersionsView();
        #endregion


        #region Properties
        //  This attribute marks the property for dependency injection
        [Inject]
        //  This property provides access to the 'WorkingVersionsService' service
        protected WorkingVersionsService WorkingVersionsService { get; set; }
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
                feedback = ex.Message;
            }
        }
        #endregion
    }
}
