using System;
using System.Collections.Generic;
using System.Linq;



namespace StemmonsMobile.DataTypes.DataType.Cases
{
    public class GetCaseNotesResponse : Response
    {
        public GetCaseNotesResponse()
        {
            this.NoteDataCollection = new List<NoteData>();
        }
        public List<NoteData> NoteDataCollection { get; set; }
        #region NoteData
        public object ResponseContent { get; set; }

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
            public UserInfo CreatedByUser { get; set; }

            private string _htmlNote;
            public string htmlNote
            {
                get { return _htmlNote; }
                set { _htmlNote = Note; }
            }
            public bool ImageVisible { get; set; }
            public bool LabelVisible { get; set; }
            public string ImageURL { get; set; }
        }
        #endregion

        #region UserInfo
        //    [Serializable]
        public class UserInfo
        {
            public int ID { get; set; }

            public string MiddleName { get; set; }

            public string FirstName { get; set; }

            public string LastName { get; set; }

            public string PrimaryJobTitle { get; set; }

            public string CellPhone { get; set; }

            public string Department { get; set; }

            public string OfficePhone { get; set; }

            public string Email { get; set; }

            public string City { get; set; }

            public string State { get; set; }

            public string DisplayName { get; set; }

            public string UserID { get; set; }

            public string Supervisor { get; set; }
            public string SupervisorSAM { get; set; }
        }
        #endregion
    }
}