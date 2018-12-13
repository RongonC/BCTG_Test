//using System;
//using System.Collections.Generic;

//namespace StemmonsMobile.DataTypes.DataType.Cases
//{
//    public partial class GetCaseTypesResponse
//    {
//        //#region ParseEmailField
//        //public class ParseEmailField
//        //{
//        //    /// <summary>
//        //    /// ID of the <see cref="CaseType"/> to which the EmailAddress belongs.
//        //    /// </summary>
//        //    public int? CaseTypeID { get; set; }


//        //    /// <summary>
//        //    /// The name, or text value.
//        //    /// </summary>

//        //    public string Name { get; set; }

//        //    /// <summary>
//        //    /// The EmailAddress, or text value.
//        //    /// </summary>
//        //    public string EmailAddress { get; set; }


//        //    /// <summary>
//        //    /// The EmailAddress, or text value.
//        //    /// </summary>
//        //    public string Keyword { get; set; }


//        //    /// <summary>
//        //    /// The EmailAddress, or text value.
//        //    /// </summary>
//        //    public string Syscode { get; set; }

//        //    /// <summary>
//        //    /// The Field name, or text value.
//        //    /// </summary>
//        //    public string CaseTypeDesc { get; set; }

//        //    /// <summary>
//        //    /// Text value of DecodeID.
//        //    /// </summary>
//        //    public string Text { get; set; }

//        //    /// <summary>
//        //    /// ID of the value object.
//        //    /// </summary>
//        //    public int? AssocTypeID { get; set; }

//        //    /// <summary>
//        //    /// ID of the value object.
//        //    /// </summary>
//        //    public string DefaultValue { get; set; }

//        //    /// <summary>
//        //    /// SYSTEM CODE for DecodeID.
//        //    /// </summary>
//        //    public string AssocDecodeStatusCode { get; set; }

//        //    public ParseEmailField()
//        //    {
//        //        CaseTypeDesc = string.Empty;
//        //        AssocTypeID = -1;
//        //        DefaultValue = string.Empty;
//        //        Syscode = string.Empty;
//        //        Text = string.Empty;

//        //        AssocDecodeStatusCode = string.Empty;
//        //        CaseTypeID = -1;
//        //    }
//        //}
//        //#endregion

//        //#region ItemValue
//        ///// <summary>
//        ///// The value object of an item in a case field with predefined options, such as in a drop down list.
//        ///// <para>Defined as ASSOC_DECODE in the database.</para>
//        ///// </summary>
//        //[Serializable]
//        //public class ItemValue
//        //{
//        //    [Obsolete("Not implemented yet", true)]
//        //    public void AddNewItem()
//        //    {
//        //    }

//        //    /// <summary>
//        //    /// ID of the value object.
//        //    /// </summary>
//        //    public int AssocDecodeID { get; set; }

//        //    /// <summary>
//        //    /// ID of the <see cref="ItemType"/> (ASSOC_TYPE in the database) to which this <see cref="ItemValue"/> belongs.
//        //    /// </summary>
//        //    public int AssocTypeID { get; set; }

//        //    /// <summary>
//        //    /// ID of the <see cref="CaseType"/> to which the <see cref="ItemValue"/>'s <see cref="ItemType"/> belongs.
//        //    /// <para>This parameter is NOT defined in database.</para>
//        //    /// </summary>
//        //    public int CaseTypeID { get; set; }

//        //    /// <summary>
//        //    /// Description (not name? Or does it depend?) of the <see cref="CaseType"/> to which the <see cref="ItemValue"/>'s <see cref="ItemType"/> belongs.
//        //    /// <para>This property is NOT defined in database.</para>
//        //    /// </summary>
//        //    public string CaseTypeDesc { get; set; }

//        //    public int? ObjectID { get; set; }

//        //    /// <summary>
//        //    /// ID value of the <see cref="ItemValue"/> when <see cref="ItemValue"/> is used in an unbound manner.
//        //    /// <para>Prepend with "P" when the source is External Data.</para>
//        //    /// <para>This property is NOT defined in database.</para>
//        //    /// </summary>
//        //    /// <remarks>
//        //    /// <see cref="ObjectCode"/> is used to prepare dropdown selections to be saved to the database.
//        //    /// </remarks>
//        //    public string ObjectCode { get; set; }

//        //    /// <summary>
//        //    /// The name, or text value, of the <see cref="ItemValue"/>.
//        //    /// </summary>
//        //    /// <remarks>
//        //    /// When <see cref="ItemValue"/> is used with Property Bags, <see cref="Name"/> is the case field name, and <see cref="TextValue"/> is the selected value (text) from a drop down list.
//        //    /// <para>Although <see cref="Name"/> is correct, <see cref="TextValue"/> may make more sense.</para>
//        //    /// </remarks>
//        //    public string Name { get; set; }

//        //    /// <summary>
//        //    /// Arbitrary text value for this <see cref="ItemValue"/>.
//        //    /// <para>This property is NOT defined in database.</para>
//        //    /// </summary>
//        //    /// <remarks>
//        //    /// When <see cref="ItemValue"/> is used with Property Bags, <see cref="Name"/> is the case field name, and <see cref="TextValue"/> is the selected value (text) from a drop down list.
//        //    /// </remarks>
//        //    [Obsolete("Need to change the references Property Bags use.")]
//        //    public string TextValue { get; set; }

//        //    /// <summary>
//        //    /// The description of the <see cref="ItemValue"/>.
//        //    /// </summary>
//        //    public string Description { get; set; }

//        //    public string GLCode { get; set; }

//        //    public string MiscCode1Desc { get; set; }

//        //    public string MiscCode1Value { get; set; }

//        //    public string MiscCode2Desc { get; set; }

//        //    public string MiscCode2Value { get; set; }

//        //    /// <summary>
//        //    /// The system code of the <see cref="ItemValue"/>.
//        //    /// </summary>
//        //    public string SystemCode { get; set; }

