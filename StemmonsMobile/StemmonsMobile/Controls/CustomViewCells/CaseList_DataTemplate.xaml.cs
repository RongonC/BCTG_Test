using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StemmonsMobile.Controls.CustomViewCells
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CaseList_DataTemplate : ViewCell
    {
        public CaseList_DataTemplate()
        {
            InitializeComponent();
        }
    }
}