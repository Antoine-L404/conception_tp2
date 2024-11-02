using Automate.Utils.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Automate.Views
{
    public partial class TaskFormWindow : Window
    {
        public EventType SelectedEventType { get; private set; } // Utilisation de EventType
        public DateTime EventDate { get; private set; }
        public bool IsConfirmed { get; private set; }

        public TaskFormWindow(DateTime selectedDate)
        {
            InitializeComponent();
            EventDate = selectedDate;
            eventDateTextBox.Text = EventDate.ToShortDateString();

            eventTypeComboBox.ItemsSource = Enum.GetValues(typeof(EventType)).Cast<EventType>();
        }

        private void OnAddButtonClick(object sender, RoutedEventArgs e)
        {
            if (eventTypeComboBox.SelectedItem is EventType selectedItem)
            {
                SelectedEventType = selectedItem; 
                IsConfirmed = true;
                this.Close();
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner un type d'événement.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void OnCancelButtonClick(object sender, RoutedEventArgs e)
        {
            IsConfirmed = false;
            this.Close();
        }
    }
}
