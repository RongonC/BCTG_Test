using System;
using System.Collections.Generic;
using System.Linq;

namespace StemmonsMobile.DataTypes.DataType.Cases
{
    public class GetCaseTypesResponse : Response
    {
        public object ResponseContent { get; set; }

        #region CaseType
        public class CaseType
        {
            /// <summary>
            /// ID of the <see cref="CaseType"/>.
            /// </summary>
            public int CaseTypeID { get; set; }

            /// <summary>
            /// The name of the <see cref="CaseType"/>.
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// The name of an item in the <see cref="CaseType"/>.
            /// </summary>
            public string InstanceName { get; set; }

            /// <summary>
            /// The plural name of items in the <see cref="CaseType"/>.
            /// </summary>
            public string InstanceNamePlural { get; set; }

            /// <summary>
            /// The description of the <see cref="CaseType"/>.
            /// </summary>
            public string Description { get; set; }

            /// <summary>
            /// URL of the custom page in SharePoint for the <see cref="CaseType"/>'s list view.
            /// </summary>
            /// <remarks>
            /// In SharePoint, items for most case types are presented using a common list view page.  However, some
            /// <see cref="CaseType"/>s require custom list views to satisfy user requirements.
            /// </remarks>
            public string SPOriginationOrverrideUrl { get; set; }

            /// <summary>
            /// Whether or not the <see cref="CaseType"/> is active.
            /// </summary>
            public bool IsActive { get; set; }

            /// <summary>
            /// When the <see cref="CaseType"/> was created.
            /// </summary>
            public DateTime CreatedDateTime { get; set; }

            /// <summary>
            /// Who created the <see cref="CaseType"/>.
            /// </summary>
            public string CreatedBy { get; set; }

            /// <summary>
            /// When the <see cref="CaseType"/> was last modified.
            /// </summary>
            public DateTime ModifiedDateTime { get; set; }

            /// <summary>
            /// Who last modified the <see cref="CaseType"/>.
            /// </summary>
            public string ModifiedBy { get; set; }

            /// <summary>
            /// Is there security on this?
            /// </summary>
            public bool HasSecurity { get; set; }

            /// <summary>
            /// What are the security groups involved
            /// </summary>
            public List<string> SecurityGroups { get; set; }

            /// <summary>
            /// Whether or not to lock controls when viewing a case the user does not own.
            /// </summary>
            public char LockWhenUnowned { get; set; }

            /// <summary>
            /// Creates a new <see cref="CaseType"/>.
            /// </summary>
            public CaseType()
            { }

            /// <summary>
            /// Creates a new <see cref="CaseType"/>.
            /// </summary>
            /// <param name="casetypeID">ID of the <see cref="CaseType"/>.</param>
            /// <param name="name">The name of the <see cref="ItemType"/>.</param>
            public CaseType(int casetypeID, string name)
            {
                CaseTypeID = casetypeID;
                Name = name;
            }


            public string SceurityType { get; set; }
            public bool HasRightsToConfigSecurity { get; set; }

        }
        #endregion

        #region BasicCase
        public class BasicCase
        {
            private string _caseTitle;

            public int rowID { get; set; }
            public int CaseID { get; set; }
            public int ListID { get; set; }
            public int CaseTypeID { get; set; }
            public string CaseTypeName { get; set; }
            public string CaseOwnerDateTime { get; set; }
            public string CaseOwnerDateTimeDateOnly
            {
                get { return GetDateOnly(CaseOwnerDateTime); }
            }
            public string CaseOwnerSAM { get; set; }
            public string CaseOwnerDisplayName { get; set; }
            public string CaseAssignDateTime { get; set; }
            public string CaseAssignDateTimeDateOnly
            {
                get { return GetDateOnly(CaseAssignDateTime); }
            }
            public string CaseAssignedToSAM { get; set; }
            public string CaseAssignedToDisplayName { get; set; }
            public string CaseClosedDateTime { get; set; }
            public string CaseClosedDateTimeDateOnly
            {
                get { return GetDateOnly(CaseClosedDateTime); }
            }
            public string CaseClosedBySAM { get; set; }
            public string CaseClosedByDisplayName { get; set; }
            public string CaseCreatedDateTime { get; set; }
            public string CaseCreatedDateTimeDateOnly
            {
                get { return GetDateOnly(CaseCreatedDateTime); }
            }
            public string CaseCreatedSAM { get; set; }
            public string CaseCreatedDisplayName { get; set; }
            public string CaseModifiedDateTime { get; set; }
            public string CaseModifiedDateTimeDateOnly
            {
                get { return GetDateOnly(CaseModifiedDateTime); }
            }

