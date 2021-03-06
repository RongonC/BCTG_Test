﻿using System;
using System.Collections.Generic;
using System.Linq;



namespace StemmonsMobile.DataTypes.DataType.Cases
{
    public class CreateCaseRequest
    {
        //public class CreateCase
        //{
        /// <summary>
        /// Case title for new case
        /// </summary>
        public string caseTitle { get; set; }
        /// <summary>
        /// Case type of new case
        /// </summary>
        public int caseType { get; set; }
        /// <summary>
        /// Case assign to name
        /// </summary>
        public string assignTo { get; set; }
        /// <summary>
        /// Current user who created this case
        /// </summary>
        public string currentUser { get; set; }
        /// <summary>
        /// Metadata values for case
        /// </summary>
        public string metaDataValues { get; set; }
        /// <summary>
        /// Case notes for new case
        /// </summary>
        public string caseNotes { get; set; }
        /// <summary>
        /// Text values of new case
        /// </summary>
        //public Dictionary<int, string> textValues { get; set; }
        public List<TextValue> textValues { get; set; }
        /// <summary>
        /// Link values of new case
        /// </summary>
        //public Dictionary<int, Dictionary<string, string>> linkValues { get; set; }
        public List<LinkValue> linkValues { get; set; }
        //}
    }

    #region TextValue
    public class TextValue
    {
        public int Key { get; set; }
        public string Value { get; set; }
    }
    #endregion

    #region LinkValue
    public class LinkValue
    {
        public int Key { get; set; }
        public List<LinkValueNested> Value { get; set; }
    }
    #endregion

    #region LinkValueNested
    public class LinkValueNested
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
    #endregion
}
