using Automate.Abstract.Services;
using Automate.Abstract.Utils;
using Automate.Abstract.ViewModels;
using Automate.Models;
using Automate.Utils.Enums;
using MongoDB.Bson;
using MongoDB.Driver;
using Moq;
using System.ComponentModel;
using System.Windows;

namespace Automate.Tests.ViewModels
{
    [TestClass]
    public class CalendarViewModelTests
    {
        private readonly string noEvenTitle = "Aucun événement";

        private CalendarViewModel? calendarViewModel;
        private Mock<ITasksServices>? tasksServicesMock;
        private Mock<INavigationUtils>? navigationUtilsMock;
        private Mock<ITaskFormViewModel>? taskFormViewModelMock;
        private Mock<PropertyChangedEventHandler>? propertyChangedMock;
        private Mock<Window>? windowMock;

        [TestInitialize]
        public void TestInitialize()
        {
            Thread thread = new(() =>
            {
                tasksServicesMock = new Mock<ITasksServices>();
                tasksServicesMock.Setup(x => x.GetTasksByDate(It.IsAny<DateTime>())).Returns(new List<UpcomingTask>());

                navigationUtilsMock = new Mock<INavigationUtils>();
                taskFormViewModelMock = new Mock<ITaskFormViewModel>();
                propertyChangedMock = new Mock<PropertyChangedEventHandler>();
                windowMock = new Mock<Window>();

                calendarViewModel = new CalendarViewModel(windowMock.Object,
                    null!, tasksServicesMock.Object, navigationUtilsMock.Object);

                calendarViewModel.PropertyChanged += propertyChangedMock.Object;
            });

            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();
        }

        [TestMethod]
        public void DateSelected_SelectedDateInvalid_AddErrorToErrorsCollection()
        {
            calendarViewModel!.DateSelected();

            Assert.IsTrue(calendarViewModel!.HasErrors);
        }

        [TestMethod]
        public void DateSelected_SelectedDateInvalid_OnPropertyChangedIsInvoked()
        {
            const string argPropertyName = "ErrorMessages";

            calendarViewModel!.DateSelected();

            propertyChangedMock!.Verify(x =>
                x.Invoke(It.IsAny<object>(), It.Is<PropertyChangedEventArgs>(args => args.PropertyName == argPropertyName)),
                Times.Once()
            );
        }

        [TestMethod]
        public void DateSelected_SelectedDateContainsNoTask_EventTitlesCountIsOne()
        {
            const int expectedCount = 1;
            calendarViewModel!.SelectedDate = DateTime.Today;
            tasksServicesMock!.Setup(x => x.GetTasksByDate(It.IsAny<DateTime>())).Returns(new List<UpcomingTask>());

            calendarViewModel.DateSelected();

            Assert.AreEqual(expectedCount, calendarViewModel.EventTitles.Count);
        }

        [TestMethod]
        public void DateSelected_SelectedDateContainsNoTask_EventTitlesValueIsCorrect()
        {
            calendarViewModel!.SelectedDate = DateTime.Today;
            tasksServicesMock!.Setup(x => x.GetTasksByDate(It.IsAny<DateTime>())).Returns(new List<UpcomingTask>());

            calendarViewModel.DateSelected();

            Assert.AreEqual(noEvenTitle, calendarViewModel.EventTitles[0]);
        }

        [TestMethod]
        public void DateSelected_SelectedDateContainsOneTask_EventTitlesCountIsOne()
        {
            const int expectedCount = 1;
            SetupForValidDateSelectedOneTask();

            calendarViewModel!.DateSelected();

            Assert.AreEqual(expectedCount, calendarViewModel.EventTitles.Count);
        }

        [TestMethod]
        public void DateSelected_SelectedDateContainsOneTask_EventTitlesValueIsCorrect()
        {
            string expectedValue = EventType.Semis.ToString();
            SetupForValidDateSelectedOneTask();

            calendarViewModel!.DateSelected();

            Assert.AreEqual(expectedValue, calendarViewModel.EventTitles[0]);
        }

        [TestMethod]
        public void DateSelected_SelectedDateContainsManyTasks_EventTitlesCountIsCorrect()
        {
            const int expectedCount = 2;
            SetupForValidDateSelectedManyTasks();

            calendarViewModel!.DateSelected();

            Assert.AreEqual(expectedCount, calendarViewModel.EventTitles.Count);
        }