//        //    /// <summary>
//        //    /// The system priority, or sort position, of the <see cref="ItemValue"/>.
//        //    /// </summary>
//        //    public int? SystemPriority { get; set; }

//        //    /// <summary>
//        //    /// Whether or not the <see cref="ItemValue"/> is active.
//        //    /// </summary>
//        //    public char IsActive { get; set; }

//        //    /// <summary>
//        //    /// Whether or not the <see cref="ItemValue"/> affects MTTR.
//        //    /// <para>This property is NOT defined in database.</para>
//        //    /// </summary>
//        //    public char? StatusMTTRAffecting { get; set; }

//        //    /// <summary>
//        //    /// When the <see cref="ItemValue"/> was created.
//        //    /// </summary>
//        //    public DateTime CreatedDateTime { get; set; }

//        //    /// <summary>
//        //    /// Who created the <see cref="ItemValue"/>.
//        //    /// </summary>
//        //    public string CreatedBy { get; set; }

//        //    /// <summary>
//        //    /// When the <see cref="ItemValue"/> was last modified.
//        //    /// </summary>
//        //    public DateTime ModifiedDateTime { get; set; }

//        //    /// <summary>
//        //    /// Who last modified the <see cref="ItemValue"/>.
//        //    /// </summary>
//        //    public string ModifiedBy { get; set; }

//        //    public string ItemPropertyBag
//        //    {
//        //        get
//        //        {
//        //            string required = "Y";
//        //            if (!IsRequired)
//        //                required = "N";

//        //            // 1. AssocTypeID
//        //            // 2. AssocDecodeID
//        //            // 3. Required (Y/N)
//        //            // 4. CaseTypeDesc
//        //            // 5. Child AssocDecodeID
//        //            // 6. Parent AssocDecodeID
//        //            // 7. CaseTypeID
//        //            // 8. AssocDecodeStatusCode

//        //            return AssocTypeID.ToString() + "|" + AssocDecodeID.ToString() + "|" + required + "|" + CaseTypeDesc.Replace('|', ' ').Replace(',', ' ') + "|" + Child + "|" + Parent + "|" + CaseTypeID + "|" + SystemCode;
//        //        }
//        //    }

//        //    /// <summary>
//        //    /// Whether or not the <see cref="ItemValue"/>'s <see cref="ItemType"/> (?) is required.
//        //    /// <para>This property is NOT defined in database.</para>
//        //    /// </summary>
//        //    public bool IsRequired { get; set; }

//        //    /// <summary>
//        //    /// The child of the <see cref="ItemValue"/> (?).
//        //    /// <para>This property is NOT defined in database.</para>
//        //    /// </summary>
//        //    public int Child { get; set; }

//        //    /// <summary>
//        //    /// The parent of the <see cref="ItemValue"/> (?).
//        //    /// <para>This property is NOT defined in database.</para>
//        //    /// </summary>
//        //    public int Parent { get; set; }

//        //    /// <summary>
//        //    /// Creates a new <see cref="ItemValue"/> with some default values.
//        //    /// </summary>
//        //    public ItemValue()
//        //    {
//        //        CaseTypeDesc = string.Empty;
//        //        AssocTypeID = -1;
//        //        AssocDecodeID = -1;
//        //        IsRequired = true;
//        //        Child = -1;
//        //        Parent = -1;
//        //        CaseTypeID = -1;
//        //    }

//        //    public override string ToString()
//        //    {
//        //        return this.Name;
//        //    }
//        //}
//        //#endregion

//        //#region Enum ActivityLogType
//        //public enum ActivityLogType
//        //{
//        //    CreatedCase,
//        //    ViewingCase,
//        //    AddedNotes,
//        //    UpdatingCase,
//        //    StatusChange,
//        //    PriorityChange,
//        //    ErrorVisibilityChange,
//        //    DispatchedCase,
//        //    TakeOwnership,
//        //    Assigned,
//        //    FileAttachment,
//        //    DownloadedFile,
//        //    ViewingActivityLog,
//        //    LocalFolderChange,
//        //    ProjectChange,
//        //    SeverityChange,
//        //    CaseTitleChanged,
//        //    ClosedCase,
//        //    ReOpenedCase,
//        //    ExportedCasePDF,
//        //    DueDate

//        //    //ApprovedCase,
//        //    //RejectedCase
//        //}
//        //#endregion

//        #region CaseData
//        /// <summary>
//        /// A case data object with strongly typed properties.
//        /// <para>If populating a list, such as a GridView, use <see cref="BasicCase"/> instead.</para>
//        /// </summary>
//        // [Serializable]
//        public class CaseData
//        {
//            public int CaseID { get; set; }
//            public int? ListID { get; set; }
//            public int CaseTypeID { get; set; }
//            public string CaseOwner { get; set; }
//            public DateTime? CaseOwnerDateTime { get; set; }
//            public string CaseAssignedTo { get; set; }
//            public DateTime? CaseAssignedDateTime { get; set; }
//            public int? CaseHopperID { get; set; }
//            public DateTime? CaseHopperDateTime { get; set; }
//            public DateTime? CaseClosedDateTime { get; set; }
//            public string CaseClosedBy { get; set; }
//            public DateTime CreateDateTime { get; set; }
//            public string CreateBySam { get; set; }
//            public string CaseCreatedDisplayName { get; set; }
//            public DateTime ModifiedDateTime { get; set; }
//            public string ModifiedBySam { get; set; }
//            public string CaseModifiedByDisplayName { get; set; }
//            public char NewestNoteOnTop { get; set; }
//            public bool LockWhenUnowned { get; set; }

//            public string CaseTypeName { get; set; }
//            public string CaseOwnerDisplayName { get; set; }
//            public string CreateByDisplayName { get; set; }
//            public string CaseAssignedToDisplayName { get; set; }
//            public string CaseClosedByDisplayName { get; set; }
//            public string ModifiedByDisplayName { get; set; }

