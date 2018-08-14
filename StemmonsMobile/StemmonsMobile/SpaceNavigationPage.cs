using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace StemmonsMobile
{
   public class SpaceNavigationPage: NavigationPage
    {


        public SpaceNavigationPage(Page root) : base(root)
        {

            BarBackgroundColor = Color.DarkBlue;
            BarTextColor = Color.White;

        }

    }
}
