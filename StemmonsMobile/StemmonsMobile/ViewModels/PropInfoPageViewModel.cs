using StemmonsMobile.DataTypes.DataType.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StemmonsMobile.ViewModels
{
     public class PropInfoPageViewModel
    {
        public string PropertyTitle { get; set; }
        public PropInfoPageViewModel(EntityClass entityClass)
        {
            PropertyTitle = entityClass.EntityTitle;
        }
    }
}