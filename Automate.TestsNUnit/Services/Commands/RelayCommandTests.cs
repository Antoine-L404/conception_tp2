using Automate.Services.Commands;

namespace Automate.TestsNUnit.Services.Commands
{
    public class RelayCommandTests
    {
        private readonly Action<object> executeWithParam = (obj) => { };
        private readonly Action executeWithoutParam = () => { };
        private readonly Func<object, bool> canExecuteWithParam = (obj) => false;
        private readonly Func<bool> canExecuteWithoutParam = () => true;

        private RelayCommand relayCommand;

        [SetUp]
        public void SetUp()
        {
            relayCommand = new RelayCommand(executeWithParam);
        }

        [Test]
        public void CanExecute()
        {
            var result = relayCommand.CanExecute(null);
        }
    }
}
