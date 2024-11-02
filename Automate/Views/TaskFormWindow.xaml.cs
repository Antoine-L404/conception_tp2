using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Automate.Views
{
    /// <summary>
    /// Interaction logic for TaskFormWindow.xaml
    /// </summary>
    public partial class TaskFormWindow : Window
    {
        public string SelectedEventType { get; private set; }
        public DateTime EventDate { get; private set; }
        public bool IsConfirmed { get; private set; }

        public TaskFormWindow(DateTime selectedDate)
        {
            InitializeComponent();
            EventDate = selectedDate;
            eventDateTextBox.Text = EventDate.ToShortDateString();
        }

        private void OnAddButtonClick(object sender, RoutedEventArgs e)
        {
            if (eventTypeComboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                SelectedEventType = selectedItem.Content.ToString();
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
