using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace DIV_Protos
{
    public class MainMenuViewModel: BaseViewModel, IPageViewModel
    {
        private ICommand _goToPCB;
        private ICommand _goToHousing;
        private ICommand _goToInternalVersion;
        private ICommand _goToPCBRework;
        private ICommand _goToOF;
        private ICommand _goToChangeOF;
        private ICommand _goToAG;
        private int selectedTab;
        private Plataform selectedPlataform;

        public ICommand GoToPCB
        {
            get
            {
                return _goToPCB ?? (_goToPCB = new RelayCommand(x =>
                {
                    if (SelectedPlataform != null)
                    {
                        Mediator.Notify("GoToPCBScreen", SelectedPlataform);
                    } else
                    {
                        MessageBox.Show("Debe seleccionar un proyecto sobre el que trabajar.", "Proyecto no seleccionado", MessageBoxButton.OK, MessageBoxImage.Question);
                    }
                }));
            }

        }

        public ICommand GoToHousing
        {
            get
            {
                return _goToHousing ?? (_goToHousing = new RelayCommand(x =>
                {
                    if (SelectedPlataform != null)
                    {
                        Mediator.Notify("GoToHousingScreen", SelectedPlataform);
                    }
                    else
                    {
                        MessageBox.Show("Debe seleccionar un proyecto sobre el que trabajar.", "Proyecto no seleccionado", MessageBoxButton.OK, MessageBoxImage.Question);
                    }
                }));
            }
        }

        public ICommand GoToInternalVersion
        {
            get
            {
                return _goToInternalVersion ?? (_goToInternalVersion = new RelayCommand(x =>
                {
                    Mediator.Notify("GoToInternalVersionScreen", "");
                }));
            }
        }

        public ICommand GoToOF
        {
            get
            {
                return _goToOF ?? (_goToOF = new RelayCommand(x =>
                {
                    if (SelectedPlataform != null)
                    {
                        Mediator.Notify("GoToOFRegistrationScreen", SelectedPlataform);
                    }
                    else
                    {
                        MessageBox.Show("Debe seleccionar un proyecto sobre el que trabajar.", "Proyecto no seleccionado", MessageBoxButton.OK, MessageBoxImage.Question);
                    }
                }));
            }

        }

        public ICommand GoToAG
        {
            get
            {
                return _goToAG ?? (_goToAG = new RelayCommand(x =>
                {
                    if (SelectedPlataform != null)
                    {
                        Mediator.Notify("GoToAGRegistrationScreen", SelectedPlataform);
                    }
                    else
                    {
                        MessageBox.Show("Debe seleccionar un proyecto sobre el que trabajar.", "Proyecto no seleccionado", MessageBoxButton.OK, MessageBoxImage.Question);
                    }
                }));
            }

        }

        public ICommand GoToPCBRework
        {
            get
            {
                return _goToPCBRework ?? (_goToPCBRework = new RelayCommand(x =>
                {
                    if (SelectedPlataform != null)
                    {
                        Mediator.Notify("GoToPCBReworkScreen", SelectedPlataform);
                    }
                    else
                    {
                        MessageBox.Show("Debe seleccionar un proyecto sobre el que trabajar.", "Proyecto no seleccionado", MessageBoxButton.OK, MessageBoxImage.Question);
                    }
                }));
            }

        }
        public ICommand GoToChangeOF
        {
            get
            {
                return _goToChangeOF ?? (_goToChangeOF = new RelayCommand(x =>
                {
                    if (SelectedPlataform != null)
                    {
                        Mediator.Notify("GoToOFChangeScreen", SelectedPlataform);
                    }
                    else
                    {
                        MessageBox.Show("Debe seleccionar un proyecto sobre el que trabajar.", "Proyecto no seleccionado", MessageBoxButton.OK, MessageBoxImage.Question);
                    }
                }));
            }

        }

        public int SelectedTab { 
            get => selectedTab;
            set
            {
                selectedTab = value;
                OnPropertyChanged(nameof(SelectedTab));
            }
         }

        public Plataform SelectedPlataform { get => selectedPlataform; set => selectedPlataform = value; }

        public MainMenuViewModel()
        {
            //MessageBox.Show("No se ha podido realizar la conexión con la base de datos. Se van a mostrar datos de prueba", "Error de conexión en la aplicación", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }
}