//            public List<MetaData> MetaDataCollection { get; set; }
//            public List<NoteData> NoteDataCollection { get; set; }

//            public char? IS_ALLOW_APPROVAL { get; set; }

//            public bool DebugMode { get; set; }
//            public string strCaseDue { get; set; }
//            public DateTime? CaseDue { get; set; }
//            public bool IsClosed { get; set; }
//            public string CaseTitle { get; set; }
//            public int? SystemPriority { get; set; }
//            //public string CaseTitle
//            //{
//            //    get
//            //    {
//            //        foreach (MetaData item in this.MetaDataCollection)
//            //        {
//            //            if (item.AssocTypeSystemCode != null)
//            //            {
//            //                if (item.AssocTypeSystemCode.ToUpper().Trim() == "TITLE")
//            //                {
//            //                    // TODO Why is this alternating?
//            //                    //return item.TextValue;
//            //                    //return item.MetaDataText;

//            //                    //return !string.IsNullOrEmpty(item.TextValue) ? item.TextValue : item.MetaDataText;
//            //                    return item.TextValue;
//            //                }
//            //            }
//            //        }
//            //        return "NO TITLE PROVIDED";

//            //    }
//            //}

//            public string CaseTitleLink
//            {
//                get
//                {
//                    foreach (MetaData item in this.MetaDataCollection)
//                    {
//                        if (item.AssocTypeSystemCode != null)
//                        {
//                            if (item.AssocTypeSystemCode.ToUpper().Trim() == "TITLE")
//                            {
//                                string debug = string.Empty;

//                                if (DebugMode)
//                                    debug = "&Debug=Y";

//                                if (item.TextValue != null && item.TextValue.Length > 40)
//                                {

//                                    return "<a href=\"ViewCase.aspx?CaseID=" + CaseID.ToString().Trim() + debug + "\" title=\"" + item.TextValue + "\">" + item.TextValue.Substring(0, 37) + "...</a>";
//                                }
//                                else
//                                {
//                                    return "<a href=\"ViewCase.aspx?CaseID=" + CaseID.ToString().Trim() + debug + "\" title=\"" + item.TextValue + "\">" + item.TextValue + "</a>";
//                                }
//                            }
//                        }
//                    }
//                    return string.Empty;
//                }
//            }

//            public string CaseModalPopupLink
//            {
//                get
//                {
//                    foreach (MetaData item in this.MetaDataCollection)
//                    {
//                        if (item.AssocTypeSystemCode != null)
//                        {
//                            if (item.AssocTypeSystemCode.ToUpper().Trim() == "TITLE")
//                            {
//                                if (item.TextValue != null && item.TextValue.Length > 40)
//                                {
//                                    return "<a href=\"ViewCase.aspx?CaseID=" + CaseID.ToString().Trim() + "\" title=\"" + item.TextValue + "\">" + item.TextValue.Substring(0, 37) + "...</a>";
//                                }
//                                else
//                                {
//                                    return "<a href=\"ViewCase.aspx?CaseID=" + CaseID.ToString().Trim() + "\" title=\"" + item.TextValue + "\">" + item.TextValue + "</a>";
//                                }
//                            }
//                        }
//                    }
//                    return string.Empty;

//                }
//            }
//            public string CaseStatusValue { get; set; }

//            public string CaseLifeDHM { get; set; }
//            public MetaData Column1 { get { if (MetaDataCollection.Count >= 1 && MetaDataCollection[0] != null) return MetaDataCollection[0]; else return new MetaData() { AssociatedDecodeName = String.Empty }; } }
//            public MetaData Column2 { get { if (MetaDataCollection.Count >= 2 && MetaDataCollection[1] != null) return MetaDataCollection[1]; else return new MetaData() { AssociatedDecodeName = String.Empty }; } }
//            public MetaData Column3 { get { if (MetaDataCollection.Count >= 3 && MetaDataCollection[2] != null) return MetaDataCollection[2]; else return new MetaData() { AssociatedDecodeName = String.Empty }; } }
//            public MetaData Column4 { get { if (MetaDataCollection.Count >= 4 && MetaDataCollection[3] != null) return MetaDataCollection[3]; else return new MetaData() { AssociatedDecodeName = String.Empty }; } }
//            public MetaData Column5 { get { if (MetaDataCollection.Count >= 5 && MetaDataCollection[4] != null) return MetaDataCollection[4]; else return new MetaData() { AssociatedDecodeName = String.Empty }; } }
//            public MetaData Column6 { get { if (MetaDataCollection.Count >= 6 && MetaDataCollection[5] != null) return MetaDataCollection[5]; else return new MetaData() { AssociatedDecodeName = String.Empty }; } }
//            public MetaData Column7 { get { if (MetaDataCollection.Count >= 7 && MetaDataCollection[6] != null) return MetaDataCollection[6]; else return new MetaData() { AssociatedDecodeName = String.Empty }; } }
//            public MetaData Column8 { get { if (MetaDataCollection.Count >= 8 && MetaDataCollection[7] != null) return MetaDataCollection[7]; else return new MetaData() { AssociatedDecodeName = String.Empty }; } }
//            public MetaData Column9 { get { if (MetaDataCollection.Count >= 9 && MetaDataCollection[8] != null) return MetaDataCollection[8]; else return new MetaData() { AssociatedDecodeName = String.Empty }; } }
//            public MetaData Column10 { get { if (MetaDataCollection.Count >= 10 && MetaDataCollection[9] != null) return MetaDataCollection[9]; else return new MetaData() { AssociatedDecodeName = String.Empty }; } }

