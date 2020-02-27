using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;

namespace AppLauncher.ViewModels
{
    public class StatusNotifierViewModel : IStatusNotifier
    {
        private bool isWorking = false;
        private bool isTryAgainVisible = false;

        /// <summary>
        /// Indicates whether launcher is working. Used to show progress bar.
        /// </summary>
        public bool IsWorking
        {
            get => isWorking;
            set {
                isWorking = value;
                NotifyPropertyChanged(nameof(IsWorking));
            }
        }

        /// <summary>
        /// Message that is displayed to the user.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Indicates whether something failed therefore try again button should be visible.
        /// </summary>
        public bool IsTryAgainVisible
        {
            get => isTryAgainVisible;
            set {
                isTryAgainVisible = value;
                NotifyPropertyChanged(nameof(IsTryAgainVisible));
            }
        }

        /// <summary>
        /// Updates Message property.
        /// If showTime is defined message will be emptied after provided time.
        /// </summary>
        /// <param name="o">Object carrying message</param>
        /// <param name="showTime">Time in ms after which message will be emptied</param>
        public void Print(object o, int showTime = 0)
        {
            Message = o.ToString();
            NotifyPropertyChanged(nameof(Message));
            if (showTime > 0) // optionally empty message
            {
                Thread.Sleep(showTime);
                Message = string.Empty;
                NotifyPropertyChanged(nameof(Message));
            }
        }

        #region INotifyPropertyChanged Implementation
        public event PropertyChangedEventHandler PropertyChanged;
        internal void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