            private string GetDateOnly(string value)
            {
                try
                {
                    if (value.Length > 5)
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

            public string CaseTitleLink
            {
                get
                {
                    string debug = string.Empty;

                    if (DebugMode) debug = "&Debug=Y";

                    if (CaseTitle != null && CaseTitle.Length > 40)
                    {
                        return "<a href=\"ViewCase.aspx?CaseID=" + CaseID.ToString().Trim() + debug + AdditionalTokenQueryString + "\" title=\"" + CaseTitle + "\">" + CaseTitle.Substring(0, 37) + "...</a>";
                    }
                    else
                    {
                        return "<a href=\"ViewCase.aspx?CaseID=" + CaseID.ToString().Trim() + debug + AdditionalTokenQueryString + "\" title=\"" + CaseTitle + "\">" + CaseTitle + "</a>";
                    }
                }
            }

            public string CaseMobileSummeryLink
            {
                get
                {
                    // Case Title
                    // PropertyWBR
                    // Type | Status
                    // Priority | Life

                    string debug = string.Empty;

                    if (DebugMode) debug = "&Debug=Y";
                    if (!string.IsNullOrEmpty(this.CaseDisplayFormat))
                    {
                        if (CaseTitle != null && CaseTitle.Length > 50)
                        {
                            string response = this.CaseDisplayFormat;
                            response = response.Replace("{CaseTitleLink}", "<a href=\"ViewCase.aspx?CaseID=" + CaseID.ToString().Trim() + debug + AdditionalTokenQueryString + "\" title=\"" + CaseTitle + "\">" + CaseTitle.Substring(0, 49) + "...");
                            response = response.Replace("{CaseTitle}", CaseTitle.Substring(0, 49) + "...");
                            response = response.Replace("{CaseDisplayID}", CaseTypeID.ToString() + "-" + ListID.ToString());
                            //response = response.Replace("{CaseTitle}", this.CaseTitle);
                            response = response.Replace("{CaseTypeName}", this.CaseTypeName);
                            response = response.Replace("{CaseStatusValue}", this.CaseStatusValue);
                            response = response.Replace("{CasePriorityValue}", this.CasePriorityValue);
                            response = response.Replace("{CaseLifeDHM}", this.CaseLifeDHM);

                            if (this.PropertyName != null)
                            {
                                response = response.Replace("{PropertyWBR}", this.PropertyName + "\r\n");
                            }
                            else
                            {
                                response = response.Replace("{PropertyWBR}", "");
                            }

                            return response;
                        }
                        else
                        {
                            string response = this.CaseDisplayFormat;
                            response = response.Replace("{CaseTitleLink}", "<a href=\"ViewCase.aspx?CaseID=" + CaseID.ToString().Trim() + debug + AdditionalTokenQueryString + "\" title=\"" + CaseTitle + "\">" + CaseTitle + "</a>");
                            response = response.Replace("{CaseTitle}", CaseTitle);
                            response = response.Replace("{CaseDisplayID}", CaseTypeID.ToString() + "-" + ListID.ToString());
                            //response = response.Replace("{CaseTitle}", this.CaseTitle);
                            response = response.Replace("{CaseTypeName}", this.CaseTypeName);
                            response = response.Replace("{CaseStatusValue}", this.CaseStatusValue);
                            response = response.Replace("{CasePriorityValue}", this.CasePriorityValue);
                            response = response.Replace("{CaseLifeDHM}", this.CaseLifeDHM);

                            if (this.PropertyName != null)
                            {
                                response = response.Replace("{PropertyWBR}", this.PropertyName + "\r\n");
                            }
                            else
                            {
                                response = response.Replace("{PropertyWBR}", "");
                            }

                            return response;
                        }
                    }
                    return string.Empty;
                }
            }

            public string AdditionalTokenQueryString { get; set; }

            public string CaseModifiedBySAM { get; set; }
            public string CaseModifiedByDisplayName { get; set; }
            public string CaseLifeDHM { get; set; }
            public string CaseTitle
            {
                get
                {
                    if (!string.IsNullOrEmpty(this._caseTitle))
                    {
                        return _caseTitle;
                    }
                    else
                    {
                        return "NO TITLE PROVIDED";
                    }
                }

                set => _caseTitle = value;
            }
            public string CaseStatusSystemName { get; set; }
            public string CaseStatusValue { get; set; }
            public string CaseStatusSystemCode { get; set; }
            public string CasePriorityValue { get; set; }
            public string CasePrioritySystemCode { get; set; }
            public string CaseCost { get; set; }
            public DateTime? CaseDue { get; set; }
            public string CategoryName { get; set; }
            public string CategorySystemCode { get; set; }
            public string PropertyName { get; set; }
            public string TenantName { get; set; }
            public string RegionName { get; set; }
            public string MarketName { get; set; }
            public string SubMarketName { get; set; }
            public string PropertyID { get; set; }
            public string TenantCode { get; set; }
            public int TenantID { get; set; }
            public string DepartmentName { get; set; }
            public string CaseTypeInstanceName { get; set; }
            public string CaseTypeInstanceNamePlural { get; set; }


            public bool DebugMode { get; set; }

            public List<MetaData> MetaDataCollection { get; set; }

            public BasicCase()
            {
                this.MetaDataCollection = new List<MetaData>();
            }

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


            public string CaseTitleModal { get; set; }

            // Will display ListID as {CaseTypeID}-{ListID} instead of {ListID} Jay Patele (10/31/2012)
            public string DisplayListID
            {
                get
                {
                    return CaseTypeID.ToString() + "-" + ListID.ToString();
                }
            }

            public string PropertyLink
            {
                get
                {
                    try
                    {
                        if (PropertyName.Length > 0)
                        {
                            if (PropertyURL.Length > 5)
                            {
                                return "<a href=\"" + PropertyURL + "\" target=\"_blank\">" + PropertyName + "</a>";
                            }
                            else
                            {
                                return PropertyName;
                            }
                        }
                    }
                    catch { }
                    return string.Empty;
                }
            }

            public string PropertyLinkImage
            {
                get
                {
                    try
                    {
                        if (PropertyName.Length > 0)
                        {
                            if (PropertyURL.Length > 5)
                            {
                                return "<a href=\"" + PropertyURL + "\" target=\"_blank\"><img title=\"Open Property Page\" border=\"0\" src=\"/_layouts/15/Images/BP.CME.SharePoint/external_link_icon.gif\"></a>";
                            }
                            else
                            {
                                return PropertyName;
                            }
                        }
                    }
                    catch { }
                    return string.Empty;
                }
            }

            public string PropertyURL { get; set; }

            public string TenantURL { get; set; }

            public string RegionURL { get; set; }

            public string MarketURL { get; set; }

            public string SubMarketURL { get; set; }

            public int DaysOverDue { get; set; }

            public string ThresholdDays { get; set; }

            public string CssStyel { get; set; }

            public string SortOnTop { get; set; }


            public string CaseDisplayFormat { get; set; }

            public string SecurityType { get; set; }

            public string PriorityValue
            {
                get
                {
                    if (CasePriorityValue != "")
                    {
                        return $"{"Priority:"}{CasePriorityValue}";
                    }
                    else
                        return $"{"Priority:0"}";

                }
            }

            public string AssignedTo
            {
                get
                {
                    return $"{"Assigned To:"}{CaseAssignedToDisplayName}";
                }
            }

            public string CaseStatus
            {
                get
                {
                    return $"{"Status:"}{CaseStatusSystemName}";
                }
            }
        }
        #endregion

        #region MetaData
        // [Serializable]
        public class MetaData
        {
            /// <summary>
            /// ID of the <see cref="MetaData"/>.
            /// </summary>
            /// <remarks>
            /// <see cref="CaseAssociatedMetaDataID"/> is defined in CASE_ASSOC_METADATA.
            /// </remarks>
            public int? CaseAssociatedMetaDataID { get; set; }

            /// <summary>
            /// ID of the <see cref="MetaData"/> if its <see cref="ItemType"/>'s AssocFieldType is T (Text), N (Number), or A (Date).
            /// </summary>
            /// <remarks>
            /// <see cref="CaseAssociatedMetaDataTextID"/> is defined in CASE_ASSOC_METADATA_TEXT.
            /// </remarks>
            public int? CaseAssociatedMetaDataTextID { get; set; }

            /// <summary>
            /// ID of the <see cref="MetaData"/> if it is a link.
            /// </summary>
            /// <remarks>
            /// <see cref="CaseAssociatedMetaDataLinkID"/> is defined in CASE_ASSOC_METADATA_LINK.
            /// </remarks>
            public int? CaseAssociatedMetaDataLinkID { get; set; }

            /// <summary>
            /// ID of the <see cref="MetaData"/> if it is a person.
            /// </summary>
            /// <remarks>
            /// <see cref="CaseAssociatedMetaDataPersonID"/> is defined in CASE_ASSOC_METADATA_PERSON.
            /// </remarks>
            public int? CaseAssociatedMetaDataPersonID { get; set; }

            /// <summary>
            /// ID of the <see cref="ItemType"/> (ASSOC_TYPE in the database) that the <see cref="MetaData"/> is.
            /// </summary>
            /// <remarks>
            /// <see cref="AssociatedTypeID"/> is defined in CASE_ASSOC_METADATA, CASE_ASSOC_METADATA_TEXT, CASE_ASSOC_METADATA_LINK, and CASE_ASSOC_METADATA_PERSON in the databse.
            /// </remarks>
            public int AssociatedTypeID { get; set; }

            /// <summary>
            /// Current name of the <see cref="ItemType"/> (ASSOC_TYPE in the database) that the <see cref="MetaData"/> is.
            /// </summary>
            /// <remarks>
            /// <see cref="AssociatedTypeName"/> duplicates the function of <see cref="FieldName"/>, but obtains the current name from the database.
            /// In addition, <see cref="AssociatedTypeName"/> comes from ASSOC_TYPE, whereas <see cref="FieldName"/> relies on a column that is only in CASE_ASSOC_METADATA.
            /// </remarks>
            public string AssociatedTypeName { get; set; }

            /// <summary>
            /// The description of the <see cref="ItemType"/> (ASSOC_TYPE in the database) that the <see cref="MetaData"/> is.
            /// </summary>
            public string AssociatedTypeDescription { get; set; }

            /// <summary>
            /// System code of the <see cref="ItemType"/> (ASSOC_TYPE in the database) that the <see cref="MetaData"/> is.
            /// </summary>
            public string AssocTypeSystemCode { get; set; }

            /// <summary>
            /// The system priority, or sort position, of the <see cref="ItemType"/> (ASSOC_TYPE in the database) that the <see cref="MetaData"/> is.
            /// </summary>
            public int? AssocTypeSystemPriority { get; set; }

            /// <summary>
            /// Type of control that should be rendered for this <see cref="MetaData"/>.
            /// <para>Based on the <see cref="MetaData"/>'s <see cref="ItemType"/>'s AssocFieldType</para>
            /// </summary>
            public TypeOfMetaDataControl TypeOfMetaDataControl { get; set; }

            [Obsolete("Unknown property, not defined in the database.", true)]
            public int AssociatedID { get; set; }

            /// <summary>
            /// ID of the <see cref="ItemValue"/> (ASSOC_DECODE in the database) that the <see cref="MetaData"/> is if its <see cref="ItemType"/>'s AssocFieldType is D (DropDownList).
            /// <para>Simply put, it is the ID of the dropdown value if the dropdown list does not come from an external source.</para>
            /// </summary>
            /// <remarks>
            /// <see cref="AssociatedDecodeID"/> is defined only in CASE_ASSOC_METADATA.
            /// </remarks>
            public int AssociatedDecodeID { get; set; }

            /// <summary>
            /// Current name of the <see cref="ItemValue"/> (ASSOC_DECODE in the database) that the <see cref="MetaData"/> is if its <see cref="ItemType"/>'s AssocFieldType is D (DropDownList).
            /// <para>Simply put, it is the friendly name of the dropdown value if the dropdown list does not come from an external source.</para>
            /// </summary>
            /// <remarks>
            /// <see cref="AssociatedDecodeName"/> duplicates the function of <see cref="FieldValue"/>, but obtains the current name from the database.
            /// In addition, <see cref="AssociatedDecodeName"/> comes from ASSOC_DECODE, whereas <see cref="FieldValue"/> relies on a column that is only in CASE_ASSOC_METADATA.
            /// </remarks>
            public string AssociatedDecodeName { get; set; }

            /// <summary>
            /// The description of the <see cref="ItemValue"/> (ASSOC_DECODE in the database) that the <see cref="MetaData"/> is if its <see cref="ItemType"/>'s AssocFieldType is D (DropDownList).
            /// </summary>
            public string AssociatedDecodeDescription { get; set; }

            /// <summary>
            /// The system code of the <see cref="ItemValue"/> (ASSOC_DECODE in the database) that the <see cref="MetaData"/> is if its <see cref="ItemType"/>'s AssocFieldType is D (DropDownList).
            /// </summary>
            public string AssocDecodeSystemCode { get; set; }

            /// <summary>
            /// The system priority, or sort position, of the <see cref="ItemValue"/> (ASSOC_DECODE in the database) that the <see cref="MetaData"/> is if its <see cref="ItemType"/>'s AssocFieldType is D (DropDownList).
            /// </summary>
            public int? AssocDecodeSystemPriority { get; set; }

            /// <summary>
            /// The name of the <see cref="MetaData"/>'s <see cref="ItemType"/> if the <see cref="ItemType"/>'s AssocFieldType is D (DropDownList) or E (External Object).
            /// <para>Simply put, it is the name of the <see cref="MetaData"/>'s field.</para>
            /// </summary>
            /// <remarks>
            /// <see cref="FieldName"/> is useful for data warehousing purposes.  It is defined only in CASE_ASSOC_METADATA.
            /// </remarks>
            public string FieldName { get; set; }
            public string ASSOC_SECURITY { get; set; }
            public string IS_REQUIRED { get; set; }



            /// <summary>
            /// The friendly (display) value of the <see cref="MetaData"/> if its <see cref="ItemType"/>'s AssocFieldType is D (DropDownList) or E (External Object).
            /// <para>Simply put, it is the friendly value of the <see cref="MetaData"/> field.</para>
            /// </summary>
            /// <remarks>
            /// The <see cref="FieldValue"/> is the name of the <see cref="MetaData"/>'s <see cref="ItemValue"/> if the <see cref="MetaData"/>'s <see cref="ItemType"/>'s AssocFieldType is D (DropDownList).
            /// If the <see cref="MetaData"/>'s <see cref="ItemType"/>'s AssocFieldType is E (External Object), then <see cref="FieldValue"/> is the name of the selected external object.
            /// <para><see cref="FieldValue"/> is defined only in CASE_ASSOC_METADATA.</para>
            /// </remarks>
            public string FieldValue { get; set; }

            /// <summary>
            /// The ID of the <see cref="MetaData"/>'s external object if the <see cref="MetaData"/>'s <see cref="ItemType"/>'s AssocFieldType is E (External Object).
            /// </summary>
            /// <remarks>
            /// <see cref="ExternalDatasourceObjectID"/> is defined only in CASE_ASSOC_METADATA.</para>
            /// </remarks>
            public string ExternalDatasourceObjectID { get; set; }

            /// <summary>
            /// Free-text value of the <see cref="MetaData"/> if its <see cref="ItemType"/>'s AssocFieldType is T (Text), N (Number), or A (Date).
            /// </summary>
            /// <remarks>
            /// <see cref="TextValue"/> is defined in CASE_ASSOC_METADATA_TEXT.
            /// </remarks>
            public string TextValue { get; set; }

            /// <summary>
            /// Free-text value of the <see cref="MetaData"/> if its <see cref="ItemType"/>'s AssocFieldType is T (Text), N (Number), or A (Date).
            /// </summary>
            /// <remarks>
            /// <see cref="MetaDataText"/> is defined in CASE_ASSOC_METADATA_TEXT.
            /// </remarks>
            [Obsolete("Use TextValue instead.", true)]
            public string MetaDataText { get; set; }

            /// <summary>
            /// Text to display in place of the link.
            /// </summary>
            /// <remarks>
            /// Defined in CASE_ASSOC_METADATA_LINK.
            /// </remarks>
            public string LinkName { get; set; }

            /// <summary>
            /// The URL of the link.
            /// </summary>
            /// <remarks>
            /// Defined in CASE_ASSOC_METADATA_LINK.
            /// </remarks>
            public string LinkURL { get; set; }

            /// <summary>
            /// Username of the <see cref="MetaData"/> if it is a person.
            /// </summary>
            /// <remarks>
            /// Defined in CASE_ASSOC_METADATA_PERSON.
            /// </remarks>
            public string PersonSAM { get; set; }

            public override string ToString()
            {
                //if (MetaDataText != null && MetaDataText.Length > 0)
                //{
                //    return MetaDataText;
                //}
                if (TextValue != null && TextValue.Length > 0)
                {
                    return TextValue;
                }
                else if (AssociatedDecodeName != null && AssociatedDecodeName.Length > 0)
                {
                    return AssociatedDecodeName;
                }
                else if (LinkName != null && LinkName.Length > 0)
                {
                    return LinkName;
                }
                else if (PersonSAM != null && PersonSAM.Length > 0)
                {
                    return PersonSAM;
                }
                else if (FieldValue != null && FieldValue.Length > 0)
                {
                    return FieldValue;
                }

                else
                {
                    return string.Empty;
                }
            }

        }
        #endregion

        #region TypeOfMetaDataControl
        public enum TypeOfMetaDataControl
        {
            DropDown,
            TextBox,
            CheckBoxArray,
            RadioArray
        }
        #endregion

        #region SystemCodeInfo
        public class SystemCodeInfo
        {
            public string SystemCodeLevel { get; set; }

            public string SystemCode { get; set; }

            public string SystemCodeName { get; set; }

            public string SystemCodeDescription { get; set; }

            public int AssocSystemCodeID { get; set; }
        }
        #endregion

        #region ItemType
        public class ItemType
        {
            [Obsolete("Use CaseType to represent a case type.")]
            private string instanceName;
            [Obsolete("Use CaseType to represent a case type.")]
            private string instanceNamePlural;

            /// <summary>
            /// ID of the <see cref="ItemType"/>
            /// </summary>
            public int AssocTypeID { get; set; }

            /// <summary>
            /// The type of field that the <see cref="ItemType"/> is, such as drop down, text, or date.
            /// </summary>
            /// <remarks>
            /// Currently, the field types are as follows:
            /// <para>D - DropDownList (pre-defined value list)</para>
            /// <para>T - Text (TextBox)</para>
            /// <para>P - (unknown)</para>
            /// <para>E - External Object (DropDownList)</para>
            /// <para>N - Number (TextBox)</para>
            /// <para>A - Date (Telerik RadDatePicker)</para>
            /// </remarks>
            public char AssocFieldType { get; set; }

            /// <summary>
            /// ID of the <see cref="CaseType"/> to which the <see cref="ItemType"/> belongs.
            /// </summary>
            public int CaseTypeID { get; set; }

            /// <summary>
            /// The name of the <see cref="ItemType"/>.
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// The description of the <see cref="ItemType"/>.
            /// </summary>
            public string Description { get; set; }

            public bool UseCommaSeparator { get; set; }

            public string SeparatorCharactor { get; set; }

            /// <summary>
            /// ID of the external data source for External Object (E) field types.
            /// </summary>
            public int? ExternalDataSourceID { get; set; }

            /// <summary>
            /// ID of the external data source entity type id for External Object (E) field types.
            /// </summary>
            public int? ExternalDataSourceEntityTypeID { get; set; }

            /// <summary>
            /// The system code of the <see cref="ItemType"/>.
            /// </summary>
            public string SystemCode { get; set; }

            /// <summary>
            /// The system priority, or sort position, of the <see cref="ItemType"/>.
            /// </summary>
            public int SystemPriority { get; set; }

            /// <summary>
            /// Whether or not the <see cref="ItemType"/> should appear on list views, such as in a GridView.
            /// </summary>
            public char? ShowOnList { get; set; }

            /// <summary>
            /// The width, in pixels, the <see cref="ItemType"/> should be when rendered.
            /// </summary>
            public int? UIWidth { get; set; }

            /// <summary>
            /// Whether or not the <see cref="ItemType"/> is required.
            /// </summary>
            public char IsRequired { get; set; }

            /// <summary>
            /// Whether or not the <see cref="ItemType"/> is active.
            /// </summary>
            [Obsolete("Should this be changed to a bool?")]
            public char IsActive { get; set; }

            /// <summary>
            /// When the <see cref="ItemType"/> was created.
            /// </summary>
            public DateTime CreatedDateTime { get; set; }

            /// <summary>
            /// Who created the <see cref="ItemType"/>.
            /// </summary>
            public string CreatedBy { get; set; }

            /// <summary>
            /// When the <see cref="ItemType"/> was last modified.
            /// </summary>
            public DateTime ModifiedDateTime { get; set; }

            /// <summary>
            /// Who last modified the <see cref="ItemType"/>.
            /// </summary>
            public string ModifiedBy { get; set; }

            /// <summary>
            /// The parent of the <see cref="ItemType"/> (?).
            /// <para>This property is NOT defined in database.</para>
            /// </summary>
            public int Parent { get; set; }

            /// <summary>
            /// The child of the <see cref="ItemType"/> (?).
            /// <para>This property is NOT defined in database.</para>
            /// </summary>
            public int Child { get; set; }

            /// <summary>
            /// The proper name of a case item.
            /// <para>This property defined in CASE_TYPE.</para>
            /// </summary>
            [Obsolete("Use CaseType to represent a case type.", true)]
            public string InstanceName
            {
                get
                {
                    if (!string.IsNullOrEmpty(this.instanceName))
                    {
                        return this.instanceName;
                    }
                    else
                    {
                        return "Item";
                    }
                }

                set
                {
                    this.instanceName = value;
                }
            }

            /// <summary>
            /// The plural proper name of a case items.
            /// <para>This property defined in CASE_TYPE.</para>
            /// </summary>
            [Obsolete("Use CaseType to represent a case type.", true)]
            public string InstanceNamePlural
            {
                get
                {
                    if (!string.IsNullOrEmpty(this.instanceNamePlural))
                    {
                        return this.instanceNamePlural;
                    }
                    else
                    {
                        return "Items";
                    }
                }

                set
                {
                    this.instanceNamePlural = value;
                }
            }

            /// <summary>
            /// Creates a new <see cref="ItemType"/>.
            /// <para>Do not use to represent a <see cref="CaseType"/>.</para>
            /// </summary>
            public ItemType()
            {

            }

            /// <summary>
            /// Creates a new <see cref="ItemType"/>.
            /// <para>Do not use to represent a <see cref="CaseType"/>.</para>
            /// </summary>
            /// <param name="casetypeid">ID of the <see cref="CaseType"/> to which the <see cref="ItemType"/> belongs.</param>
            /// <param name="name">The name of the <see cref="ItemType"/>.</param>
            public ItemType(int casetypeid, string name)
            {
                CaseTypeID = casetypeid;
                Name = name;
            }

            public override string ToString()
            {
                return this.Name;
            }

            public string SecurityType { get; set; }
            public string AssociatedTypeSecurity { get; set; }
            public string CalculationFormula { get; set; }
            public int? CalculationFrequencyMin { get; set; }
            public char? IsFroceRecalculation { get; set; }
        }
        #endregion

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
            public string CreateBy { get; set; }
            public DateTime ModifiedDateTime { get; set; }
            public string ModifiedBy { get; set; }
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

            public string CaseTitle
            {
                get
                {
                    foreach (MetaData item in this.MetaDataCollection)
                    {
                        if (item.AssocTypeSystemCode != null)
                        {
                            if (item.AssocTypeSystemCode.ToUpper().Trim() == "TITLE")
                            {
                                // TODO Why is this alternating?
                                //return item.TextValue;
                                //return item.MetaDataText;

                                //return !string.IsNullOrEmpty(item.TextValue) ? item.TextValue : item.MetaDataText;
                                return item.TextValue;
                            }
                        }
                    }
                    return string.Empty;

                }
            }

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

                                if (DebugMode) debug = "&Debug=Y";

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

        }
        #endregion

