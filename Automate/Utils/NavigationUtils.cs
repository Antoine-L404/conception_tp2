using System.Windows;
using Automate.Abstract.Utils;

namespace Automate.Utils
{
    public class NavigationUtils : INavigationUtils
    {
        public void NavigateTo<T>() where T : Window, new()
        {
            var window = new T();
            window.Show();
        }

        public void NavigateToAndCloseCurrentWindow<T>(Window currentWindow) where T : Window, new()
        {
            NavigateTo<T>();
            Close(currentWindow);
        }

        public void Close(Window window)
        {
            window.Close();
        }
    }
}
