using PP_02.ViewModels;
using PP_02.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace PP_02.Utils
{
    public class FrameManager : INotifyPropertyChanged
    {
        /* Класс отвечает за фреймы */
        #region fields

        private static FrameManager _context;
        private List<BaseViewModel> viewModels = new List<BaseViewModel>(); // Список всех окон в программе

        #endregion

        #region getters

        /// <summary>
        /// Возвращает контекст класса
        /// </summary>
        public static FrameManager Context
        {
            get
            {
                if (_context == null)
                    _context = new FrameManager();
                return _context;
            }
        }

        /// <summary>
        /// Возвращает текущую страницу
        /// </summary>
        public BaseViewModel CurrentViewModel
        {
            get
            {
                return viewModels.Last();
            }
            set {}
        }

        /// <summary>
        /// Возвращает bool значение возможности перехода на страницу назад
        /// </summary>
        public bool CanGoBack
        {
            get
            {
                return viewModels.Count > 1;
            }
            set { }
        }

        #endregion

        #region public methods

        /// <summary>
        /// Добавляет новую страницу и переходит на нее
        /// </summary>
        /// <param name="viewModel"></param>
        public void AddViewModel(BaseViewModel viewModel)
        {
            viewModels.Add(viewModel);
            AllPropertyChanged();
        }

        /// <summary>
        /// Возвращается на страницу назад если это возможно
        /// </summary>
        public void GoBackViewModel()
        {
            if (viewModels.Count != 0)
            {
                viewModels.RemoveAt(viewModels.Count - 1);
                AllPropertyChanged();
            }
        }

        /// <summary>
        /// Возвращается на главную страницу
        /// </summary>
        public void GoMainViewModel()
        {
            while (CanGoBack)
            {
                GoBackViewModel();
            }
        }

        #endregion

        #region self methods

        protected virtual void OnPropertyChanged([CallerMemberName]string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Обновляет все getters
        /// </summary>
        private void AllPropertyChanged()
        {
            OnPropertyChanged("CurrentViewModel");
            OnPropertyChanged("CanGoBack");
        }

        #endregion

        #region events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}