        #region NoteData
        // [Serializable]
        public class NoteData
        {
            public int CaseNoteID { get; set; }
            public int CaseID { get; set; }
            public int NoteTypeID { get; set; }
            public string Note { get; set; }
            public DateTime CreatedDateTime { get; set; }
            public string CreatedBy { get; set; }
            public string BColor { get; set; }
            public string FColor { get; set; }
            public string DisplayNote { get; set; }
            public string NoteTypeName { get; set; }
            public GetUserInfoResponse.UserInfo CreatedByUser { get; set; }
        }
        #endregion

        //#region UserInfo
        //[Serializable]
        //public class UserInfo
        //{
        //    public int ID { get; set; }

        //    public string MiddleName { get; set; }

        //    public string FirstName { get; set; }

        //    public string LastName { get; set; }

        //    public string PrimaryJobTitle { get; set; }

        //    public string CellPhone { get; set; }

        //    public string Department { get; set; }

        //    public string OfficePhone { get; set; }

        //    public string Email { get; set; }

        //    public string City { get; set; }

        //    public string State { get; set; }

        //    public string DisplayName { get; set; }

        //    public string UserID { get; set; }
        //}
        //#endregion

        //#region SystemConfig
        //public class SystemConfig
        //{
        //    public string SystemCode { get; set; }
        //    public string Name { get; set; }
        //    public string Value { get; set; }
        //    public DateTime CreatedDateTime { get; set; }
        //    public string CreatedBy { get; set; }
        //    public DateTime ModifiedDateTime { get; set; }
        //    public string ModifiedBy { get; set; }
        //}
        //#endregion

