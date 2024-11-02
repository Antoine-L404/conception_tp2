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

        public void Close(Window window)
        {
            window.Close();
        }
    }

}
