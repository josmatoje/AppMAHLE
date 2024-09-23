using Aspose.Cells;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace DIV_Protos
{
    public class AGRegistrationViewModel : BaseViewModel, IPageViewModel
    {
        #region atributos
        private ICommand _backMenu;
        private string actualUser;
        private Plataform plataform;
        private string[] assembly;
        private string selectedAssembly;
        private bool enableOpenFIle = false;
        private string excelPath;
        private string excelName;
        private Visibility visiblePExcelContent = Visibility.Collapsed;
        private Visibility visibleSAExcelContent = Visibility.Collapsed;
        private ObservableCollection<ManufacturingAG> excelRegisters = new ObservableCollection<ManufacturingAG>();
        private SubassemblyDefinition subAssemblyRegister;
        private List<string> images;
        private string numberOfImages;
        private Visibility visibleImageTextContent = Visibility.Collapsed;
        #endregion
        #region propiedades públicas
        public ICommand BackMenu
        {
            get
            {
                return _backMenu ?? (_backMenu = new RelayCommand(x =>
                {
                    //Borrado de datos al salir

                    Mediator.Notify("GoToMenuScreen", "");
                }));
            }
        }
        public string ActualUser { get => actualUser; set => actualUser = value; }
        public Plataform Plataform { get => plataform; set => plataform = value; }
        public string[] Assembly { get => assembly; set => assembly = value; }
        public bool EnableOpenFIle { get => enableOpenFIle; set => enableOpenFIle = value; }
        public string ExcelPath { get => excelPath; set => excelPath = value; }
        public string ExcelName { get => excelName; set => excelName = value; }
        public Visibility VisiblePExcelContent { get => visiblePExcelContent; set => visiblePExcelContent = value; }
        public ObservableCollection<ManufacturingAG> ExcelRegisters { get => excelRegisters; set => excelRegisters = value; }
        public SubassemblyDefinition SubAssemblyRegister { get => subAssemblyRegister; set => subAssemblyRegister = value; }
        public List<string> Images { get => images; set => images = value; }
        public string NumberOfImages { get => numberOfImages; set => numberOfImages = value; }
        public Visibility VisibleImageTextContent { get => visibleImageTextContent; set => visibleImageTextContent = value; }
        #endregion
        #region Constructor
        public AGRegistrationViewModel()
        {
            Images = new List<string>();
        }
        #endregion
        #region Metodos públicos
        public void showExcel(OpenFileDialog excel)
        {
            ExcelPath = excel.FileName;

            String excelSafeName = excel.SafeFileName;
            String[] splited;
            String renamed;

            splited = excelSafeName.Split('.');
            renamed = excelSafeName.Replace($".{splited.Last()}", "");
            if (renamed.Equals(""))
            {
                renamed = excelSafeName;
            }
            ExcelName = renamed;
            OnPropertyChanged(nameof(ExcelName));

            Workbook wb = new Workbook(ExcelPath);
            WorksheetCollection collection = wb.Worksheets;
            Worksheet worksheet = collection[0];

            int rows = worksheet.Cells.MaxDataRow;
            int cols = worksheet.Cells.MaxDataColumn;

            int lastRow = 1;
            Visibility excelVisibility = Visibility.Visible;
            ExcelRegisters.Clear();

            if (cols != 10)
            {
                MessageBox.Show("El número de lineas esperado no corresponde, asegurese de estar en el último formato de modelo para Assembly Guidlines.",
                                    "Error de formato",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Warning);
                VisiblePExcelContent = Visibility.Collapsed;
                OnPropertyChanged(nameof(VisiblePExcelContent));
            }
            else
            {
                while (!worksheet.Cells[lastRow, 0].StringValue.Equals(String.Empty))
                {
                    try
                    {
                        ExcelRegisters.Add(new ManufacturingAG(worksheet.Cells[lastRow, 0].IntValue,
                                                                worksheet.Cells[lastRow, 1].IntValue,
                                                                worksheet.Cells[lastRow, 2].StringValue,
                                                                worksheet.Cells[lastRow, 3].IntValue,
                                                                worksheet.Cells[lastRow, 4].StringValue,
                                                                worksheet.Cells[lastRow, 5].StringValue,
                                                                worksheet.Cells[lastRow, 6].StringValue,
                                                                worksheet.Cells[lastRow, 7].StringValue,
                                                                worksheet.Cells[lastRow, 8].StringValue,
                                                                worksheet.Cells[lastRow, 9].StringValue,
                                                                worksheet.Cells[lastRow, 10].StringValue));
                    } catch {
                        MessageBox.Show("El formato de datos no es el esperado, asegurese de introducir Numeros enteros en las casillas NUM, PROCESS, y OPERATION.",
                                            "Error de formato",
                                            MessageBoxButton.OK,
                                            MessageBoxImage.Warning);

                        excelVisibility = Visibility.Collapsed;
                        break;
                    }
                    lastRow++;
                }
                OnPropertyChanged(nameof(ExcelRegisters));

                VisiblePExcelContent = excelVisibility;
                OnPropertyChanged(nameof(VisiblePExcelContent));
            }
        }
        public void selectImages(OpenFileDialog image)
        {
            foreach (String img in image.FileNames)
            {
                if (!Images.Contains(img))
                {
                    Images.Add(img); //Mantener imagenes anteriores y añadir nuevas (si se seleccionan fotos de varias carpetas)
                }
            }
            NumberOfImages = "Imagenes preparadas para guardadas: " + Images.Count;
            OnPropertyChanged(nameof(NumberOfImages));

            VisibleImageTextContent = Visibility.Visible;
            OnPropertyChanged(nameof(VisibleImageTextContent));
        }
        public void uploadAG()
        {
            List<string> imageNamesDDBB;
            String[] splited;
            String nombreArchivo;
            String renamed;
            bool duplicatePfotos = false;
            bool error = false;
            List<string> resul = new List<string>();

            if (Images.Count > 0)
            {
                imageNamesDDBB = SQLServer_DDBB_BusinessLogic.GetPictureList();
                
                foreach (String img in Images)
                {
                    
                    nombreArchivo = Path.GetFileName(img);
                    splited = nombreArchivo.Split('.');
                    renamed = nombreArchivo.Replace($".{splited.Last()}", "");
                    if (renamed.Equals(""))
                    {
                        renamed = nombreArchivo;
                    }
                    byte[] imagenData = File.ReadAllBytes(img);

                    if (!imageNamesDDBB.Contains(renamed))
                    {
                        //DDBB_MySQL.UpPictureFromFolder(img, out error);
                        SQLServer_DDBB_BusinessLogic.Insert_Picture(renamed, imagenData);
                    } else
                    {
                        duplicatePfotos = true;
                    }
                }
            }
            foreach (ManufacturingAG ag in ExcelRegisters)
            {
                error = !SQLServer_DDBB_BusinessLogic.Insert_DefinitionProcess(ExcelName, ag.Num, ag.Process, ag.ProcessDesc, ag.Operation, ag.OperationDesc, ag.Picture, ag.Satation, ag.Test, ag.Input, ag.ScrewDriver, ag.Scam_dmc);
                
                if (error)
                {
                    SQLServer_DDBB_BusinessLogic.Delete_DefinitionProcess(ExcelName);
                    break;
                }
            }

            if (!error)
            {
                if (duplicatePfotos)
                {
                    MessageBox.Show("Operación realizada con exito. \nHabía fotos en la base de dato que ya existían y no han sido subidas.",
                                    "Datos subidos con éxito",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Warning);
                } else { 
                    MessageBox.Show("Operación realizada con exito.",
                                    "Datos subidos con éxito",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Information);
                }

                VisiblePExcelContent = Visibility.Collapsed;
                OnPropertyChanged(nameof(VisiblePExcelContent));

                ExcelPath = String.Empty;
                ExcelName = String.Empty;
                OnPropertyChanged(nameof(ExcelName));
                Images.Clear();
                NumberOfImages = "";
                OnPropertyChanged(nameof(NumberOfImages));
                ExcelRegisters.Clear();
                OnPropertyChanged(nameof(ExcelRegisters));
                VisibleImageTextContent = Visibility.Collapsed;
                OnPropertyChanged(nameof(VisibleImageTextContent));
            }
        }
        #endregion
    }
}
