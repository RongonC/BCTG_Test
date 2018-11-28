using System;
using System.Collections.Generic;
using System.ComponentModel;
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
            public int CaseTypeID { get; set; }
            public int NoteTypeID { get; set; }
            public string Note
            {
                get
                {
                    return _note;
                }

                set
                {
                    _note = value;
                    OnPropertyChanged("Note");
                }
            }

            public DateTime CreatedDateTime { get; set; }
            public string CreatedBy { get; set; }
            public string BColor { get; set; }
            public string FColor { get; set; }
            public string DisplayNote { get; set; }
            public string NoteTypeName { get; set; }
            public UserInfo CreatedByUser { get; set; }

            private string _htmlNote;
            private string _note;
            private bool _imageVisible;
            private bool _labelVisible;
            private string _imageURL;

            public string HtmlNote
            {
                get
                {
                    return _htmlNote;
                }
                set
                {
                    _htmlNote = Note;
                    OnPropertyChanged("HtmlNote");
                }
            }
            public bool ImageVisible
            {
                get { return _imageVisible; }
                set
                {
                    _imageVisible = value;
                    OnPropertyChanged("ImageVisible");
                }
            }
            public bool LabelVisible
            {
                get { return _labelVisible; }
                set
                {
                    _labelVisible = value;
                    OnPropertyChanged("LabelVisible");
                }
            }
            public string ImageURL
            {
                get { return _imageURL; }
                set
                {
                    _imageURL = value;
                    OnPropertyChanged("ImageURL");
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;

            protected virtual void OnPropertyChanged(string propertyName)
            {
                var changed = PropertyChanged;
                if (changed != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                }
            }
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