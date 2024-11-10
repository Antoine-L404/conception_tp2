using Automate.Utils.Enums;
using System;

namespace Automate.Utils.Actions
{
    public class CalendarAction
    {
        public CalendarActionType ActionType { get; }
        public DateTime? Date { get; }
        public string? Title { get; }

        public CalendarAction(CalendarActionType actionType)
            : this(actionType, null, null)
        { }

        public CalendarAction(CalendarActionType actionType, DateTime date)
            : this(actionType, date, null)
        { }

        public CalendarAction(CalendarActionType actionType, DateTime? date, string? selectedEventTitle)
        {
            ActionType = actionType;
            Date = date;
            Title = selectedEventTitle;
        }
    }
}
