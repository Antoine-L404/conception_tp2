using Automate.Utils.Enums;
using System;
using System.Linq;
using System.Windows;

namespace Automate.Views
{
    public partial class TaskFormWindow : Window
    {
        public EventType SelectedEventType { get; private set; }
        public DateTime EventDate { get; private set; }
        public bool IsConfirmed { get; private set; }

        public TaskFormWindow(DateTime selectedDate, EventType? initialEventType = null)
        {
            InitializeComponent();
            EventDate = selectedDate;
            eventDateTextBox.Text = EventDate.ToShortDateString();

            eventTypeComboBox.Items.Clear();
            eventTypeComboBox.ItemsSource = Enum.GetValues(typeof(EventType)).Cast<EventType>();

            if (initialEventType.HasValue)
            {
                eventTypeComboBox.SelectedItem = initialEventType.Value;
            }
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
