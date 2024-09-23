using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIV_Protos
{
    public class MainWindowViewModel : BaseViewModel
    {
        #region atributes
        //Private
        private IPageViewModel _currentPageViewModel;
        private List<IPageViewModel> _pageViewModels;
        private Plataform selectedPlataform;

        public List<IPageViewModel> PageViewModels
        {
            get
            {
                if (_pageViewModels == null)
                    _pageViewModels = new List<IPageViewModel>();

                return _pageViewModels;
            }
        }

        //Public
        public IPageViewModel CurrentPageViewModel
        {
            get
            {
                return _currentPageViewModel;
            }
            set
            {
                _currentPageViewModel = value;
                OnPropertyChanged("CurrentPageViewModel");
            }
        }

        public Plataform SelectedPlataform { get => selectedPlataform; set => selectedPlataform = value; }
        #endregion

        #region CONSTRUCTOR
        public MainWindowViewModel()
        {
            // Añadir los viewModels de cada vista al listado de viewModels
            PageViewModels.Add(new MainMenuViewModel());
            PageViewModels.Add(new UserLoginViewModel());
            PageViewModels.Add(new PCBRegistrationViewModel());
            PageViewModels.Add(new HousingRegistrationViewModel());
            PageViewModels.Add(new PCBReworkViewModel());
            PageViewModels.Add(new InternalVersionViewModel());
            PageViewModels.Add(new OFRegistrationViewModel());
            PageViewModels.Add(new AGRegistrationViewModel());
            PageViewModels.Add(new OFChangeViewModel());
            PageViewModels.Add(new ProyectSelectorViewModel());

            CurrentPageViewModel = PageViewModels[9]; //Iniciar en el último viewModel añadido (ProyectSelectorViewModel)

            // Definir los tokens que escucharán la solicitud de cambio de vista y asignar las acciones a cada token para realizar el cambio de vista
            Mediator.Subscribe("SelectPlataform", SelectPlataform);
            Mediator.Subscribe("GoToMenuScreen", OnGoMenuScreen);
            Mediator.Subscribe("GoToUserLoginScreen", OnGoUserLoginScreen);
            Mediator.Subscribe("GoToPCBScreen", OnGoPCBScreen);
            Mediator.Subscribe("NavigateToPCBScreen", NavigateToPCBScreen);
            Mediator.Subscribe("GoToHousingScreen", OnGoHousingScreen);
            Mediator.Subscribe("NavigateToHousingScreen", NavigateToHousingScreen);
            Mediator.Subscribe("GoToPCBReworkScreen", OnGoPCBReworkScreen);
            Mediator.Subscribe("NavigateToPCBReworkScreen", NavigateToPCBReworkScreen);
            Mediator.Subscribe("GoToInternalVersionScreen", OnGoInternalVersionScreen);
            Mediator.Subscribe("NavigateToInternalVersionScreen", NavigateToInternalVersionScreen);
            Mediator.Subscribe("GoToOFRegistrationScreen", OnGoOFRegistrationScreen);
            Mediator.Subscribe("NavigateToOFRegistrationScreen", NavigateToOFRegistrationScreen);
            Mediator.Subscribe("GoToAGRegistrationScreen", OnGoAGRegistrationScreen);
            Mediator.Subscribe("NavigateToAGRegistrationScreen", NavigateToAGRegistrationScreen);
            Mediator.Subscribe("GoToOFChangeScreen", OnGoOFChangeScreen);
            Mediator.Subscribe("NavigateToOFChangeScreen", NavigateToOFChangeScreen);
        }
        #endregion

        #region METHODS
        //Private

        private void ChangeViewModel(IPageViewModel viewModel)
        {
            if (!PageViewModels.Contains(viewModel))
                PageViewModels.Add(viewModel);

            CurrentPageViewModel = PageViewModels
                .FirstOrDefault(vm => vm == viewModel);
        }

        // Metodos para seleccionar una plataforma
        private void SelectPlataform(object obj)
        {
            ((MainMenuViewModel)PageViewModels[0]).SelectedPlataform = (Plataform)obj;
        }

        // Metodos para realizar la navegación
        private void OnGoMenuScreen(object obj)
        {
            ChangeViewModel(PageViewModels[0]);
        }

        private void OnGoUserLoginScreen(object obj)
        {
            ChangeViewModel(PageViewModels[1]);
        }

        private void OnGoPCBScreen(object obj)
        {
            ChangeViewModel(PageViewModels[1]);
            ((UserLoginViewModel)PageViewModels[1]).NextPage = 2;
            SelectedPlataform = (Plataform)obj;
        }
        private void NavigateToPCBScreen(object obj)
        {
            ChangeViewModel(PageViewModels[2]);
            ((PCBRegistrationViewModel)PageViewModels[2]).ActualUser = (string)obj;
            ((PCBRegistrationViewModel)PageViewModels[2]).Plataform = SelectedPlataform;
        }

        private void OnGoHousingScreen(object obj)
        {
            ChangeViewModel(PageViewModels[1]);
            ((UserLoginViewModel)PageViewModels[1]).NextPage = 3;
            SelectedPlataform = (Plataform) obj;
        }
        private void NavigateToHousingScreen(object obj)
        {
            ChangeViewModel(PageViewModels[3]);
            ((HousingRegistrationViewModel)PageViewModels[3]).ActualUser = (string)obj;
            ((HousingRegistrationViewModel)PageViewModels[3]).Plataform = SelectedPlataform;
        }
        private void OnGoPCBReworkScreen(object obj)
        {
            ChangeViewModel(PageViewModels[1]);
            ((UserLoginViewModel)PageViewModels[1]).NextPage = 4;
            SelectedPlataform = (Plataform)obj;
        }
        private void NavigateToPCBReworkScreen(object obj)
        {
            ChangeViewModel(PageViewModels[4]);
            ((PCBReworkViewModel)PageViewModels[4]).ActualUser = (string)obj;
            ((PCBReworkViewModel)PageViewModels[4]).Plataform = SelectedPlataform;
        }

        private void OnGoInternalVersionScreen(object obj)
        {
            ChangeViewModel(PageViewModels[1]);
            ((UserLoginViewModel)PageViewModels[1]).NextPage = 5;
        }
        private void NavigateToInternalVersionScreen(object obj)
        {
            ChangeViewModel(PageViewModels[5]);
            ((InternalVersionViewModel)PageViewModels[5]).ActualUser = (string)obj;
        }

        private void OnGoOFRegistrationScreen(object obj)
        {
            ChangeViewModel(PageViewModels[1]);
            ((UserLoginViewModel)PageViewModels[1]).NextPage = 6;
            SelectedPlataform = (Plataform)obj;
        }
        private void NavigateToOFRegistrationScreen(object obj)
        {
            ChangeViewModel(PageViewModels[6]);
            ((OFRegistrationViewModel)PageViewModels[6]).ActualUser = (string)obj;
            ((OFRegistrationViewModel)PageViewModels[6]).Plataform = SelectedPlataform;
        }
        private void OnGoAGRegistrationScreen(object obj)
        {
            ChangeViewModel(PageViewModels[1]);
            ((UserLoginViewModel)PageViewModels[1]).NextPage = 7;
            SelectedPlataform = (Plataform)obj;
        }
        private void NavigateToAGRegistrationScreen(object obj)
        {
            ChangeViewModel(PageViewModels[7]);
            ((AGRegistrationViewModel)PageViewModels[7]).ActualUser = (string)obj;
            ((AGRegistrationViewModel)PageViewModels[7]).Plataform = SelectedPlataform;
        }
        private void OnGoOFChangeScreen(object obj)
        {
            ChangeViewModel(PageViewModels[1]);
            ((UserLoginViewModel)PageViewModels[1]).NextPage = 8;
            SelectedPlataform = (Plataform)obj;
        }
        private void NavigateToOFChangeScreen(object obj)
        {
            ChangeViewModel(PageViewModels[8]);
            ((OFChangeViewModel)PageViewModels[8]).ActualUser = (string)obj;
            ((OFChangeViewModel)PageViewModels[8]).Plataform = SelectedPlataform;
        }

        #endregion
    }
}