using PP_02.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PP_02.Utils;
using System.Windows.Controls;
using PP_02.Models;
using PP_02.Views;

namespace PP_02.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        /* ViewModel, которая привязана к главной странице приложения - списку услуг */

        #region fields

        private Sort? _currentSort;
        private DiscountItem? _currentDiscountItem;
        private String _searchText;
        private String _sample;

        #endregion

        #region getters

        public List<Service> ServiceList
        {
            get
            {
                var services = PP_02Entities.GetContext().Service.ToList();
                int servicesTotalCount = services.Count;
                if (CurrentSort != null)
                {
                    switch (CurrentSort)
                    {
                        case Sort.asc:
                            services = services.OrderBy(v => v.Cost).ToList();
                            break;
                        case Sort.desc:
                            services = services.OrderByDescending(v => v.Cost).ToList();
                            break;
                    }
                }
                if (CurrentDiscountItem != null)
                {
                    var minDiscount = CurrentDiscountItem.Value.Value.Item1;
                    var maxDiscount = CurrentDiscountItem.Value.Value.Item2;
                    services = services.Where(v => 
                    {
                        var discountPerc = (v.Discount??0) * 100;
                        return discountPerc >= minDiscount && discountPerc <= maxDiscount;
                    }).ToList();
                }
                if (!String.IsNullOrWhiteSpace(SearchText))
                {
                    services = services
                        .Where(v => v.Title.ToLower().Contains(SearchText.ToLower()) 
                        || v.Description.ToLower().Contains(SearchText.ToLower())).ToList();
                }
                this.Sample = $"{services.Count} из {servicesTotalCount}";
                return services;
            }
        }

        public List<DiscountItem> DiscountList
        {
            get
            {
                var discountList = new List<(int, int)>()
                {
                    (0, 100),
                    (0, 4),
                    (5, 14),
                    (15, 29),
                    (30, 69),
                    (70, 99)
                };
                return discountList.Select(v => new DiscountItem(v)).ToList();
            }
        }

        public Sort? CurrentSort
        {
            get
            {
                return this._currentSort;
            }
            set
            {
                this._currentSort = value;
                this.UpdatePropertyChanged();
            }
        }

        public DiscountItem? CurrentDiscountItem
        {
            get
            {
                return this._currentDiscountItem;
            }
            set
            {
                this._currentDiscountItem = value;
                UpdatePropertyChanged();
            }
        }

        public String SearchText
        {
            get
            {
                return this._searchText;
            }
            set
            {
                this._searchText = value;
                this.UpdatePropertyChanged();
            }
        }

        public String Sample
        {
            get
            {
                return this._sample;
            }
            set 
            {
                this._sample = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region commands

        public RelayCommand ChangeCreateServiceCommand { get; private set; }
        public RelayCommand ChangeEditServiceCommand { get; private set; }
        public RelayCommand ServiceDeleteCommand { get; private set; }
        public RelayCommand ChangeClientServiceListCommand { get; private set; }

        #endregion

        public MainViewModel ()
        {
            this.ChangeCreateServiceCommand = new RelayCommand(v => ChangeEditService());
            this.ChangeEditServiceCommand = new RelayCommand(v => ChangeEditService(v as Service));
            this.ServiceDeleteCommand = new RelayCommand(v => ServiceDelete(v as Service));
            this.ChangeClientServiceListCommand = new RelayCommand(v => ChangeClientServiceList());
            this.CurrentSort = Sort.asc;
            this.CurrentDiscountItem = DiscountList.First();
        }

        #region self methods

        private void ChangeClientServiceList()
        {
            FrameManager.Context.AddViewModel(new ClientServiceListViewModel());
        }

        private void ChangeEditService(Service service = null)
        {
            FrameManager.Context.AddViewModel(new EditServiceViewModel(service));
        }

        /// <summary>
        /// Вынес удаление в отдельный метод 
        /// </summary>
        public Action ServiceDeleteFunc(Service service)
        {
            if (!AdminManager.Context.IsAuth)
            {
                throw new Exception("Пользователь не авторизован как админ");
            }
            if (service.ClientService.ToList().Count != 0)
            {
                throw new Exception("Вы не можете удалить услуги, у которых есть записи");
            }
            // Проверка нужно ли удалять доп изображения и вызов функции их удаления
            var servicePhoto = service.ServicePhoto.ToList();
            if (servicePhoto.Count != 0 && MessageBoxShow.QuestionMessage($"Хотите также удалить {servicePhoto.Count} дополнительных фотографий услуги?"))
            {
                PP_02Entities.GetContext().ServicePhoto.RemoveRange(servicePhoto);
                try
                {
                    PP_02Entities.GetContext().SaveChanges();
                }
                catch
                {
                    throw new Exception("Произошла ошибка при удалении дополнительных фотографий услуги");
                }
            }
            try
            {
                PP_02Entities.GetContext().Service.Remove(service);
                PP_02Entities.GetContext().SaveChanges();
                return () => MessageBoxShow.SuccessMessage("Услуга успешно удалена!");
            }
            catch
            {
                throw new Exception("Произошла ошибка при удалении услуги!");
            }
            finally
            {
                UpdatePropertyChanged();
                PP_02Entities.GetContext().Update();
            }
        }

        /// <summary>
        /// Метод удаления услуги
        /// </summary>
        /// <param name="service">Объект услуги</param>
        private void ServiceDelete(Service service)
        {
            if (MessageBoxShow.QuestionMessage($"Вы точно хотите удалить данную услугу с ID {service.ID}?"))
            {
                try
                {
                    ServiceDeleteFunc(service)();
                }
                catch(Exception ex)
                {
                    MessageBoxShow.ErrorMessage(ex.Message);
                }
                
            }
        }

        private void UpdatePropertyChanged()
        {
            OnPropertyChanged("CurrentSort");
            OnPropertyChanged("ServiceList");
            OnPropertyChanged("CurrentDiscountItem");
            OnPropertyChanged("Sample");
        }

        #endregion

        #region struct

        public struct DiscountItem
        {
            public String Label
            {
                get
                {
                    if (Value.Item1 == 0 && Value.Item2 == 100)
                    {
                        return "Все элементы";
                    }
                    return $"от {Value.Item1}% до {Value.Item2 + 1}%";
                }
            }

            public (int, int) Value { get; private set; }

            public DiscountItem((int, int) value)
            {
                this.Value = value;
            }
        }

        #endregion
    }
}
