using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using Automate.Commands;

namespace Automate.Views
{
    public partial class CalendarWindow : Window
    {
        private readonly CalendarCommand _calendarCommand;

        public CalendarWindow()
        {
            InitializeComponent();
            _calendarCommand = new CalendarCommand();
        }

        private void MyCalendar_Loaded(object sender, RoutedEventArgs e)
        {
            _calendarCommand.Execute(myCalendar);
        }

        private void MyCalendar_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            // Permet de trouvé la case sélectionner dans un calendar de XAML
            var calendarDayButton = FindParent<CalendarDayButton>(e.OriginalSource as DependencyObject);

            if (calendarDayButton != null && calendarDayButton.DataContext is DateTime selectedDate)
            {
                _calendarCommand.Execute(new Tuple<Calendar, Popup, TextBlock, TextBlock, DateTime>(
                    myCalendar, myPopup, myPopupTitle, myPopupText, selectedDate));

                e.Handled = true;
            }
        }
        private static T FindParent<T>(DependencyObject child) where T : DependencyObject
        {
            DependencyObject parentObject = VisualTreeHelper.GetParent(child);
            if (parentObject == null) return null;
            if (parentObject is T parent) return parent;
            return FindParent<T>(parentObject);
        }

    }
}
