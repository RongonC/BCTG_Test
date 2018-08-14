namespace StemmonsMobile.DataTypes.DataType.Entity
{
    public class EntityTypeListData
    {
        public string EntityTypeID { get; set; }
        public string EntityTypeName { get; set; }
        public string InstanceNameSingular { get; set; }
        public string InstanceNamePlural { get; set; }
        public string SecurityType { get; set; }
        public string HasRightsToConfigSecurity { get; set; }
        public string ShowAsRssFeed { get; set; }
        public string EntityTypeCategoryID { get; set; }
        public string EntityTypeCategoryName { get; set; }
        public string CategorySystemCode { get; set; }
        public string ActiveCount { get; set; }
        public string InactiveCount { get; set; }
        public string TotalCount { get; set; }
        public string AssignedToUser { get; set; }
        public string CreatedByUser { get; set; }
        public string OwnedByUser { get; set; }
        public string InactivatedByUser { get; set; }
        public string LastCreatedDateTime { get; set; }
        public string HasExternalEntityAssocType { get; set; }
        public string EntityListLink { get; set; }
        public string NewEntityText { get; set; }
        public string NewEntityLink { get; set; }
        public string NewEntityLinkPopUp { get; set; }
        public int? ASSOCIATED_TO_USER { get; set; }

        public string Owner { get; set; }
    }
}
