

namespace StemmonsMobile.DataTypes.DataType.Entity
{
    public class Entity_Notes
    {
        public int EntityId { get; set; }
        public int EntityTypeId { get; set; }
        public string ID { get; set; }
        public string Entity { get; set; }
        public string EntityNoteType { get; set; }
        public string EntityNoteTypeByName { get; set; }
        public string Note { get; set; }
        public string BColor { get; set; }
        public string FColor { get; set; }
        public string IsActive { get; set; }
        public string CreatedDatetime { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedDatetime { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedByFullName { get; set; }
        public string SystemCode { get; set; }
        public string AllowManualUse { get; set; }
        public string htmlNote { get; set; }
        private bool _lableVisible;
        private bool _imageVisible;
        public string ImageURL { get; set; }
        public bool LabelVisible
        {
            get { return _lableVisible; }
            set
            {
                _lableVisible = value;
            }
        }
        public bool ImageVisible
        {
            get { return _imageVisible; }
            set
            {
                _imageVisible =value;
            }
        }
    }
}
