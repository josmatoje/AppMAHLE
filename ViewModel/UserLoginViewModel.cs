using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace DIV_Protos
{
    public class UserLoginViewModel : BaseViewModel, IPageViewModel
    {

        #region atributos
        private ICommand _backMenu;
        private ICommand _navigateTo;
        private int nextPage;
        private string user;
        private SortedList keysRol = new SortedList();
        #endregion
        #region propiedades públicas
        public ICommand BackMenu
        {
            get
            {
                return _backMenu ?? (_backMenu = new RelayCommand(x =>
                {
                    Mediator.Notify("GoToMenuScreen", "");
                }));
            }
        }
        public ICommand NavigateTo
        {
            get
            {
                return _navigateTo ?? (_navigateTo = new RelayCommand(x =>
                {
                    login();
                }));
            }
        }

        public int NextPage { get => nextPage; set => nextPage = value; }
        public string User { get => user; set => user = value; }
        #endregion
        #region Constructor
        public UserLoginViewModel()
        {
            NextPage = 1;
            //UNDONE: user for debug - ¡¡¡REMOVE ON FINALL VERSION!!!
            string userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            if (SQLServer_DDBB_BusinessLogic.GetUserRol(userName).Count != 0)
            {
                User = userName;
            }
            
            keysRol.Add(2, "Input_SN_PCB");
            keysRol.Add(3, "Input_SN_Housing");
            keysRol.Add(4, "Input_Rework");
            keysRol.Add(5, "Input_VI");
            keysRol.Add(6, "Input_OF");
            keysRol.Add(7, "Input_AG");
            keysRol.Add(8, "Change_OF");
        }
        #endregion
        #region Métodos públicos
        public void login()
        {
            List<string> roles = SQLServer_DDBB_BusinessLogic.GetUserRol(User);

            if (roles.Count > 0 && hasPermision(roles))
            {

                switch (NextPage)
                {
                    case 2:
                        Mediator.Notify("NavigateToPCBScreen", User);
                        break;
                    case 3:
                        Mediator.Notify("NavigateToHousingScreen", User);
                        break;
                    case 4:
                        Mediator.Notify("NavigateToPCBReworkScreen", User);
                        break;
                    case 5:
                        Mediator.Notify("NavigateToInternalVersionScreen", User);
                        break;
                    case 6:
                        Mediator.Notify("NavigateToOFRegistrationScreen", User);
                        break;
                    case 7:
                        Mediator.Notify("NavigateToAGRegistrationScreen", User);
                        break;
                    case 8:
                        Mediator.Notify("NavigateToOFChangeScreen", User);
                        break;
                    default:
                        Mediator.Notify("GoToMenuScreen", "");
                        break;
                }

                //UNDONE: user for debug - ¡¡¡REMOVE coments  ON FINALL VERSION!!!
                //        | | | | | | |
                //        V V V V V V V
                //User = String.Empty;
                //OnPropertyChanged(nameof(User));
            }
            else
            {
                MessageBox.Show("El usuario no existe o no tiene acceso a este módulo.", 
                                "Acceso denegado", 
                                MessageBoxButton.OK, 
                                MessageBoxImage.Error);
            }
        }

        private bool hasPermision(List<string> roles)
        {
            bool hasPermision = false;
            foreach (string role in roles)
            {
                if (role.Equals(keysRol.GetByIndex(keysRol.IndexOfKey(NextPage))))
                {
                    hasPermision = true;
                }
            }

            return hasPermision;
        }
        #endregion
    }
}
