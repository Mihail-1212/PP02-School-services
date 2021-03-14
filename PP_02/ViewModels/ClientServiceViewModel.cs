using PP_02.Models;
using PP_02.MVVM;
using PP_02.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PP_02.ViewModels
{
    class ClientServiceViewModel : BaseViewModel
    {
        #region fields

        private ClientService _clientService;

        #endregion

        #region getters

        public List<Service> ServiceList
        {
            get
            {
                return PP_02Entities.GetContext().Service.ToList();
            }
        }

        public List<Client> ClientList
        {
            get
            {
                return PP_02Entities.GetContext().Client.ToList();
            }
        }

        public ClientService ClientService
        {
            get
            {
                return this._clientService;
            }
            set
            {
                this._clientService = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region commands

        public RelayCommand SaveChangesCommand { get; private set; }

        #endregion

        public ClientServiceViewModel(Service service)
        {
            this.SaveChangesCommand = new RelayCommand(v => SaveChanges(), v => IsValid(v as DependencyObject));
            UpdatePropertyChanged();
            ClientService = new ClientService();
            ClientService.Service = service;
            UpdatePropertyChanged();
        }

        #region self methods

        void SaveChanges()
        {
            if (this.ClientService.GetClient == null || this.ClientService.GetService == null)
            {
                MessageBoxShow.ErrorMessage("Вы не выбрали услугу или клиента!");
                return;
            }
            try
            {
                PP_02Entities.GetContext().ClientService.Add(this.ClientService);
                PP_02Entities.GetContext().SaveChanges();
                MessageBoxShow.SuccessMessage("Услуга успешно записана");
                FrameManager.Context.GoBackViewModel();
            }
            catch
            {
                PP_02Entities.GetContext().Update();
                MessageBoxShow.ErrorMessage("Произошла ошибка при записи услуги");
            }
            finally
            {
                UpdatePropertyChanged();
            }
        }

        void UpdatePropertyChanged()
        {
            OnPropertyChanged("ClientService");
            OnPropertyChanged("ClientList");
            OnPropertyChanged("ServiceList");
        }

        #endregion
    }
}