//            public MetaData Column11 { get { if (MetaDataCollection.Count >= 11 && MetaDataCollection[10] != null) return MetaDataCollection[10]; else return new MetaData() { AssociatedDecodeName = String.Empty }; } }
//            public MetaData Column12 { get { if (MetaDataCollection.Count >= 12 && MetaDataCollection[11] != null) return MetaDataCollection[11]; else return new MetaData() { AssociatedDecodeName = String.Empty }; } }
//            public MetaData Column13 { get { if (MetaDataCollection.Count >= 13 && MetaDataCollection[12] != null) return MetaDataCollection[12]; else return new MetaData() { AssociatedDecodeName = String.Empty }; } }
//            public MetaData Column14 { get { if (MetaDataCollection.Count >= 14 && MetaDataCollection[13] != null) return MetaDataCollection[13]; else return new MetaData() { AssociatedDecodeName = String.Empty }; } }
//            public MetaData Column15 { get { if (MetaDataCollection.Count >= 15 && MetaDataCollection[14] != null) return MetaDataCollection[14]; else return new MetaData() { AssociatedDecodeName = String.Empty }; } }
//            public MetaData Column16 { get { if (MetaDataCollection.Count >= 16 && MetaDataCollection[15] != null) return MetaDataCollection[15]; else return new MetaData() { AssociatedDecodeName = String.Empty }; } }
//            public MetaData Column17 { get { if (MetaDataCollection.Count >= 17 && MetaDataCollection[16] != null) return MetaDataCollection[16]; else return new MetaData() { AssociatedDecodeName = String.Empty }; } }
//            public MetaData Column18 { get { if (MetaDataCollection.Count >= 18 && MetaDataCollection[17] != null) return MetaDataCollection[17]; else return new MetaData() { AssociatedDecodeName = String.Empty }; } }
//            public MetaData Column19 { get { if (MetaDataCollection.Count >= 19 && MetaDataCollection[18] != null) return MetaDataCollection[18]; else return new MetaData() { AssociatedDecodeName = String.Empty }; } }
//            public MetaData Column20 { get { if (MetaDataCollection.Count >= 20 && MetaDataCollection[19] != null) return MetaDataCollection[19]; else return new MetaData() { AssociatedDecodeName = String.Empty }; } }

//            public int CaseLife { get; set; }

//            public string CaseLifeString
//            {
//                get
//                {
//                    TimeSpan t = TimeSpan.FromMinutes(CaseLife);

//                    string response = string.Format("{0:D2} : {1:D2} : {2:D2}",
//                                            t.Days,
//                                            t.Hours,
//                                            t.Minutes);


//                    return response;
//                }
//            }

//            public MetaData SystemStatus { get; set; }

//            public CaseData()
//            {
//                this.NoteDataCollection = new List<NoteData>();
//                this.MetaDataCollection = new List<MetaData>();
//            }

//            public bool CaseTypeHasSecurity { get; set; }

//            public List<string> CaseTypeSecurityGroups { get; set; }

//            public string SceurityType { get; set; }

//            public string CaseStatus
//            {
//                get
//                {
//                    if (!string.IsNullOrEmpty(this.CaseStatusValue))
//                    {
//                        return $"{"Status : "}{CaseStatusValue}";
//                    }
//                    else
//                    {
//                        return string.Empty;
//                    }

//                }
//            }
//            public string CasePriorityValue { get; set; }
//            public string PriorityValue
//            {
//                get
//                {
//                    if (!string.IsNullOrEmpty(this.CasePriorityValue))
//                    {
//                        return $"{"Priority : "}{CasePriorityValue}";
//                    }
//                    else
//                    {
//                        return string.Empty;
//                    }
//                }
//            }

//            public string AssignedTo
//            {
//                get
//                {
//                    if (!string.IsNullOrEmpty(this.CaseAssignedToDisplayName))
//                    {
//                        return $"{"Assigned To : "}{CaseAssignedToDisplayName}";
//                    }
//                    else
//                    {
//                        return string.Empty;
//                    }
//                }
//            }
//            public string CaseDueDateTimeDateOnly
//            {
//                get { return GetDateOnly(strCaseDue); }
//            }
//            public string DueDate
//            {
//                get
//                {
//                    if (!string.IsNullOrEmpty(this.CaseDueDateTimeDateOnly))
//                    {
//                        return $"{"DueDate : "}{CaseDueDateTimeDateOnly}";
//                    }
//                    else
//                    {
//                        return string.Empty;
//                    }
//                }
//            }

//            public string CreateBy
//            {
//                get
//                {
//                    if (!string.IsNullOrEmpty(this.CaseCreatedDisplayName))
//                    {
//                        return $"{"Created By : "}{CaseCreatedDisplayName}";
//                    }
//                    else
//                    {
//                        return string.Empty;
//                    }


//                }
//            }
//            public string CaseCreatedDateTime { get; set; }
//            public string CaseCreatedDateTimeDateOnly
//            {
//                get { return GetDateOnly(CaseCreatedDateTime); }
//            }
//            public string CreatedOn
//            {
//                get
//                {
//                    if (!string.IsNullOrEmpty(this.CaseCreatedDateTimeDateOnly))
//                    {
//                        return $"{"Created On : "}{CaseCreatedDateTimeDateOnly}";
//                    }
//                    else
//                    {
//                        return string.Empty;
//                    }

//                }
//            }

//            public string ModifiedBy
//            {
//                get
//                {
//                    if (!string.IsNullOrEmpty(this.CaseModifiedByDisplayName))
//                    {
//                        return $"{"Modified By : "}{CaseModifiedByDisplayName}";
//                    }
//                    else
//                    {
//                        return string.Empty;
//                    }


//                }
//            }
//            public string CaseModifiedDateTime { get; set; }
//            public string CaseModifiedDateTimeDateOnly
//            {
//                get
//                {
//                    return GetDateOnly(CaseModifiedDateTime);
//                }
//            }


