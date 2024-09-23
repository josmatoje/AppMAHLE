using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows;

namespace DIV_Protos
{
    public class SQLServer_DDBB_BusinessLogic
    {
        #region Read Methods
        public static ObservableCollection<Plataform> GetProject()
        {
            DataTable dt = new DataTable();
            ObservableCollection<Plataform> plataforms = new ObservableCollection<Plataform>();

            try
            {
                dt = SQLServer_DDBB_Conection.GetProject();
            }
            catch (Exception error)
            {
                MessageBox.Show($"Ha ocurrido un error al consultar los datos.\n{error.Message}",
                                "Error consulta",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
            }

            foreach (DataRow row in dt.Select())
            {
                plataforms.Add(new Plataform((int)row["IdItemProject"], (string)row["ProjectName"]));
            }
            return plataforms;
        }

        public static List<string> GetCategoryList()
        {
            DataTable dt = new DataTable();
            List<string> rols = new List<string>();

            try
            {
                dt = SQLServer_DDBB_Conection.GetCategoryList();
            }
            catch (Exception error)
            {
                MessageBox.Show($"Ha ocurrido un error al consultar los datos.\n{error.Message}",
                                "Error consulta",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
            }

            foreach (DataRow row in dt.Select())
            {
                rols.Add((string)row["CategoryDescription"]);
            }
            return rols;
        }

        public static List<string> GetItemDescriptionList()
        {
            DataTable dt = new DataTable();
            List<string> categories = new List<string>();

            try
            {
                dt = SQLServer_DDBB_Conection.GetItemDescriptionList();
            }
            catch (Exception error)
            {
                MessageBox.Show($"Ha ocurrido un error al consultar los datos.\n{error.Message}",
                                "Error de conexión",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
            }

            foreach (DataRow row in dt.Select())
            {
                categories.Add((string)row["DescriptionItem"]);
            }
            return categories;
        }
        public static List<string> GetWindChillListFromCategory(string category)
        {
            DataTable dt = new DataTable();
            List<string> rols = new List<string>();

            try
            {
                dt = SQLServer_DDBB_Conection.GetWindChillListFromCategory(category);
            }
            catch (Exception error)
            {
                MessageBox.Show($"Ha ocurrido un error al consultar los datos.\n{error.Message}",
                                "Error consulta",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
            }

            foreach (DataRow row in dt.Select())
            {
                rols.Add((string)row["WindChillCode"]);
            }
            return rols;
        }
        public static ObservableCollection<ItemPCBDefinition> GetHardwarePCBParametersFromProject(string ProjectName)
        {
            DataTable dt = new DataTable();
            ObservableCollection<ItemPCBDefinition> rols = new ObservableCollection<ItemPCBDefinition>();

            try
            {
                dt = SQLServer_DDBB_Conection.GetHardwarePCBParametersFromProject(ProjectName);
            }
            catch (Exception error)
            {
                MessageBox.Show($"Ha ocurrido un error al consultar los datos.\n{error.Message}",
                                "Error consulta",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
            }


            foreach (DataRow row in dt.Select())
            {
                rols.Add(new ItemPCBDefinition((int)row["IdItemBomPCB"], (string)row["HardwareReference"], (string)row["ReferenceName"], (string)row["LayoutBOM"]));
            }
            return rols;
        }
        
        public static bool ExistsAndInsertSN_PCB(string SerialNumber, string ReferenceName, string LayoutBOM, string ProyectName, string CategorySN,
                                bool InsertSN, string Batch, string UserName, string WindchillCode, out string error_consult)
        {
            bool status = true;
            DataTable dt = new DataTable();
            List<string> rols = new List<string>();
            error_consult = "OK";
            try
            {
                dt = SQLServer_DDBB_Conection.ExistsAndInsertSN_PCB(SerialNumber, ReferenceName, LayoutBOM, ProyectName, CategorySN,
                               InsertSN, Batch, UserName, WindchillCode);
            }
            catch (Exception error)
            {
                status = false;
                error_consult = error.Message;
                MessageBox.Show($"Ha ocurrido un error al consultar los datos.\n{error.Message}",
                                "Error consulta",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
            }
            return status;
        }
        public static bool ExistsAndInsertSN(string SerialNumber , string ProyectName, string CategorySN,
								bool InsertSN, string UserName, string WindchillCode, out string error_consult)
        {
            bool status = true;
            DataTable dt = new DataTable();
            List<string> rols = new List<string>();
            error_consult = "OK";
            try
            {
                dt = SQLServer_DDBB_Conection.ExistsAndInsertSN(SerialNumber, ProyectName, CategorySN,
                               InsertSN, UserName, WindchillCode);
            }
            catch (Exception error)
            {
                status = false;
                error_consult = error.Message;
                MessageBox.Show($"Ha ocurrido un error al consultar los datos.\n{error.Message}",
                                "Error consulta",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
            }
            return status;
        }
        public static bool ExistSNPCBFromProject(string SerialNumber, string ProjectName, out ItemPCBRework itemPCBRework, out string error_consult)
        {
            bool status = true;
            DataTable dt = new DataTable();
            ItemPCBRework rols = new ItemPCBRework();
            error_consult = "OK";
            try
            {
                dt = SQLServer_DDBB_Conection.ExistSNPCBFromProject(SerialNumber, ProjectName);
                
            }
            catch (Exception error)
            {
                status = false;
                error_consult = error.Message;
                MessageBox.Show($"Ha ocurrido un error al consultar los datos.\n{error.Message}",
                                "Error consulta",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
            }
            if (dt.Rows.Count == 0)
            {
                status = false;
                error_consult = "SN no encontrado";
                MessageBox.Show($"Ha ocurrido un error al consultar los datos.\n{error_consult}",
                                "Error consulta",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
                rols = new ItemPCBRework();
            }
            else
            {
                foreach (DataRow row in dt.Select())
                {
                    rols=(new ItemPCBRework((string)row["LayoutBOM"], (int)row["IdSNRegisterPCB"], (string)row["CodeSN"], (string)row["HardwareReference"], (int)row["ReworkPCB"]));

                }
            }
            itemPCBRework = rols;
            return status;
        }
        public static Dictionary<int, int> GetReworksFromLayoutBOM(string LayoutBOM, out string error_consult)
        {
            int i = 0;
            DataTable dt = new DataTable();
            Dictionary<int, int> rols = new Dictionary<int, int>();
            error_consult = "OK";
            try
            {
                dt = SQLServer_DDBB_Conection.GetReworksFromLayoutBOM(LayoutBOM);

            }
            catch (Exception error)
            {
                error_consult = error.Message;
                MessageBox.Show($"Ha ocurrido un error al consultar los datos.\n{error.Message}",
                                "Error consulta",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
            }
            if (dt.Rows.Count == 0)
            {
                error_consult = "Reworks no encontrados";
                MessageBox.Show($"Ha ocurrido un error al consultar los datos, no se ha encontrado ningún listado de retrabajos para esta PCB.\n{error_consult}",
                                "Error consulta",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
                rols = new Dictionary<int, int>();
            }
            else
            {
                foreach (DataRow row in dt.Select())
                {
                    rols.Add(i,(int)row["ReworkNum"]);
                    i++;
                }
            }
            return rols;
        }
        public static void InsertOFItemDefinition(string codeOF, string internalName, string UserName, string DescripcionOF, int quantity, out string error_consult)
        {
            int i = 0;
            DataTable dt = new DataTable();
            Dictionary<int, int> rols = new Dictionary<int, int>();
            error_consult = "OK";
            try
            {
                dt = SQLServer_DDBB_Conection.InsertOFItemDefinition(codeOF, internalName, UserName, DescripcionOF, quantity);

            }
            catch (Exception error)
            {
                error_consult = error.Message;
                MessageBox.Show($"Ha ocurrido un error al consultar los datos.\n{error.Message}",
                                "Error consulta",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
            }
        }
        public static ObservableCollection<ItemDefinitionOF> GetListItemDefinition()
        {
            int i = 0;
            DataTable dt = new DataTable();
            ObservableCollection<ItemDefinitionOF> rols = new ObservableCollection<ItemDefinitionOF>();
            try
            {
                dt = SQLServer_DDBB_Conection.GetListItemDefinition();

            }
            catch (Exception error)
            {
                MessageBox.Show($"Ha ocurrido un error al consultar los datos.\n{error.Message}",
                                "Error de conexión",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
            }
            if (dt.Rows.Count == 0)
            {
                MessageBox.Show($"Ha ocurrido un error al consultar los datos, no se ha encontrado listado de ItemDefinition.\n",
                                "Error consulta",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
                rols = new ObservableCollection<ItemDefinitionOF>();
            }
            else
            {
                foreach (DataRow row in dt.Select())
                {
                    rols.Add(new ItemDefinitionOF((string)row["DescriptionReference"], (string)row["InternalName"]));
                }
            }
            return rols;
        }
        public static OFChangeDescrip GetLastInforFromSN(string serialNumber, out int lastOP, out int CountOpPending,  out string error_consult)
        {
            int i = 0;
            DataTable dt = new DataTable();
            OFChangeDescrip rols = new OFChangeDescrip() ;
            int stringlastOP = 0;
            int stringCountOpPending = 0;
            error_consult = "OK";

            try
            {
                dt = SQLServer_DDBB_Conection.GetLastInforFromSN(serialNumber);

            }
            catch (Exception error)
            {
                error_consult = error.Message;
                MessageBox.Show($"Ha ocurrido un error al consultar los datos.\n{error.Message}",
                                "Error consulta",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
            }
            foreach (DataRow row in dt.Select())
            {
                rols = new OFChangeDescrip((string)row["CodeOF"], (string)row["InternalName"]);
                stringlastOP = (int)row["LastOP"];
                stringCountOpPending = (int)row["CountOpPending"];
            }
            lastOP = stringlastOP;
            CountOpPending= stringCountOpPending;

            return rols;
        }
        public static OFChangeDescrip GetInternalVersionFromOF(string orderFabricacion, out string error_consult)
        {
            DataTable dt = new DataTable();
            OFChangeDescrip rols = new OFChangeDescrip();
            error_consult = "OK";
            try
            {
                dt = SQLServer_DDBB_Conection.GetInternalVersionFromOF(orderFabricacion);

            }
            catch (Exception error)
            {
                error_consult = error.Message;
                MessageBox.Show($"Ha ocurrido un error al consultar los datos.\n{error.Message}",
                                "Error consulta",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
            }
            if (dt.Rows.Count == 0)
            {
                MessageBox.Show($"Ha ocurrido un error al consultar los datos, no se ha AG para esta OF.\n",
                                "Error consulta",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
                rols = new OFChangeDescrip();
            }
            else
            {
                foreach (DataRow row in dt.Select())
                {
                    rols = new OFChangeDescrip(orderFabricacion, (string)row["InternalName"]);
                }
            }


            return rols;
        }
        public static ObservableCollection<AGInfor_ChangeOF> GetManufacturingFromOF(string orderFabrication, out string error_consult)
        {
            int i = 0;
            DataTable dt = new DataTable();
            ObservableCollection<AGInfor_ChangeOF> rols = new ObservableCollection<AGInfor_ChangeOF>();
            error_consult = "OK";

            try
            {
                dt = SQLServer_DDBB_Conection.GetManufacturingFromOF(orderFabrication);

            }
            catch (Exception error)
            {
                error_consult = error.Message;
                MessageBox.Show($"Ha ocurrido un error al consultar los datos.\n{error.Message}",
                                "Error consulta",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
            }
            if (dt.Rows.Count == 0)
            {
                MessageBox.Show($"Ha ocurrido un error al consultar los datos, no se ha encontrado AG para este OF.\n",
                                "Error consulta",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
                rols = new ObservableCollection<AGInfor_ChangeOF>();
            }
            else
            {
                foreach (DataRow row in dt.Select())
                {
                    rols.Add(new AGInfor_ChangeOF((string)row["InternalName"], (int)row["ProcessNum"], (string)row["ProcessDescription"], (int)row["Num"], (string)row["OperationDescription"]));
                }
            }
            return rols;
        }
        public static List<string> GetUserRol(string userName)
        {
            DataTable dt = new DataTable();
            List<string> rols= new List<string>();

            try
            {
                dt = SQLServer_DDBB_Conection.GetUserRol(userName);
            }
            catch (Exception error)
            {
                MessageBox.Show($"Ha ocurrido un error al consultar los datos.\n{error.Message}",
                                "Error consulta",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
            }

            foreach (DataRow row in dt.Select())
            {
                rols.Add((string)row["RolDescription"]);
            }
            return rols;
        }
        public static ObservableCollection<PCBReference> GetPCBReferences(int idPlataform)
        {
            DataTable dt = new DataTable();
            ObservableCollection<PCBReference> pcbs = new ObservableCollection<PCBReference>();

            try
            {
                dt = SQLServer_DDBB_Conection.GetPCBReferences(idPlataform);
            }
            catch (Exception error)
            {
                MessageBox.Show($"Ha ocurrido un error al consultar los datos.\n{error.Message}",
                                "Error consulta", 
                                MessageBoxButton.OK, 
                                MessageBoxImage.Error);
            }

            foreach (DataRow row in dt.Select())
            {
                pcbs.Add(new PCBReference((int)row["IdItemReference"],(string)row["ItemDescription"],(string)row["Reference"],idPlataform));
            }

            return pcbs;
        }
        public static ObservableCollection<PCBReference> GetPCBBLayoutBOMFrom(int reference)
        {
            DataTable dt = new DataTable();
            ObservableCollection<PCBReference> pcbs = new ObservableCollection<PCBReference>();

            try
            {
                dt = SQLServer_DDBB_Conection.GetPCBBLayoutBOMFrom(reference);
            }
            catch (Exception error)
            {
                MessageBox.Show($"Ha ocurrido un error al consultar los datos.\n{error.Message}",
                                "Error consulta",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
            }

            foreach (DataRow row in dt.Select())
            {
                pcbs.Add(new PCBReference((int)row["IdItemReference"], (string)row["ItemDescription"], (string)row["Reference"], -1));
            }

            return pcbs;
        }
        public static List<string> GetPictureList()
        {
            DataTable dt = new DataTable();
            List<string> imagesName = new List<string>();

            try
            {
                dt = SQLServer_DDBB_Conection.GetPictureList();
            }
            catch (Exception error)
            {
                MessageBox.Show($"Ha ocurrido un error al consultar los datos.\n{error.Message}",
                                "Error consulta",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
            }

            foreach (DataRow row in dt.Select())
            {
                imagesName.Add((string)row["PictureName"]);
            }
            return imagesName;
        }
        public static int GetIdItemDefinition(string projectName, string sampleName, string partNumber, string internalName, string descriptionReference, bool isTrazability, string dateRegistration)
        {
            DataTable dt = new DataTable();
            int idItemDefinition = -1;

            try
            {
                dt = SQLServer_DDBB_Conection.GetIdItemDefinition(projectName, sampleName, partNumber, internalName, descriptionReference, isTrazability, dateRegistration);
            }
            catch (Exception error)
            {
                MessageBox.Show($"Ha ocurrido un error al consultar los datos.\n{error.Message}",
                                "Error de conexión",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
            }
            if (dt.Select().Length > 0)
            {
                DataRow row = dt.Select()[0];
                idItemDefinition = (int)row["IdItemDefinition"];
            }
            return idItemDefinition;
        }
        public static Dictionary<string, List<string>> GetElectronicHardwareList(int idItemDefinition)
        {
            DataTable dt = new DataTable();
            Dictionary<string, List<string>> hardwareList = new Dictionary<string, List<string>>();

            try
            {
                dt = SQLServer_DDBB_Conection.GetElectronicHardwareList(idItemDefinition);
            }
            catch (Exception error)
            {
                MessageBox.Show($"Ha ocurrido un error al consultar los datos.\n{error.Message}",
                                "Error de conexión",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
            }

            foreach (DataRow row in dt.Select())
            {
                string key = (string)row["ReferenceName"];
                string value = (string)row["HardwareReference"];
                List<string> list = new List<string>();
                if (hardwareList.ContainsKey(key))
                {
                    hardwareList.TryGetValue(key, out list);
                    hardwareList.Remove(key);
                }
                list.Add(value);
                hardwareList.Add(key, list);
            }
            return hardwareList;
        }
        public static Dictionary<string, string> GetMechanicalHardwareList(int idItemDefinition)
        {
            DataTable dt = new DataTable();
            Dictionary<string, string> mechanicalList = new Dictionary<string, string>();

            try
            {
                dt = SQLServer_DDBB_Conection.GetMechanicalHardwareList(idItemDefinition);
            }
            catch (Exception error)
            {
                MessageBox.Show($"Ha ocurrido un error al consultar los datos.\n{error.Message}",
                                "Error de conexión",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
            }

            foreach (DataRow row in dt.Select())
            {
                mechanicalList.Add((string)row["MechanicalDescription"], (string)row["MechanicalData"]);
            }
            return mechanicalList;
        }
        public static Dictionary<string, string> GetSoftwareList(int idItemDefinition)
        {
            DataTable dt = new DataTable();
            Dictionary<string, string> softwareList = new Dictionary<string, string>();

            try
            {
                dt = SQLServer_DDBB_Conection.GetSoftwareList(idItemDefinition);
            }
            catch (Exception error)
            {
                MessageBox.Show($"Ha ocurrido un error al consultar los datos.\n{error.Message}",
                                "Error de conexión",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
            }

            foreach (DataRow row in dt.Select())
            {
                softwareList.Add((string)row["SoftwareDescription"], (string)row["SoftwareData"]);
            }
            return softwareList;
        }
        public static string GetProcess(int idItemDefinition)
        {
            DataTable dt = new DataTable();
            string processName = "";

            try
            {
                dt = SQLServer_DDBB_Conection.GetProcess(idItemDefinition);
            }
            catch (Exception error)
            {
                MessageBox.Show($"Ha ocurrido un error al consultar los datos.\n{error.Message}",
                                "Error de conexión",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
            }

            if (dt.Select().Length > 0)
            {
                DataRow row = dt.Select()[0];
                processName = (string)row["ProcessDesignation"];
            }
            return processName;
        }
        public static Dictionary<string, string> GetLabellingList(int idItemDefinition)
        {
            DataTable dt = new DataTable();
            Dictionary<string, string> labellingList = new Dictionary<string, string>();

            try
            {
                dt = SQLServer_DDBB_Conection.GetLabellingList(idItemDefinition);
            }
            catch (Exception error)
            {
                MessageBox.Show($"Ha ocurrido un error al consultar los datos.\n{error.Message}",
                                "Error de conexión",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
            }

            foreach (DataRow row in dt.Select())
            {
                labellingList.Add((string)row["LabellingDescription"], (string)row["LabellingData"]);
            }
            return labellingList;
        }
        public static Dictionary<string, string> GetTestList(int idItemDefinition)
        {
            DataTable dt = new DataTable();
            Dictionary<string, string> testList = new Dictionary<string, string>();

            try
            {
                dt = SQLServer_DDBB_Conection.GetTestList(idItemDefinition);
            }
            catch (Exception error)
            {
                MessageBox.Show($"Ha ocurrido un error al consultar los datos.\n{error.Message}",
                                "Error de conexión",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
            }

            foreach (DataRow row in dt.Select())
            {
                testList.Add((string)row["TestDescription"], (string)row["VersionTest"]);
            }
            return testList;
        }
        #endregion
        #region Insert Procedures
        public static bool Insert_Picture(string renamed, byte[] imagenData)
        {
            bool correct = true;
            try
            {
                SQLServer_DDBB_Conection.Insert_Picture(renamed, imagenData);
            }
            catch (Exception error)
            {
                correct = false;
                MessageBox.Show($"Ha ocurrido un error.\n{error.Message}",
                                "Error al guardar la Imagen",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
            }
            return correct;
        }
        public static bool Insert_PCB(string user, string SN_Number, int idReference, int layout, int bom, string batch)
        {
            bool correct = true;
            try
            {
                SQLServer_DDBB_Conection.Insert_PCB(user, SN_Number, idReference, layout, bom, batch);
            }
            catch (Exception error)
            {
                correct = false;
                MessageBox.Show($"Ha ocurrido un error.\n{error.Message}",
                                "Error al guardar la PCB",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
            }
            return correct;
        }
        public static void InsertReworkSN(string serialNumber, int reworkNum, string UserName, out string error_consult)
        {
            int i = 0;
            DataTable dt = new DataTable();
            Dictionary<int, int> rols = new Dictionary<int, int>();
            error_consult = "OK";
            try
            {
                dt = SQLServer_DDBB_Conection.InsertReworkSN(serialNumber, reworkNum, UserName);

            }
            catch (Exception error)
            {
                error_consult = error.Message;
                MessageBox.Show($"Ha ocurrido un error al consultar los datos.\n{error.Message}",
                                "Error de conexión",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
            }
        }
        public static void InsertChangeOF(string serialNumber, string oldOF, string newOF, string comment, int opStart, string userName, out string error_consult)
        {
            int i = 0;
            DataTable dt = new DataTable();
            error_consult = "";
            try
            {
                dt = SQLServer_DDBB_Conection.InsertChangeOF(serialNumber, oldOF, newOF, comment, opStart, userName);

            }
            catch (Exception error)
            {
                error_consult = error.Message;
                MessageBox.Show($"Ha ocurrido un error al consultar los datos.\n{error.Message}",
                                "Error de conexión",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
            }
        }
        public static bool Insert_ItemDefinition(string projectName, string sampleName, string partNumber, string descriptionItem, string internalName, string descriptionReference, bool isTrazability, string pictureName, byte[] pictureData, string dateRegistration, string userName, out int idInternalVersion)
        {
            bool correct = true;
            idInternalVersion = -1;
            try
            {
                SQLServer_DDBB_Conection.Insert_ItemDefinition(projectName, sampleName, partNumber, descriptionItem, internalName, descriptionReference, isTrazability, pictureName, pictureData, dateRegistration, userName, out idInternalVersion);

            }
            catch (Exception error)
            {
                correct = false;
                MessageBox.Show($"Ha ocurrido un error.\n{error.Message}",
                                "Error al guardar la versión interna",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
            }
            return correct;
        }
        public static bool Insert_HardwareVersion(int idItemDefinition, string hardwarePCB)
        {
            bool correct = true;
            try
            {
                SQLServer_DDBB_Conection.Insert_HardwareVersion(idItemDefinition, hardwarePCB);
            }
            catch (Exception error)
            {
                correct = false;
                MessageBox.Show($"Ha ocurrido un error.\n{error.Message}",
                                "Error al guardar el hardware de la versión interna",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
            }
            return correct;
        }
        public static bool Insert_MechanicalVersion(int idItemDefinition, string parameter, string data)
        {
            bool correct = true;
            try
            {
                SQLServer_DDBB_Conection.Insert_MechanicalVersion(idItemDefinition, parameter, data);

            }
            catch (Exception error)
            {
                correct = false;
                MessageBox.Show($"Ha ocurrido un error.\n{error.Message}",
                                "Error al guardar la mecánica de la versión interna",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
            }
            return correct;
        }
        public static bool Insert_SoftwareVersion(int idItemDefinition, string parameter, string data)
        {
            bool correct = true;
            try
            {
                SQLServer_DDBB_Conection.Insert_SoftwareVersion(idItemDefinition, parameter, data);

            }
            catch (Exception error)
            {
                correct = false;
                MessageBox.Show($"Ha ocurrido un error.\n{error.Message}",
                                "Error al guardar el software de la versión interna",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
            }
            return correct;
        }
        public static bool Insert_ProcessVersion(int idItemDefinition, string processDesignation)
        {
            bool correct = true;
            try
            {
                SQLServer_DDBB_Conection.Insert_ProcessVersion(idItemDefinition, processDesignation);

            }
            catch (Exception error)
            {
                correct = false;
                MessageBox.Show($"Ha ocurrido un error.\n{error.Message}",
                                "Error al guardar el process de la versión interna",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
            }
            return correct;
        }
        public static bool Insert_LabellingVersion(int idItemDefinition, string parameter, string data)
        {
            bool correct = true;
            try
            {
                SQLServer_DDBB_Conection.Insert_LabellingVersion(idItemDefinition, parameter, data);

            }
            catch (Exception error)
            {
                correct = false;
                MessageBox.Show($"Ha ocurrido un error.\n{error.Message}",
                                "Error al guardar el labelling de la versión interna",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
            }
            return correct;
        }
        public static bool Insert_TestVersion(int idItemDefinition, string testDescription, int versionTest)
        {
            bool correct = true;
            try
            {
                SQLServer_DDBB_Conection.Insert_TestVersion(idItemDefinition, testDescription, versionTest);

            }
            catch (Exception error)
            {
                correct = false;
                MessageBox.Show($"Ha ocurrido un error.\n{error.Message}",
                                "Error al guardar el test de la versión interna",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
            }
            return correct;
        }
        public static bool Insert_DefinitionProcess(string processName, int num, int processNum, string processDescription, int operation, string operationDescription, string pictureName, string stationName, string definitionTest, string inputType, string screwCode, string referenceAssembly)
        {
            bool correct = true;
            try
            {
                SQLServer_DDBB_Conection.Insert_DefinitionProcess(processName, num, processNum, processDescription, operation, operationDescription, pictureName, stationName, definitionTest, inputType, screwCode, referenceAssembly);
            }
            catch (Exception error)
            {
                correct = false;
                MessageBox.Show($"Ha ocurrido un error.\n{error.Message}",
                                "Error al guardar los procesos",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
            }
            return correct;
        }
        #endregion
        #region Delete Procedures
        public static bool Delete_ItemDefinition(int idItemDefinition)
        {
            bool correct = true;
            try
            {
                SQLServer_DDBB_Conection.Delete_ItemDefinition(idItemDefinition);
            }
            catch (Exception error)
            {
                correct = false;
                MessageBox.Show($"Ha ocurrido en la ItemDefinition.\n{error.Message}",
                                "Error al borrar la ItemDefinition",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
            }
            return correct;
        }
        public static bool Delete_HardwareVersion(int idItemDefinition)
        {
            bool correct = true;
            try
            {
                SQLServer_DDBB_Conection.Delete_HardwareVersion(idItemDefinition);
            }
            catch (Exception error)
            {
                correct = false;
                MessageBox.Show($"Ha ocurrido un error en el bloque de Hardware.\n{error.Message}",
                                "Error al borrar la ItemDefinition",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
            }
            return correct;
        }
        public static bool Delete_MechanicalVersion(int idItemDefinition)
        {
            bool correct = true;
            try
            {
                SQLServer_DDBB_Conection.Delete_MechanicalVersion(idItemDefinition);
            }
            catch (Exception error)
            {
                correct = false;
                MessageBox.Show($"Ha ocurrido un error en el bloque de Mechanical.\n{error.Message}",
                                "Error al borrar la ItemDefinition",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
            }
            return correct;
        }
        public static bool Delete_SoftwareVersion(int idItemDefinition)
        {
            bool correct = true;
            try
            {
                SQLServer_DDBB_Conection.Delete_SoftwareVersion(idItemDefinition);
            }
            catch (Exception error)
            {
                correct = false;
                MessageBox.Show($"Ha ocurrido un error en el bloque de Software.\n{error.Message}",
                                "Error al borrar la ItemDefinition",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
            }
            return correct;
        }
        public static bool Delete_ProcessVersion(int idItemDefinition)
        {
            bool correct = true;
            try
            {
                SQLServer_DDBB_Conection.Delete_ProcessVersion(idItemDefinition);
            }
            catch (Exception error)
            {
                correct = false;
                MessageBox.Show($"Ha ocurrido un error en el bloque de Process.\n{error.Message}",
                                "Error al borrar la ItemDefinition",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
            }
            return correct;
        }
        public static bool Delete_LabellingVersion(int idItemDefinition)
        {
            bool correct = true;
            try
            {
                SQLServer_DDBB_Conection.Delete_LabellingVersion(idItemDefinition);
            }
            catch (Exception error)
            {
                correct = false;
                MessageBox.Show($"Ha ocurrido un error en el bloque de Labelling.\n{error.Message}",
                                "Error al borrar la ItemDefinition",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
            }
            return correct;
        }
        public static bool Delete_TestingVersion(int idItemDefinition)
        {
            bool correct = true;
            try
            {
                SQLServer_DDBB_Conection.Delete_TestingVersion(idItemDefinition);
            }
            catch (Exception error)
            {
                correct = false;
                MessageBox.Show($"Ha ocurrido un error en el bloque de Testing.\n{error.Message}",
                                "Error al borrar la ItemDefinition",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
            }
            return correct;
        }
        public static bool Delete_DefinitionProcess(string processName)
        {
            bool correct = true;
            try
            {
                SQLServer_DDBB_Conection.Delete_DefinitionProcess(processName);
            }
            catch (Exception error)
            {
                correct = false;
                MessageBox.Show($"Ha ocurrido un error.\n{error.Message}",
                                "Error al borrar los procesos",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
            }
            return correct;
        }
        #endregion
        #region Update Procedure

        #endregion
    }
}