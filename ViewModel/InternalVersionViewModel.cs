using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows.Input;
using System.Web;
using System.Text.RegularExpressions;
using System.Windows;
using Microsoft.Win32;
using Aspose.Cells;
using System.Collections.ObjectModel;
using System.Windows.Controls.Primitives;

namespace DIV_Protos
{
    public class InternalVersionViewModel: BaseViewModel, IPageViewModel
    {

        #region atributos
        private ICommand _backMenu;
        private ICommand _save;
        private string actualUser;
        private List<string> itemDescriptions;
        private string selectedItemDescription;
        private string excelPath;
        private string excelName;
        private ReleaseDefinition releaseDefinitionExcel;
        private int idInternalVersion;
        private int insertedBlockCount;
        string pictutreName = null;
        byte[] bytemap = null;
        private Visibility visibleExcelContent = Visibility.Collapsed;
        private bool enableUploadButton = false;

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

        public ICommand Save
        {
            get
            {
                return _save ?? (_save = new RelayCommand(x =>
                {

                }));
            }
        }

        public string ActualUser { get => actualUser; set => actualUser = value; }
        public List<string> ItemDescriptions { get => itemDescriptions; set => itemDescriptions = value; }
        public string SelectedItemDescription { get => selectedItemDescription; set => selectedItemDescription = value; }
        public string ExcelPath { get => excelPath; set => excelPath = value; }
        public string ExcelName { get => excelName; set => excelName = value; }
        public ReleaseDefinition ReleaseDefinitionExcel { get => releaseDefinitionExcel; set => releaseDefinitionExcel = value; }
        public int IdInternalVersion { get => idInternalVersion; set => idInternalVersion = value; }
        public int InsertedBlockCount { get => insertedBlockCount; set => insertedBlockCount = value; }
        public string PictutreName { get => pictutreName; set => pictutreName = value; }
        public byte[] Bytemap { get => bytemap; set => bytemap = value; }
        public Visibility VisibleExcelContent { 
            get => visibleExcelContent;
            set { 
                visibleExcelContent = value; 
                EnableUploadButton = value == Visibility.Visible;
                OnPropertyChanged(nameof(EnableUploadButton));
            }
        }
        public bool EnableUploadButton { get => enableUploadButton; set => enableUploadButton = value; }
        #endregion
        #region Constructor
        public InternalVersionViewModel()
        {
            ItemDescriptions = SQLServer_DDBB_BusinessLogic.GetItemDescriptionList();
            InsertedBlockCount = 0;
        }
        #endregion
        #region metodos públicos
        public void showExcel(OpenFileDialog excel)
        {
            ExcelPath = excel.FileName;
            ExcelName = excel.SafeFileName;
            OnPropertyChanged(nameof(ExcelName));

            try
            {
                Workbook wb = new Workbook(ExcelPath);
                WorksheetCollection collection = wb.Worksheets;
                Worksheet worksheet = collection[0];

                int rows = worksheet.Cells.MaxDataRow;
                int cols = worksheet.Cells.MaxDataColumn;
                int lastRow = 8;

                if (ExcelName.Contains(worksheet.Cells[4, 1].StringValue))
                {
                    List<string> electronicHardwareList;
                    Dictionary<string, List<string>> electroniHardware = new Dictionary<string, List<string>>();
                    Dictionary<string, string> mechanicalHardware = new Dictionary<string, string>();
                    Dictionary<string, string> softwareHardware = new Dictionary<string, string>();
                    Dictionary<string, string> labellingHardware = new Dictionary<string, string>();
                    Dictionary<string, string> testingHardware = new Dictionary<string, string>();

                    //Definition
                    releaseDefinitionExcel = new ReleaseDefinition(
                                                                    worksheet.Cells[1, 1].StringValue,
                                                                    worksheet.Cells[2, 1].StringValue,
                                                                    worksheet.Cells[3, 1].StringValue,
                                                                    worksheet.Cells[4, 1].StringValue,
                                                                    worksheet.Cells[5, 1].StringValue,
                                                                    worksheet.Cells[6, 1].StringValue.Equals("YES"),
                                                                    worksheet.Cells[7, 1].StringValue);

                    //Hardware
                    releaseDefinitionExcel.ElectronicBlock = worksheet.Cells[lastRow, 3].StringValue.Equals("YES");
                    lastRow++;
                    while (!worksheet.Cells[lastRow, 0].StringValue.Equals("mHW eBOM"))
                    {
                        if (releaseDefinitionExcel.ElectronicBlock)
                        {
                            electronicHardwareList = new List<string>
                                {
                                    worksheet.Cells[lastRow, 1].StringValue ?? "",
                                    worksheet.Cells[lastRow, 2].StringValue ?? ""
                                };

                            electroniHardware.Add(worksheet.Cells[lastRow, 0].StringValue, electronicHardwareList);
                        }
                        lastRow++;
                    }
                    releaseDefinitionExcel.ElectronicHardware = electroniHardware;

                    //Mechanical
                    releaseDefinitionExcel.MechanicalBlock = worksheet.Cells[lastRow, 2].StringValue.Equals("YES");
                    lastRow++;
                    while (!worksheet.Cells[lastRow, 0].StringValue.Equals("Software"))
                    {
                        if (releaseDefinitionExcel.MechanicalBlock)
                        {
                            mechanicalHardware.Add(worksheet.Cells[lastRow, 0].StringValue, worksheet.Cells[lastRow, 1].StringValue);
                        }
                        lastRow++;
                    }
                    releaseDefinitionExcel.MechanicalHardware = mechanicalHardware;

                    //Software
                    releaseDefinitionExcel.SoftwareBlock = worksheet.Cells[lastRow, 2].StringValue.Equals("YES");
                    lastRow++;
                    while (!worksheet.Cells[lastRow, 0].StringValue.Equals("Process"))
                    {
                        if (releaseDefinitionExcel.SoftwareBlock)
                        {
                            softwareHardware.Add(worksheet.Cells[lastRow, 0].StringValue, worksheet.Cells[lastRow, 1].StringValue);
                        }
                        lastRow++;
                    }
                    releaseDefinitionExcel.SoftwareHardware = softwareHardware;

                    //Process
                    releaseDefinitionExcel.ProcessBlock = worksheet.Cells[lastRow, 2].StringValue.Equals("YES");
                    lastRow++;
                    if (releaseDefinitionExcel.ProcessBlock)
                    {
                        releaseDefinitionExcel.Process = worksheet.Cells[lastRow, 1].StringValue;
                    }
                    lastRow++;

                    //Labelling
                    releaseDefinitionExcel.LabellingBlock = worksheet.Cells[lastRow, 2].StringValue.Equals("YES");
                    lastRow++;
                    while (!worksheet.Cells[lastRow, 0].StringValue.Equals("Testing"))
                    {
                        if (releaseDefinitionExcel.LabellingBlock)
                        {
                            labellingHardware.Add(worksheet.Cells[lastRow, 0].StringValue, worksheet.Cells[lastRow, 1].StringValue);
                        }
                        lastRow++;
                    }
                    releaseDefinitionExcel.LabellingHardware = labellingHardware;

                    //Testing
                    releaseDefinitionExcel.TestingBlock = worksheet.Cells[lastRow, 2].StringValue.Equals("YES");
                    lastRow++;
                    while (!worksheet.Cells[lastRow, 0].StringValue.Equals("END"))
                    {
                        if (releaseDefinitionExcel.TestingBlock)
                        {
                            testingHardware.Add(worksheet.Cells[lastRow, 0].StringValue, worksheet.Cells[lastRow, 1].StringValue);
                        }
                        lastRow++;
                    }
                    releaseDefinitionExcel.TestingHardware = testingHardware;

                    OnPropertyChanged(nameof(ReleaseDefinitionExcel));

                    VisibleExcelContent = Visibility.Visible;
                    OnPropertyChanged(nameof(VisibleExcelContent));
                }
                else
                {
                    MessageBox.Show($"La {SelectedItemDescription} definida no se corresponde con la seleccionada.\n\nPor favor revise que la versión interna que aparece en el titulo del Excel introducido corresponde con la casilla de 'Internal name' del mismo.",
                                    "Versión interna no válida",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Warning);
                    VisibleExcelContent = Visibility.Collapsed;
                    OnPropertyChanged(nameof(VisibleExcelContent));
                }
            } catch(Exception error)
            {
                MessageBox.Show($"Ha ocurrido un error al cargar el excel.\n{error}",
                                    "Versión interna no válida",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Warning);
                VisibleExcelContent = Visibility.Collapsed;
                OnPropertyChanged(nameof(VisibleExcelContent));
            }
        }