//            public string ModifiedOn
//            {
//                get
//                {
//                    if (!string.IsNullOrEmpty(this.CaseModifiedDateTimeDateOnly))
//                    {
//                        return $"{"Modified On : "}{CaseModifiedDateTimeDateOnly}";
//                    }
//                    else
//                    {
//                        return string.Empty;
//                    }
//                }
//            }

//            private string GetDateOnly(string value)
//            {
//                try
//                {
//                    if (!string.IsNullOrEmpty(value) && value.Length > 5)
//                    {
//                        int spaceIsAt = value.IndexOf(' ');
//                        if (spaceIsAt > 0)
//                        {
//                            return value.Substring(0, spaceIsAt);
//                        }
//                    }
//                }
//                catch
//                {
//                }
//                return string.Empty;
//            }

//        }
//        #endregion
//    }
//}


using System;
using System.Collections.Generic;

namespace StemmonsMobile.DataTypes.DataType.Cases
{
    public partial class GetCaseTypesResponse
    {
        //#region ParseEmailField
        //public class ParseEmailField
        //{
        //    /// <summary>
        //    /// ID of the <see cref="CaseType"/> to which the EmailAddress belongs.
        //    /// </summary>
        //    public int? CaseTypeID { get; set; }


        //    /// <summary>
        //    /// The name, or text value.
        //    /// </summary>

        //    public string Name { get; set; }

        //    /// <summary>
        //    /// The EmailAddress, or text value.
        //    /// </summary>
        //    public string EmailAddress { get; set; }


        //    /// <summary>
        //    /// The EmailAddress, or text value.
        //    /// </summary>
        //    public string Keyword { get; set; }


        //    /// <summary>
        //    /// The EmailAddress, or text value.
        //    /// </summary>
        //    public string Syscode { get; set; }

        //    /// <summary>
        //    /// The Field name, or text value.
        //    /// </summary>
        //    public string CaseTypeDesc { get; set; }

        //    /// <summary>
        //    /// Text value of DecodeID.
        //    /// </summary>
        //    public string Text { get; set; }

        //    /// <summary>
        //    /// ID of the value object.
        //    /// </summary>
        //    public int? AssocTypeID { get; set; }

        //    /// <summary>
        //    /// ID of the value object.
        //    /// </summary>
        //    public string DefaultValue { get; set; }

        //    /// <summary>
        //    /// SYSTEM CODE for DecodeID.
        //    /// </summary>
        //    public string AssocDecodeStatusCode { get; set; }

        //    public ParseEmailField()
        //    {
        //        CaseTypeDesc = string.Empty;
        //        AssocTypeID = -1;
        //        DefaultValue = string.Empty;
        //        Syscode = string.Empty;
        //        Text = string.Empty;

        //        AssocDecodeStatusCode = string.Empty;
        //        CaseTypeID = -1;
        //    }
        //}
        //#endregion

        //#region ItemValue
        ///// <summary>
        ///// The value object of an item in a case field with predefined options, such as in a drop down list.
        ///// <para>Defined as ASSOC_DECODE in the database.</para>
        ///// </summary>
        //[Serializable]
        //public class ItemValue
        //{
        //    [Obsolete("Not implemented yet", true)]
        //    public void AddNewItem()
        //    {
        //    }

        //    /// <summary>
        //    /// ID of the value object.
        //    /// </summary>
        //    public int AssocDecodeID { get; set; }

        //    /// <summary>
        //    /// ID of the <see cref="ItemType"/> (ASSOC_TYPE in the database) to which this <see cref="ItemValue"/> belongs.
        //    /// </summary>
        //    public int AssocTypeID { get; set; }

        //    /// <summary>
        //    /// ID of the <see cref="CaseType"/> to which the <see cref="ItemValue"/>'s <see cref="ItemType"/> belongs.
        //    /// <para>This parameter is NOT defined in database.</para>
        //    /// </summary>
        //    public int CaseTypeID { get; set; }

        //    /// <summary>
        //    /// Description (not name? Or does it depend?) of the <see cref="CaseType"/> to which the <see cref="ItemValue"/>'s <see cref="ItemType"/> belongs.
        //    /// <para>This property is NOT defined in database.</para>
        //    /// </summary>
        //    public string CaseTypeDesc { get; set; }

        //    public int? ObjectID { get; set; }

        //    /// <summary>
        //    /// ID value of the <see cref="ItemValue"/> when <see cref="ItemValue"/> is used in an unbound manner.
        //    /// <para>Prepend with "P" when the source is External Data.</para>
        //    /// <para>This property is NOT defined in database.</para>
        //    /// </summary>
        //    /// <remarks>
        //    /// <see cref="ObjectCode"/> is used to prepare dropdown selections to be saved to the database.
        //    /// </remarks>
        //    public string ObjectCode { get; set; }

        //    /// <summary>
        //    /// The name, or text value, of the <see cref="ItemValue"/>.
        //    /// </summary>
        //    /// <remarks>
        //    /// When <see cref="ItemValue"/> is used with Property Bags, <see cref="Name"/> is the case field name, and <see cref="TextValue"/> is the selected value (text) from a drop down list.
        //    /// <para>Although <see cref="Name"/> is correct, <see cref="TextValue"/> may make more sense.</para>
        //    /// </remarks>
        //    public string Name { get; set; }

        //    /// <summary>
        //    /// Arbitrary text value for this <see cref="ItemValue"/>.
        //    /// <para>This property is NOT defined in database.</para>
        //    /// </summary>
        //    /// <remarks>
        //    /// When <see cref="ItemValue"/> is used with Property Bags, <see cref="Name"/> is the case field name, and <see cref="TextValue"/> is the selected value (text) from a drop down list.
        //    /// </remarks>
        //    [Obsolete("Need to change the references Property Bags use.")]
        //    public string TextValue { get; set; }

        //    /// <summary>
        //    /// The description of the <see cref="ItemValue"/>.
        //    /// </summary>
        //    public string Description { get; set; }

