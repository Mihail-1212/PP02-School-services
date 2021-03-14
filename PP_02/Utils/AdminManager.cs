using PP_02.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PP_02.Utils
{
    public class AdminManager : INotifyPropertyChanged
    {
        /* Хранит объект администратора, если был вход */

        #region fields

        private static AdminManager _context;
        private Admin _admin;

        #endregion

        #region getters

        public static AdminManager Context
        {
            get
            {
                if (_context == null)
                {
                    _context = new AdminManager();
                }
                return _context;
            }
        }

        public Admin Admin
        {
            get
            {
                return this._admin;
            }
            private set
            {
                this._admin = value;
                OnPropertyChanged();
            }
        }

        protected void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        public bool IsAuth
        {
            get
            {
                return Admin != null;
            }
            set { }
        }


        #endregion

        #region public methods

        public bool TryLogin(String code)
        {
            var admin = PP_02Entities.GetContext().Admin.ToList().FirstOrDefault(v => v.code == code);
            Admin = admin;
            UpdateGetters();
            return admin != null;
        }

        public void TryLogout()
        {
            Admin = null;
            UpdateGetters();
        }
        #endregion

        #region self methods

        void UpdateGetters()
        {
            OnPropertyChanged("Admin");
            OnPropertyChanged("IsAuth");
        }

        #endregion

        #region events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}
