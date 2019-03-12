using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StemmonsMobile.Datatemplates
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EntityListDataTemplate : ViewCell
    {
        public EntityListDataTemplate()
        {
            InitializeComponent();
        }
    }
}