﻿
using System;
using System.Collections.Generic;
using System.Linq;
 

namespace StemmonsMobile.DataTypes.DataType.Standards
{
    public class AddSecurityDepartmentRequest

    {
        public int iTopLevelID { get; set; }
        public int iBasicNameID { get; set; }
        public int iSubDepartmentID { get; set; }
        public int iJobFunctionID { get; set; }
        public int iJobTitleID { get; set; }
        public char cIsAdd { get; set; }
        public char cIs_Edit { get; set; }
        public char cIs_Read { get; set; }
        public char cIs_Delete { get; set; }
        public int iAppID { get; set; }
        public int iAppAssocMetadataID { get; set; }
        public string sCreatedBy { get; set; } 
        public string sMessage { get; set; }
        public int iStandardsResourceSecurityID { get; set; }
            
    }
}