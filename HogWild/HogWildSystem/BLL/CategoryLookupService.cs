using BYSResults;
using HogWildSystem.DAL;
using HogWildSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HogWildSystem.BLL
{
    public class CategoryLookupService
    {
        #region Fields
        //  hog wild context
        private readonly HogWildContext _hogWildContext;
        #endregion

        //  constructor for the CategoryLookupService class.
        internal CategoryLookupService(HogWildContext hogWildContext)
        {
            //  Initialize the _hogWildContext field with the provided HogWildContext instance.
            _hogWildContext = hogWildContext;
        }

        public Result<List<LookupView>> GetLookups(string categoryName)
        {
            //  create a result container
            var result = new Result<List<LookupView>>();

            #region Business Rules
            //  Rule:   category name cannot be empty
            //  Rule:   RemoveFromViewFlag must be false (soft delete)
            if (string.IsNullOrWhiteSpace(categoryName))
            {
                return result.AddError(new Error("Missing Information",
                                        "Please provide a valid category name"));
            }
            #endregion


            var lookups = _hogWildContext.Lookups
                              .Where(x => x.Category.CategoryName == categoryName)
                              .Select(x => new LookupView
                              {
                                  LookupID = x.LookupID,
                                  CategoryID = x.CategoryID,
                                  Name  = x.Name,
                                  RemoveFromViewFlag = x.RemoveFromViewFlag
                              })
                              .OrderBy(x => x.Name)
                              .ToList();

            // If not lookups were found with the category name
            if(lookups == null || lookups.Count() == 0)
            {
                return result.AddError(new Error("No Lookups",
                    $"No lookups for category name '{categoryName}' was found"));
            }

            //  return the result
            return result.WithValue(lookups);

        }
    }
}
