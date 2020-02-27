using System.Windows.Input;

namespace AppLauncher.Commands
{
    public static class CommandsManager
    {
        public static ICommand ApplicationClose => new ApplicationClose();
        public static ICommand ApplicationMinimize => new ApplicationMinimize();
        public static ICommand TryAgain => new TryAgain();
    }
}
