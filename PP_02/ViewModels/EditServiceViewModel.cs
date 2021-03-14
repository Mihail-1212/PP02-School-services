using Microsoft.Win32;
using PP_02.Models;
using PP_02.MVVM;
using PP_02.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace PP_02.ViewModels
{
    class EditServiceViewModel : BaseViewModel
    {
        #region fields

        private Service _service;
        private OpenFileDialog _opf;

        #endregion

        #region getters

        public Service Service
        {
            get
            {
                return this._service;
            }
            set
            {
                this._service = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region commands

        public RelayCommand ServiceMainImageChangeCommand { get; private set; }
        public RelayCommand SaveServiceCommand { get; private set; }
        public RelayCommand AddAdditivePhotoCommand { get; private set; }
        public RelayCommand DeleteAdditivePhotoCommand { get; private set; }
        public RelayCommand ChangeViewClientServiceCommand { get; private set; }

        #endregion

        public EditServiceViewModel(){}

        public EditServiceViewModel(Service service = null)
        {
            this.ServiceMainImageChangeCommand = new RelayCommand(v => ServiceMainImageChange());
            this.SaveServiceCommand = new RelayCommand(v => SaveService(), v => IsValid(v as DependencyObject));
            this.AddAdditivePhotoCommand = new RelayCommand(v => AddAdditivePhoto(v as Service));
            this.DeleteAdditivePhotoCommand = new RelayCommand(v => DeleteAdditivePhoto(v as ServicePhoto));
            this.ChangeViewClientServiceCommand = new RelayCommand(v => ChangeViewClientService());
            if (service == null)
            {
                service = new Service();
            }
            this.Service = service;

            _opf = new OpenFileDialog();
            _opf.Title = "Выбор главного изображения";
            _opf.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";
        }

        #region self methods

        private void ChangeViewClientService()
        {
            FrameManager.Context.AddViewModel(new ClientServiceViewModel(Service));
        }

        private void AddAdditivePhoto(Service service)
        {
            if (_opf.ShowDialog() == true)
            {
                var additivePhoto = new ServicePhoto();
                additivePhoto.PhotoPath = ImageManager.LoadImage(_opf.FileName, Constants.ServiceAdditiveImgFolder);
                additivePhoto.ServiceID = Service.ID;
                try
                {
                    PP_02Entities.GetContext().ServicePhoto.Add(additivePhoto);
                    PP_02Entities.GetContext().SaveChanges();
                    MessageBoxShow.SuccessMessage("Изображение добавлено!");
                }
                catch
                {
                    MessageBoxShow.ErrorMessage("Ошибка сохранения изображения");
                }
                finally
                {
                    UpdatePropertyChanged();
                }
            }
        }

        private void DeleteAdditivePhoto(ServicePhoto servicePhoto)
        {
            if (MessageBoxShow.QuestionMessage("Вы точно хотите удалить данное дополнительное изображение? (Все изменения услуги также сохранятся)"))
            {
                try
                {
                    PP_02Entities.GetContext().ServicePhoto.Remove(servicePhoto);
                    PP_02Entities.GetContext().SaveChanges();
                    MessageBoxShow.SuccessMessage("Изображение удалено!");
                }
                catch
                {
                    MessageBoxShow.ErrorMessage("Ошибка удаления изображения");
                }
                finally
                {
                    UpdatePropertyChanged();
                }
            }
        }

        private void UpdatePropertyChanged()
        {
            var oldService = Service;
            Service = null;
            Service = oldService;
            OnPropertyChanged("Service");
        }

        private void ServiceMainImageChange()
        {
            if (_opf.ShowDialog() == true)
            {
                this.Service.MainImagePath = ImageManager.LoadImage(_opf.FileName, Constants.ServiceImgFolder);
            }
            UpdatePropertyChanged();
        }

        private void SaveService()
        {
            var sameTitleServicesCount = PP_02Entities.GetContext().Service.ToList().Where(v => v.Title.Equals(Service.Title)).Count();
            if (Service.ID == 0 && sameTitleServicesCount != 0 
                || Service.ID != 0 && sameTitleServicesCount > 1)
            {
                MessageBoxShow.ErrorMessage("Услуга с таким названием уже существует!");
                return;
            }
            if (MessageBoxShow.QuestionMessage("Вы точно хотите сохранить изменения?"))
            {
                if (Service.ID == 0)
                {
                    PP_02Entities.GetContext().Service.Add(Service);
                }
                try
                {
                    PP_02Entities.GetContext().SaveChanges();
                    FrameManager.Context.GoBackViewModel();
                    MessageBoxShow.SuccessMessage("Информация об услуге успешно сохранена!");
                }
                catch
                {
                    MessageBoxShow.ErrorMessage("Ошибка сохранения!");
                }
            }
            UpdatePropertyChanged();
        }

        #endregion
    }
}
