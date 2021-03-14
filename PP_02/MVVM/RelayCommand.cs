using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PP_02.MVVM
{
    public class RelayCommand : ICommand
    {
        #region private fields

        Action<object> execute;
        Func<object, bool> canExecute;

        #endregion

        public RelayCommand (Action<object> execute, Func<object, bool> canExecute=null)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        #region ICommand members

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested += value; }
        }

        public bool CanExecute(object parameter)
        {
            return this.canExecute == null || this.canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            this.execute(parameter);
        }

        #endregion
    }
}