        //    public string GLCode { get; set; }

        //    public string MiscCode1Desc { get; set; }

        //    public string MiscCode1Value { get; set; }

        //    public string MiscCode2Desc { get; set; }

        //    public string MiscCode2Value { get; set; }

        //    /// <summary>
        //    /// The system code of the <see cref="ItemValue"/>.
        //    /// </summary>
        //    public string SystemCode { get; set; }

        //    /// <summary>
        //    /// The system priority, or sort position, of the <see cref="ItemValue"/>.
        //    /// </summary>
        //    public int? SystemPriority { get; set; }

        //    /// <summary>
        //    /// Whether or not the <see cref="ItemValue"/> is active.
        //    /// </summary>
        //    public char IsActive { get; set; }

        //    /// <summary>
        //    /// Whether or not the <see cref="ItemValue"/> affects MTTR.
        //    /// <para>This property is NOT defined in database.</para>
        //    /// </summary>
        //    public char? StatusMTTRAffecting { get; set; }

        //    /// <summary>
        //    /// When the <see cref="ItemValue"/> was created.
        //    /// </summary>
        //    public DateTime CreatedDateTime { get; set; }

        //    /// <summary>
        //    /// Who created the <see cref="ItemValue"/>.
        //    /// </summary>
        //    public string CreatedBy { get; set; }

        //    /// <summary>
        //    /// When the <see cref="ItemValue"/> was last modified.
        //    /// </summary>
        //    public DateTime ModifiedDateTime { get; set; }

        //    /// <summary>
        //    /// Who last modified the <see cref="ItemValue"/>.
        //    /// </summary>
        //    public string ModifiedBy { get; set; }

        //    public string ItemPropertyBag
        //    {
        //        get
        //        {
        //            string required = "Y";
        //            if (!IsRequired)
        //                required = "N";

        //            // 1. AssocTypeID
        //            // 2. AssocDecodeID
        //            // 3. Required (Y/N)
        //            // 4. CaseTypeDesc
        //            // 5. Child AssocDecodeID
        //            // 6. Parent AssocDecodeID
        //            // 7. CaseTypeID
        //            // 8. AssocDecodeStatusCode

        //            return AssocTypeID.ToString() + "|" + AssocDecodeID.ToString() + "|" + required + "|" + CaseTypeDesc.Replace('|', ' ').Replace(',', ' ') + "|" + Child + "|" + Parent + "|" + CaseTypeID + "|" + SystemCode;
        //        }
        //    }

        //    /// <summary>
        //    /// Whether or not the <see cref="ItemValue"/>'s <see cref="ItemType"/> (?) is required.
        //    /// <para>This property is NOT defined in database.</para>
        //    /// </summary>
        //    public bool IsRequired { get; set; }

        //    /// <summary>
        //    /// The child of the <see cref="ItemValue"/> (?).
        //    /// <para>This property is NOT defined in database.</para>
        //    /// </summary>
        //    public int Child { get; set; }

        //    /// <summary>
        //    /// The parent of the <see cref="ItemValue"/> (?).
        //    /// <para>This property is NOT defined in database.</para>
        //    /// </summary>
        //    public int Parent { get; set; }

        //    /// <summary>
        //    /// Creates a new <see cref="ItemValue"/> with some default values.
        //    /// </summary>
        //    public ItemValue()
        //    {
        //        CaseTypeDesc = string.Empty;
        //        AssocTypeID = -1;
        //        AssocDecodeID = -1;
        //        IsRequired = true;
        //        Child = -1;
        //        Parent = -1;
        //        CaseTypeID = -1;
        //    }

        //    public override string ToString()
        //    {
        //        return this.Name;
        //    }
        //}
        //#endregion

        //#region Enum ActivityLogType
        //public enum ActivityLogType
        //{
        //    CreatedCase,
        //    ViewingCase,
        //    AddedNotes,
        //    UpdatingCase,
        //    StatusChange,
        //    PriorityChange,
        //    ErrorVisibilityChange,
        //    DispatchedCase,
        //    TakeOwnership,
        //    Assigned,
        //    FileAttachment,
        //    DownloadedFile,
        //    ViewingActivityLog,
        //    LocalFolderChange,
        //    ProjectChange,
        //    SeverityChange,
        //    CaseTitleChanged,
        //    ClosedCase,
        //    ReOpenedCase,
        //    ExportedCasePDF,
        //    DueDate

        //    //ApprovedCase,
        //    //RejectedCase
        //}
        //#endregion

        #region CaseData
        /// <summary>
        /// A case data object with strongly typed properties.
        /// <para>If populating a list, such as a GridView, use <see cref="BasicCase"/> instead.</para>
        /// </summary>
        // [Serializable]
        public class CaseData
        {
            public int CaseID { get; set; }
            public int? ListID { get; set; }
            public int CaseTypeID { get; set; }
            public string CaseOwner { get; set; }
            public DateTime? CaseOwnerDateTime { get; set; }
            public string CaseAssignedTo { get; set; }
            public DateTime? CaseAssignedDateTime { get; set; }
            public int? CaseHopperID { get; set; }
            public DateTime? CaseHopperDateTime { get; set; }
            public DateTime? CaseClosedDateTime { get; set; }
            public string CaseClosedBy { get; set; }
            public DateTime CreateDateTime { get; set; }
            public string CreateBySam { get; set; }
            public string CaseCreatedDisplayName { get; set; }
            public DateTime ModifiedDateTime { get; set; }
            public string ModifiedBySam { get; set; }
            public string CaseModifiedByDisplayName { get; set; }
            public char NewestNoteOnTop { get; set; }
            public bool LockWhenUnowned { get; set; }

            public string CaseTypeName { get; set; }
            public string CaseOwnerDisplayName { get; set; }
            public string CreateByDisplayName { get; set; }
            public string CaseAssignedToDisplayName { get; set; }
            public string CaseClosedByDisplayName { get; set; }
            public string ModifiedByDisplayName { get; set; }