        [TestMethod]
        public void DateSelected_SelectedDateContainsManyTasks_EventTitlesValuesAreCorrect()
        {
            string expectedTask1 = EventType.Semis.ToString();
            string expectedTask2 = EventType.Entretien.ToString();
            SetupForValidDateSelectedManyTasks();

            calendarViewModel!.DateSelected();

            Assert.AreEqual(expectedTask1, calendarViewModel.EventTitles[0]);
            Assert.AreEqual(expectedTask2, calendarViewModel.EventTitles[1]);
        }

        [TestMethod]
        public void AddTask_SelectedDateInvalid_AddErrorToErrorsCollection()
        {
            calendarViewModel!.AddTask();

            Assert.IsTrue(calendarViewModel!.HasErrors);
        }

        [TestMethod]
        public void AddTask_SelectedDateInvalid_OnPropertyChangedIsInvoked()
        {
            const string argPropertyName = "ErrorMessages";

            calendarViewModel!.AddTask();

            propertyChangedMock!.Verify(x =>
                x.Invoke(It.IsAny<object>(), It.Is<PropertyChangedEventArgs>(args => args.PropertyName == argPropertyName)),
                Times.Once()
            );
        }

        [TestMethod]
        public void AddTask_SelectedDateIsValid_CallTasksServices()
        {
            SetupForValidAddTask();

            calendarViewModel!.AddTask();

            tasksServicesMock!.Verify(x => x.CreateTask(It.IsAny<UpcomingTask>()), Times.Once());
        }

        [TestMethod]
        public void AddTask_SelectedDateIsValid_EventTitlesCountIsOne()
        {
            const int expectedCount = 1;
            SetupForValidAddTask();

            calendarViewModel!.AddTask();

            Assert.AreEqual(expectedCount, calendarViewModel.EventTitles.Count);
        }

        [TestMethod]
        public void AddTask_SelectedDateIsValid_EventTitlesValueIsCorrect()
        {
            string expectedValue = EventType.Semis.ToString();
            SetupForValidAddTask();

            calendarViewModel!.AddTask();

            Assert.AreEqual(expectedValue, calendarViewModel.EventTitles[0]);
        }

        [TestMethod]
        public void AddTask_SelectedDateValid_OnPropertyChangedIsInvoked()
        {
            const string argPropertyName = "SuccessMessage";
            SetupForValidAddTask();

            calendarViewModel!.AddTask();

            propertyChangedMock!.Verify(x =>
                x.Invoke(It.IsAny<object>(), It.Is<PropertyChangedEventArgs>(args => args.PropertyName == argPropertyName)),
                Times.AtLeastOnce()
            );
        }

        [TestMethod]
        public void EditTask_SelectedDateInvalid_AddErrorToErrorsCollection()
        {
            calendarViewModel!.EditTask();

            Assert.IsTrue(calendarViewModel!.HasErrors);
        }

        [TestMethod]
        public void EditTask_SelectedDateInvalid_OnPropertyChangedIsInvoked()
        {
            const string argPropertyName = "ErrorMessages";

            calendarViewModel!.EditTask();

            propertyChangedMock!.Verify(x =>
                x.Invoke(It.IsAny<object>(), It.Is<PropertyChangedEventArgs>(args => args.PropertyName == argPropertyName)),
                Times.Once()
            );
        }

        [TestMethod]
        public void EditTask_SelectedEventTitleInvalid_AddErrorToErrorsCollection()
        {
            calendarViewModel!.SelectedDate = DateTime.Today;

            calendarViewModel.EditTask();

            Assert.IsTrue(calendarViewModel!.HasErrors);
        }

        [TestMethod]
        public void EditTask_SelectedEventTitleInvalid_OnPropertyChangedIsInvoked()
        {
            const string argPropertyName = "ErrorMessages";
            calendarViewModel!.SelectedDate = DateTime.Today;

            calendarViewModel.EditTask();

            propertyChangedMock!.Verify(x =>
                x.Invoke(It.IsAny<object>(), It.Is<PropertyChangedEventArgs>(args => args.PropertyName == argPropertyName)),
                Times.AtLeastOnce()
            );
        }

        [TestMethod]
        public void EditTask_TaskToEditInvalid_AddErrorToErrorsCollection()
        {
            calendarViewModel!.SelectedEventTitle = EventType.Semis.ToString();
            calendarViewModel.SelectedDate = DateTime.Today;

            calendarViewModel.EditTask();

            Assert.IsTrue(calendarViewModel!.HasErrors);
        }

