using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using System.Collections.ObjectModel;

namespace DIV_Protos
{

    public class ProyectSelectorViewModel : BaseViewModel, IPageViewModel
    {

        #region atributos
        private Plataform selectedPlataform;
        private ObservableCollection<Plataform> plataforms = new ObservableCollection<Plataform>();
        private bool enabledBtn = false;
        #endregion
        #region propiedades públicas
        public Plataform SelectedPlataform
        {
            get => selectedPlataform; set
            {
                selectedPlataform = value;
                OnPropertyChanged(nameof(SelectedPlataform));
                Mediator.Notify("SelectPlataform", value);
                EnabledBtn = true;
                OnPropertyChanged(nameof(EnabledBtn));
            }
        }
        public ObservableCollection<Plataform> Plataforms { get => plataforms; set => plataforms = value; }
        public bool EnabledBtn { get => enabledBtn; set => enabledBtn = value; }
        #endregion
        #region Constructor
        public ProyectSelectorViewModel()
        {
            string error = "";
            Plataforms = SQLServer_DDBB_BusinessLogic.GetProject();
            Plataforms = SQLServer_DDBB_BusinessLogic.GetProject();
            OnPropertyChanged(nameof(Plataforms));
        }
        #endregion
        #region Métodos públicos
        #endregion
    }
}
