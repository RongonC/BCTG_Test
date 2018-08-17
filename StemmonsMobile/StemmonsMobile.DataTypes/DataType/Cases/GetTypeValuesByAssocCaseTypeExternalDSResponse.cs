
using System;
using System.Collections.Generic;
using System.Linq;


namespace StemmonsMobile.DataTypes.DataType.Cases
{
    public class GetTypeValuesByAssocCaseTypeExternalDSResponse : Response
    {
        public object ResponseContent { get; set; }

        #region ItemValue
        /// <summary>
        /// The value object of an item in a case field with predefined options, such as in a drop down list.
        /// <para>Defined as ASSOC_DECODE in the database.</para>
        /// </summary>
        //[Serializable]
        public class ItemValue
        {
            [Obsolete("Not implemented yet", true)]
            public void AddNewItem()
            {
            }

            /// <summary>
            /// ID of the value for the Ext DS Selected Index Value.
            /// </summary>
            public int ID { get; set; }

            /// <summary>
            /// ID of the value object.
            /// </summary>
            public int AssocDecodeID { get; set; }

            /// <summary>
            /// ID of the <see cref="ItemType"/> (ASSOC_TYPE in the database) to which this <see cref="ItemValue"/> belongs.
            /// </summary>
            public int AssocTypeID { get; set; }

            /// <summary>
            /// ID of the <see cref="CaseType"/> to which the <see cref="ItemValue"/>'s <see cref="ItemType"/> belongs.
            /// <para>This parameter is NOT defined in database.</para>
            /// </summary>
            public int CaseTypeID { get; set; }

            /// <summary>
            /// Description (not name? Or does it depend?) of the <see cref="CaseType"/> to which the <see cref="ItemValue"/>'s <see cref="ItemType"/> belongs.
            /// <para>This property is NOT defined in database.</para>
            /// </summary>
            public string CaseTypeDesc { get; set; }

            public int? ObjectID { get; set; }

            /// <summary>
            /// ID value of the <see cref="ItemValue"/> when <see cref="ItemValue"/> is used in an unbound manner.
            /// <para>Prepend with "P" when the source is External Data.</para>
            /// <para>This property is NOT defined in database.</para>
            /// </summary>
            /// <remarks>
            /// <see cref="ObjectCode"/> is used to prepare dropdown selections to be saved to the database.
            /// </remarks>
            public string ObjectCode { get; set; }

            /// <summary>
            /// The name, or text value, of the <see cref="ItemValue"/>.
            /// </summary>
            /// <remarks>
            /// When <see cref="ItemValue"/> is used with Property Bags, <see cref="Name"/> is the case field name, and <see cref="TextValue"/> is the selected value (text) from a drop down list.
            /// <para>Although <see cref="Name"/> is correct, <see cref="TextValue"/> may make more sense.</para>
            /// </remarks>
            public string Name { get; set; }

            /// <summary>
            /// Arbitrary text value for this <see cref="ItemValue"/>.
            /// <para>This property is NOT defined in database.</para>
            /// </summary>
            /// <remarks>
            /// When <see cref="ItemValue"/> is used with Property Bags, <see cref="Name"/> is the case field name, and <see cref="TextValue"/> is the selected value (text) from a drop down list.
            /// </remarks>
            [Obsolete("Need to change the references Property Bags use.")]
            public string TextValue { get; set; }

            /// <summary>
            /// The description of the <see cref="ItemValue"/>.
            /// </summary>
            public string Description { get; set; }

            public string GLCode { get; set; }

            public string MiscCode1Desc { get; set; }

            public string MiscCode1Value { get; set; }

            public string MiscCode2Desc { get; set; }

            public string MiscCode2Value { get; set; }

            /// <summary>
            /// The system code of the <see cref="ItemValue"/>.
            /// </summary>
            public string SystemCode { get; set; }

            /// <summary>
            /// The system priority, or sort position, of the <see cref="ItemValue"/>.
            /// </summary>
            public int? SystemPriority { get; set; }

            /// <summary>
            /// Whether or not the <see cref="ItemValue"/> is active.
            /// </summary>
            public char IsActive { get; set; }

            /// <summary>
            /// Whether or not the <see cref="ItemValue"/> affects MTTR.
            /// <para>This property is NOT defined in database.</para>
            /// </summary>
            public char? StatusMTTRAffecting { get; set; }

            /// <summary>
            /// When the <see cref="ItemValue"/> was created.
            /// </summary>
            public string CreatedDateTime { get; set; }

            /// <summary>
            /// Who created the <see cref="ItemValue"/>.
            /// </summary>
            public string CreatedBy { get; set; }

            /// <summary>
            /// When the <see cref="ItemValue"/> was last modified.
            /// </summary>
            public string ModifiedDateTime { get; set; }

            /// <summary>
            /// Who last modified the <see cref="ItemValue"/>.
            /// </summary>
            public string ModifiedBy { get; set; }

            public string ItemPropertyBag
            {
                get
                {
                    string required = "Y";
                    if (!IsRequired)
                        required = "N";

                    // 1. AssocTypeID
                    // 2. AssocDecodeID
                    // 3. Required (Y/N)
                    // 4. CaseTypeDesc
                    // 5. Child AssocDecodeID
                    // 6. Parent AssocDecodeID
                    // 7. CaseTypeID
                    // 8. AssocDecodeStatusCode

                    return AssocTypeID.ToString() + "|" + AssocDecodeID.ToString() + "|" + required + "|" + CaseTypeDesc?.Replace('|', ' ').Replace(',', ' ') + "|" + Child + "|" + Parent + "|" + CaseTypeID + "|" + SystemCode;
                }
            }

            /// <summary>
            /// Whether or not the <see cref="ItemValue"/>'s <see cref="ItemType"/> (?) is required.
            /// <para>This property is NOT defined in database.</para>
            /// </summary>
            public bool IsRequired { get; set; }

            /// <summary>
            /// The child of the <see cref="ItemValue"/> (?).
            /// <para>This property is NOT defined in database.</para>
            /// </summary>
            public int Child { get; set; }

            /// <summary>
            /// The parent of the <see cref="ItemValue"/> (?).
            /// <para>This property is NOT defined in database.</para>
            /// </summary>
            public int Parent { get; set; }

            /// <summary>
            /// Creates a new <see cref="ItemValue"/> with some default values.
            /// </summary>
            public ItemValue()
            {
                CaseTypeDesc = string.Empty;
                AssocTypeID = -1;
                AssocDecodeID = -1;
                IsRequired = true;
                Child = -1;
                Parent = -1;
                CaseTypeID = -1;
            }

            public override string ToString()
            {
                return this.Name;
            }
        }
        #endregion
    }
}