        #region RequestAlertSubscriber
        // [Serializable]
        public class RequestAlertSubscriber
        {
            public CaseData NewCaseData { get; set; }

            public CaseData OldCaseData { get; set; }

            public bool SendAlertToAssignedPerson { get; set; }
        }
        #endregion

        #region RequestAlertAssignedPerson
        /// <summary>
        /// Object for asynchronously processing assignment alerts using MS Messaging Queue
        /// </summary>
       // [Serializable]
        public class RequestAlertAssignedPerson
        {
            public string NewCaseOwner { get; set; }

            public string UserAssigningCase { get; set; }

            public CaseData PostAssignData { get; set; }
        }
        #endregion

        #region HopperInfo
        //[Serializable]
        public class HopperInfo
        {
            public int ID { get; set; }

            public string SystemCode { get; set; }

            /// <summary>
            /// Hopper's friendly name
            /// </summary>
            public string Name { get; set; }

            public string Description { get; set; }

            public string EmailAddress { get; set; }

            public bool SuppressAutoAlert { get; set; }

            public bool IsActive { get; set; }

            /// <summary>
            /// When the <see cref="HopperInfo"/> was created.
            /// </summary>
            public DateTime CreatedDateTime { get; set; }

            /// <summary>
            /// Who created the <see cref="HopperInfo"/>.
            /// </summary>
            public string CreatedBy { get; set; }

