using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace StemmonsMobile
{
    public static class Commands
    {
        public static ICommand ToggleExpandCommand { get; } = new ToggleExpandCommand();
        //test

    }
}
