using PP_02.Models;
using PP_02.MVVM;
using PP_02.Utils;
using PP_02.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace PP_02.ViewModels
{
    class MainWindowViewModel : BaseViewModel
    {
        /* ViewModel, которая привязана к окну приложения */

        #region commands

        public RelayCommand GoBackPageCommand { get; private set; }
        public RelayCommand LogoutAdminCommand { get; private set; }

        #endregion

        public MainWindowViewModel()
        {
            FrameManager.Context.AddViewModel(new MainViewModel());         // Задается главная ViewModel
            this.GoBackPageCommand = new RelayCommand(v => GoBackPage());
            this.LogoutAdminCommand = new RelayCommand(v => this.LogoutAdmin());
        }

        #region self methods

        private void GoBackPage()
        {
            PP_02Entities.GetContext().Update();
            FrameManager.Context.GoBackViewModel();
        }

        private void LogoutAdmin()
        {
            AdminManager.Context.TryLogout();
            FrameManager.Context.GoMainViewModel();
        }

        #endregion
    }
}