            /// <summary>
            /// When the <see cref="HopperInfo"/> was last modified.
            /// </summary>
            public DateTime ModifiedDateTime { get; set; }

            /// <summary>
            /// Who last modified the <see cref="HopperInfo"/>.
            /// </summary>
            public string ModifiedBy { get; set; }

            /// <summary>
            /// Hopper's display name for case assignments
            /// </summary>
            public string DisplayName
            {
                get
                {
                    return "[HOPPER: " + this.Name + "]";
                }

            }

            /// <summary>
            /// Hopper's username
            /// </summary>
            public string UserName
            {
                get
                {
                    // Username as defined in spi_GetUserInformation
                    return "HOPPER_" + this.SystemCode;
                }
            }
        }
        #endregion

        #region CaseDataChange
        // [Serializable]
        public class CaseDataChange
        {
            public string OldValue { get; set; }

            public string NewValue { get; set; }

            public string AssocTypeSystemCode { get; set; }

            public CaseDataChange() { }


            public CaseDataChange(string oldValue, string newValue, string systemCode = null)
            {
                this.OldValue = oldValue;
                this.NewValue = newValue;
                this.AssocTypeSystemCode = systemCode;
            }
        }
        #endregion

        #region Subscription
        //  [Serializable]
        public class Subscription
        {
            public int SubscriptionID { get; set; }

