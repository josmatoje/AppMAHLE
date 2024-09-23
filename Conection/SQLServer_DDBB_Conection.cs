using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.ObjectModel;
using System.Runtime.Remoting.Messaging;
using System.Windows;
using Google.Protobuf.WellKnownTypes;
using System.Windows.Input;
using System.Data.SqlTypes;
using System.Windows.Markup;
using static Org.BouncyCastle.Asn1.Cmp.Challenge;

namespace DIV_Protos
{
    public class SQLServer_DDBB_Conection
    {
        SqlConnection conn;
        #region atributes
        public static readonly object LockDB = new object();
        public static bool connection_ddbb = false;
        private static string conn_string = "server=Server;user=User;password=Password6;database=Database;";
        private static SqlConnection connection = new SqlConnection(conn_string);
        private static string ColumSpliter = "<<split>>";
        #endregion
        #region init-close-reset
        public static bool InitDDBB(out string err_msg)
        {
            err_msg = "";
            bool status = false;
            try
            {
                if (connection_ddbb == false)
                {
                    connection.Open();
                    connection_ddbb = true;
                }
                status = true;
            }
            catch (Exception ex)
            {
                err_msg = ex.Message;
                status = false;
            }

            return status;
        }
        public static bool CloseDDBB()
        {
            bool status = false;
            try
            {
                connection.Close();
                connection_ddbb = false;
                status = true;
            }
            catch
            {
                status = false;
            }
            return status;
        }
        public static bool ResetConecctionDDBB()
        {
            String err_msg = "";
            bool status = false;
            status = SQLServer_DDBB_Conection.CloseDDBB();
            if (status)
            {
                status = SQLServer_DDBB_Conection.InitDDBB(out err_msg);
            }
            return status;

        }
        #endregion
        #region Read Methods
        public static DataTable GetProject(bool firstTime = true) 
        {
            DataTable tableResul = new DataTable();

            try
            {
                SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM ItemProject", connection);

                adapter.Fill(tableResul);

            }
            catch (Exception ex)
            {
                if ((SQLServer_DDBB_Conection.connection.State != ConnectionState.Open && SQLServer_DDBB_Conection.connection.State != ConnectionState.Executing) && firstTime)
                {
                    if (ResetConecctionDDBB())
                    {
                        return GetProject(false);
                    }
                    else
                    {
                        throw;
                    }
                }
                else
                {
                    throw;
                }
            }
            return tableResul;
        }
        public static DataTable GetUserRol(string userCode, bool firstTime = true)
        {
            DataTable tableResul = new DataTable();

            try
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "SELECT RolDescription FROM Center_RolDefinitions_View " +
                                        "WHERE @UserCode LIKE CONCAT('%',UserCode,'%')";
                command.Parameters.AddWithValue("@UserCode", userCode);

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(tableResul);

            }
            catch (Exception ex)
            {
                if ((SQLServer_DDBB_Conection.connection.State != ConnectionState.Open && SQLServer_DDBB_Conection.connection.State != ConnectionState.Executing) && firstTime)
                {
                    if (ResetConecctionDDBB())
                    {
                        return GetProject(false);
                    }
                    else
                    {
                        throw;
                    }
                }
                else
                {
                    throw;
                }
            }
            return tableResul;
        }
        public static DataTable GetCategoryList(bool firstTime = true)
        {
            DataTable tableResul = new DataTable();

            try
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "SELECT CategoryDescription FROM [ItemCategory]";

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(tableResul);

            }
            catch (Exception ex)
            {
                if ((SQLServer_DDBB_Conection.connection.State != ConnectionState.Open && SQLServer_DDBB_Conection.connection.State != ConnectionState.Executing) && firstTime)
                {
                    if (ResetConecctionDDBB())
                    {
                        return GetCategoryList(false);
                    }
                    else
                    {
                        throw;
                    }
                }
                else
                {
                    throw;
                }
            }
            return tableResul;
        }
        
            public static DataTable GetHardwarePCBParametersFromProject(string ProjectName, bool firstTime = true)
        {
            DataTable tableResul = new DataTable();

            try
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "SELECT IdItemBomPCB, HardwareReference, ReferenceName, LayoutBOM FROM HardwarePCB_Parameters_View WHERE ProjectName = @ProjectName";
                command.Parameters.AddWithValue("@ProjectName", ProjectName);

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(tableResul);

            }
            catch (Exception ex)
            {
                if ((SQLServer_DDBB_Conection.connection.State != ConnectionState.Open && SQLServer_DDBB_Conection.connection.State != ConnectionState.Executing) && firstTime)
                {
                    if (ResetConecctionDDBB())
                    {
                        return GetHardwarePCBParametersFromProject(ProjectName, false);
                    }
                    else
                    {
                        throw;
                    }
                }
                else
                {
                    throw;
                }
            }
            return tableResul;
        }
        public static DataTable GetWindChillListFromCategory(string CategoryWindChill, bool firstTime = true)
        {
            DataTable tableResul = new DataTable();

            try
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "GetWindChillListFromCategory";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@CategoryWindChill", CategoryWindChill);
                command.Parameters["@CategoryWindChill"].Direction = ParameterDirection.Input;

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(tableResul);

            }
            catch (Exception ex)
            {
                if ((SQLServer_DDBB_Conection.connection.State != ConnectionState.Open && SQLServer_DDBB_Conection.connection.State != ConnectionState.Executing) && firstTime)
                {
                    if (ResetConecctionDDBB())
                    {
                        return GetWindChillListFromCategory(CategoryWindChill, false);
                    }
                    else
                    {
                        throw;
                    }
                }
                else
                {
                    throw;
                }
            }
            return tableResul;
        }
        public static DataTable ExistsAndInsertSN(string SerialNumber, string ProyectName, string CategorySN,
                                bool InsertSN, string UserName, string WindchillCode,  bool firstTime = true)
        {
            DataTable tableResul = new DataTable();
            int idSN=-999;
            try
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "ExistsAndInsertSN";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@SerialNumber", SerialNumber);
                command.Parameters["@SerialNumber"].Direction = ParameterDirection.Input;
                command.Parameters.AddWithValue("@ProyectName", ProyectName);
                command.Parameters["@ProyectName"].Direction = ParameterDirection.Input;
                command.Parameters.AddWithValue("@CategorySN", CategorySN);
                command.Parameters["@CategorySN"].Direction = ParameterDirection.Input;
                command.Parameters.AddWithValue("@InsertSN", InsertSN);
                command.Parameters["@InsertSN"].Direction = ParameterDirection.Input;
                command.Parameters.AddWithValue("@UserName", UserName);
                command.Parameters["@UserName"].Direction = ParameterDirection.Input;
                command.Parameters.AddWithValue("@WindChillCode", WindchillCode);
                command.Parameters["@WindChillCode"].Direction = ParameterDirection.Input;
                command.Parameters.AddWithValue("@idSerialNumber", idSN);
                command.Parameters["@idSerialNumber"].Direction = ParameterDirection.Output;

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(tableResul);

            }
            catch (Exception ex)
            {
                if ((SQLServer_DDBB_Conection.connection.State != ConnectionState.Open && SQLServer_DDBB_Conection.connection.State != ConnectionState.Executing) && firstTime)
                {
                    if (ResetConecctionDDBB())
                    {
                        return ExistsAndInsertSN( SerialNumber,  ProyectName,  CategorySN,
                                 InsertSN,  UserName, WindchillCode,  false);
                    }
                    else
                    {
                        throw;
                    }
                }
                else
                {
                    throw;
                }
            }
            return tableResul;
        }
        public static DataTable InsertOFItemDefinition(string codeOF, string internalName, string UserName,
                              string DescripcionOF, int quantity,  bool firstTime = true)
        {
            DataTable tableResul = new DataTable();
            int idSN = -999;
            try
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "InsertOFItemDefinition";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@codeOF", codeOF);
                command.Parameters["@codeOF"].Direction = ParameterDirection.Input;
                command.Parameters.AddWithValue("@internalName", internalName);
                command.Parameters["@internalName"].Direction = ParameterDirection.Input;
                command.Parameters.AddWithValue("@UserName", UserName);
                command.Parameters["@UserName"].Direction = ParameterDirection.Input;
                command.Parameters.AddWithValue("@DescripcionOF", DescripcionOF);
                command.Parameters["@DescripcionOF"].Direction = ParameterDirection.Input;
                command.Parameters.AddWithValue("@quantity", quantity);
                command.Parameters["@quantity"].Direction = ParameterDirection.Input;

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(tableResul);

            }
            catch (Exception ex)
            {
                if ((SQLServer_DDBB_Conection.connection.State != ConnectionState.Open && SQLServer_DDBB_Conection.connection.State != ConnectionState.Executing) && firstTime)
                {
                    if (ResetConecctionDDBB())
                    {
                        return InsertOFItemDefinition(codeOF, internalName, UserName,
                                 DescripcionOF, quantity, false);
                    }
                    else
                    {
                        throw;
                    }
                }
                else
                {
                    throw;
                }
            }
            return tableResul;
        }


        public static DataTable GetListItemDefinition(bool firstTime = true)
        {
            DataTable tableResul = new DataTable();
            try
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "SELECT  ID.DescriptionReference AS DescriptionReference, ID.InternalName AS InternalName "+
                "FROM ItemDefinition AS ID WHERE ID.EnableItem = 1 ORDER BY ID.DateRegistration ASC" ;
                   // " LEFT JOIN PictureStorage AS PS ON ID.IdPictureStorage = PS.IdPictureStorage ";

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(tableResul);
            }
            catch (Exception ex)
            {
                if ((SQLServer_DDBB_Conection.connection.State != ConnectionState.Open && SQLServer_DDBB_Conection.connection.State != ConnectionState.Executing) && firstTime)
                {
                    if (ResetConecctionDDBB())
                    {
                        return GetListItemDefinition(false);
                    }
                    else
                    {
                        throw;
                    }
                }
                else
                {
                    throw;
                }
            }
            return tableResul;
        }
        public static DataTable GetReworksFromLayoutBOM(string LayoutBOM, bool firstTime = true)
        {
            DataTable tableResul = new DataTable();
            int idSN = -999;
            try
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "  SELECT ReworkNum" +
                                        " FROM ReworkFromHWRef_Project_View AS RWHA" +
                                        " INNER JOIN HardwarePCB_Parameters_View HPCBP ON RWHA.IdItemBomPCB = HPCBP.IdItemBomPCB" +
                                        " WHERE HPCBP.LayoutBOM = @LayoutBOM order by ReferenceDescription, ReworkNum";


                command.Parameters.AddWithValue("@LayoutBOM", LayoutBOM);

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(tableResul);
            }
            catch (Exception ex)
            {
                if ((SQLServer_DDBB_Conection.connection.State != ConnectionState.Open && SQLServer_DDBB_Conection.connection.State != ConnectionState.Executing) && firstTime)
                {
                    if (ResetConecctionDDBB())
                    {
                        return GetReworksFromLayoutBOM(LayoutBOM,  false);
                    }
                    else
                    {
                        throw;
                    }
                }
                else
                {
                    throw;
                }
            }
            return tableResul;
        }
        
        public static DataTable GetLastInforFromSN(string SerialNumber, bool firstTime = true)
        {
            DataTable tableResul = new DataTable();
            int idSN = -999;
            try
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "GetLastInforFromSN";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@SerialNumber", SerialNumber);
                command.Parameters["@SerialNumber"].Direction = ParameterDirection.Input;

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(tableResul);
            }
            catch (Exception ex)
            {
                if ((SQLServer_DDBB_Conection.connection.State != ConnectionState.Open && SQLServer_DDBB_Conection.connection.State != ConnectionState.Executing) && firstTime)
                {
                    if (ResetConecctionDDBB())
                    {
                        return GetLastInforFromSN(SerialNumber, false);
                    }
                    else
                    {
                        throw;
                    }
                }
                else
                {
                    throw;
                }
            }
            return tableResul;
        }
        public static DataTable GetItemDescriptionList(bool firstTime = true)
        {
            DataTable tableResul = new DataTable();

            try
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "SELECT DescriptionItem FROM ItemDescription";

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(tableResul);

            }
            catch (Exception ex)
            {
                if ((SQLServer_DDBB_Conection.connection.State != ConnectionState.Open && SQLServer_DDBB_Conection.connection.State != ConnectionState.Executing) && firstTime)
                {
                    if (ResetConecctionDDBB())
                    {
                        return GetItemDescriptionList(false);
                    }
                    else
                    {
                        throw;
                    }
                }
                else
                {
                    throw;
                }
            }
            return tableResul;
        }
        public static DataTable GetInternalVersionFromOF(string orderFabrication, bool firstTime = true)
        {
            DataTable tableResul = new DataTable();
            int idSN = -999;
            try
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "SELECT ID.InternalName FROM OrderFabrication AS OFN" +
                                        " INNER JOIN ItemDefinition ID ON OFN.IdItemDefinition = ID.IdItemDefinition" +
                                         " WHERE OFN.CodeOF = @orderFabrication";

                command.Parameters.AddWithValue("@orderFabrication", orderFabrication);

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(tableResul);
            }
            catch (Exception ex)
            {
                if ((SQLServer_DDBB_Conection.connection.State != ConnectionState.Open && SQLServer_DDBB_Conection.connection.State != ConnectionState.Executing) && firstTime)
                {
                    if (ResetConecctionDDBB())
                    {
                        return GetInternalVersionFromOF(orderFabrication, false);
                    }
                    else
                    {
                        throw;
                    }
                }
                else
                {
                    throw;
                }
            }
            return tableResul;
        }
        public static DataTable InsertChangeOF(string serialNumber, string oldOF, string newOF, string comment, int opStart, string userName, bool firstTime = true)
        {
            DataTable tableResul = new DataTable();
            int idSN = -999;
            try
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "InsertChangeOF";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@SerialNumber", serialNumber);
                command.Parameters["@SerialNumber"].Direction = ParameterDirection.Input;
                command.Parameters.AddWithValue("@oldOF", oldOF);
                command.Parameters["@oldOF"].Direction = ParameterDirection.Input;
                command.Parameters.AddWithValue("@newOF", newOF);
                command.Parameters["@newOF"].Direction = ParameterDirection.Input;
                command.Parameters.AddWithValue("@comments", comment);
                command.Parameters["@comments"].Direction = ParameterDirection.Input;
                command.Parameters.AddWithValue("@opStart", opStart);
                command.Parameters["@opStart"].Direction = ParameterDirection.Input;
                command.Parameters.AddWithValue("@UserName", userName);
                command.Parameters["@UserName"].Direction = ParameterDirection.Input;

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(tableResul);
            }
            catch (Exception ex)
            {
                if ((SQLServer_DDBB_Conection.connection.State != ConnectionState.Open && SQLServer_DDBB_Conection.connection.State != ConnectionState.Executing) && firstTime)
                {
                    if (ResetConecctionDDBB())
                    {
                        return InsertChangeOF(serialNumber, oldOF, newOF, comment, opStart, userName, false);
                    }
                    else
                    {
                        throw;
                    }
                }
                else
                {
                    throw;
                }
            }
            return tableResul;
        }
        public static DataTable GetManufacturingFromOF(string orderFabrication, bool firstTime = true)
        {
            DataTable tableResul = new DataTable();
            int idSN = -999;
            try
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = " SELECT  ID.InternalName,PD.ProcessNum, PDP.ProcessDescription, PD.Num, PD.OperationDescription"+
                                      " FROM OrderFabrication AS OFN"+
                                      " INNER JOIN ItemDefinition ID ON OFN.IdItemDefinition = ID.IdItemDefinition" +
                                      " INNER JOIN ItemDefinition_VersionProcess IDVP ON ID.IdItemDefinition = IDVP.IdItemDefinition"+
                                      " INNER JOIN VersionProcess VP ON IDVP.IdVersionProcess = VP.IdVersionProcess"+
                                      " INNER JOIN ProcessDefinition PD ON VP.IdVersionProcess = PD.IdVersionProcess"+
                                      " INNER JOIN ProcessDescription PDP ON PD.IdProcessDescription = PDP.IdProcessDescription" +
                                      " WHERE OFN.CodeOF = @OrderFabrication";

                command.Parameters.AddWithValue("@OrderFabrication", orderFabrication);

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(tableResul);
            }
            catch (Exception ex)
            {
                if ((SQLServer_DDBB_Conection.connection.State != ConnectionState.Open && SQLServer_DDBB_Conection.connection.State != ConnectionState.Executing) && firstTime)
                {
                    if (ResetConecctionDDBB())
                    {
                        return GetManufacturingFromOF(orderFabrication, false);
                    }
                    else
                    {
                        throw;
                    }
                }
                else
                {
                    throw;
                }
            }
            return tableResul;
        }

        public static DataTable InsertReworkSN(string SerialNumber, int reworkNum, string UserName, bool firstTime = true)
        {
            DataTable tableResul = new DataTable();
            int idSN = -999;
            try
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "InsertReworkSN";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@SerialNumber", SerialNumber);
                command.Parameters["@SerialNumber"].Direction = ParameterDirection.Input;
                command.Parameters.AddWithValue("@ReworkNum", reworkNum);
                command.Parameters["@ReworkNum"].Direction = ParameterDirection.Input;
                command.Parameters.AddWithValue("@UserName", UserName);
                command.Parameters["@UserName"].Direction = ParameterDirection.Input;

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(tableResul);

            }
            catch (Exception ex)
            {
                if ((SQLServer_DDBB_Conection.connection.State != ConnectionState.Open && SQLServer_DDBB_Conection.connection.State != ConnectionState.Executing) && firstTime)
                {
                    if (ResetConecctionDDBB())
                    {
                        return InsertReworkSN(SerialNumber, reworkNum, UserName, false);
                    }
                    else
                    {
                        throw;
                    }
                }
                else
                {
                    throw;
                }
            }
            return tableResul;
        }
        public static DataTable ExistSNPCBFromProject(string SerialNumber, string ProjectName, bool firstTime = true)
        {
            DataTable tableResul = new DataTable();
            int idSN = -999;
            try
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "SELECT  LIPCB.LayoutBOM, SNRPCB.IdSNRegisterPCB,  SNDAW.CodeSN,  SNDAW.HardwareReference, ISNULL(MRW.Rework,0) AS ReworkPCB " +
                                    "FROM SNDefinitionAndWindChill_View AS SNDAW  " +
                                    "INNER JOIN SNRegisterPCB AS SNRPCB ON SNDAW.IdSNRegister = SNRPCB.IdSNRegister " +
                                    "INNER JOIN HardwarePCB_Parameters_View AS LIPCB ON SNRPCB.IdItemBomPCB = LIPCB.IdItemBomPCB " +
                                    "LEFT JOIN (SELECT IdSNRegisterPCB, MAX(Rework) as Rework FROM ManufacturingReworkPCB " +
                                    "GROUP BY IdSNRegisterPCB) AS MRW ON SNRPCB.IdSNRegisterPCB = MRW.IdSNRegisterPCB " +
                                    "WHERE SNDAW.CodeSN = @SerialNumber AND SNDAW.ProjectName = @ProjectName";
                command.Parameters.AddWithValue("@SerialNumber", SerialNumber);
                command.Parameters.AddWithValue("@ProjectName", ProjectName);

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(tableResul);
            }
            catch (Exception ex)
            {
                if ((SQLServer_DDBB_Conection.connection.State != ConnectionState.Open && SQLServer_DDBB_Conection.connection.State != ConnectionState.Executing) && firstTime)
                {
                    if (ResetConecctionDDBB())
                    {
                        return ExistSNPCBFromProject(SerialNumber, ProjectName, false);
                    }
                    else
                    {
                        throw;
                    }
                }
                else
                {
                    throw;
                }
            }
            return tableResul;
        }
        public static DataTable ExistsAndInsertSN_PCB(string SerialNumber,string ReferenceName, string LayoutBOM, string ProyectName, string CategorySN,
                               bool InsertSN, string Batch, string UserName, string WindchillCode, bool firstTime = true)
        {
            DataTable tableResul = new DataTable();
            int idSN = -999;
            try
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "ExistsAndInsertSN_PCB";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@SerialNumber", SerialNumber);
                command.Parameters["@SerialNumber"].Direction = ParameterDirection.Input;
                command.Parameters.AddWithValue("@ReferenceName", ReferenceName);
                command.Parameters["@ReferenceName"].Direction = ParameterDirection.Input;
                command.Parameters.AddWithValue("@LayoutBOM", LayoutBOM);
                command.Parameters["@LayoutBOM"].Direction = ParameterDirection.Input;
                command.Parameters.AddWithValue("@ProyectName", ProyectName);
                command.Parameters["@ProyectName"].Direction = ParameterDirection.Input;
                command.Parameters.AddWithValue("@CategorySN", CategorySN);
                command.Parameters["@CategorySN"].Direction = ParameterDirection.Input;
                command.Parameters.AddWithValue("@InsertSN", InsertSN);
                command.Parameters["@InsertSN"].Direction = ParameterDirection.Input;
                command.Parameters.AddWithValue("@Batch", Batch);
                command.Parameters["@Batch"].Direction = ParameterDirection.Input;
                command.Parameters.AddWithValue("@UserName", UserName);
                command.Parameters["@UserName"].Direction = ParameterDirection.Input;
                command.Parameters.AddWithValue("@WindChillCode", WindchillCode);
                command.Parameters["@WindChillCode"].Direction = ParameterDirection.Input;
                command.Parameters.AddWithValue("@idSerialNumber", idSN);
                command.Parameters["@idSerialNumber"].Direction = ParameterDirection.Output;

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(tableResul);

            }
            catch (Exception ex)
            {
                if ((SQLServer_DDBB_Conection.connection.State != ConnectionState.Open && SQLServer_DDBB_Conection.connection.State != ConnectionState.Executing) && firstTime)
                {
                    if (ResetConecctionDDBB())
                    {
                        return ExistsAndInsertSN_PCB(SerialNumber, ReferenceName, LayoutBOM, ProyectName, CategorySN, 
                                 InsertSN, Batch, UserName, WindchillCode, false);
                    }
                    else
                    {
                        throw;
                    }
                }
                else
                {
                    throw;
                }
            }
            return tableResul;
        }


        public static DataTable GetPCBReferences(int idPlataform, bool firstTime = true)
        {
            DataTable tableResul = new DataTable();
            try
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "SELECT IdItemReference, ItemDescription, Reference FROM ItemReference " +
                                        "WHERE IdProject = @idPlataforma AND IdItemCategory = 1";
                command.Parameters.AddWithValue("@idPlataforma", idPlataform);

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(tableResul);

                /*
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                         //datas = data.Split(new string[] { ColumSpliter }, StringSplitOptions.None);

                        rows_data.Add(new PCBReference(reader.GetInt32(0), reader.GetString(1), reader.GetString(2),1));
                    }
                }
                */
            }
            catch (Exception ex)
            {
                if ((SQLServer_DDBB_Conection.connection.State != ConnectionState.Open && SQLServer_DDBB_Conection.connection.State != ConnectionState.Executing) && firstTime)
                {
                    if (ResetConecctionDDBB())
                    {
                        return GetPCBReferences(idPlataform, false);
                    }
                    else
                    {
                        throw;
                    }
                }
                else
                {
                    throw;
                }
            }
            return tableResul;
        }

        public static DataTable GetPCBBLayoutBOMFrom(int reference, bool firstTime = true)
        {
            DataTable tableResul = new DataTable();

            try
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;

                command.CommandText = "SELECT * FROM LayoutBomFromPCBReference " +
                                            "WHERE IdItemReference = @reference ";
                command.Parameters.AddWithValue("@reference", reference);

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(tableResul);
            }
            catch (Exception ex)
            {
                if ((SQLServer_DDBB_Conection.connection.State != ConnectionState.Open && SQLServer_DDBB_Conection.connection.State != ConnectionState.Executing) && firstTime)
                {
                    if (ResetConecctionDDBB())
                    {
                        return GetPCBBLayoutBOMFrom(reference, false);
                    }
                    else
                    {
                        throw;
                    }
                }
                else
                {
                    throw;
                }
            }

            return tableResul;
        }

        public static DataTable GetPictureList(bool firstTime = true)
        {
            DataTable tableResul = new DataTable();

            try
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;

                command.CommandText = "SELECT PictureName FROM PictureStorage";

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(tableResul);
            }
            catch (Exception ex)
            {
                if ((SQLServer_DDBB_Conection.connection.State != ConnectionState.Open && SQLServer_DDBB_Conection.connection.State != ConnectionState.Executing) && firstTime)
                {
                    if (ResetConecctionDDBB())
                    {
                        return GetPictureList(false);
                    }
                    else
                    {
                        throw;
                    }
                }
                else
                {
                    throw;
                }
            }

            return tableResul;
        }
        public static DataTable GetIdItemDefinition(string projectName, string sampleName, string partNumber, string internalName, string descriptionReference, bool isTrazability, string dateRegistration, bool firstTime = true)
        {
            DataTable tableResul = new DataTable();

            try
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;

                command.CommandText = "SELECT IdItemDefinition FROM ItemDefinition AS ID " +
                                        "INNER JOIN ItemPartNumber AS IPN ON ID.IdItemPN = IPN.IdItemPN " +
                                        "INNER JOIN ItemSample AS S ON IPN.IdItemSample = S.IdItemSample " +
                                        "INNER JOIN ItemProject AS P ON S.IdItemProject = P.IdItemProject " +
                                        "WHERE ProjectName = @projectName AND " +
                                                "ItemSampleName = @sampleName AND " +
                                                "ItemPNName = @partNumber AND " +
                                                "InternalName = @internalName AND " +
                                                "DescriptionReference = @descriptionReference AND " +
                                                "IsTrazability = @isTrazability AND " +
                                                "DateRegistration = CONVERT(DATETIME, @dateRegistration, 103)";
                command.Parameters.AddWithValue("@projectName", projectName);
                command.Parameters.AddWithValue("@sampleName", sampleName);
                command.Parameters.AddWithValue("@partNumber", partNumber);
                command.Parameters.AddWithValue("@internalName", internalName);
                command.Parameters.AddWithValue("@descriptionReference", descriptionReference);
                command.Parameters.AddWithValue("@isTrazability", isTrazability ? 1 : 0);
                command.Parameters.AddWithValue("@dateRegistration", dateRegistration);

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(tableResul);
            }
            catch (Exception ex)
            {
                if ((SQLServer_DDBB_Conection.connection.State != ConnectionState.Open && SQLServer_DDBB_Conection.connection.State != ConnectionState.Executing) && firstTime)
                {
                    if (ResetConecctionDDBB())
                    {
                        return GetIdItemDefinition(projectName, sampleName, partNumber, internalName, descriptionReference, isTrazability, dateRegistration, false);
                    }
                    else
                    {
                        throw;
                    }
                }
                else
                {
                    throw;
                }
            }

            return tableResul;
        }
        public static DataTable GetElectronicHardwareList(int idItemDefinition, bool firstTime = true)
        {
            DataTable tableResul = new DataTable();

            try
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;

                command.CommandText = "SELECT ReferenceName, HardwareReference FROM UnifiedHardwarePCB " +
                                        "WHERE IdItemDefinition = @idItemDefinition";
                command.Parameters.AddWithValue("@idItemDefinition", idItemDefinition);

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(tableResul);
            }
            catch (Exception ex)
            {
                if ((SQLServer_DDBB_Conection.connection.State != ConnectionState.Open && SQLServer_DDBB_Conection.connection.State != ConnectionState.Executing) && firstTime)
                {
                    if (ResetConecctionDDBB())
                    {
                        return GetElectronicHardwareList(idItemDefinition, false);
                    }
                    else
                    {
                        throw;
                    }
                }
                else
                {
                    throw;
                }
            }

            return tableResul;
        }
        public static DataTable GetMechanicalHardwareList(int idItemDefinition, bool firstTime = true)
        {
            DataTable tableResul = new DataTable();

            try
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;

                command.CommandText = "SELECT MechanicalDescription, MechanicalData FROM UnifiedMechanicalParameter " +
                                        "WHERE IdItemDefinition = @idItemDefinition";
                command.Parameters.AddWithValue("@idItemDefinition", idItemDefinition);

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(tableResul);
            }
            catch (Exception ex)
            {
                if ((SQLServer_DDBB_Conection.connection.State != ConnectionState.Open && SQLServer_DDBB_Conection.connection.State != ConnectionState.Executing) && firstTime)
                {
                    if (ResetConecctionDDBB())
                    {
                        return GetMechanicalHardwareList(idItemDefinition, false);
                    }
                    else
                    {
                        throw;
                    }
                }
                else
                {
                    throw;
                }
            }

            return tableResul;
        }
        public static DataTable GetSoftwareList(int idItemDefinition, bool firstTime = true)
        {
            DataTable tableResul = new DataTable();

            try
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;

                command.CommandText = "SELECT SoftwareDescription, SoftwareData FROM UnifiedSoftwareParameter " +
                                        "WHERE IdItemDefinition = @idItemDefinition";
                command.Parameters.AddWithValue("@idItemDefinition", idItemDefinition);

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(tableResul);
            }
            catch (Exception ex)
            {
                if ((SQLServer_DDBB_Conection.connection.State != ConnectionState.Open && SQLServer_DDBB_Conection.connection.State != ConnectionState.Executing) && firstTime)
                {
                    if (ResetConecctionDDBB())
                    {
                        return GetSoftwareList(idItemDefinition, false);
                    }
                    else
                    {
                        throw;
                    }
                }
                else
                {
                    throw;
                }
            }

            return tableResul;
        }
        public static DataTable GetProcess(int idItemDefinition, bool firstTime = true)
        {
            DataTable tableResul = new DataTable();

            try
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;

                command.CommandText = "SELECT ProcessDesignation FROM UnifiedProcess " +
                                        "WHERE IdItemDefinition = @idItemDefinition";
                command.Parameters.AddWithValue("@idItemDefinition", idItemDefinition);

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(tableResul);
            }
            catch (Exception ex)
            {
                if ((SQLServer_DDBB_Conection.connection.State != ConnectionState.Open && SQLServer_DDBB_Conection.connection.State != ConnectionState.Executing) && firstTime)
                {
                    if (ResetConecctionDDBB())
                    {
                        return GetProcess(idItemDefinition, false);
                    }
                    else
                    {
                        throw;
                    }
                }
                else
                {
                    throw;
                }
            }

            return tableResul;
        }
        public static DataTable GetLabellingList(int idItemDefinition, bool firstTime = true)
        {
            DataTable tableResul = new DataTable();

            try
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;

                command.CommandText = "SELECT LabellingDescription, LabellingData FROM UnifiedLabellingParameter " +
                                        "WHERE IdItemDefinition = @idItemDefinition";
                command.Parameters.AddWithValue("@idItemDefinition", idItemDefinition);

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(tableResul);
            }
            catch (Exception ex)
            {
                if ((SQLServer_DDBB_Conection.connection.State != ConnectionState.Open && SQLServer_DDBB_Conection.connection.State != ConnectionState.Executing) && firstTime)
                {
                    if (ResetConecctionDDBB())
                    {
                        return GetLabellingList(idItemDefinition, false);
                    }
                    else
                    {
                        throw;
                    }
                }
                else
                {
                    throw;
                }
            }

            return tableResul;
        }
        public static DataTable GetTestList(int idItemDefinition, bool firstTime = true)
        {
            DataTable tableResul = new DataTable();

            try
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;

                command.CommandText = "SELECT TestDescription, VersionTest FROM UnifiedTestParameter " +
                                        "WHERE IdItemDefinition = @idItemDefinition";
                command.Parameters.AddWithValue("@idItemDefinition", idItemDefinition);

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(tableResul);
            }
            catch (Exception ex)
            {
                if ((SQLServer_DDBB_Conection.connection.State != ConnectionState.Open && SQLServer_DDBB_Conection.connection.State != ConnectionState.Executing) && firstTime)
                {
                    if (ResetConecctionDDBB())
                    {
                        return GetTestList(idItemDefinition, false);
                    }
                    else
                    {
                        throw;
                    }
                }
                else
                {
                    throw;
                }
            }

            return tableResul;
        }
        #endregion
        #region Insert Procedures
        public static DataTable Insert_Picture(string pictureName, byte[] pictureData, bool firstTime = true)
        {
            DataTable tableResul = new DataTable();
            try
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "INSERT INTO PictureStorage(PictureName, PictureData) " +
                                        "VALUES (@pictureName, @pictureData)";
                command.Parameters.AddWithValue("@pictureName", pictureName);
                command.Parameters.AddWithValue("@pictureData", pictureData);

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(tableResul);

            }
            catch (Exception ex)
            {
                if ((SQLServer_DDBB_Conection.connection.State != ConnectionState.Open && SQLServer_DDBB_Conection.connection.State != ConnectionState.Executing) && firstTime)
                {
                    if (ResetConecctionDDBB())
                    {
                        return Insert_Picture(pictureName, pictureData, false);
                    }
                    else
                    {
                        throw;
                    }
                }
                else
                {
                    throw;
                }
            }

            return tableResul;
        }
        public static DataTable Insert_PCB(string user, string SN_Number, int idReference, int layout, int bom, string batch, bool firstTime = true)
        {
            DataTable tableResul = new DataTable();
            try
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "InsertSN_withLayoutBom";
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@userName", user);
                command.Parameters["@userName"].Direction = ParameterDirection.Input;

                command.Parameters.AddWithValue("@snNumber", SN_Number);
                command.Parameters["@snNumber"].Direction = ParameterDirection.Input;

                command.Parameters.AddWithValue("@idReference", idReference);
                command.Parameters["@idReference"].Direction = ParameterDirection.Input;

                command.Parameters.AddWithValue("@layout", layout);
                command.Parameters["@layout"].Direction = ParameterDirection.Input;

                command.Parameters.AddWithValue("@bom", bom);
                command.Parameters["@bom"].Direction = ParameterDirection.Input;

                command.Parameters.AddWithValue("@batch", batch);
                command.Parameters["@batch"].Direction = ParameterDirection.Input;

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(tableResul);

            }
            catch (Exception ex)
            {
                if ((SQLServer_DDBB_Conection.connection.State != ConnectionState.Open && SQLServer_DDBB_Conection.connection.State != ConnectionState.Executing) && firstTime)
                {
                    if (ResetConecctionDDBB())
                    {
                        return Insert_PCB(user, SN_Number, idReference, layout, bom, batch, false);
                    }
                    else
                    {
                        throw;
                    }
                }
                else
                {
                    throw;
                }
            }

            return tableResul;
        }
        public static DataTable Insert_ItemDefinition(string projectName, string sampleName, string partNumber, string descriptionItem, string internalName, string descriptionReference, bool isTrazability, string pictureName, byte[] pictureData, string dateRegistration, string userName, out int idInternalVersion, bool firstTime = true)
        {
            DataTable tableResul = new DataTable();
            idInternalVersion = 0;
            try
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "InsertItemDefinition";
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@projectName", (object)projectName ?? DBNull.Value);
                command.Parameters["@projectName"].Direction = ParameterDirection.Input;

                command.Parameters.AddWithValue("@sampleName", (object)sampleName ?? DBNull.Value);
                command.Parameters["@sampleName"].Direction = ParameterDirection.Input;

                command.Parameters.AddWithValue("@partNumber", (object)partNumber ?? DBNull.Value);
                command.Parameters["@partNumber"].Direction = ParameterDirection.Input;

                command.Parameters.AddWithValue("@descriptionItem", (object)descriptionItem ?? DBNull.Value);
                command.Parameters["@descriptionItem"].Direction = ParameterDirection.Input;

                command.Parameters.AddWithValue("@internalName", (object)internalName ?? DBNull.Value);
                command.Parameters["@internalName"].Direction = ParameterDirection.Input;

                command.Parameters.AddWithValue("@descriptionReference", (object)descriptionReference ?? DBNull.Value);
                command.Parameters["@descriptionReference"].Direction = ParameterDirection.Input;

                command.Parameters.AddWithValue("@isTrazability", (object)isTrazability ?? DBNull.Value);
                command.Parameters["@isTrazability"].Direction = ParameterDirection.Input;

                command.Parameters.AddWithValue("@pictureName", (object)pictureName ?? DBNull.Value);
                command.Parameters["@pictureName"].Direction = ParameterDirection.Input;

                command.Parameters.AddWithValue("@pictureData", (object)pictureData ?? SqlBinary.Null);
                command.Parameters["@pictureData"].Direction = ParameterDirection.Input;

                command.Parameters.AddWithValue("@dateRegistration", (object)dateRegistration ?? DBNull.Value);
                command.Parameters["@dateRegistration"].Direction = ParameterDirection.Input;

                command.Parameters.AddWithValue("@userName", (object)userName ?? DBNull.Value);
                command.Parameters["@userName"].Direction = ParameterDirection.Input;

                command.Parameters.Add("@idInternalVersion", SqlDbType.Int).Direction = ParameterDirection.Output;

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(tableResul);

                idInternalVersion = Convert.ToInt32(command.Parameters["@idInternalVersion"].Value);

            }
            catch (Exception ex)
            {
                if ((SQLServer_DDBB_Conection.connection.State != ConnectionState.Open && SQLServer_DDBB_Conection.connection.State != ConnectionState.Executing) && firstTime)
                {
                    if (ResetConecctionDDBB())
                    {
                        return Insert_ItemDefinition(projectName, sampleName, partNumber, descriptionItem, internalName, descriptionReference, isTrazability, pictureName, pictureData, dateRegistration, userName, out idInternalVersion, false);
                    }
                    else
                    {
                        throw;
                    }
                }
                else
                {
                    throw;
                }
            }

            return tableResul;
        }
        public static DataTable Insert_HardwareVersion(int idItemDefinition, string hardwarePCB, bool firstTime = true)
        {
            DataTable tableResul = new DataTable();
            try
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "InsertVersionHardware";
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@idItemDefinition", (object)idItemDefinition ?? DBNull.Value);
                command.Parameters["@idItemDefinition"].Direction = ParameterDirection.Input;

                command.Parameters.AddWithValue("@hardwarePCB", (object)hardwarePCB ?? DBNull.Value);
                command.Parameters["@hardwarePCB"].Direction = ParameterDirection.Input;

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(tableResul);

            }
            catch (Exception ex)
            {
                if ((SQLServer_DDBB_Conection.connection.State != ConnectionState.Open && SQLServer_DDBB_Conection.connection.State != ConnectionState.Executing) && firstTime)
                {
                    if (ResetConecctionDDBB())
                    {
                        return Insert_HardwareVersion(idItemDefinition, hardwarePCB, false);
                    }
                    else
                    {
                        throw;
                    }
                }
                else
                {
                    throw;
                }
            }

            return tableResul;
        }
        public static DataTable Insert_MechanicalVersion(int idItemDefinition, string parameter, string data, bool firstTime = true)
        {
            DataTable tableResul = new DataTable();
            try
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "InsertMechanicalVersionPrameter";
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@idItemDefinition", idItemDefinition);
                command.Parameters["@idItemDefinition"].Direction = ParameterDirection.Input;

                command.Parameters.AddWithValue("@parameter", parameter);
                command.Parameters["@parameter"].Direction = ParameterDirection.Input;

                command.Parameters.AddWithValue("@data", data);
                command.Parameters["@data"].Direction = ParameterDirection.Input;

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(tableResul);

            }
            catch (Exception ex)
            {
                if ((SQLServer_DDBB_Conection.connection.State != ConnectionState.Open && SQLServer_DDBB_Conection.connection.State != ConnectionState.Executing) && firstTime)
                {
                    if (ResetConecctionDDBB())
                    {
                        return Insert_MechanicalVersion(idItemDefinition, parameter, data, false);
                    }
                    else
                    {
                        throw;
                    }
                }
                else
                {
                    throw;
                }
            }

            return tableResul;
        }
        public static DataTable Insert_SoftwareVersion(int idItemDefinition, string parameter, string data, bool firstTime = true)
        {
            DataTable tableResul = new DataTable();
            try
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "InsertSoftwareVersionPrameter";
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@idItemDefinition", idItemDefinition);
                command.Parameters["@idItemDefinition"].Direction = ParameterDirection.Input;

                command.Parameters.AddWithValue("@parameter", parameter);
                command.Parameters["@parameter"].Direction = ParameterDirection.Input;

                command.Parameters.AddWithValue("@data", data);
                command.Parameters["@data"].Direction = ParameterDirection.Input;

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(tableResul);

            }
            catch (Exception ex)
            {
                if ((SQLServer_DDBB_Conection.connection.State != ConnectionState.Open && SQLServer_DDBB_Conection.connection.State != ConnectionState.Executing) && firstTime)
                {
                    if (ResetConecctionDDBB())
                    {
                        return Insert_SoftwareVersion(idItemDefinition, parameter, data, false);
                    }
                    else
                    {
                        throw;
                    }
                }
                else
                {
                    throw;
                }
            }

            return tableResul;
        }
        public static DataTable Insert_ProcessVersion(int idItemDefinition, string processDesignation, bool firstTime = true)
        {
            DataTable tableResul = new DataTable();
            try
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "InsertProcessVersion";
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@idItemDefinition", idItemDefinition);
                command.Parameters["@idItemDefinition"].Direction = ParameterDirection.Input;

                command.Parameters.AddWithValue("@processDesignation", processDesignation);
                command.Parameters["@processDesignation"].Direction = ParameterDirection.Input;

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(tableResul);

            }
            catch (Exception ex)
            {
                if ((SQLServer_DDBB_Conection.connection.State != ConnectionState.Open && SQLServer_DDBB_Conection.connection.State != ConnectionState.Executing) && firstTime)
                {
                    if (ResetConecctionDDBB())
                    {
                        return Insert_ProcessVersion(idItemDefinition, processDesignation, false);
                    }
                    else
                    {
                        throw;
                    }
                }
                else
                {
                    throw;
                }
            }

            return tableResul;
        }
        public static DataTable Insert_LabellingVersion(int idItemDefinition, string parameter, string data, bool firstTime = true)
        {
            DataTable tableResul = new DataTable();
            try
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "InsertLabellingVersionPrameter";
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@idItemDefinition", idItemDefinition);
                command.Parameters["@idItemDefinition"].Direction = ParameterDirection.Input;

                command.Parameters.AddWithValue("@parameter", parameter);
                command.Parameters["@parameter"].Direction = ParameterDirection.Input;

                command.Parameters.AddWithValue("@data", data);
                command.Parameters["@data"].Direction = ParameterDirection.Input;

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(tableResul);

            }
            catch (Exception ex)
            {
                if ((SQLServer_DDBB_Conection.connection.State != ConnectionState.Open && SQLServer_DDBB_Conection.connection.State != ConnectionState.Executing) && firstTime)
                {
                    if (ResetConecctionDDBB())
                    {
                        return Insert_LabellingVersion(idItemDefinition, parameter, data, false);
                    }
                    else
                    {
                        throw;
                    }
                }
                else
                {
                    throw;
                }
            }

            return tableResul;
        }
        public static DataTable Insert_TestVersion(int idItemDefinition, string testDescription, int versionTest, bool firstTime = true)
        {
            DataTable tableResul = new DataTable();
            try
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "InsertTestVersion";
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@idItemDefinition", idItemDefinition);
                command.Parameters["@idItemDefinition"].Direction = ParameterDirection.Input;

                command.Parameters.AddWithValue("@testDescription", testDescription);
                command.Parameters["@testDescription"].Direction = ParameterDirection.Input;

                command.Parameters.Add("@versionTest", SqlDbType.Int).Value = versionTest;
                command.Parameters["@versionTest"].Direction = ParameterDirection.Input;

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(tableResul);

            }
            catch (Exception ex)
            {
                if ((SQLServer_DDBB_Conection.connection.State != ConnectionState.Open && SQLServer_DDBB_Conection.connection.State != ConnectionState.Executing) && firstTime)
                {
                    if (ResetConecctionDDBB())
                    {
                        return Insert_TestVersion(idItemDefinition, testDescription, versionTest, false);
                    }
                    else
                    {
                        throw;
                    }
                }
                else
                {
                    throw;
                }
            }

            return tableResul;
        }
        public static DataTable Insert_DefinitionProcess(string processName, int num, int processNum, string processDescription, int operation, string operationDescription, string pictureName, string stationName, string definitionTest, string inputType, string screwCode, string referenceAssembly, bool firstTime = true)
        {
            DataTable tableResul = new DataTable();
            try
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "InsertProcessDefinition";
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@processDesignation", processName);
                command.Parameters["@processDesignation"].Direction = ParameterDirection.Input;

                command.Parameters.AddWithValue("@num", num);
                command.Parameters["@num"].Direction = ParameterDirection.Input;

                command.Parameters.AddWithValue("@processNum", processNum);
                command.Parameters["@processNum"].Direction = ParameterDirection.Input;

                command.Parameters.AddWithValue("@processDescription", processDescription);
                command.Parameters["@processDescription"].Direction = ParameterDirection.Input;

                command.Parameters.AddWithValue("@operation", operation);
                command.Parameters["@operation"].Direction = ParameterDirection.Input;

                command.Parameters.AddWithValue("@operationDescription", operationDescription);
                command.Parameters["@operationDescription"].Direction = ParameterDirection.Input;

                command.Parameters.AddWithValue("@pictureName", pictureName);
                command.Parameters["@pictureName"].Direction = ParameterDirection.Input;

                command.Parameters.AddWithValue("@stationName", stationName);
                command.Parameters["@stationName"].Direction = ParameterDirection.Input;

                command.Parameters.AddWithValue("@definitionTest", definitionTest);
                command.Parameters["@definitionTest"].Direction = ParameterDirection.Input;

                command.Parameters.AddWithValue("@inputType", inputType);
                command.Parameters["@inputType"].Direction = ParameterDirection.Input;

                command.Parameters.AddWithValue("@screwCode", screwCode);
                command.Parameters["@screwCode"].Direction = ParameterDirection.Input;

                command.Parameters.AddWithValue("@referenceAssembly", referenceAssembly);
                command.Parameters["@referenceAssembly"].Direction = ParameterDirection.Input;

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(tableResul);
            }
            catch (Exception ex)
            {
                if ((SQLServer_DDBB_Conection.connection.State != ConnectionState.Open && SQLServer_DDBB_Conection.connection.State != ConnectionState.Executing) && firstTime)
                {
                    if (ResetConecctionDDBB())
                    {
                        return Insert_DefinitionProcess(processName, num, processNum, processDescription, operation, operationDescription, pictureName, stationName, definitionTest, inputType, screwCode, referenceAssembly, false);
                    }
                    else
                    {
                        throw;
                    }
                }
                else
                {
                    throw;
                }
            }

            return tableResul;
        }
        #endregion
        #region Delete Procedures
        public static DataTable Delete_HardwareVersion(int idItemDefinition, bool firstTime = true)
        {
            DataTable tableResul = new DataTable();
            try
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "DeleteHardwareVersion";
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@idItemDefinition", idItemDefinition);
                command.Parameters["@idItemDefinition"].Direction = ParameterDirection.Input;

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(tableResul);
            }
            catch (Exception ex)
            {
                if ((SQLServer_DDBB_Conection.connection.State != ConnectionState.Open && SQLServer_DDBB_Conection.connection.State != ConnectionState.Executing) && firstTime)
                {
                    if (ResetConecctionDDBB())
                    {
                        return Delete_HardwareVersion(idItemDefinition, false);
                    }
                    else
                    {
                        throw;
                    }
                }
                else
                {
                    throw;
                }
            }

            return tableResul;
        }
        public static DataTable Delete_ItemDefinition(int idItemDefinition, bool firstTime = true)
        {
            DataTable tableResul = new DataTable();
            try
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "DeleteItemDefinition";
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@idItemDefinition", idItemDefinition);
                command.Parameters["@idItemDefinition"].Direction = ParameterDirection.Input;

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(tableResul);
            }
            catch (Exception ex)
            {
                if ((SQLServer_DDBB_Conection.connection.State != ConnectionState.Open && SQLServer_DDBB_Conection.connection.State != ConnectionState.Executing) && firstTime)
                {
                    if (ResetConecctionDDBB())
                    {
                        return Delete_ItemDefinition(idItemDefinition, false);
                    }
                    else
                    {
                        throw;
                    }
                }
                else
                {
                    throw;
                }
            }

            return tableResul;
        }
        public static DataTable Delete_MechanicalVersion(int idItemDefinition, bool firstTime = true)
        {
            DataTable tableResul = new DataTable();
            try
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "DeleteMechanicalVersion";
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@idItemDefinition", idItemDefinition);
                command.Parameters["@idItemDefinition"].Direction = ParameterDirection.Input;

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(tableResul);
            }
            catch (Exception ex)
            {
                if ((SQLServer_DDBB_Conection.connection.State != ConnectionState.Open && SQLServer_DDBB_Conection.connection.State != ConnectionState.Executing) && firstTime)
                {
                    if (ResetConecctionDDBB())
                    {
                        return Delete_MechanicalVersion(idItemDefinition, false);
                    }
                    else
                    {
                        throw;
                    }
                }
                else
                {
                    throw;
                }
            }

            return tableResul;
        }
        public static DataTable Delete_SoftwareVersion(int idItemDefinition, bool firstTime = true)
        {
            DataTable tableResul = new DataTable();
            try
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "DeleteSoftwareVersion";
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@idItemDefinition", idItemDefinition);
                command.Parameters["@idItemDefinition"].Direction = ParameterDirection.Input;

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(tableResul);
            }
            catch (Exception ex)
            {
                if ((SQLServer_DDBB_Conection.connection.State != ConnectionState.Open && SQLServer_DDBB_Conection.connection.State != ConnectionState.Executing) && firstTime)
                {
                    if (ResetConecctionDDBB())
                    {
                        return Delete_SoftwareVersion(idItemDefinition, false);
                    }
                    else
                    {
                        throw;
                    }
                }
                else
                {
                    throw;
                }
            }

            return tableResul;
        }
        public static DataTable Delete_ProcessVersion(int idItemDefinition, bool firstTime = true)
        {
            DataTable tableResul = new DataTable();
            try
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "DeleteProcessVersion";
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@idItemDefinition", idItemDefinition);
                command.Parameters["@idItemDefinition"].Direction = ParameterDirection.Input;

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(tableResul);
            }
            catch (Exception ex)
            {
                if ((SQLServer_DDBB_Conection.connection.State != ConnectionState.Open && SQLServer_DDBB_Conection.connection.State != ConnectionState.Executing) && firstTime)
                {
                    if (ResetConecctionDDBB())
                    {
                        return Delete_ProcessVersion(idItemDefinition, false);
                    }
                    else
                    {
                        throw;
                    }
                }
                else
                {
                    throw;
                }
            }

            return tableResul;
        }
        public static DataTable Delete_LabellingVersion(int idItemDefinition, bool firstTime = true)
        {
            DataTable tableResul = new DataTable();
            try
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "DeleteLabellingVersion";
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@idItemDefinition", idItemDefinition);
                command.Parameters["@idItemDefinition"].Direction = ParameterDirection.Input;

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(tableResul);
            }
            catch (Exception ex)
            {
                if ((SQLServer_DDBB_Conection.connection.State != ConnectionState.Open && SQLServer_DDBB_Conection.connection.State != ConnectionState.Executing) && firstTime)
                {
                    if (ResetConecctionDDBB())
                    {
                        return Delete_LabellingVersion(idItemDefinition, false);
                    }
                    else
                    {
                        throw;
                    }
                }
                else
                {
                    throw;
                }
            }

            return tableResul;
        }
        public static DataTable Delete_TestingVersion(int idItemDefinition, bool firstTime = true)
        {
            DataTable tableResul = new DataTable();
            try
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "DeleteTestingVersion";
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@idItemDefinition", idItemDefinition);
                command.Parameters["@idItemDefinition"].Direction = ParameterDirection.Input;

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(tableResul);
            }
            catch (Exception ex)
            {
                if ((SQLServer_DDBB_Conection.connection.State != ConnectionState.Open && SQLServer_DDBB_Conection.connection.State != ConnectionState.Executing) && firstTime)
                {
                    if (ResetConecctionDDBB())
                    {
                        return Delete_TestingVersion(idItemDefinition, false);
                    }
                    else
                    {
                        throw;
                    }
                }
                else
                {
                    throw;
                }
            }

            return tableResul;
        }
        public static DataTable Delete_DefinitionProcess(string processName, bool firstTime = true)
        {
            DataTable tableResul = new DataTable();
            try
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "DeleteProcessDefinition";
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@processDesignation", processName);
                command.Parameters["@processDesignation"].Direction = ParameterDirection.Input;

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(tableResul);
            }
            catch (Exception ex)
            {
                if ((SQLServer_DDBB_Conection.connection.State != ConnectionState.Open && SQLServer_DDBB_Conection.connection.State != ConnectionState.Executing) && firstTime)
                {
                    if (ResetConecctionDDBB())
                    {
                        return Delete_DefinitionProcess(processName, false);
                    }
                    else
                    {
                        throw;
                    }
                }
                else
                {
                    throw;
                }
            }

            return tableResul;
        }
        #endregion
    }
}