        [TestMethod]
        public void EditTask_TaskToEditInvalid_OnPropertyChangedIsInvoked()
        {
            const string argPropertyName = "ErrorMessages";
            calendarViewModel!.SelectedDate = DateTime.Today;
            calendarViewModel.SelectedEventTitle = EventType.Entretien.ToString();

            calendarViewModel.EditTask();

            propertyChangedMock!.Verify(x =>
                x.Invoke(It.IsAny<object>(), It.Is<PropertyChangedEventArgs>(args => args.PropertyName == argPropertyName)),
                Times.AtLeastOnce()
            );
        }

        [TestMethod]
        public void EditTask_TaskToEditValid_RemoveErrorsFromErrorsCollection()
        {
            SetupForValidEditTask();

            calendarViewModel!.EditTask();

            Assert.IsFalse(calendarViewModel!.HasErrors);
        }

        [TestMethod]
        public void EditTask_TaskToEditValid_CallTasksServices()
        {
            SetupForValidEditTask();

            calendarViewModel!.EditTask();

            tasksServicesMock!.Verify(
                x => x.UpdateTask(It.IsAny<ObjectId>(), It.IsAny<UpdateDefinition<UpcomingTask>>()), Times.Once());
        }

        [TestMethod]
        public void EditTask_SelectedDateIsValid_EventTitlesCountIsOne()
        {
            const int expectedCount = 1;
            SetupForValidEditTask();

            calendarViewModel!.EditTask();

            Assert.AreEqual(expectedCount, calendarViewModel.EventTitles.Count);
        }

        [TestMethod]
        public void EditTask_SelectedDateIsValid_EventTitlesValueIsCorrect()
        {
            string expectedValue = EventType.Semis.ToString();
            SetupForValidEditTask();

            calendarViewModel!.EditTask();

            Assert.AreEqual(expectedValue, calendarViewModel.EventTitles[0]);
        }

        [TestMethod]
        public void EditTask_SelectedDateValid_OnPropertyChangedIsInvoked()
        {
            const string argPropertyName = "SuccessMessage";
            SetupForValidEditTask();

            calendarViewModel!.EditTask();

            propertyChangedMock!.Verify(x =>
                x.Invoke(It.IsAny<object>(), It.Is<PropertyChangedEventArgs>(args => args.PropertyName == argPropertyName)),
                Times.AtLeastOnce()
            );
        }

        [TestMethod]
        public void DeleteTask_SelectedDateInvalid_AddErrorToErrorsCollection()
        {
            calendarViewModel!.DeleteTask();

            Assert.IsTrue(calendarViewModel!.HasErrors);
        }

        [TestMethod]
        public void DeleteTask_SelectedDateInvalid_OnPropertyChangedIsInvoked()
        {
            const string argPropertyName = "ErrorMessages";

            calendarViewModel!.DeleteTask();

            propertyChangedMock!.Verify(x =>
                x.Invoke(It.IsAny<object>(), It.Is<PropertyChangedEventArgs>(args => args.PropertyName == argPropertyName)),
                Times.Once()
            );
        }

        [TestMethod]
        public void DeleteTask_SelectedEventTitleInvalid_AddErrorToErrorsCollection()
        {
            calendarViewModel!.SelectedDate = DateTime.Today;

            calendarViewModel.DeleteTask();

            Assert.IsTrue(calendarViewModel!.HasErrors);
        }

        [TestMethod]
        public void DeleteTask_SelectedEventTitleInvalid_OnPropertyChangedIsInvoked()
        {
            const string argPropertyName = "ErrorMessages";
            calendarViewModel!.SelectedDate = DateTime.Today;

            calendarViewModel.DeleteTask();

            propertyChangedMock!.Verify(x =>
                x.Invoke(It.IsAny<object>(), It.Is<PropertyChangedEventArgs>(args => args.PropertyName == argPropertyName)),
                Times.AtLeastOnce()
            );
        }

        [TestMethod]
        public void DeleteTask_TaskToDeleteInvalid_AddErrorToErrorsCollection()
        {
            calendarViewModel!.SelectedEventTitle = EventType.Semis.ToString();
            calendarViewModel.SelectedDate = DateTime.Today;

            calendarViewModel.DeleteTask();

            Assert.IsTrue(calendarViewModel!.HasErrors);
        }