        public void uploadVersion()
        {
            bool correctInsert = true;
            bool isFirstInsertedItemDefinition = false;
            Dictionary<string, List<string>> hardwareBlockList;
            Dictionary<string, string> blockList;
            bool isItemDefinitionEnable = true;
            try
            {
                //Busqueda del id versión interna ya creado
                idInternalVersion = SQLServer_DDBB_BusinessLogic.GetIdItemDefinition(ReleaseDefinitionExcel.ProjectName, ReleaseDefinitionExcel.Sample, ReleaseDefinitionExcel.PartNumber, ReleaseDefinitionExcel.InternalName, ReleaseDefinitionExcel.DescriptionReference, ReleaseDefinitionExcel.Trazability, ReleaseDefinitionExcel.Date);
                //Si no existe
                if (idInternalVersion == -1)
                {
                    correctInsert = SQLServer_DDBB_BusinessLogic.Insert_ItemDefinition(ReleaseDefinitionExcel.ProjectName, ReleaseDefinitionExcel.Sample, ReleaseDefinitionExcel.PartNumber, SelectedItemDescription, ReleaseDefinitionExcel.InternalName, ReleaseDefinitionExcel.DescriptionReference, ReleaseDefinitionExcel.Trazability, PictutreName, Bytemap , ReleaseDefinitionExcel.Date, ActualUser, out idInternalVersion);
                    isFirstInsertedItemDefinition = true;
                }
                
                //Hardware
                if (correctInsert && ReleaseDefinitionExcel.ElectronicBlock)
                {
                    //Buscar listado de Haedware
                    hardwareBlockList = SQLServer_DDBB_BusinessLogic.GetElectronicHardwareList(IdInternalVersion);
                    isItemDefinitionEnable &= ReleaseDefinitionExcel.ElectronicHardware.Count > 0;
                    //Si existen datos
                    if (hardwareBlockList.Count == 0)
                    {
                        foreach (var hardwareList in ReleaseDefinitionExcel.ElectronicHardware)
                        {
                            foreach (var hardware in hardwareList.Value)
                            {
                                if (hardware != String.Empty) { 
                                    correctInsert = SQLServer_DDBB_BusinessLogic.Insert_HardwareVersion(idInternalVersion, hardware);
                                    if (!correctInsert)
                                    {
                                        break;
                                    } else
                                    {
                                        ReleaseDefinitionExcel.InsertHardware = true;
                                    }
                                }
                            }
                            if (!correctInsert)
                            {
                                break;
                            }
                        }
                    } else //Comprueba excel y bbdd
                    {
                        foreach (var hardware in hardwareBlockList)
                        {
                            if (ReleaseDefinitionExcel.ElectronicHardware.ContainsKey(hardware.Key))
                            {
                                foreach (var hardwareValue in hardware.Value)
                                {
                                    if (!ReleaseDefinitionExcel.ElectronicHardware[hardware.Key].Contains(hardwareValue))
                                    {
                                        correctInsert = false;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                correctInsert = false;
                                break;
                            }
                        }
                        if (!correctInsert)
                        {
                            MessageBox.Show("El bloque de hardware registrado en la Base de Datos no corresponde con el que se intenta guardar.",
                                                        "Bloque hardware incorrecto.",
                                                        MessageBoxButton.OK,
                                                        MessageBoxImage.Warning);
                        }
                    }
                    
                }

                //Mechanical
                if (correctInsert && ReleaseDefinitionExcel.MechanicalBlock)
                {
                    blockList = SQLServer_DDBB_BusinessLogic.GetMechanicalHardwareList(IdInternalVersion);
                    isItemDefinitionEnable &= ReleaseDefinitionExcel.MechanicalHardware.Count > 0;
                    if (blockList.Count == 0)
                    {
                        if (ReleaseDefinitionExcel.MechanicalHardware.Values.All((item) => item.Equals(String.Empty))) //Si todos están rellenos
                        {
                            foreach (var mechanical in ReleaseDefinitionExcel.MechanicalHardware)
                            {
                                correctInsert = SQLServer_DDBB_BusinessLogic.Insert_MechanicalVersion(idInternalVersion, mechanical.Key, mechanical.Value);
                                if (!correctInsert)
                                {
                                    break;
                                }
                                else
                                {
                                    ReleaseDefinitionExcel.InsertMechanical = true;
                                }
                            }

                        } else if (ReleaseDefinitionExcel.MechanicalHardware.Values.Any((item) => item.Equals(String.Empty)) && ReleaseDefinitionExcel.MechanicalHardware.Values.Any((item) => !item.Equals(String.Empty)))
                        { //Si encuentra algunos parámetros vacios en el excel aborta el insert
                            correctInsert = false;

                            MessageBox.Show("El bloque de mecánica del excel no está relleno por completo.",
                                                        "Bloque mecánica incompleto.",
                                                        MessageBoxButton.OK,
                                                        MessageBoxImage.Warning);
                        } 
                    } else
                    {
                        foreach (var mechanical in blockList)
                        {
                            if (ReleaseDefinitionExcel.MechanicalHardware.ContainsKey(mechanical.Key))
                            {
                                if (!ReleaseDefinitionExcel.MechanicalHardware[mechanical.Key].Equals(mechanical.Value))
                                {
                                    correctInsert = false;
                                    break;
                                }
                            } else
                            {
                                correctInsert = false;
                                break;
                            }
                        }
                        if (!correctInsert)
                        {
                            MessageBox.Show("El bloque de mecánica registrado en la Base de Datos no corresponde con el que se intenta guardar.",
                                                        "Bloque mecánica incorrecto.",
                                                        MessageBoxButton.OK,
                                                        MessageBoxImage.Warning);
                        }
                    }


                }

                //Software
                if (correctInsert && ReleaseDefinitionExcel.SoftwareBlock)
                {
                    blockList = SQLServer_DDBB_BusinessLogic.GetSoftwareList(IdInternalVersion);
                    isItemDefinitionEnable &= ReleaseDefinitionExcel.SoftwareHardware.Count > 0;
                    if (blockList.Count == 0)
                    {
                        foreach (var software in ReleaseDefinitionExcel.SoftwareHardware)
                        {
                            correctInsert = SQLServer_DDBB_BusinessLogic.Insert_SoftwareVersion(idInternalVersion, software.Key, software.Value);
                            if (!correctInsert)
                            {
                                break;
                            }
                            else
                            {
                                ReleaseDefinitionExcel.InsertSoftware = true;
                            }
                        }
                    } else
                    {
                        foreach (var software in blockList)
                        {
                            if (ReleaseDefinitionExcel.SoftwareHardware.ContainsKey(software.Key))
                            {
                                if (!ReleaseDefinitionExcel.SoftwareHardware[software.Key].Equals(software.Value))
                                {
                                    correctInsert = false;
                                    break;
                                }
                            }
                            else
                            {
                                correctInsert = false;
                                break;
                            }
                        }
                        if (!correctInsert)
                        {
                            MessageBox.Show("El bloque de software registrado en la Base de Datos no corresponde con el que se intenta guardar.",
                                                        "Bloque software incorrecto.",
                                                        MessageBoxButton.OK,
                                                        MessageBoxImage.Warning);
                        }
                    }
                }

                //Process
                if (correctInsert && ReleaseDefinitionExcel.ProcessBlock)
                {
                    string processName = SQLServer_DDBB_BusinessLogic.GetProcess(IdInternalVersion);
                    isItemDefinitionEnable &= !ReleaseDefinitionExcel.Process.Equals(String.Empty);
                    if (processName.Equals(String.Empty))
                    {
                        correctInsert = SQLServer_DDBB_BusinessLogic.Insert_ProcessVersion(idInternalVersion,ReleaseDefinitionExcel.Process);
                        ReleaseDefinitionExcel.InsertProcess = true;
                    } else if (!processName.Equals(ReleaseDefinitionExcel.Process))
                    {
                        correctInsert = false;
                        MessageBox.Show("El proceso registrado en la Base de Datos no corresponde con el que se intenta guardar.",
                                                "Proceso incorrecto",
                                                MessageBoxButton.OK,
                                                MessageBoxImage.Warning);
                    }
                }

                //Labelling
                if (correctInsert && ReleaseDefinitionExcel.LabellingBlock)
                {
                    blockList = SQLServer_DDBB_BusinessLogic.GetLabellingList(IdInternalVersion);
                    isItemDefinitionEnable &= ReleaseDefinitionExcel.LabellingHardware.Count > 0;
                    if (blockList.Count == 0)
                    {
                        foreach (var labelling in ReleaseDefinitionExcel.LabellingHardware)
                        {
                            correctInsert = SQLServer_DDBB_BusinessLogic.Insert_LabellingVersion(idInternalVersion, labelling.Key, labelling.Value);
                            if (!correctInsert)
                            {
                                break;
                            }
                            else
                            {
                                ReleaseDefinitionExcel.InsertLabelling = true;
                            }
                        }
                    } else
                    {
                        foreach (var labelling in blockList)
                        {
                            if (ReleaseDefinitionExcel.LabellingHardware.ContainsKey(labelling.Key))
                            {
                                if (!ReleaseDefinitionExcel.LabellingHardware[labelling.Key].Equals(labelling.Value))
                                {
                                    correctInsert = false;
                                    break;
                                }
                            }
                            else
                            {
                                correctInsert = false;
                                break;
                            }
                        }
                        if (!correctInsert)
                        {
                            MessageBox.Show("El bloque de labelling registrado en la Base de Datos no corresponde con el que se intenta guardar.",
                                                        "Bloque labelling incorrecto.",
                                                        MessageBoxButton.OK,
                                                        MessageBoxImage.Warning);
                        }
                    }
                }

                //Testing
                if (correctInsert && ReleaseDefinitionExcel.TestingBlock)
                {
                    blockList = SQLServer_DDBB_BusinessLogic.GetTestList(IdInternalVersion);
                    isItemDefinitionEnable &= ReleaseDefinitionExcel.TestingHardware.Count > 0;
                    if (blockList.Count == 0)
                    {
                        foreach (var test in ReleaseDefinitionExcel.TestingHardware)
                        {
                            try
                            {
                                correctInsert = SQLServer_DDBB_BusinessLogic.Insert_TestVersion(idInternalVersion, test.Key, Int32.Parse(test.Value));
                            }
                            catch (Exception ex)
                            {
                                correctInsert = false;
                                MessageBox.Show("La versión de test debe de ser un numero entero.",
                                                "Versión de test incorrecta",
                                                MessageBoxButton.OK,
                                                MessageBoxImage.Warning);
                            }
                            if (!correctInsert)
                            {
                                break;
                            }
                            else
                            {
                                ReleaseDefinitionExcel.InsertTest = true;
                            }
                        }
                    } else
                    {
                        foreach (var test in blockList)
                        {
                            if (ReleaseDefinitionExcel.TestingHardware.ContainsKey(test.Key))
                            {
                                if (!ReleaseDefinitionExcel.TestingHardware[test.Key].Equals(test.Value))
                                {
                                    correctInsert = false;
                                    break;
                                }
                            }
                            else
                            {
                                correctInsert = false;
                                break;
                            }
                        }
                        if (!correctInsert)
                        {
                            MessageBox.Show("El bloque de test registrado en la Base de Datos no corresponde con el que se intenta guardar.",
                                                        "Bloque test incorrecto.",
                                                        MessageBoxButton.OK,
                                                        MessageBoxImage.Warning);
                        }
                    }
                }
                if (correctInsert && isItemDefinitionEnable) //Se han introducido datos en todos los bloques
                {
                    //Se actualiza a 1 el enable

                    //Si falla algo correctInsert = false
                }

                if (correctInsert)
                {
                    if (isItemDefinitionEnable)
                    {
                        MessageBox.Show("Operación realizada con exito.\nItemDefinition habilitada para producción.",
                                    "Datos subidos con éxito",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Information);
                    } else
                    {
                        MessageBox.Show("Operación realizada con exito.\nDatos subidos sin completar. Debe rellenar los datos de todos los bloques obligatorios.",
                                    "Datos subidos con éxito",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Exclamation);
                    }

                    VisibleExcelContent = Visibility.Collapsed;
                    OnPropertyChanged(nameof(VisibleExcelContent));

                    ExcelPath = String.Empty;
                    ExcelName = String.Empty;
                    OnPropertyChanged(nameof(ExcelName));
                } else
                {
                    if (ReleaseDefinitionExcel.InsertHardware)
                    {
                        SQLServer_DDBB_BusinessLogic.Delete_HardwareVersion(IdInternalVersion);
                    }
                    if (ReleaseDefinitionExcel.InsertMechanical)
                    {
                        SQLServer_DDBB_BusinessLogic.Delete_MechanicalVersion(IdInternalVersion);
                    }
                    if (ReleaseDefinitionExcel.InsertSoftware)
                    {
                        SQLServer_DDBB_BusinessLogic.Delete_SoftwareVersion(IdInternalVersion);
                    }
                    if (ReleaseDefinitionExcel.InsertProcess)
                    {
                        SQLServer_DDBB_BusinessLogic.Delete_ProcessVersion(IdInternalVersion);
                    }
                    if (ReleaseDefinitionExcel.InsertLabelling)
                    {
                        SQLServer_DDBB_BusinessLogic.Delete_LabellingVersion(IdInternalVersion);
                    }
                    if (ReleaseDefinitionExcel.InsertTest)
                    {
                        SQLServer_DDBB_BusinessLogic.Delete_TestingVersion(IdInternalVersion);
                    }

                    if (isFirstInsertedItemDefinition)
                    {
                        //Borrar ItemDefinition
                    }

                    MessageBox.Show("El bloque de test registrado en la Base de Datos no corresponde con el que se intenta guardar.",
                                                "Bloque test incorrecto.",
                                                MessageBoxButton.OK,
                                                MessageBoxImage.Warning);
                }
            }catch(Exception error)
            {
                MessageBox.Show($"Error desconocido:\n\n{error.Message}\n",
                                "Error Fatal :(",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
            }
        }
        #endregion
    }
}
