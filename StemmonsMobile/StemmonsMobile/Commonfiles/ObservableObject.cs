using CameraApplication.Annotations;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace StemmonsMobile.Commonfiles
{
    public class ObservableObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]

        protected virtual void OnPropertyChanged(
            [CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
