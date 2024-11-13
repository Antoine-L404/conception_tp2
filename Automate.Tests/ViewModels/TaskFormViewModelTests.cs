using Automate.Abstract.Utils;
using Automate.Utils.Enums;
using Automate.ViewModels;
using Moq;
using System.ComponentModel;
using System.Windows;

namespace Automate.Tests.ViewModels
{
    [TestClass]
    public class TaskFormViewModelTests
    {
        private TaskFormViewModel? taskFormViewModel;
        private Mock<Window>? mockWindow;
        private Mock<INavigationUtils>? mockNavigationUtils;
        private Mock<PropertyChangedEventHandler>? propertyChangedMock;

        [TestInitialize]
        public void TestInitialize()
        {
            Thread thread = new(() =>
            {
                mockWindow = new Mock<Window>();
                mockNavigationUtils = new Mock<INavigationUtils>();
                propertyChangedMock = new Mock<PropertyChangedEventHandler>();

                taskFormViewModel = new TaskFormViewModel(mockWindow.Object, new DateTime(), mockNavigationUtils.Object);

                taskFormViewModel.PropertyChanged += propertyChangedMock.Object;
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
        public void SetEventType_WhenErrorCollectionHasErrorsAndNewValueIsValid_RemovesErrors()
        {
            taskFormViewModel!.AddTask();
            const EventType eventType = EventType.Recolte;

            taskFormViewModel!.SelectedEventType = eventType;

            Assert.AreEqual(false, taskFormViewModel!.HasErrors);
        }

        [TestMethod]
        public void SetEventType_WhenErrorCollectionHasErrorsAndNewValueIsValid_OnPropertyChangedIsInvoked()
        {
            const string argPropertyName = "ErrorMessages";

            taskFormViewModel!.AddTask();
            const EventType eventType = EventType.Recolte;

            taskFormViewModel!.SelectedEventType = eventType;

            propertyChangedMock!.Verify(x =>
                x.Invoke(It.IsAny<object>(), It.Is<PropertyChangedEventArgs>(args => args.PropertyName == argPropertyName)),
                Times.AtLeastOnce()
            );
        }

        [TestMethod]
        public void AddTask_EventTypeIsInvalid_ErrorCollectionHasErrors()
        {
            taskFormViewModel!.AddTask();

            Assert.AreEqual(true, taskFormViewModel.HasErrors);
        }

        [TestMethod]
        public void AddTask_EventTypeIsInvalid_OnPropertyChangedIsInvoked()
        {
            const string argPropertyName = "ErrorMessages";

            taskFormViewModel!.AddTask();

            propertyChangedMock!.Verify(x =>
                x.Invoke(It.IsAny<object>(), It.Is<PropertyChangedEventArgs>(args => args.PropertyName == argPropertyName)),
                Times.Once()
            );
        }

        [TestMethod]
        public void Cancel_CloseWindow()
        {
            taskFormViewModel!.Cancel();

            mockNavigationUtils!.Verify(x => x.Close(It.IsAny<Window>()), Times.Once());
        }
    }
}
