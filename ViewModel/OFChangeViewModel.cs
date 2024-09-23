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
    public class OFChangeViewModel: BaseViewModel, IPageViewModel
    {
        #region atributos
        private Plataform plataform;
        private ICommand _backMenu;
        private Visibility showValues = Visibility.Hidden;
        private ObservableCollection<AGInfor_ChangeOF> processAG = new ObservableCollection<AGInfor_ChangeOF>();
        private Visibility visibleSaveChangeOF = Visibility.Collapsed;
        private Visibility visibleResearchSN = Visibility.Collapsed;
        private Visibility visibleOF = Visibility.Collapsed;
        private Visibility visiblePendingSN = Visibility.Collapsed;
        private List<string[]> rows_data_ddbb_historySN = new List<string[]>();
        private OFChangeDescrip oldOFIV = new OFChangeDescrip();
        private OFChangeDescrip newOFIV = new OFChangeDescrip();
        private string actualUser;
        private string description;
        private string oldDescription;
        private string internalVersion;
        private string oldInternalVersion;
        private string serialNumber;
        private string opPendingView;
        private string opSave;
        private bool pending = false;
        private bool enableOk;
        private int historySNPending;
        private int lastOPSN;
        private string comment="";

        //private bool enableOk;
        #endregion

        #region propiedades públicas
        //public bool EnableOk { get => enableOk; set => enableOk = value; }
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

        public Plataform Plataform { get => plataform; set => plataform = value; }

        public string ActualUser { get => actualUser; set => actualUser = value; }

        public string Description
        {
            get => description;
            set
            {
                description = value;
                EnableOk = isEnableButton();
                OnPropertyChanged(nameof(EnableOk));
            }
        }
        public string InternalVersion { get => internalVersion; set => internalVersion = value; }
        public string SerialNumber
        {
            get => serialNumber;
            set
            {
                serialNumber = value;
                EnableOk = isEnableButton();
                OnPropertyChanged(nameof(EnableOk));
            }
        }
        public string OldDescription { get => oldDescription; set => oldDescription = value; }
        public string OldInternalVersion { get => oldInternalVersion; set => oldInternalVersion = value; }
        public Visibility ShowValues { get => showValues; set => showValues = value; }
        public ObservableCollection<AGInfor_ChangeOF> ProcessAG { get => processAG; set => processAG = value; }
        public string OpSave
        {
            get => opSave;
            set
            {
                opSave = value;
                EnableOk = isEnableButton();
                OnPropertyChanged(nameof(EnableOk));
            }

        }
        public int HistorySNPending { get => historySNPending; set => historySNPending = value; }
        public bool Pending { get => pending; set => pending = value; }
        public Visibility VisibleSaveChangeOF { get => visibleSaveChangeOF; set => visibleSaveChangeOF = value; }
        public string OpPendingView { get => opPendingView; set => opPendingView = value; }
        public Visibility VisibleResearchSN { get => visibleResearchSN; set => visibleResearchSN = value; }
        public Visibility VisiblePendingSN { get => visiblePendingSN; set => visiblePendingSN = value; }
        public bool EnableOk { get => enableOk; set => enableOk = value; }
        public OFChangeDescrip OldOFIV { get => oldOFIV; set => oldOFIV = value; }
        public OFChangeDescrip NewOFIV { get => newOFIV; set => newOFIV = value; }
        public string Comment { get => comment; set => comment = value; }
        public int LastOPSN { get => lastOPSN; set => lastOPSN = value; }
        public Visibility VisibleOF { get => visibleOF; set => visibleOF = value; }

        #endregion
        #region Metodos públicos
        public bool ExistSerialNumberOFandVI()
        {
            bool status = true;
            String error = String.Empty;
            int historySNPending = 0;

            OldOFIV = SQLServer_DDBB_BusinessLogic.GetLastInforFromSN(SerialNumber, out int lastOPSN,  out historySNPending, out error);
            LastOPSN = lastOPSN;
            HistorySNPending = historySNPending;
            OldDescription = OldOFIV.CodeOF;
            OldInternalVersion = OldOFIV.InternalName;
            OnPropertyChanged(nameof(OldDescription));
            OnPropertyChanged(nameof(OldInternalVersion));

            if (OldInternalVersion != "")
            {
                OpPendingView = "Este SN tiene " + HistorySNPending + " operaciones pendientes";
                OpPendingView = Convert.ToString(OpPendingView + "\nUltima operación: " + LastOPSN);
                OnPropertyChanged(nameof(OpPendingView));
                VisiblePendingSN = Visibility.Visible;
                OnPropertyChanged(nameof(VisiblePendingSN));
                VisibleResearchSN = Visibility.Visible;
                OnPropertyChanged(nameof(VisibleResearchSN));
                VisibleOF = Visibility.Visible;
                OnPropertyChanged(nameof(VisibleOF));
            }
            else
            {
                OldDescription = String.Empty;
                OnPropertyChanged(nameof(OldDescription));
                OpSave = "";
                OnPropertyChanged(nameof(OpSave));
                oldInternalVersion = String.Empty;
                OnPropertyChanged(nameof(OldInternalVersion));
                Description = String.Empty;
                OnPropertyChanged(nameof(Description));
                SerialNumber = String.Empty;
                OnPropertyChanged(nameof(SerialNumber));
                InternalVersion = String.Empty;
                OnPropertyChanged(nameof(InternalVersion));
                ShowValues = Visibility.Hidden;
                OnPropertyChanged(nameof(ShowValues));
                VisibleResearchSN = Visibility.Hidden;
                OnPropertyChanged(nameof(VisibleResearchSN));
                VisiblePendingSN = Visibility.Hidden;
                OnPropertyChanged(nameof(VisiblePendingSN));
                VisibleSaveChangeOF = Visibility.Hidden;
                OnPropertyChanged(nameof(VisibleSaveChangeOF));
                status = false;
                Comment = "";
                OnPropertyChanged(nameof(Comment));
                VisibleOF = Visibility.Hidden;
                OnPropertyChanged(nameof(VisibleOF));
            }
            return status;
        }
        public bool ExistInternalVersion()
        {
            bool status = true;
            String error = String.Empty;
            ProcessAG = new ObservableCollection<AGInfor_ChangeOF>();

            NewOFIV = SQLServer_DDBB_BusinessLogic.GetInternalVersionFromOF(Description, out error);
            InternalVersion = NewOFIV.InternalName;
            OnPropertyChanged(nameof(InternalVersion));

            if (InternalVersion != "")
            {
                
                ProcessAG = SQLServer_DDBB_BusinessLogic.GetManufacturingFromOF(Description, out error);
                OnPropertyChanged(nameof(ProcessAG));
                if (ProcessAG.Count != 0)
                {
                    VisibleSaveChangeOF = Visibility.Visible;
                    OnPropertyChanged(nameof(VisibleSaveChangeOF));
                }
                else
                {
                    VisibleSaveChangeOF = Visibility.Hidden;
                    OnPropertyChanged(nameof(VisibleSaveChangeOF));
                    status = false;
                }
            }
            else
            {
                VisibleSaveChangeOF = Visibility.Hidden;
                OnPropertyChanged(nameof(VisibleSaveChangeOF));
                status = false;
            }
            return status;
        }

        public bool uploadChangeOF()
        {
            bool status = true;
            String error = String.Empty;
            string status_SN = String.Empty;
            List<string> rows_data_ddbb = new List<string>();
            string message = string.Empty;
            int result_int;

            if (int.TryParse(OpSave, out result_int))
            {
                foreach(AGInfor_ChangeOF item in ProcessAG)
                {
                    if (item.Num == Convert.ToInt16(OpSave))
                    {
                        SQLServer_DDBB_BusinessLogic.InsertChangeOF(SerialNumber, OldDescription, Description, Comment, Convert.ToInt16(OpSave), ActualUser, out error);
                        if (error.Equals(String.Empty))
                        {
                            MessageBox.Show("Actualizacion de OF completada con exito.",
                                            "Datos subidos con éxito",
                                            MessageBoxButton.OK,
                                            MessageBoxImage.Information);
                            OldDescription = String.Empty;
                            OnPropertyChanged(nameof(OldDescription));
                            OpSave = "";
                            OnPropertyChanged(nameof(OpSave));
                            oldInternalVersion = String.Empty;
                            OnPropertyChanged(nameof(OldInternalVersion));
                            Description = String.Empty;
                            OnPropertyChanged(nameof(Description));
                            SerialNumber = String.Empty;
                            OnPropertyChanged(nameof(SerialNumber));
                            InternalVersion = String.Empty;
                            OnPropertyChanged(nameof(InternalVersion));
                            ShowValues = Visibility.Hidden;
                            OnPropertyChanged(nameof(ShowValues));
                            VisibleResearchSN = Visibility.Hidden;
                            OnPropertyChanged(nameof(VisibleResearchSN));
                            VisiblePendingSN = Visibility.Hidden;
                            OnPropertyChanged(nameof(VisiblePendingSN));
                            VisibleSaveChangeOF = Visibility.Hidden;
                            OnPropertyChanged(nameof(VisibleSaveChangeOF));
                            Comment = "";
                            OnPropertyChanged(nameof(Comment));
                            VisibleOF = Visibility.Hidden;
                            OnPropertyChanged(nameof(VisibleOF));
                        }
                        else
                        {

                        }
                        break;
                    }
                    if(item.Num == ProcessAG[ProcessAG.Count-1].Num)
                    {
                        System.Windows.MessageBox.Show("La operacion introducida no se encuentra dentro del listado mostrado de operaciones permitidas.\n" + " Intentelo de nuevo, si el problema persiste avise a su Responsable.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        OpSave = "";
                    }
                    
                }
            }
            else
            {
                System.Windows.MessageBox.Show("Formato valor introducido incorrecto.\n" + " Intentelo de nuevo, si el problema persiste avise a su Responsable.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                OpSave = "";
            }
            return status;
        }
        public bool isEnableButton()
        {
            bool enable = false;
            if (Description != null && SerialNumber != null && OpSave != null)
            {
                enable = !Description.Equals(String.Empty) && !SerialNumber.Equals(String.Empty) && !OpSave.Equals(String.Empty);
            }

            return enable;
        }
        #endregion
    }
}