            public int? CaseID { get; set; }

            public int CaseTypeID { get; set; }

            public string EmailAddress { get; set; }

            public int? AssocSystemCodeID { get; set; }

            /// <summary>
            /// System code of the ItemType (field, ASSOC_TYPE in the database) to which the subscription relates.
            /// </summary>
            public string AssocTypeSystemCode { get; set; }

            /// <summary>
            /// The type of alert, case-level (C) or list-level (L).
            /// </summary>
            public char AlertType { get; set; }

            public char AlertFrequency { get; set; }

            public byte? AlertHour { get; set; }

            public DateTime? LastNotifiedDateTime { get; set; }

            public char IsActive { get; set; }

            /// <summary>
            /// When the <see cref="Subscription"/> was created.
            /// </summary>
            public DateTime CreatedDateTime { get; set; }

            /// <summary>
            /// Who created the <see cref="Subscription"/>.
            /// </summary>
            public string CreatedBy { get; set; }

            /// <summary>
            /// When the <see cref="Subscription"/> was last modified.
            /// </summary>
            public DateTime ModifiedDateTime { get; set; }

            /// <summary>
            /// Who last modified the <see cref="Subscription"/>.
            /// </summary>
            public string ModifiedBy { get; set; }
        }
        #endregion

        #region CaseTypeCreateBy
        public class CaseTypeCreateBy
        {
            public int CaseTypeId { get; set; }
            public string CaseTypeName { get; set; }
        }
        #endregion