            public List<MetaData> MetaDataCollection { get; set; }
            public List<NoteData> NoteDataCollection { get; set; }

            public char? IS_ALLOW_APPROVAL { get; set; }

            public bool DebugMode { get; set; }
            public string strCaseDue { get; set; }
            public DateTime? CaseDue { get; set; }
            public bool IsClosed { get; set; }
            public string CaseTitle { get; set; }
            public int? SystemPriority { get; set; }
            //public string CaseTitle
            //{
            //    get
            //    {
            //        foreach (MetaData item in this.MetaDataCollection)
            //        {
            //            if (item.AssocTypeSystemCode != null)
            //            {
            //                if (item.AssocTypeSystemCode.ToUpper().Trim() == "TITLE")
            //                {
            //                    // TODO Why is this alternating?
            //                    //return item.TextValue;
            //                    //return item.MetaDataText;

            //                    //return !string.IsNullOrEmpty(item.TextValue) ? item.TextValue : item.MetaDataText;
            //                    return item.TextValue;
            //                }
            //            }
            //        }
            //        return "NO TITLE PROVIDED";

            //    }
            //}

            public string CaseTitleLink
            {
                get
                {
                    foreach (MetaData item in this.MetaDataCollection)
                    {
                        if (item.AssocTypeSystemCode != null)
                        {
                            if (item.AssocTypeSystemCode.ToUpper().Trim() == "TITLE")
                            {
                                string debug = string.Empty;

                                if (DebugMode)
                                    debug = "&Debug=Y";

                                if (item.TextValue != null && item.TextValue.Length > 40)
                                {

                                    return "<a href=\"ViewCase.aspx?CaseID=" + CaseID.ToString().Trim() + debug + "\" title=\"" + item.TextValue + "\">" + item.TextValue.Substring(0, 37) + "...</a>";
                                }
                                else
                                {
                                    return "<a href=\"ViewCase.aspx?CaseID=" + CaseID.ToString().Trim() + debug + "\" title=\"" + item.TextValue + "\">" + item.TextValue + "</a>";
                                }
                            }
                        }
                    }
                    return string.Empty;
                }
            }

            public string CaseModalPopupLink
            {
                get
                {
                    foreach (MetaData item in this.MetaDataCollection)
                    {
                        if (item.AssocTypeSystemCode != null)
                        {
                            if (item.AssocTypeSystemCode.ToUpper().Trim() == "TITLE")
                            {
                                if (item.TextValue != null && item.TextValue.Length > 40)
                                {
                                    return "<a href=\"ViewCase.aspx?CaseID=" + CaseID.ToString().Trim() + "\" title=\"" + item.TextValue + "\">" + item.TextValue.Substring(0, 37) + "...</a>";
                                }
                                else
                                {
                                    return "<a href=\"ViewCase.aspx?CaseID=" + CaseID.ToString().Trim() + "\" title=\"" + item.TextValue + "\">" + item.TextValue + "</a>";
                                }
                            }
                        }
                    }
                    return string.Empty;

                }
            }
            public string CaseStatusValue { get; set; }

            public string CaseLifeDHM { get; set; }
            public MetaData Column1 { get { if (MetaDataCollection.Count >= 1 && MetaDataCollection[0] != null) return MetaDataCollection[0]; else return new MetaData() { AssociatedDecodeName = String.Empty }; } }
            public MetaData Column2 { get { if (MetaDataCollection.Count >= 2 && MetaDataCollection[1] != null) return MetaDataCollection[1]; else return new MetaData() { AssociatedDecodeName = String.Empty }; } }
            public MetaData Column3 { get { if (MetaDataCollection.Count >= 3 && MetaDataCollection[2] != null) return MetaDataCollection[2]; else return new MetaData() { AssociatedDecodeName = String.Empty }; } }
            public MetaData Column4 { get { if (MetaDataCollection.Count >= 4 && MetaDataCollection[3] != null) return MetaDataCollection[3]; else return new MetaData() { AssociatedDecodeName = String.Empty }; } }
            public MetaData Column5 { get { if (MetaDataCollection.Count >= 5 && MetaDataCollection[4] != null) return MetaDataCollection[4]; else return new MetaData() { AssociatedDecodeName = String.Empty }; } }
            public MetaData Column6 { get { if (MetaDataCollection.Count >= 6 && MetaDataCollection[5] != null) return MetaDataCollection[5]; else return new MetaData() { AssociatedDecodeName = String.Empty }; } }
            public MetaData Column7 { get { if (MetaDataCollection.Count >= 7 && MetaDataCollection[6] != null) return MetaDataCollection[6]; else return new MetaData() { AssociatedDecodeName = String.Empty }; } }
            public MetaData Column8 { get { if (MetaDataCollection.Count >= 8 && MetaDataCollection[7] != null) return MetaDataCollection[7]; else return new MetaData() { AssociatedDecodeName = String.Empty }; } }
            public MetaData Column9 { get { if (MetaDataCollection.Count >= 9 && MetaDataCollection[8] != null) return MetaDataCollection[8]; else return new MetaData() { AssociatedDecodeName = String.Empty }; } }
            public MetaData Column10 { get { if (MetaDataCollection.Count >= 10 && MetaDataCollection[9] != null) return MetaDataCollection[9]; else return new MetaData() { AssociatedDecodeName = String.Empty }; } }

