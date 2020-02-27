using System.ComponentModel;

namespace AppLauncher.ViewModels
{
    public interface IStatusNotifier : INotifyPropertyChanged
    {
        string Message { get; set; }
        bool IsWorking { get; set; }
        bool IsTryAgainVisible { get; set; }

        void Print(object o, int showTime = 0);
    }
}