        [TestMethod]
        public void DeleteTask_TaskToDeleteInvalid_OnPropertyChangedIsInvoked()
        {
            const string argPropertyName = "ErrorMessages";
            calendarViewModel!.SelectedEventTitle = EventType.Semis.ToString();
            calendarViewModel.SelectedDate = DateTime.Today;

            calendarViewModel.DeleteTask();

            propertyChangedMock!.Verify(x =>
                x.Invoke(It.IsAny<object>(), It.Is<PropertyChangedEventArgs>(args => args.PropertyName == argPropertyName)),
                Times.AtLeastOnce()
            );
        }

        [TestMethod]
        public void DeleteTask_TaskToDeleteValid_RemoveErrorsFromErrorsCollection()
        {
            SetupForValidDeleteTask();

            calendarViewModel!.DeleteTask();

            Assert.IsFalse(calendarViewModel!.HasErrors);
        }

        [TestMethod]
        public void DeleteTask_TaskToDeleteValid_CallTasksServices()
        {
            SetupForValidDeleteTask();

            calendarViewModel!.DeleteTask();

            tasksServicesMock!.Verify(x => x.DeleteTask(It.IsAny<ObjectId>()), Times.Once());
        }

        [TestMethod]
        public void DeleteTask_SelectedDateValid_OnPropertyChangedIsInvoked()
        {
            const string argPropertyName = "SuccessMessage";
            SetupForValidDeleteTask();

            calendarViewModel!.DeleteTask();

            propertyChangedMock!.Verify(x =>
                x.Invoke(It.IsAny<object>(), It.Is<PropertyChangedEventArgs>(args => args.PropertyName == argPropertyName)),
                Times.AtLeastOnce()
            );
        }

        private void SetupForValidDateSelectedOneTask()
        {
            UpcomingTask returnedTask = new UpcomingTask() { Title = EventType.Semis };

            calendarViewModel!.SelectedDate = DateTime.Today;
            tasksServicesMock!.Setup(x => x.GetTasksByDate(It.IsAny<DateTime>()))
                .Returns(new List<UpcomingTask>() { returnedTask });
        }

        private void SetupForValidDateSelectedManyTasks()
        {
            UpcomingTask returnedTask1 = new UpcomingTask() { Title = EventType.Semis };
            UpcomingTask returnedTask2 = new UpcomingTask() { Title = EventType.Entretien };

            calendarViewModel!.SelectedDate = DateTime.Today;
            tasksServicesMock!.Setup(x => x.GetTasksByDate(It.IsAny<DateTime>()))
                .Returns(new List<UpcomingTask>() { returnedTask1, returnedTask2 });
        }

        private void SetupForValidAddTask()
        {
            SetupForValidDateSelectedOneTask();

            DateTime selectedDate = DateTime.Today;
            calendarViewModel!.SelectedDate = selectedDate;

            taskFormViewModelMock!.Setup(x => x.SelectedEventType).Returns(EventType.Semis);
            navigationUtilsMock!.Setup(x => x.GetTaskFormValues(selectedDate, null)).Returns(taskFormViewModelMock.Object);
        }

        private void SetupForValidEditTask()
        {
            SetupForValidDateSelectedOneTask();

            DateTime selectedDate = DateTime.Today;

            calendarViewModel!.SelectedEventTitle = EventType.Semis.ToString();
            calendarViewModel.SelectedDate = selectedDate;

            UpcomingTask existingTask = new UpcomingTask() { Title = EventType.Semis };
            calendarViewModel!.SelectedDate = selectedDate;
            tasksServicesMock!.Setup(x => x.GetTasksByDate(It.IsAny<DateTime>()))
                .Returns(new List<UpcomingTask>() { existingTask });

            taskFormViewModelMock!.Setup(x => x.SelectedEventType).Returns(EventType.Arrosage);
            navigationUtilsMock!.Setup(
                x => x.GetTaskFormValues(selectedDate, It.IsAny<EventType>())).Returns(taskFormViewModelMock.Object);

            calendarViewModel.DateSelected();
        }

        private void SetupForValidDeleteTask()
        {
            SetupForValidDateSelectedOneTask();

            DateTime selectedDate = DateTime.Today;

            calendarViewModel!.SelectedEventTitle = EventType.Semis.ToString();
            calendarViewModel.SelectedDate = selectedDate;

            UpcomingTask existingTask = new UpcomingTask() { Title = EventType.Semis };
            calendarViewModel!.SelectedDate = selectedDate;
            tasksServicesMock!.Setup(x => x.GetTasksByDate(It.IsAny<DateTime>()))
                .Returns(new List<UpcomingTask>() { existingTask });

            calendarViewModel.DateSelected();
        }
    }
}