            public MetaData Column11 { get { if (MetaDataCollection.Count >= 11 && MetaDataCollection[10] != null) return MetaDataCollection[10]; else return new MetaData() { AssociatedDecodeName = String.Empty }; } }
            public MetaData Column12 { get { if (MetaDataCollection.Count >= 12 && MetaDataCollection[11] != null) return MetaDataCollection[11]; else return new MetaData() { AssociatedDecodeName = String.Empty }; } }
            public MetaData Column13 { get { if (MetaDataCollection.Count >= 13 && MetaDataCollection[12] != null) return MetaDataCollection[12]; else return new MetaData() { AssociatedDecodeName = String.Empty }; } }
            public MetaData Column14 { get { if (MetaDataCollection.Count >= 14 && MetaDataCollection[13] != null) return MetaDataCollection[13]; else return new MetaData() { AssociatedDecodeName = String.Empty }; } }
            public MetaData Column15 { get { if (MetaDataCollection.Count >= 15 && MetaDataCollection[14] != null) return MetaDataCollection[14]; else return new MetaData() { AssociatedDecodeName = String.Empty }; } }
            public MetaData Column16 { get { if (MetaDataCollection.Count >= 16 && MetaDataCollection[15] != null) return MetaDataCollection[15]; else return new MetaData() { AssociatedDecodeName = String.Empty }; } }
            public MetaData Column17 { get { if (MetaDataCollection.Count >= 17 && MetaDataCollection[16] != null) return MetaDataCollection[16]; else return new MetaData() { AssociatedDecodeName = String.Empty }; } }
            public MetaData Column18 { get { if (MetaDataCollection.Count >= 18 && MetaDataCollection[17] != null) return MetaDataCollection[17]; else return new MetaData() { AssociatedDecodeName = String.Empty }; } }
            public MetaData Column19 { get { if (MetaDataCollection.Count >= 19 && MetaDataCollection[18] != null) return MetaDataCollection[18]; else return new MetaData() { AssociatedDecodeName = String.Empty }; } }
            public MetaData Column20 { get { if (MetaDataCollection.Count >= 20 && MetaDataCollection[19] != null) return MetaDataCollection[19]; else return new MetaData() { AssociatedDecodeName = String.Empty }; } }

            public int CaseLife { get; set; }

            public string CaseLifeString
            {
                get
                {
                    TimeSpan t = TimeSpan.FromMinutes(CaseLife);

                    string response = string.Format("{0:D2} : {1:D2} : {2:D2}",
                                            t.Days,
                                            t.Hours,
                                            t.Minutes);


                    return response;
                }
            }

            public MetaData SystemStatus { get; set; }

            public CaseData()
            {
                this.NoteDataCollection = new List<NoteData>();
                this.MetaDataCollection = new List<MetaData>();
            }

            public bool CaseTypeHasSecurity { get; set; }

            public List<string> CaseTypeSecurityGroups { get; set; }

            public string SceurityType { get; set; }

            public string CaseStatus
            {
                get
                {
                    if (!string.IsNullOrEmpty(this.CaseStatusValue))
                    {
                        return $"{"Status : "}{CaseStatusValue}";
                    }
                    else
                    {
                        return string.Empty;
                    }

                }
            }
            public string CasePriorityValue { get; set; }
            public string PriorityValue
            {
                get
                {
                    if (!string.IsNullOrEmpty(this.CasePriorityValue))
                    {
                        return $"{"Priority : "}{CasePriorityValue}";
                    }
                    else
                    {
                        return string.Empty;
                    }
                }
            }

            public string AssignedTo
            {
                get
                {
                    if (!string.IsNullOrEmpty(this.CaseAssignedToDisplayName))
                    {
                        return $"{"Assigned To : "}{CaseAssignedToDisplayName}";
                    }
                    else
                    {
                        return string.Empty;
                    }
                }
            }
            public string CaseDueDateTimeDateOnly
            {
                get { return GetDateOnly(strCaseDue); }
            }
            public string DueDate
            {
                get
                {
                    if (!string.IsNullOrEmpty(this.CaseDueDateTimeDateOnly))
                    {
                        return $"{"DueDate : "}{CaseDueDateTimeDateOnly}";
                    }
                    else
                    {
                        return string.Empty;
                    }
                }
            }

            public string CreateBy
            {
                get
                {
                    if (!string.IsNullOrEmpty(this.CaseCreatedDisplayName))
                    {
                        return $"{"Created By : "}{CaseCreatedDisplayName}";
                    }
                    else
                    {
                        return string.Empty;
                    }


                }
            }
            public string CaseCreatedDateTime { get; set; }
            public string CaseCreatedDateTimeDateOnly
            {
                get { return GetDateOnly(CaseCreatedDateTime); }
            }
            public string CreatedOn
            {
                get
                {
                    if (!string.IsNullOrEmpty(this.CaseCreatedDateTimeDateOnly))
                    {
                        return $"{"Created On : "}{CaseCreatedDateTimeDateOnly}";
                    }
                    else
                    {
                        return string.Empty;
                    }

                }
            }

            public string ModifiedBy
            {
                get
                {
                    if (!string.IsNullOrEmpty(this.CaseModifiedByDisplayName))
                    {
                        return $"{"Modified By : "}{CaseModifiedByDisplayName}";
                    }
                    else
                    {
                        return string.Empty;
                    }


                }
            }
            public string CaseModifiedDateTime { get; set; }
            public string CaseModifiedDateTimeDateOnly
            {
                get
                {
                    return GetDateOnly(CaseModifiedDateTime);
                }
            }


            public string ModifiedOn
            {
                get
                {
                    if (!string.IsNullOrEmpty(this.CaseModifiedDateTimeDateOnly))
                    {
                        return $"{"Modified On : "}{CaseModifiedDateTimeDateOnly}";
                    }
                    else
                    {
                        return string.Empty;
                    }
                }
            }

            private string GetDateOnly(string value)
            {
                try
                {
                    if (!string.IsNullOrEmpty(value) && value.Length > 5)
                    {
                        int spaceIsAt = value.IndexOf(' ');
                        if (spaceIsAt > 0)
                        {
                            return value.Substring(0, spaceIsAt);
                        }
                    }
                }
                catch
                {
                }
                return string.Empty;
            }

        }
        #endregion
    }
}