using BYSResults;
using HogWildSystem.DAL;
using HogWildSystem.ViewModels;

namespace HogWildSystem.BLL
{
    public class PartService
    {
        #region Fields
        //  hog wild context
        private readonly HogWildContext _hogWildContext;
        #endregion

        //  constructor for the PartService class.
        internal PartService(HogWildContext hogWildContext)
        {
            //  Initialize the _hogWildContext field with the provided HogWildContext instance.
            _hogWildContext = hogWildContext;
        }

        public Result<List<PartView>> GetParts(int partCategoryID, string description, List<int> existingPartIDs)
        {
            //	Create a Result container that will hold either a 
            //		list of PartView on success or any accumulated errors on failure
            var result = new Result<List<PartView>>();

            #region Business Rules
            //	These are processing rules that need to be satisfied
            //		for valid data
            //		rule:	Both part id must be valid and/or description cannot be empty
            //		rule: 	Part IDs in existing part IDs will be ignored
            //		rule:	RemoveFromViewFlag must be false


            //	Both part id must be valid and/or description cannot be empty
            if (partCategoryID == 0 && string.IsNullOrWhiteSpace(description))
            {
                return result.AddError(new Error("Missing Information",
                                    "Please provide either a category and/or description"));
            }
            #endregion

            //	need to update description parametrs so we are not searching on
            //	 an empty string value.  Otherwise, this would return all records
            Guid tempGuid = Guid.NewGuid();
            if (string.IsNullOrWhiteSpace(description))
            {
                description = tempGuid.ToString();
            }

            //	igrore any parts that are in the "existing part IDs" list.
            //	ensure that we are compairing uppercase value for description
            var parts = _hogWildContext.Parts
                            .Where(p => !existingPartIDs.Contains(p.PartID)
                                && (description.Length > 0
                                && description != tempGuid.ToString()
                                && partCategoryID > 0
                                    ? (p.Description.ToUpper().Contains(description.ToUpper())
                                        && p.PartCategoryID == partCategoryID)
                                    : (p.Description.ToUpper().Contains(description.ToUpper())
                                        || p.PartCategoryID == partCategoryID)
                                && !p.RemoveFromViewFlag))
                        .Select(p => new PartView
                        {
                            PartID = p.PartID,
                            PartCategoryID = p.PartCategoryID,
                            CategoryName = p.PartCategory.Name,
                            Description = p.Description,
                            Cost = p.Cost,
                            Price = p.Price,
                            ROL = p.ROL,
                            QOH = p.QOH,
                            Taxable = p.Taxable,
                            RemoveFromViewFlag = p.RemoveFromViewFlag
                        })
                        .OrderBy(p => p.Description)
                        .ToList();

            //  if no parts were found
            if (parts == null || parts.Count() == 0)
            {
                //need to exit because we did not find any parts
                return result.AddError(new Error("No Parts", "No parts were found"));
            }

            //	return the result
            return result.WithValue(parts);
        }

        //	Get the part
        public Result<PartView> GetPart(int partID)
        {
            // Create a Result container that will hold either a
            //	PartView objects on success or any accumulated errors on failure
            var result = new Result<PartView>();
            #region Business Rules
            //	These are processing rules that need to be satisfied
            //		rule:	partID must be valid
            //		rule: 	RemoveFromViewFlag must be false
            if (partID == 0)
            {
                //  need to exit because we have no part information
                return result.AddError(new Error("Missing Information",
                                "Please provide a valid part id"));
            }
            #endregion

            var part = _hogWildContext.Parts
                            .Where(p => (p.PartID == partID
                                         && !p.RemoveFromViewFlag))
                            .Select(p => new PartView
                            {
                                PartID = p.PartID,
                                PartCategoryID = p.PartCategoryID,
                                //  PartCategory is an alias for Lookup
                                CategoryName = p.PartCategory.Name,
                                Description = p.Description,
                                Cost = p.Cost,
                                Price = p.Price,
                                ROL = p.ROL,
                                QOH = p.QOH,
                                Taxable = (bool)p.Taxable,
                                RemoveFromViewFlag = p.RemoveFromViewFlag
                            }).FirstOrDefault();

            //  if no part were found
            if (part == null)
            {
                //  need to exit because we did not find any part
                return result.AddError(new Error("No part", "No part were found"));
            }

            //  return the result
            return result.WithValue(part);
        }
    }
}
