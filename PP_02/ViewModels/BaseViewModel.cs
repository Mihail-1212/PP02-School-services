using PP_02.MVVM;
using PP_02.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace PP_02.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        /* Базовая ViewModel */

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string pop="")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(pop));
        }

        protected bool IsValid(DependencyObject obj)
        {
            if (obj == null)
            {
                return false;
            }
                
            return !Validation.GetHasError(obj) &&
                LogicalTreeHelper.GetChildren(obj).OfType<DependencyObject>().All(IsValid);
        }
    }
}
