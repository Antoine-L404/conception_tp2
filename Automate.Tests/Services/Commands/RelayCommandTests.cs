using Automate.Services.Commands;

namespace Automate.Tests.Services.Commands
{
    [TestClass]
    public class RelayCommandTests
    {
        private readonly Action<object>? executeWithParam = (obj) => { };
        private readonly Action? executeWithoutParam = () => { };
        private readonly Func<object, bool>? canExecuteWithParam = (obj) => false;
        private readonly Func<bool>? canExecuteWithoutParam = () => true;

        private readonly RelayCommand relayCommand;

        public RelayCommandTests()
        {
            relayCommand = new RelayCommand(executeWithParam);
        }

        [TestMethod]
        public void CanExecute()
        {
            var result = relayCommand.CanExecute(null);
        }
    }
}
