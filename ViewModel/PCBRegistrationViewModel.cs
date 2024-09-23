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
    class PCBRegistrationViewModel : BaseViewModel, IPageViewModel
    {
        #region atributos
        private ICommand _backMenu;
        private string actualUser;
        private Plataform plataform;
        private ObservableCollection<PCBReference> pcbsRefs = new ObservableCollection<PCBReference>();
        private ObservableCollection<string> pcbReferences = new ObservableCollection<string>();
        private ObservableCollection<string> pcbLayoutBOM = new ObservableCollection<string>();
        private string snNumber = "";
        private int idReferencePCB = -1;
        private string referenceText = "";
        private string layoutBom = "";
        private string lote = "";
        private ObservableCollection<SavedPCB> savedPCBs = new ObservableCollection<SavedPCB>();
        private int lastPCBSaved = 1;
        private int insertedPCBs = 0;
        private bool snTexBoxEnabled = false;
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
        public string ActualUser { get => actualUser; set => actualUser = value; }
        public Plataform Plataform { get => plataform; set => plataform = value; }
        public ObservableCollection<PCBReference> PcbsRefs {
            get =>  pcbsRefs;
            set => pcbsRefs = value; 
        }
        public ObservableCollection<string> PcbReferences {
            get
            {
                if (pcbReferences.Count == 0)
                {
                    String err_msg = String.Empty;
                    DDBB_MySQL.GetPCBReferences(Plataform.Id, out pcbsRefs, out err_msg);
                if (err_msg == null || err_msg == String.Empty)
                {
                    foreach (var pcbRef in pcbsRefs)
                    {
                        pcbReferences.Add("LG-" + pcbRef.Reference + "-" + pcbRef.ReferenceName);
                    };
                }
                OnPropertyChanged(nameof(PcbReferences));
            }
                return pcbReferences;
            }
            set => pcbReferences = value; 
        }
        public ObservableCollection<string> PcbLayoutBOM { get => pcbLayoutBOM; set => pcbLayoutBOM = value; }
        public string SnNumber { get => snNumber; set => snNumber = value; }
        public int IdReferencePCB { get => idReferencePCB; set => idReferencePCB = value; }
        public string ReferenceText {
            get => referenceText; 
            set {
                referenceText = value;
                IdReferencePCB = PcbsRefs.Where(pcb  => value.Contains(pcb.ReferenceName)).First().Id;

                ObservableCollection<string> pcbsComp = new ObservableCollection<string>();
                PcbLayoutBOM = new ObservableCollection<string>();
                LayoutBom = String.Empty;
                String error;
                try
                {
                    DDBB_MySQL.GetPCBBLayoutBOMFrom(IdReferencePCB, out pcbsComp, out error);

                    if (error == null || error == String.Empty)
                    {
                        foreach (var pcbCompon in pcbsComp)
                        {
                            PcbLayoutBOM.Add(pcbCompon);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Ha ocurrido Actualizando los Layouts.", "Error de conexión", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (Exception ex)
                {
                    error = ex.Message; 
                    MessageBox.Show("Ha ocurrido Actualizando los Layouts.", "Error de conexión", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                OnPropertyChanged(nameof(PcbLayoutBOM));
            }
        }
        public string LayoutBom { 
            get => layoutBom;
            set
            {
                layoutBom = value;
                SnTexBoxEnabled = (value != null && value != String.Empty);
                OnPropertyChanged(nameof(SnTexBoxEnabled));
            }
        }
        public string Lote { get => lote; set => lote = value; }
        public ObservableCollection<SavedPCB> SavedPCBs { get => savedPCBs; set => savedPCBs = value; }
        public int LastPCBSaved { get => lastPCBSaved; set => lastPCBSaved = value; }
        public bool SnTexBoxEnabled { get => snTexBoxEnabled; set => snTexBoxEnabled = value; }
        public int InsertedPCBs { get => insertedPCBs; set => insertedPCBs = value; }
        #endregion
        #region Constructor
        public PCBRegistrationViewModel()
        {
        }
        #endregion
        #region Metodos públicos
        public bool SavePCB()
        {
            bool saved = false;
            String error;
            int bom = 0;
            int layout = 0;
            if (LayoutBom != String.Empty)
            {
                string[] datas = LayoutBom.Split(new string[] { "." }, StringSplitOptions.None);
                if (datas.Length == 2 )
                {
                    layout = int.Parse(datas[0]);
                    bom = int.Parse(datas[1]);
                }
            }

            if (DDBB_MySQL.ExistsSNFromRegPCB(SnNumber, out error) || DDBB_MySQL.ExistsSNFromRegHousing(SnNumber, out error))
            {
                MessageBox.Show($"El SN {SnNumber} ya existe. No se pueden introducir SN repetidos.", "SN repetido", MessageBoxButton.OK, MessageBoxImage.Warning);
                error = "Repetido";
            }
            else
            {
                SQLServer_DDBB_Conection.Insert_PCB(ActualUser, SnNumber, IdReferencePCB, layout, bom, Lote);
            }

            SavedPCB newPcb = new SavedPCB(LastPCBSaved, SnNumber, ReferenceText, LayoutBom, Lote, saved);
            LastPCBSaved += 1;
            if (saved)
            {
                InsertedPCBs += 1;
                OnPropertyChanged(nameof(InsertedPCBs));
            }

            if (error == "Repetido")
            {
                newPcb.Saved = "EXISTENTE";
            }

            SavedPCBs.Add(newPcb);
            OnPropertyChanged(nameof(SavedPCBs));

            SnNumber = String.Empty;
            OnPropertyChanged(nameof(SnNumber));

            Lote = "";
            OnPropertyChanged(nameof(Lote));

            return saved;
        }

        #endregion
    }
}
