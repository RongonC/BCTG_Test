
using System;
using System.Collections.Generic;
using System.Linq;

namespace StemmonsMobile.DataTypes.DataType.Cases
{
    public class GetCaseTypesRequest
    {
        public string caseTypeId { get; set; }
        public string userName { get; set; }
        public string caseOwnerSAM { get; set; }
        public string caseAssgnSAM { get; set; }
        public string caseCloseBySAM { get; set; }
        public string caseCreateBySAM { get; set; }
        public string propertyId { get; set; }
        public int? tenant_Id { get; set; }
        public string tenant_Code { get; set; }
        public char? showOpenClosedCasesType { get; set; }
        public string searchQuery { get; set; }
        public char? showPastDueCases { get; set; }
        public string CaseId { get; set; }
        public string Title { get; set; }
        public string CasesAssgnTo { get; set; }
        public string metaDataValues { get; set; }
        public string caseNotes { get; set; }

        #region ParentSelectedValues
        public class ParentSelectedValues
        {
            public int ParentAssocTypeId { get; set; }
            public string ParentSelectedExternalDatasourceObjectId { get; set; }
        }
        #endregion



        
    }
    #region UploadFileToCaseTypeModel
    public class UploadFileToCaseTypeModel
    {
        /// <summary>
        /// Case id for file upload
        /// </summary>
        public int caseNumber { get; set; }
        /// <summary>
        /// File Description
        /// </summary>
        public string fileDescription { get; set; }
        /// <summary>
        /// Date and time of upload
        /// </summary>
        public DateTime dateTime { get; set; }
        ///// <summary>
        ///// Assoc system code Id
        ///// </summary>
        public string fileName { get; set; }
        ///// <summary>
        ///// Uploaded file size
        ///// </summary>
        public int fileSize { get; set; }
        ///// <summary>
        ///// Binay file data
        ///// </summary>
        public byte[] fileBinary { get; set; }
        /// <summary>
        /// External URI to locate
        /// </summary>
        public string externalURI { get; set; }
        /// <summary>
        /// Is file need to add to case notes?
        /// </summary>
        public char addToCaseNotes { get; set; }
        /// <summary>
        /// System code
        /// </summary>
        public string systemCode { get; set; }
        /// <summary>
        /// Is active or not
        /// </summary>
        public char isActive { get; set; }
        /// <summary>
        /// Current user
        /// </summary>
        public string currentUser { get; set; }
    }
    #endregion
    public class UploadFileToCaseRequest
    {
        public int caseNumber { get; set; }
        public string fileDescription { get; set; }
        public DateTime dateTime { get; set; }
        public string fileName { get; set; }
        public int fileSize { get; set; }
        public byte[] fileBinary { get; set; }
        public string externalURI { get; set; }
        public char addToCaseNotes { get; set; }
        public string systemCode { get; set; }
        public char isActive { get; set; }
        public string currentUser { get; set; }
    }

    public class UploadFileToCaseResponse : Response
    {
        public object ResponseContent { get; set; }
    }
}