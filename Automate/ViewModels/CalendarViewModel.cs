using Automate.Services.Commands;

namespace Automate.ViewModels
{
    public class CalendarViewModel
    {
        public CalendarCommand CalendarCommand { get; }

        public CalendarViewModel()
        {
            CalendarCommand = new CalendarCommand();
        }
    }
}
