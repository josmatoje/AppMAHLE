using MySqlX.XDevAPI.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DIV_Protos
{
    class PCBReworkViewModel : BaseViewModel, IPageViewModel
    {
        #region atributos
        private ICommand _backMenu;
        private Plataform plataform;
        private string actualUser;
        private string snNumber;
        private string referenceText;
        private string layoutBomRework;
        private string rework;
        private ItemPCBRework actualPCB;
        private ObservableCollection<PCBDefinition> savedReworks = new ObservableCollection<PCBDefinition>();
        private int lastPCBSaved = 0;
        private int insertedPCBs = 0;
        private bool reworkIsEnabled;
        private int orderPCB=0;
        private Dictionary<int, int> list_ReworkActualPCB = new Dictionary<int, int>();
        private PCBDefinition savePCBRework = new PCBDefinition();
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
        public string SnNumber { 
            get => snNumber;
            set
            {
                snNumber = value;

            }
        }
        public Plataform Plataform { get => plataform; set => plataform = value; }
        public string ActualUser { get => actualUser; set => actualUser = value; }
        public string ReferenceText { get => referenceText; set => referenceText = value; }
        public string LayoutBomRework { get => layoutBomRework; set => layoutBomRework = value; }
        public string Rework { get => rework; set => rework = value; }
        public ItemPCBRework ActualPCB { get => actualPCB; set => actualPCB = value; }
        public ObservableCollection<PCBDefinition> SavedReworks { get => savedReworks; set => savedReworks = value; }
        public int LastPCBSaved { get => lastPCBSaved; set => lastPCBSaved = value; }
        public int InsertedPCBs { get => insertedPCBs; set => insertedPCBs = value; }
        public bool ReworkIsEnabled { get => reworkIsEnabled; set => reworkIsEnabled = value; }
        public int OrderPCB { get => orderPCB; set => orderPCB = value; }
        public Dictionary<int, int> List_ReworkActualPCB { get => list_ReworkActualPCB; set => list_ReworkActualPCB = value; }
        public PCBDefinition SavePCBRework1 { get => savePCBRework; set => savePCBRework = value; }
        #endregion
        #region Constructor
        public PCBReworkViewModel()
        {
        }
        #endregion
        #region Metodos públicos
        public bool ExistPCB()
        {
            bool status = true;
            String error;
            ItemPCBRework pCBDefinition = new ItemPCBRework();
            SQLServer_DDBB_BusinessLogic.ExistSNPCBFromProject(SnNumber, Plataform.PlataformName, out pCBDefinition, out error);
           
            if (pCBDefinition.CodeSN != "")
            {
               
                ActualPCB = pCBDefinition;
                ReferenceText = ActualPCB.HardwareReference;
                OnPropertyChanged(nameof(ReferenceText));
                OrderPCB = LastPCBSaved;
                List_ReworkActualPCB = SQLServer_DDBB_BusinessLogic.GetReworksFromLayoutBOM(ActualPCB.LayoutBOM, out error);
                if (List_ReworkActualPCB.Count == 0)
                {
                    LastPCBSaved += 1;
                    PCBDefinition newPCBRework = new PCBDefinition(LastPCBSaved, SnNumber, ActualPCB.HardwareReference, "", error);
                    ReworkIsEnabled = false;
                    OnPropertyChanged(nameof(ReworkIsEnabled));
                    SavedReworks.Add(newPCBRework);
                    OnPropertyChanged(nameof(SavedReworks));
                    SnNumber = String.Empty;
                    OnPropertyChanged(nameof(SnNumber));
                    ReferenceText = String.Empty;
                    OnPropertyChanged(nameof(ReferenceText));
                    ActualPCB = new ItemPCBRework();
                }
                else
                {
                    ReworkIsEnabled = true;
                    OnPropertyChanged(nameof(ReworkIsEnabled));
                }
            }
            else
            {
                LastPCBSaved += 1;
                ReworkIsEnabled = false;
                OnPropertyChanged(nameof(ReworkIsEnabled));
                SavedReworks.Add(new PCBDefinition(LastPCBSaved, SnNumber, "", "", error));
                OnPropertyChanged(nameof(SavedReworks));
                 SnNumber = String.Empty;
                 OnPropertyChanged(nameof(SnNumber));
                 ReferenceText = String.Empty;
                 OnPropertyChanged(nameof(ReferenceText));
                 ActualPCB = new ItemPCBRework();

            }
            return status;
        }

        public bool SavePCBRework()
        {
            int intRework;
            int nextRework;
            int actualRework;
            String error;
            ItemPCBRework pCBDefinition = new ItemPCBRework();
            PCBDefinition newPCBRework = new PCBDefinition();

            try
            {
                LastPCBSaved += 1;
                intRework = int.Parse(Rework);
                if (ActualPCB.ReworkPCB == 0)
                {
                    actualRework = 0;
                }
                else
                {
                    actualRework = (List_ReworkActualPCB.FirstOrDefault(x => x.Value == ActualPCB.ReworkPCB).Key) + 1;
                }

                if (List_ReworkActualPCB.ContainsValue(intRework))
                    {
                        nextRework = (List_ReworkActualPCB.FirstOrDefault(x => x.Value == intRework).Key)+1;
                        if (nextRework == actualRework)
                        {
                            MessageBox.Show($"El rework {Rework} ya existe para el SN {SnNumber}. No se pueden introducir reworks repetidos para un mismo SN.", "Rework repetido", MessageBoxButton.OK, MessageBoxImage.Warning);
                            error = "Rework Repetido";
                            PCBDefinition newPCBReworkOLD = new PCBDefinition(LastPCBSaved, SnNumber, "", Convert.ToString(intRework), error);
                            newPCBRework = newPCBReworkOLD;
                        }
                        else if (nextRework != actualRework + 1)
                        {
                            MessageBox.Show($"El rework {Rework} no es valido, no sigue la secuencia establecida. Consulte el listado de reworks disponibles para esta PCB con su responsable.", "Rework no permitido", MessageBoxButton.OK, MessageBoxImage.Warning);
                            error = "Rework No Válido";
                            PCBDefinition newPCBReworkOLD = new PCBDefinition(LastPCBSaved, SnNumber, "", Convert.ToString(intRework), error);
                            newPCBRework = newPCBReworkOLD;
                        }
                        else
                        {
                            SQLServer_DDBB_BusinessLogic.InsertReworkSN(SnNumber, intRework, ActualUser, out error);
                            SQLServer_DDBB_BusinessLogic.ExistSNPCBFromProject(SnNumber, Plataform.PlataformName, out pCBDefinition, out error);
                            ActualPCB = pCBDefinition;
                            InsertedPCBs += 1;
                            OnPropertyChanged(nameof(InsertedPCBs));
                            PCBDefinition newPCBReworkOLD = new PCBDefinition(LastPCBSaved, SnNumber, ActualPCB.HardwareReference, Convert.ToString(intRework), error);
                            newPCBRework = newPCBReworkOLD;
                        }
                    }
                    else
                    {
                        MessageBox.Show($"El rework {Rework} no esta definido para esta PCB en la BBDD. Consulte el listado de reworks con la persona correspondiente.", "Rework desconocido", MessageBoxButton.OK, MessageBoxImage.Warning);
                        error = "Rework Desconocido";
                        PCBDefinition newPCBReworkOLD = new PCBDefinition(LastPCBSaved, SnNumber, "", Convert.ToString(intRework), error);
                        newPCBRework = newPCBReworkOLD;
                    }
                    
                    SavePCBRework1 = newPCBRework;
                    ReworkIsEnabled = false;
                    OnPropertyChanged(nameof(ReworkIsEnabled));
                    SavedReworks.Add(SavePCBRework1);
                    OnPropertyChanged(nameof(SavePCBRework1));
                    SnNumber = String.Empty;
                    OnPropertyChanged(nameof(SnNumber));
                    ReferenceText = String.Empty;
                    OnPropertyChanged(nameof(ReferenceText));
                    LayoutBomRework = String.Empty;
                    OnPropertyChanged(nameof(LayoutBomRework));
                    Rework = String.Empty;
                    OnPropertyChanged(nameof(Rework));

                    return true;
            }
            catch (Exception ex)
            {
                Rework = String.Empty;
                OnPropertyChanged(nameof(Rework));

                MessageBox.Show("No se ha podido realizar el rework. Compruebe que los datos introducidos son correctos.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                return false;
            }
        }

        #endregion
    }
}
