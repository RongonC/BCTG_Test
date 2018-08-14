using System;
using System.Collections.Generic;
using System.Linq;


namespace StemmonsMobile.DataTypes.DataType.Cases
{
    public class AddCaseNotesRequest
    {
        /// <summary>
        /// Case id for file upload
        /// </summary>
        public int caseID { get; set; }
        /// <summary>
        /// Activity log type enum value
        /// </summary>
        public int noteTypeId { get; set; }
        /// <summary>
        /// Notes of case
        /// </summary>
        public string note { get; set; }
        /// <summary>
        /// Force date time
        /// </summary>
        public DateTime forceDateTime { get; set; }
        /// <summary>
        /// Notes is active
        /// </summary>
        public char isActive { get; set; }
        /// <summary>
        /// Created datetime
        /// </summary>
        public DateTime createDateTime { get; set; }
        /// <summary>
        /// Current user user name
        /// </summary>
        public string currentUser { get; set; }

        public string currentUserFullName { get; set; }

    }
}