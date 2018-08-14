using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StemmonsMobile.Commonfiles
{
    public interface ICameraProvider
    {

        Task<CameraResult> TakePictureAsync();
    }
}
