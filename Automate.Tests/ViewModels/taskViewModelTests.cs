using Automate.Abstract.Utils;
using Automate.Utils;
using Automate.Utils.Enums;
using Automate.ViewModels;
using Automate.Views;
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
        private Mock<INavigationUtils>? mockNavigationUtils;

        [TestInitialize]
        public void TestInitialize()
        {
            Thread thread = new(() =>
            {
                mockWindow = new Mock<Window>();
                mockPropertyChanged = new Mock<PropertyChangedEventHandler>();
                taskFormViewModel = new TaskFormViewModel(mockWindow.Object, new DateTime(), new NavigationUtils());
                mockNavigationUtils = new Mock<INavigationUtils>();

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
        public void AddEvent_EventTypeIsInvalid_ErrorCollectionHasErrors()
        {
            taskFormViewModel!.AddEvent();

            Assert.AreEqual(true, taskFormViewModel.HasErrors);
        }

        [TestMethod]
        public void SetEventType_WhenErrorCollectionHasErrors_And_NewValueIsValid_RemoveErrors()
        {
            taskFormViewModel!.AddEvent();

            const EventType eventType = EventType.Recolte;

            taskFormViewModel!.SelectedEventType = eventType;

            Assert.AreEqual(false, taskFormViewModel!.HasErrors);
        }
    }
}
