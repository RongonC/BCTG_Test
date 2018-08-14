
using System;
using System.Collections.Generic;
using System.Linq;


namespace StemmonsMobile.DataTypes.DataType.Cases
{
    public class CreateCaseOptimizedResponse:Response
    {
        public object ResponseContent { get; set; }

        #region ParseEmailField
        public class ParseEmailField
        {
            /// <summary>
            /// ID of the <see cref="CaseType"/> to which the EmailAddress belongs.
            /// </summary>
            public int? CaseTypeID { get; set; }


            /// <summary>
            /// The name, or text value.
            /// </summary>

            public string Name { get; set; }

            /// <summary>
            /// The EmailAddress, or text value.
            /// </summary>
            public string EmailAddress { get; set; }


            /// <summary>
            /// The EmailAddress, or text value.
            /// </summary>
            public string Keyword { get; set; }


            /// <summary>
            /// The EmailAddress, or text value.
            /// </summary>
            public string Syscode { get; set; }

            /// <summary>
            /// The Field name, or text value.
            /// </summary>
            public string CaseTypeDesc { get; set; }

            /// <summary>
            /// Text value of DecodeID.
            /// </summary>
            public string Text { get; set; }

            /// <summary>
            /// ID of the value object.
            /// </summary>
            public int? AssocTypeID { get; set; }

            /// <summary>
            /// ID of the value object.
            /// </summary>
            public string DefaultValue { get; set; }

            /// <summary>
            /// SYSTEM CODE for DecodeID.
            /// </summary>
            public string AssocDecodeStatusCode { get; set; }

            public ParseEmailField()
            {
                CaseTypeDesc = string.Empty;
                AssocTypeID = -1;
                DefaultValue = string.Empty;
                Syscode = string.Empty;
                Text = string.Empty;

                AssocDecodeStatusCode = string.Empty;
                CaseTypeID = -1;
            }
        }
        #endregion

        #region Enum ActivityLogType
        public enum ActivityLogType
        {
            CreatedCase,
            ViewingCase,
            AddedNotes,
            UpdatingCase,
            StatusChange,
            PriorityChange,
            ErrorVisibilityChange,
            DispatchedCase,
            TakeOwnership,
            Assigned,
            FileAttachment,
            DownloadedFile,
            ViewingActivityLog,
            LocalFolderChange,
            ProjectChange,
            SeverityChange,
            CaseTitleChanged,
            ClosedCase,
            ReOpenedCase,
            ExportedCasePDF,
            DueDate,
            Approved,
            Declined
        }
        #endregion
    }
}