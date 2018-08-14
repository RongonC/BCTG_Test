namespace StemmonsMobile.DataTypes.DataType.Entity
{
    public class Entity_categoryList
    {
        public Entity_categoryList(int? _entityTypeCategoryID, string _entityTypeCategoryName, string _categorySystemCode, string _categoryDescription, string _isDefault)
        {
            EntityTypeCategoryID = _entityTypeCategoryID;
            EntityTypeCategoryName = _entityTypeCategoryName;
            CategorySystemCode = _categorySystemCode;
            CategoryDescription = _categoryDescription;
            IsDefault = _isDefault;
        }

        public int? EntityTypeCategoryID { get; set; }
        public string EntityTypeCategoryName { get; set; }
        public string CategorySystemCode { get; set; }
        public string CategoryDescription { get; set; }
        public string IsDefault { get; set; }
    }
}
