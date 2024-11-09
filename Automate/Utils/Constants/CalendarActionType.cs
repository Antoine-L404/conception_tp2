using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automate.Utils.Constants
{
    public enum CalendarActionType
    {
        Add,
        Edit,
        Delete,
        Click,
        MonthChanged
    }

    public class CalendarAction
    {
        public CalendarActionType ActionType { get; }
        public DateTime Date { get; }
        public string Title { get; }

    public CalendarAction(CalendarActionType actionType)
        {
            ActionType = actionType;
        }
        public CalendarAction(CalendarActionType actionType, DateTime date)
        {
            ActionType = actionType;
            Date = date;
        }

        public CalendarAction(CalendarActionType actionType, DateTime date, string selectedEventTitle)
        {
            ActionType = actionType;
            Date = date;
            Title = selectedEventTitle;
        }
    }
}
