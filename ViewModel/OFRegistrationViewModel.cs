using Aspose.Cells.ExternalConnections;
using Org.BouncyCastle.Utilities;
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
    public class OFRegistrationViewModel: BaseViewModel, IPageViewModel
    {

        #region atributos
        private ICommand _backMenu;
        private ICommand _saveOF;
        private Plataform plataform;
        private string actualUser;
        private string description;
        private ObservableCollection<ItemDefinitionOF> typeOF = new ObservableCollection<ItemDefinitionOF>();
        private ObservableCollection<SavedOFs> savedOFs = new ObservableCollection<SavedOFs>();
        private string selectedTypeTitle;
        private string selectedType;
        private Visibility showSelector = Visibility.Hidden;
        private string quantity;
        private OF lastOF = new OF();
        private bool enableOk;
        private List<string> list_ItemTypeOF = new List<string>();
        private Visibility showValues = Visibility.Hidden;
        private string descriptionOF;
        private int lastOFSaved = 1;
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
        public ICommand SaveOFCommand
        {
            get
            {
                return _saveOF ?? (_saveOF = new RelayCommand(x =>
                {
                    SaveOF();
                }));
            }
        }
        public Plataform Plataform { get => plataform; set => plataform = value; }
        public string ActualUser { get => actualUser; set => actualUser = value; }
        public string Description { 
            get => description;
            set
            {
                description = value;
                EnableOk = isEnableButton();
                OnPropertyChanged(nameof(EnableOk));
            }
        }
        public ObservableCollection<ItemDefinitionOF> TypeOF { get => typeOF; set => typeOF = value; }
        
        public string SelectedTypeTitle { get => selectedTypeTitle; set => selectedTypeTitle = value; }
        public Visibility ShowSelector { get => showSelector; set => showSelector = value; }
      
        public string Quantity { 
            get => quantity;
            set 
            {
                quantity = value;
                EnableOk = isEnableButton();
                OnPropertyChanged(nameof(EnableOk));
            }
        }
        public OF LastOF { get => lastOF; set => lastOF = value; }
        public bool EnableOk { get => enableOk; set => enableOk = value; }
        public Visibility ShowValues { get => showValues; set => showValues = value; }
        public List<string> List_ItemTypeOF { get => list_ItemTypeOF; set => list_ItemTypeOF = value; }
        public string DescriptionOF
        {
            get => descriptionOF;
            set
            {
                descriptionOF = value;
                EnableOk = isEnableButton();
                OnPropertyChanged(nameof(EnableOk));
            }
        }
        public string SelectedType
        {
            get => selectedType;
            set
            {
                selectedType = value;
                EnableOk = isEnableButton();
                OnPropertyChanged(nameof(EnableOk));

            }
                 
        }

        public ObservableCollection<SavedOFs> SavedOFs { get => savedOFs; set => savedOFs = value; }
        public int LastOFSaved { get => lastOFSaved; set => lastOFSaved = value; }
        #endregion

        #region constructor
        public OFRegistrationViewModel()
        {
            typeOF = SQLServer_DDBB_BusinessLogic.GetListItemDefinition();
            foreach(ItemDefinitionOF item in typeOF)
            {
                List_ItemTypeOF.Add(item.InternalName);
            }
        }
        #endregion
        #region métodos públicos
        public bool SaveOF()
        {
            bool saved = false;
            SavedOFs newOF = new SavedOFs();

            if (description.Equals(String.Empty))
            {

            }
            else
            {
                String error = String.Empty;
                int quantyInt;
                try
                {
                    quantyInt= Convert.ToInt16(Quantity);
                    if(quantyInt>0)
                    {
                        foreach (ItemDefinitionOF item in typeOF)
                        {
                            if (item.InternalName == SelectedType)
                            {
                                SQLServer_DDBB_BusinessLogic.InsertOFItemDefinition(description, item.InternalName, ActualUser, DescriptionOF, quantyInt, out error);
                                SavedOFs newOFS = new SavedOFs(LastOFSaved, description, quantyInt, item.InternalName, error);
                                newOF = newOFS;
                                break;
                            }
                        }
                        LastOFSaved += 1;
                        SavedOFs.Add(newOF);
                        OnPropertyChanged(nameof(SavedOFs));
                        ShowValues = Visibility.Visible;
                        OnPropertyChanged(nameof(ShowValues));
                        LastOF.Description = Description;
                        LastOF.Quantity = Quantity;
                        OnPropertyChanged(nameof(LastOF));
                        Description = String.Empty;
                        Quantity = String.Empty;
                        DescriptionOF = String.Empty;
                        OnPropertyChanged(nameof(Description));
                        OnPropertyChanged(nameof(Quantity));
                        OnPropertyChanged(nameof(DescriptionOF));
                    }
                    else
                    {
                        ShowValues = Visibility.Hidden;
                        OnPropertyChanged(nameof(ShowValues));
                        MessageBox.Show("La cantidad no puede ser un valor negativo.",
                                            "Error de formato",
                                            MessageBoxButton.OK,
                                            MessageBoxImage.Warning);
                        Quantity = "";
                        OnPropertyChanged(nameof(Quantity));
                    }
                    
                }
                catch
                {
                    ShowValues = Visibility.Hidden;
                    OnPropertyChanged(nameof(ShowValues));
                    MessageBox.Show("La cantidad debe ser un valor numérico.",
                                        "Error de formato",
                                        MessageBoxButton.OK,
                                        MessageBoxImage.Warning);
                    Quantity = "";
                    OnPropertyChanged(nameof(Quantity));
                }
            }

            return saved;
        }

        public bool isEnableButton()
        {
            bool enable = false;
            if (Description != null && Quantity != null && DescriptionOF != null && SelectedType != null)
            {
                enable = !Description.Equals(String.Empty) && !Quantity.Equals(String.Empty) && !DescriptionOF.Equals(String.Empty) && !SelectedType.Equals(String.Empty);
            }

            return enable;
        }
        #endregion
    }
}
