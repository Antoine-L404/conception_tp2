using Automate.Utils;
using Automate.Utils.Enums;
using Automate.ViewModels;
using Moq;
using System.ComponentModel;
using System.Windows;

namespace Automate.Tests.ViewModels
{
    [TestClass]
    public class TaskViewModelTests
    {
        private TaskFormViewModel? taskFormViewModel;
        private Mock<Window>? mockWindow;
        private Mock<PropertyChangedEventHandler>? mockPropertyChanged;

        [TestInitialize]
        public void TestInitialize()
        {
            Thread thread = new(() =>
            {
                mockWindow = new Mock<Window>();
                mockPropertyChanged = new Mock<PropertyChangedEventHandler>();
                taskFormViewModel = new TaskFormViewModel(mockWindow.Object, new DateTime(), new NavigationUtils());

                taskFormViewModel.PropertyChanged += mockPropertyChanged.Object;
            });

            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();
        }

        [TestMethod]
        public void SetEventType_ValueIsValid_EventTypeIsCorrectlySet()
        {
            const EventType eventType = EventType.Recolte;

            taskFormViewModel!.SelectedEventType = eventType;

            Assert.AreEqual(eventType, taskFormViewModel.SelectedEventType);
        }

        [TestMethod]
        public void SetEventDate_ValueIsValid_EventDateIsCorrectlySet()
        {
            DateTime eventdate = DateTime.Now;

            taskFormViewModel!.EventDate = eventdate.ToShortDateString();

            Assert.Equals(eventdate, taskFormViewModel.EventDate);
        }
    }
}
