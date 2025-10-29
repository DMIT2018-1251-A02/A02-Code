// ***********************************************************************
// Assembly         : HogWildSystem
// Author           : James Thompson
// Created          : 10-29-2025
//
// Last Modified By : James Thompson
// Last Modified On : 10-29-2025
// ***********************************************************************
// <copyright file="WorkingVersionsView.cs" company="HogWildSystem">
//     Copyright (c) Northern Alberta Institute of Technology. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace HogWildSystem.ViewModels
{
    /// <summary>
    /// Class WorkingVersionsView.
    /// </summary>
    public class WorkingVersionsView
    {
        /// <summary>
        /// Gets or sets the version identifier.
        /// </summary>
        /// <value>The version identifier.</value>
        public int VersionID { get; set; }
        /// <summary>
        /// Gets or sets the major.
        /// </summary>
        /// <value>The major.</value>
        public int Major { get; set; }
        /// <summary>
        /// Gets or sets the minor.
        /// </summary>
        /// <value>The minor.</value>
        public int Minor { get; set; }
        /// <summary>
        /// Gets or sets the build.
        /// </summary>
        /// <value>The build.</value>
        public int Build { get; set; }
        /// <summary>
        /// Gets or sets the revision.
        /// </summary>
        /// <value>The revision.</value>
        public int Revision { get; set; }
        /// <summary>
        /// Gets or sets as of date.
        /// </summary>
        /// <value>As of date.</value>
        public DateTime AsOfDate { get; set; }
        /// <summary>
        /// Gets or sets the comments.
        /// </summary>
        /// <value>The comments.</value>
        public string Comments { get; set; }
    }
}
