using Automate.Abstract.Utils;
using Automate.Utils.Enums;
using Automate.ViewModels;
using Moq;
using System.Windows;

namespace Automate.Tests.ViewModels
{
    [TestClass]
    public class TaskFormViewModelTests
    {
        private TaskFormViewModel? taskFormViewModel;
        private Mock<Window>? mockWindow;
        private Mock<INavigationUtils>? mockNavigationUtils;

        [TestInitialize]
        public void TestInitialize()
        {
            Thread thread = new(() =>
            {
                mockWindow = new Mock<Window>();
                mockNavigationUtils = new Mock<INavigationUtils>();
                taskFormViewModel = new TaskFormViewModel(mockWindow.Object, new DateTime(), mockNavigationUtils.Object);
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
        public void SetEventType_WhenErrorCollectionHasErrors_And_NewValueIsValid_RemovesErrors()
        {
            taskFormViewModel!.AddTask();
            const EventType eventType = EventType.Recolte;

            taskFormViewModel!.SelectedEventType = eventType;

            Assert.AreEqual(false, taskFormViewModel!.HasErrors);
        }

        [TestMethod]
        public void AddTask_EventTypeIsInvalid_ErrorCollectionHasErrors()
        {
            taskFormViewModel!.AddTask();

            Assert.AreEqual(true, taskFormViewModel.HasErrors);
        }

        [TestMethod]
        public void Cancel_CloseWindow()
        {
            taskFormViewModel!.Cancel();

            mockNavigationUtils!.Verify(x => x.Close(It.IsAny<Window>()), Times.Once());
        }
    }
}
