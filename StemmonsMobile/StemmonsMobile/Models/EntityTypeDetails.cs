namespace StemmonsMobile.Views.Entity
{
    public class EntityTypeDetails
    {
        public string Name { get; set; }
        public string Type_value { get; set; }
        public string Img { get; set; }

        public EntityTypeDetails(string _name, string _value, string _img)
        {
            Name = _name;
            Type_value = _value;
            Img = _img;
        }
    }
}
