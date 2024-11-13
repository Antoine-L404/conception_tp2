using Automate.Abstract.Services;
using Automate.Abstract.Utils;
using Automate.Abstract.ViewModels;
using Automate.Models;
using Automate.Utils.Enums;
using Automate.ViewModels;
using MongoDB.Bson;
using MongoDB.Driver;
using Moq;
using System.Windows;
using System.Windows.Controls;

namespace Automate.Tests.ViewModels
{
    [TestClass]
    public class CalendarViewModelTests
    {
        private readonly string noEvenTitle = "Aucun événement";

        private CalendarViewModel? calendarViewModel;
        private Mock<ITasksServices>? tasksServicesMock;
        private Mock<Calendar>? calendarMock;
        private Mock<INavigationUtils>? navigationUtilsMock;
        private Mock<Window>? windowMock;
        private Mock<ITaskFormViewModel>? taskFormViewModelMock;

        [TestInitialize]
        public void TestInitialize()
        {
            Thread thread = new(() =>
            {
                tasksServicesMock = new Mock<ITasksServices>();
                tasksServicesMock.Setup(x => x.GetTasksByDate(It.IsAny<DateTime>())).Returns(new List<UpcomingTask>());

                calendarMock = new Mock<Calendar>();
                navigationUtilsMock = new Mock<INavigationUtils>();
                windowMock = new Mock<Window>();
                taskFormViewModelMock = new Mock<ITaskFormViewModel>();

                calendarViewModel = new CalendarViewModel(
                    null, tasksServicesMock.Object, navigationUtilsMock.Object);
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
        public void EditTask_SelectedDateInvalid_AddErrorToErrorsCollection()
        {
            calendarViewModel!.EditTask();

            Assert.IsTrue(calendarViewModel!.HasErrors);
        }

        [TestMethod]
        public void EditTask_SelectedEventTitleInvalid_AddErrorToErrorsCollection()
        {
            calendarViewModel!.EditTask();

            Assert.IsTrue(calendarViewModel!.HasErrors);
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
    }
}
