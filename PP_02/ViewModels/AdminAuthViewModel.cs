using PP_02.MVVM;
using PP_02.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PP_02.ViewModels
{
    class AdminAuthViewModel : BaseViewModel
    {
        #region fields

        private String _code;

        #endregion

        #region getters

        public String Code
        {
            get
            {
                return this._code;
            }
            set
            {
                this._code = value;
                this.OnPropertyChanged();
            }
        }

        #endregion

        #region commands

        public RelayCommand EnterCommand { get; private set; }

        #endregion

        public AdminAuthViewModel()
        {
            this.EnterCommand = new RelayCommand(v => this.Enter());
        }

        #region self methods

        private void Enter()
        {
            if (AdminManager.Context.TryLogin(this.Code))
                MessageBoxShow.SuccessMessage("Вы успешно авторизовались");
            else
                MessageBoxShow.ErrorMessage("Ошибка авторизации!");
            this.Code = "";
        }

        #endregion
    }
}
