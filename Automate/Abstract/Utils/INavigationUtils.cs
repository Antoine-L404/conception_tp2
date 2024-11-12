using System.Windows;

namespace Automate.Abstract.Utils
{
    public interface INavigationUtils
    {
        void Close(Window window);
        void NavigateTo<T>() where T : Window, new();
        void NavigateToAndCloseCurrentWindow<T>(Window currentWindow) where T : Window, new();
    }
}