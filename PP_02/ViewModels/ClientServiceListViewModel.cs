using PP_02.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace PP_02.ViewModels
{
    class ClientServiceListViewModel : BaseViewModel
    {
        #region constants

        const int HOURS_UPDATE_COUNT = 0;
        const int MINUTES_UPDATE_COUNT = 0;
        const int SECONDS_UPDATE_COUNT = 30;

        #endregion

        #region fields

        DispatcherTimer timer;

        #endregion

        #region getters

        public List<ClientService> ClientServiceList
        {
            get
            {
                return PP_02Entities.GetContext().ClientService.ToList().Where(v => (v.StartTime.Date == DateTime.Today
                || v.StartTime.Date == DateTime.Today.AddDays(1)) && v.StartTime >= DateTime.Now).OrderBy(v => v.StartTime).ToList();
            }
        }

        #endregion

        public ClientServiceListViewModel()
        {
            timer = new DispatcherTimer();
            timer.Tick += new EventHandler(TimerTick);
            timer.Interval = new TimeSpan(HOURS_UPDATE_COUNT, MINUTES_UPDATE_COUNT, SECONDS_UPDATE_COUNT);
            timer.Start();
        }

        #region self fields

        /// <summary>
        /// Метод, обвноляющий привязку ClientServiceList
        /// </summary>
        void TimerTick(Object sender, EventArgs e)
        {
            UpdatePropertyChanged();
        }

        void UpdatePropertyChanged()
        {
            OnPropertyChanged("ClientServiceList");
        }

        #endregion
    }
}
