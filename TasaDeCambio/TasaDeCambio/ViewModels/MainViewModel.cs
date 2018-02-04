using GalaSoft.MvvmLight.Command;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TasaDeCambio.Models;
using TasaDeCambio.Services;
using tasas.Services;
using Xamarin.Forms;

namespace TasaDeCambio.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {

        #region Propiedades
        public string Amount
        {
            get { return _Amount; }
            set
            {
                if (_Amount != value)
                {
                    _Amount = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Amount)));
                }
            }
        }

        public ObservableCollection<Tasa> Rates
        {
            get { return _Rates; }
            set
            {
                if (_Rates != value)
                {
                    _Rates = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Rates)));
                }
            }
        }


        public Tasa SourceRate
        {
            get { return _SourceRate; }
            set
            {
                if (_SourceRate != value)
                {
                    _SourceRate = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SourceRate)));
                }
            }
        }

        public Tasa TargetRate
        {
            get { return _TargetRate; }
            set
            {
                if (_TargetRate != value)
                {
                    _TargetRate = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TargetRate)));
                }
            }
        }

        public bool IsRunning
        {
            get { return _IsRunning; }
            set
            {
                if (_IsRunning != value)
                {
                    _IsRunning = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsRunning)));
                }
            }
        }

        public bool IsEnabled
        {
            get { return _IsEnabled; }
            set
            {
                if (_IsEnabled != value)
                {
                    _IsEnabled = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsEnabled)));
                }
            }
        }


        public string Result
        {
            get { return _Result; }
            set
            {
                if (_Result != value)
                {
                    _Result = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Result)));
                }
            }
        }

        public string Status
        {
            get { return _Status; }
            set
            {
                if (_Status != value)
                {
                    _Status = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Status)));
                }
            }
        }


        public string Title
        {
            get { return _Title; }
            set
            {
                if (_Title != value)
                {
                    _Title = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Title)));
                }
            }
        }

        #endregion



        #region atributos
        string _Amount;
        ObservableCollection<Tasa> _Rates;
        Tasa _SourceRate;
        Tasa _TargetRate;
        bool _IsRunning;
        bool _IsEnabled;
        string _Result;
        string _Status;
        string _Title;
        List<Tasa> rates;
        #endregion




        #region eventos
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion


        #region constructor
        public MainViewModel()
        {
            AppiService = new AppiService();
            AppiDialog = new AppiDialog();
            AppiDataService = new AppiDataService();
            Title = Resources.Resource.Title;
            LoadRates();
        }

        #endregion

        #region servicios

        AppiDialog AppiDialog;
        AppiService AppiService;
        AppiDataService AppiDataService;
        #endregion



        #region comandos

        public ICommand ChangeCommand
        {
            get
            {
                return new RelayCommand(changeCommand);
            }
        }

        public ICommand ConvertCommmand
        {
            get
            {
                return new RelayCommand(convertCommmand);
            }
        }

        #endregion





        #region metodos
        void changeCommand()
        {
            var aux = SourceRate;
            SourceRate = TargetRate;
            TargetRate = aux;
            convertCommmand();
        }

        async  void convertCommmand()
        {
            if (string.IsNullOrEmpty(Amount))
            {
               await AppiDialog.ShowMessage("Error", "Debes Ingresar un monto...");
               return;
            }

            decimal amount = 0;
            if (!decimal.TryParse(Amount, out amount))
            {
                await AppiDialog.ShowMessage("Error", "Debes Ingresar un valor numerico...");
                return;
            }

            if (SourceRate == null)
            {
                await AppiDialog.ShowMessage("Error", "Debes seleccionar una tasa de origen...");
                return;
            }

            if (TargetRate == null)
            {
                await AppiDialog.ShowMessage("Error", "Debes seleccionar una tasa de destino...");
                return;
            }

            var amountConverted = amount /
                                  (decimal)SourceRate.TaxRate *
                                  (decimal)TargetRate.TaxRate;

            Result = string.Format(
                "{0} ${1:N2} = {2} ${3:N2}",
                SourceRate.Code,
                amount,
                TargetRate.Code,
                amountConverted);
          }
  
       

        async void LoadRates()
            {
                IsRunning = true;
                Result = "Cargando.........";

            var connection = await AppiService.CheckConnection();

            if (!connection.IsSuccess)
            {
                LoadLocalData();
            }
            else
            {
                await LoadDataFromAPI();
            }

            if (rates.Count == 0)
            {
                IsRunning = false;
                IsEnabled = false;
                Result = "No hay conexion a Internet." +
                    "Intente nuevamente";
                Status = "No hay tasas Cargadas.";
                return;
            }

            Rates = new ObservableCollection<Tasa>(rates);

            IsRunning = false;
            IsEnabled = true;
            Result = "Listo para convertir...!";
        }


        void LoadLocalData()
        {
            rates = AppiDataService.Get<Tasa>(false);
            Status = "Tasas obtenidas desde la BBDD.";
        }

        async Task LoadDataFromAPI()
        {
            var url = "http://apiexchangerates.azurewebsites.net"; //Application.Current.Resources["URLAPI"].ToString();

            var response = await AppiService.GetList<Tasa>(
                url,
                "/api/Rates");

            if (!response.IsSuccess)
            {
                LoadLocalData();
                return;
            }

            // Storage data local
            rates = (List<Tasa>)response.Result;
            AppiDataService.DeleteAll<Tasa>();
            AppiDataService.Save(rates);

            Status = "Tasas descargadas desde Internet.";
        }




        #endregion


    }
}
