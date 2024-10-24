using System.Windows;

namespace Automate.Utils
{
    public class NavigationUtils
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