        //#region GetTypesByCaseType
        //public class GetTypesByCaseType
        //{
        //    public int AssocTypeId { get; set; }
        //    public char AssocFieldType { get; set; }
        //    public int CaseTypeId { get; set; }
        //    public string Name { get; set; }
        //    public string Description { get; set; }
        //    public int ExternalDataSourceId { get; set; }
        //    public string SystemCode { get; set; }
        //    public int SystemPriority { get; set; }
        //    public string ShowOnList { get; set; }
        //    public char IsRequired { get; set; }
        //    public char IsActive { get; set; }
        //    public DateTime CreatedDateTime { get; set; }
        //    public string CreatedBy { get; set; }
        //    public DateTime ModifiedDateTime { get; set; }
        //    public string ModifiedBy { get; set; }
        //    public string SeparatorCharacter { get; set; }
        //    public int ExternalDataSourceEntityTypeId { get; set; }
        //}
        //#endregion

        #region ExternalObjectDataItem
        public class ExternalObjectDataItem
        {
            public int? ID { get; set; }

            public string NAME { get; set; }

            public string DESCRIPTION { get; set; }
        }
        #endregion

        #region AssocTypeCascade
        public class AssocTypeCascade
        {
            public int ID { get; set; }

            public int ChildAssocTypeId { get; set; }

            public int ParentAssocTypeId { get; set; }


        }
        #endregion

        #region ParentSelectedValues
        public class ParentSelectedValues
        {
            public int ParentAssocTypeId { get; set; }
            public string ParentSelectedExternalDatasourceObjectId { get; set; }
        }
        #endregion

        #region AssocDecodeValue
        public class AssocDecodeValue
        {
            public int DecodeId { get; set; }
            public string DecodeValue { get; set; }
        }
        #endregion
    }

   
}