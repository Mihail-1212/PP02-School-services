using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PP_02.ViewModels;
using PP_02.Models;
using PP_02.Utils;

namespace PP_02_UnitTests
{
    [TestClass]
    public class UnitTestDelete
    {
        private Service CreateService(bool haveAdditivePhotos = false, bool haveClientService=false)
        {
            Service service = new Service()
            {
                Title = "",
                Cost = 0,
                DurationInSeconds = 0
            };
            PP_02Entities.GetContext().Service.Add(service);
            PP_02Entities.GetContext().SaveChanges();
            if (haveAdditivePhotos)
            {
                ServicePhoto servicePhoto = new ServicePhoto()
                {
                    PhotoPath = ""
                };
                servicePhoto.ServiceID = service.ID;
                PP_02Entities.GetContext().ServicePhoto.Add(servicePhoto);
                PP_02Entities.GetContext().SaveChanges();
            }
            if (haveClientService)
            {
                Client client = new Client()
                {
                    FirstName="",
                    LastName="",
                    Patronymic="",
                    RegistrationDate = DateTime.Now,
                    Email="",
                    Phone="",
                    GenderCode="ж",
                    PhotoPath=""
                };
                PP_02Entities.GetContext().Client.Add(client);
                PP_02Entities.GetContext().SaveChanges();

                ClientService clientService = new ClientService()
                {
                    StartTime=DateTime.Now,
                    ClientID = client.ID,
                    ServiceID = service.ID
                };
                PP_02Entities.GetContext().ClientService.Add(clientService);
                PP_02Entities.GetContext().SaveChanges();
            }
            PP_02Entities.GetContext().Update();
            return service;
        }

        /// <summary>
        /// Пользователь не авторизован
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(Exception), "Пользователь не авторизован как админ")]
        public void Test_1_UserNotAuth()
        {
            var mainViewModel = new MainViewModel();
            AdminManager.Context.TryLogout();
            mainViewModel.ServiceDeleteFunc(CreateService());
        }

        /// <summary>
        /// Услуга без дополнительных фотографий и без записей, пользователь авторизован
        /// </summary>
        [TestMethod]
        public void Test_2_ServiceNoPhotoNoClient()
        {
            var mainViewModel = new MainViewModel();
            AdminManager.Context.TryLogin("0000");
            var result = mainViewModel.ServiceDeleteFunc(CreateService());
            Assert.IsTrue(result is Action);
        }

        /// <summary>
        /// Услуга без дополнительных фотографий и с записями, пользователь авторизован
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(Exception), "Вы не можете удалить услуги, у которых есть записи")]
        public void Test_3_ServiceNoPhotoYesClient()
        {
            var mainViewModel = new MainViewModel();
            AdminManager.Context.TryLogin("0000");
            mainViewModel.ServiceDeleteFunc(CreateService(false, true));
        }

        /// <summary>
        /// Услуга с дополнительными фотографиями и без записей, пользователь авторизован, 
        /// выбрал да удалению услуги и да - удалению доп. фотографий
        /// </summary>
        [TestMethod]
        public void Test_4_ServiceYesPhotoNoClient()
        {
            var mainViewModel = new MainViewModel();
            AdminManager.Context.TryLogin("0000");
            var result = mainViewModel.ServiceDeleteFunc(CreateService(true, false));
            Assert.IsTrue(result is Action);
        }

        /// <summary>
        /// Услуга с дополнительными фотографиями и с записями, пользователь авторизован, 
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(Exception), "Вы не можете удалить услуги, у которых есть записи")]
        public void Test_5_ServiceYesPhotoYesClient()
        {
            var mainViewModel = new MainViewModel();
            AdminManager.Context.TryLogin("0000");
            var result = mainViewModel.ServiceDeleteFunc(CreateService(true, true));
        }
    }
}
