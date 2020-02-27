#region Copyright © 2020 Pawel Bielecki, Bielecki Limited https://github.com/BieleckiLtd
//    The MIT License(MIT)
//    
//    Permission is hereby granted, free of charge, to any person obtaining a copy of this software
//    and associated documentation files (the “Software”), to deal in the Software without restriction,
//    including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense,
//    and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so,
//    subject to the following conditions:
//
//    The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
//
//    THE SOFTWARE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
//    INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
//    IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
//    DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
#endregion

using AppLauncher.ViewModels;
using System.Windows;
using System.Windows.Input;

namespace AppLauncher
{
    public partial class SplashScreen : Window
    {
        public SplashScreen()
        {
            InitializeComponent();
            DataContext = new SplashScreenViewModel();
        }



        /// <summary>
        /// Enables window dragging
        /// </summary>
        private void DragMove(object sender, MouseButtonEventArgs e)
        {
            // Can only call DragMove when primary mouse button is down
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        /// <summary>
        /// Keeps window always on the top
        /// </summary>
        private void Window_Deactivated(object sender, System.EventArgs e)
        {
            Window window = (Window)sender;
            window.Topmost = true;
        }
    }
}
