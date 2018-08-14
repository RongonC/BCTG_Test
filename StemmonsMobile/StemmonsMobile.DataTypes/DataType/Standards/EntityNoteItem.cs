/* (c) 2015 Boxer Enterprise.
 * All Rights Reserved. No part of this software may
 * be reproduced or redistributed without Boxer Enterprise's
 * express prior written consent.
*/

using StemmonsMobile.DataTypes.DataType.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataAccess.DataTypes
{
    public class EntityNoteItem
    {
        // From Column Name: ENTITY_NOTE_ID
        public int ID { get; set; }

        // From Column Name: ENTITY_ID
        public EntityClass Entity { get; set; }

        // From Column Name: ENTITY_NOTE_TYPE_ID
        public EntityNoteType EntityNoteType { get; set; }

        public string EntityNoteTypeByName { get; set; }
        // From Column Name: NOTE
        public string Note { get; set; }

        public string BColor { get; set; }

        public string FColor { get; set; }

        // From Column Name: IS_ACTIVE
        public char IsActive { get; set; }

        // From Column Name: CREATED_DATETIME
        public DateTime CreatedDatetime { get; set; }

        // From Column Name: CREATED_BY
        public string CreatedBy { get; set; }

        // From Column Name: MODIFIED_DATETIME
        public DateTime? ModifiedDatetime { get; set; }

        // From Column Name: MODIFIED_BY
        public string ModifiedBy { get; set; }

        public string ModifiedByFullName { get; set; }

        public string SystemCode { get; set; }

        public char? AllowManualUse { get; set; }
    }
}
