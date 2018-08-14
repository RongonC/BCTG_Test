using System;
using System.Collections.Generic;
using System.Linq;
 

namespace StemmonsMobile.DataTypes.DataType.Entity
{
    public class LazyLoad
    {
        public class Entity : EntityType
        {
            public int? EntityID { get; set; }

            public int? ListID { get; set; }

            public string IsDeleted { get; set; }

            public string EntityTypeNameForHome { get; set; }
            public string EntitySecurityType { get; set; }

            public int? MasterEntityID { get; set; }

            public string MasterEntityRelationDescription { get; set; }
            public string EntityCreatedByUserName { get; set; }
            public string EntityCreatedByFullName { get; set; }
            public string EntityCreatedByPhone { get; set; }
            public string EntityCreatedByEmail { get; set; }
            public Guid? EntityCreatedByGUID { get; set; }
            public DateTime? EntityCreatedDateTime { get; set; }

            public DateTime? EntityModifiedDateTimeHome { get; set; }
            public string EntityTitle { get; set; }
            public string CreatedBY { get; set; }

            public string EntityModifiedByUserName { get; set; }
            public string EntityModifiedByFullName { get; set; }
            public string EntityModifiedByPhone { get; set; }
            public string EntityModifiedByEmail { get; set; }
            public Guid? EntityModifiedByGUID { get; set; }
            public DateTime? EntityModifiedDateTime { get; set; }

            public string EntitiyAssignedToUserName { get; set; }
            public string EntityAssignedToFullName { get; set; }
            public string EntityAssignedToEmail { get; set; }
            public string EntityAssignedToPhone { get; set; }
            public Guid? EntityAssignedToGUID { get; set; }
            public DateTime? EntityAssignedToDateTime { get; set; }

            public string EntitiyOwnedByUserName { get; set; }
            public string EntityOwnedByFullName { get; set; }
            public string EntityOwnedByEmail { get; set; }
            public string EntityOwnedByPhone { get; set; }
            public Guid? EntityOwnedByGUID { get; set; }
            public DateTime? EntityOwnedByDateTime { get; set; }

            public string SHOW_ENTITIES_ACTIVE_INACTIVE { get; set; }
            public string SHOW_ENTITIES_ASSIGNED_TO_USER { get; set; }
            public string SHOW_ENTITIES_CREATED_BY_USER { get; set; }
            public string SHOW_ENTITIES_OWNED_BY_USER { get; set; }
            public string SHOW_ENTITIES_INACTIVE_BY_USER { get; set; }

            public bool HasRightsToConfigSecurity { get; set; }

            public List<EntityType> EntityTypeValue { get; set; }

            public List<EntityTypeScreenConfiguration> ScreenConfigurationSettings { get; set; }

            public string OpenPageType { get; set; }
        }

    }
}