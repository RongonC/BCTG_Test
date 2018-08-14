/* (c) 2015 Boxer Enterprise.
 * All Rights Reserved. No part of this software may
 * be reproduced or redistributed without Boxer Enterprise's
 * express prior written consent.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entities.DataAccess.DataTypes
{
    public class EntityNoteType
    {
        // From Column Name: ENTITY_NOTE_TYPE_ID
        public int ID { get; set; }

        // From Column Name: NAME
        public string Name { get; set; }

        // From Column Name: DESCRIPTION
        public string Description { get; set; }

        // From Column Name: BCOLOR
        public string Bcolor { get; set; }

        // From Column Name: FCOLOR
        public string Fcolor { get; set; }

        // From Column Name: SYSTEM_CODE
        public string SystemCode { get; set; }

        // From Column Name: ALLOW_MANUAL_USE
        public char? AllowManualUse { get; set; }

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

    }
}
