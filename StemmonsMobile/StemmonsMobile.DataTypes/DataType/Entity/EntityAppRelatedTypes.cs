namespace StemmonsMobile.DataTypes.DataType.Entity
{
    public class EntityAppRelatedTypes
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public string AreaId { get; set; }
        public string ITEM_INSTANCE_TRAN_ID { get; set; }

        public EntityAppRelatedTypes(string _name, string _id )
        {
            Name = _name;
            Id = _id;
        }

    }
}
