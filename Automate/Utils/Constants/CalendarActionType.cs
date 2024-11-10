using Automate.Utils.Enums;
using System;

namespace Automate.Utils.Constants
{
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
