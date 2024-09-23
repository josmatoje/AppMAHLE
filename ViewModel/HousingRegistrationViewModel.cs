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
    class HousingRegistrationViewModel : BaseViewModel, IPageViewModel
    {
        #region atributos
        private ICommand _backMenu;
        private ICommand _save;
        private string actualUser;
        private Plataform plataform;
        private List<string> mechanicalVersionCode = new List<string>();
        private ObservableCollection<ItemPCBDefinition> referencesPCB = new ObservableCollection<ItemPCBDefinition>();
        private List<string> categoryName = new List<string>();
        private List<string> list_ReferemcesPCB = new List<string>();
        private List<string> list_LayoutBOMPCB = new List<string>();
        private string layoutBomSelected;
        private string categorySelected;
        private String snNumber;
        private string mechanicalSelectedVersion;
        private string referencesPCBSelected;
        private bool snTexBoxEnabled = false;
        private bool mechanicalSelectorEnable = false;
        private bool layoutBOMEnable = false;
        private string errorMessage;
        private Visibility showError = Visibility.Hidden;
        private Visibility windChillVisibility = Visibility.Hidden;
        private Visibility snVisibility = Visibility.Hidden;
        private Visibility pcbVisibility = Visibility.Hidden;
        private ObservableCollection<SavedHousing> savedHousings = new ObservableCollection<SavedHousing>();
        private int lastHousingSaved = 1;
        private int insertedHousings = 0;
        private bool focusSNTB;
        private string lotePCB;
        #endregion
        #region propiedades púnlicas
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

        public ICommand Save
        {
            get
            {
                return _save ?? (_save = new RelayCommand(x =>
                {
                    SaveHousing();
                }));
            }
        }
        public string ActualUser { get => actualUser; set => actualUser = value; }
        public Plataform Plataform { get => plataform; set => plataform = value; }
        public List<string> MechanicalVersionCode {
            get
            {
                return mechanicalVersionCode;
            }
            set => mechanicalVersionCode = value;
        }
    
        public string SnNumber { 
            get => snNumber;
            set
            {
                snNumber = value;
                if (value != String.Empty)
                {
                    ShowError = Visibility.Hidden;
                    OnPropertyChanged(nameof(ShowError));
                }
            }
        }
        public string MechanicalSelectedVersion { 
            get => mechanicalSelectedVersion;
            set
            {
                mechanicalSelectedVersion = value;
            }
        }
        public bool SnTexBoxEnabled { get => snTexBoxEnabled; set => snTexBoxEnabled = value; }
        public Visibility ShowError { get => showError; set => showError = value; }
        public string ErrorMessage { get => errorMessage; set => errorMessage = value; }
        public ObservableCollection<SavedHousing> SavedHousings { get => savedHousings; set => savedHousings = value; }
        public int LastHousingSaved { get => lastHousingSaved; set => lastHousingSaved = value; }
        public int InsertedHousings { get => insertedHousings; set => insertedHousings = value; }
        public List<string> CategoryName
        {
            get
            {
                List<string> categoryName = new List<string>();
                categoryName = SQLServer_DDBB_BusinessLogic.GetCategoryList();
                return categoryName;
            }
            set => categoryName = value; }
        
        public string CategorySelected
        {
            get => categorySelected;
            set
            {
               List<string> mechanicalVersionCode = new List<string>();
               categorySelected = value;
                if (value != null)
                {
                    SnVisibility = Visibility.Visible;
                    OnPropertyChanged(nameof(SnVisibility));
                    MechanicalSelectedVersion = "";
                    OnPropertyChanged(nameof(MechanicalSelectedVersion));
                    SnTexBoxEnabled = value != "PCB";
                    OnPropertyChanged(nameof(SnTexBoxEnabled));

                    if (value == "PCB")
                    {
                        WindChillVisibility=Visibility.Hidden;
                        PcbVisibility = Visibility.Visible;
                        List_ReferemcesPCB = new List<string>();
                        ReferencesPCBSelected = "";
                        OnPropertyChanged(nameof(ReferencesPCBSelected));
                        List_LayoutBOMPCB = new List<string>();
                        OnPropertyChanged(nameof(List_LayoutBOMPCB));
                        LayoutBomSelected = "";
                        OnPropertyChanged(nameof(LayoutBomSelected));
                        ReferencesPCB = SQLServer_DDBB_BusinessLogic.GetHardwarePCBParametersFromProject(Plataform.PlataformName);
                        foreach(ItemPCBDefinition element in ReferencesPCB)
                        {
                            if(!List_ReferemcesPCB.Contains(element.ReferenceName))
                            {
                                List_ReferemcesPCB.Add(element.ReferenceName);
                            }
                        }
                        OnPropertyChanged(nameof(List_ReferemcesPCB));
                    }
                    else
                    {
                        WindChillVisibility = Visibility.Visible;
                        PcbVisibility = Visibility.Hidden;
                        MechanicalVersionCode = SQLServer_DDBB_BusinessLogic.GetWindChillListFromCategory(categorySelected);
                        OnPropertyChanged(nameof(MechanicalVersionCode));
                        MechanicalSelectorEnable = true;
                    }
                    OnPropertyChanged(nameof(WindChillVisibility));
                    OnPropertyChanged(nameof(PcbVisibility));
                    OnPropertyChanged(nameof(MechanicalSelectorEnable));
                }
            }
        }

        public bool MechanicalSelectorEnable { get => mechanicalSelectorEnable; set => mechanicalSelectorEnable = value; }
        public bool FocusSNTB { get => focusSNTB; set => focusSNTB = value; }
        public Visibility WindChillVisibility { get => windChillVisibility; set => windChillVisibility = value; }
        public Visibility PcbVisibility { get => pcbVisibility; set => pcbVisibility = value; }
        public Visibility SnVisibility { get => snVisibility; set => snVisibility = value; }
        public ObservableCollection<ItemPCBDefinition> ReferencesPCB { get => referencesPCB; set => referencesPCB = value; }
        public string ReferencesPCBSelected
        {
            get => referencesPCBSelected;
            set
            {
                LayoutBOMEnable = value != "";
                OnPropertyChanged(nameof(LayoutBOMEnable));
                referencesPCBSelected = value;
                if(value != "")
                {
                    LayoutBOMEnable = true;
                    List_LayoutBOMPCB = new List<string>();
                    foreach (ItemPCBDefinition element in ReferencesPCB)
                    {
                        if (element.ReferenceName == referencesPCBSelected)
                        {
                            if (!List_LayoutBOMPCB.Contains(element.LayoutBOM))
                            {
                                List_LayoutBOMPCB.Add(element.LayoutBOM);
                            }
                        }
                    }
                    OnPropertyChanged(nameof(List_LayoutBOMPCB));
                    LayoutBomSelected = "";
                    OnPropertyChanged(nameof(LayoutBomSelected));
                }
            }
        }
        public List<string> List_ReferemcesPCB { get => list_ReferemcesPCB; set => list_ReferemcesPCB = value; }
        public List<string> List_LayoutBOMPCB { get => list_LayoutBOMPCB; set => list_LayoutBOMPCB = value; }
        public string LayoutBomSelected
        {
            get => layoutBomSelected;
            set
            {
                layoutBomSelected = value;
                SnTexBoxEnabled = value != "";
                OnPropertyChanged(nameof(SnTexBoxEnabled));
                LotePCB = String.Empty;
                OnPropertyChanged(nameof(LotePCB));
            }
        }
        public bool LayoutBOMEnable { get => layoutBOMEnable; set => layoutBOMEnable = value; }
        public string LotePCB
        {
            get => lotePCB;
            set
            {
                lotePCB = value;
            }
        }

        #endregion
        #region constructores
        public HousingRegistrationViewModel()
        {
            SnNumber = String.Empty;
        }
        #endregion
        #region métodos públicos
        public bool SaveHousing()
        {
            bool saved = false;
            string error = "OK";
            SavedHousing newHousing = new SavedHousing();
            if (CategorySelected=="PCB")
            {
                string ReferenceName = string.Empty;
                string definePCB = string.Empty;
                string LayoutBOM = string.Empty;
                foreach (ItemPCBDefinition element in ReferencesPCB)
                {
                    if(element.LayoutBOM == LayoutBomSelected)
                    {
                        ReferenceName = element.ReferenceName;
                        definePCB = element.PcbDefinition;
                        LayoutBOM = element.LayoutBOM;
                        break;
                    }
                }
                saved = SQLServer_DDBB_BusinessLogic.ExistsAndInsertSN_PCB(SnNumber, ReferenceName, LayoutBOM,  Plataform.PlataformName, CategorySelected, true, LotePCB, ActualUser, MechanicalSelectedVersion, out error);
                SavedHousing newPCB = new SavedHousing(LastHousingSaved, SnNumber, definePCB, CategorySelected, error);
                newHousing = newPCB;
            }
            else
            {
                saved = SQLServer_DDBB_BusinessLogic.ExistsAndInsertSN(SnNumber, Plataform.PlataformName, CategorySelected, true, ActualUser, MechanicalSelectedVersion, out error);
                SavedHousing newCMechanical = new SavedHousing(LastHousingSaved, SnNumber, MechanicalSelectedVersion, CategorySelected, error);
                newHousing = newCMechanical;
            }
            LastHousingSaved += 1;
            if (saved)
            {
                InsertedHousings += 1;
                OnPropertyChanged(nameof(InsertedHousings));
            }
            SavedHousings.Add(newHousing);
            OnPropertyChanged(nameof(SavedHousings));

            SnNumber = String.Empty;
            OnPropertyChanged(nameof(SnNumber));
            
            return saved;
        }
        #endregion
    }
}
