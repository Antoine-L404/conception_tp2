using Automate.Models;
using Automate.Services.Commands;
using Automate.Views;
using System;
using System.Windows;
using System.Windows.Input;

namespace Automate.ViewModels
{
    public class CalendarViewModel
    {
        public CalendarCommand CalendarCommand { get; }
        public ICommand OnAddEventClick { get; }

        private DateTime? selectedDate;
        public DateTime? SelectedDate
        {
            get => selectedDate;
            set
            {
                selectedDate = value;
            }
        }

        public CalendarViewModel()
        {
            CalendarCommand = new CalendarCommand();

            OnAddEventClick = new RelayCommand(AddEvent);
        }


        private void AddEvent()
        {
            if (selectedDate is not null)
            {
                CalendarCommand.Execute(selectedDate);
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner une date dans le calendrier.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

    }
}
