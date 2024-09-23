using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data;
using System.Net;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Windows.Controls.Primitives;
using System.Windows.Markup;
using Org.BouncyCastle.Utilities;
using Google.Protobuf.WellKnownTypes;
using MySqlX.XDevAPI.Common;
using System.Windows.Input;
using Aspose.Cells;
using System.Collections;
using static Org.BouncyCastle.Asn1.Cmp.Challenge;
using System.IO;
using Org.BouncyCastle.Crypto;
namespace DIV_Protos
{
    public static class DDBB_MySQL
    {
        #region atributes
        public static readonly object LockDB = new object();
        public static bool connection_ddbb = false;
        private static MySqlConnectionStringBuilder conn_string = new MySqlConnectionStringBuilder();
        private static MySqlConnection connection = new MySqlConnection();
        private static string ColumSpliter = "<<split>>";
        #endregion
        #region init-close-reset
        public static bool InitDDBB(out string err_msg)
        {
            err_msg = "";
            bool status = false;
            try
            {
                if(connection_ddbb == false)
                {
                    conn_string.Server = "server";
                    conn_string.UserID = "user";
                    conn_string.Password = "password";
                    conn_string.Database = "ddbb";
                    conn_string.SslMode = MySqlSslMode.None;
                    string conexion = conn_string.ToString();
                    connection = new MySqlConnection(conn_string.ToString());
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
                connection.ClearAllPoolsAsync();
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
            status = DDBB_MySQL.CloseDDBB();
            if (status)
            {
                status = DDBB_MySQL.InitDDBB(out err_msg);
            }
            return status;
            
        }
        #endregion
        //Lector de datos
        private static bool ReadDataFromStoredProcess(MySqlDataReader Read_data, out List<string> result)
        {
            bool status = false;
            List<string> aux_result = new List<string>();
            string line = "";
            object value;
            try
            {
                while (Read_data.Read())
                {
                    try
                    {
                        for (int i = 0; i < Read_data.FieldCount; i++)
                        {
                            value = Read_data.GetValue(i);
                            if (value != null)
                            {
                                line += value.ToString();
                            }
                            else
                            {
                                line += " ";
                            }
                            if (i != Read_data.FieldCount-1)
                            {
                                line += ColumSpliter;
                            }
                        }
                    }
                    catch
                    {
                        line += " ";
                    }
                    aux_result.Add(line);
                    line = "";
                }
                status = true;
            }
            catch
            {
                status = false;
            }
            result = aux_result;
            return status;
        }

        //MIS Metodos

        public static bool GetPlataform(out ObservableCollection<Plataform> plataforms, out string err_msg, bool firstTime = true)
        {
            bool status = false;
            List<string> _resul = new List<string>();
            err_msg = "";
            plataforms = new ObservableCollection<Plataform>();
            String[] datas;

            try
            {
                MySqlCommand command = new MySqlCommand();
                command.Connection = connection;
                command.CommandText = "SELECT idPlataforma, Nombre_Plataforma FROM Plataforma;";
                command.ExecuteNonQuery();

                MySqlDataReader Read_data = command.ExecuteReader();
                status = ReadDataFromStoredProcess(Read_data, out _resul);
                Read_data.Close();

                foreach (var data in _resul)
                {
                    datas = data.Split(new string[] { ColumSpliter }, StringSplitOptions.None);
                    if (datas.Length == 2)
                    {
                        plataforms.Add(new Plataform(int.Parse(datas[0]), datas[1]));
                    }
                }

            }
            catch (Exception ex)
            {
                err_msg = ex.Message;
                if ((DDBB_MySQL.connection.State != ConnectionState.Open || DDBB_MySQL.connection.State != ConnectionState.Executing) && firstTime)
                {
                    if (ResetConecctionDDBB())
                    {
                        return GetPlataform(out plataforms, out err_msg,  false);
                    } else
                    {
                        return false;
                    }
                    
                }
                else
                {
                    DDBB_MySQL.CloseDDBB();
                }
            }

            return status;
        }
        public static List<string> GetUserRol(string User, bool firstTime = true)
        {
            List<string> _resul = new List<string>();
            string err_msg = "";

            try
            {
                MySqlCommand command = new MySqlCommand();
                command.Connection = connection;
                command.CommandText = "SELECT Descripcion FROM RolesUsers " +
                                        "WHERE INSTR(@ordenador, Ordenador) != 0;";
                command.Parameters.AddWithValue("@ordenador", User);
                command.ExecuteNonQuery();

                MySqlDataReader Read_data = command.ExecuteReader();
                ReadDataFromStoredProcess(Read_data, out _resul);
                Read_data.Close();
            }
            catch (Exception ex)
            {
                err_msg = ex.Message;
                if ((DDBB_MySQL.connection.State != ConnectionState.Open || DDBB_MySQL.connection.State != ConnectionState.Executing) && firstTime)
                {
                    if (ResetConecctionDDBB())
                    {
                        _resul = GetUserRol(User, false);
                        return _resul;
                    }
                } else
                {
                    DDBB_MySQL.CloseDDBB();
                }
            }
            //rows_data = _resul;
            return _resul;
        }

        public static bool GetPCBReferences(int idPlataform, out ObservableCollection<PCBReference> rows_data, out string err_msg, bool firstTime = true)
        {
            bool status = false;
            rows_data = new ObservableCollection<PCBReference>();
            err_msg = "";
            List<string> _resul = new List<string>();
            String[] datas;

            try
            {
                MySqlCommand command = new MySqlCommand();
                command.Connection = connection;
                command.CommandText = "SELECT idReferences_PCBs, Description_Reference, Reference, idPlataforma FROM References_PCBs " +
                                        "WHERE idPlataforma = @idPlataforma;";
                command.Parameters.AddWithValue("@idPlataforma", idPlataform);
                command.ExecuteNonQuery();

                MySqlDataReader Read_data = command.ExecuteReader();
                status = ReadDataFromStoredProcess(Read_data, out _resul);
                Read_data.Close();
                foreach (var data in _resul)
                {
                    datas = data.Split(new string[] { ColumSpliter }, StringSplitOptions.None);
                    if(datas.Length == 4)
                    {
                        rows_data.Add(new PCBReference(datas));
                    } else if (data.Length >= 2)
                    {
                        rows_data.Add(new PCBReference(datas[0], datas[1]));
                    }
                }
            }
            catch (Exception ex)
            {
                err_msg = ex.Message;
                if ((DDBB_MySQL.connection.State != ConnectionState.Open || DDBB_MySQL.connection.State != ConnectionState.Executing) && firstTime)
                {
                    if (ResetConecctionDDBB())
                    {
                        return GetPCBReferences(idPlataform, out rows_data, out err_msg, false);
                    }
                    else
                    {
                        _resul.Add("3");
                        return false;
                    }

                }
                else
                {
                    DDBB_MySQL.CloseDDBB();
                }
            }
            return status;
        }
        public static bool GetPCBBLayoutBOMFrom(int reference, out ObservableCollection<string> rows_data, out string err_msg, bool firstTime = true)
        {
            bool status = false;
            List<string> _resul = new List<string>();
            ObservableCollection<string>  temporalRows = new ObservableCollection<string>();
            err_msg = "";
            rows_data = new ObservableCollection<string>();

            try
            {
                MySqlCommand command = new MySqlCommand();
                command.Connection = connection;

                command.CommandText = "SELECT idReferences_PCBs, `Layout.Bom` FROM LayoutBomFromPCBReference " +
                                            "WHERE idReferences_PCBs = @reference ";
                command.Parameters.AddWithValue("@reference", reference);
                command.ExecuteNonQuery();

                MySqlDataReader Read_data = command.ExecuteReader();
                status = ReadDataFromStoredProcess(Read_data, out _resul);
                Read_data.Close();
                _resul.ForEach (register => temporalRows.Add(register.Split(new string[] { ColumSpliter }, StringSplitOptions.None)[1]));
                rows_data = temporalRows;

            }
            catch (Exception ex)
            {
                err_msg = ex.Message;
                if ((DDBB_MySQL.connection.State != ConnectionState.Open || DDBB_MySQL.connection.State != ConnectionState.Executing) && firstTime)
                {
                    if (ResetConecctionDDBB())
                    {
                        return GetPCBBLayoutBOMFrom(reference, out rows_data, out err_msg, false);
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    DDBB_MySQL.CloseDDBB();
                }
            }

            return status;
        }
        public static bool ExistsSNFromRegPCB(string SN_Number, out string err_msg, bool firstTime = true)
        {
            bool status = false;
            List<string> _resul = new List<string>();
            err_msg = "";

            try
            {
                MySqlCommand command = new MySqlCommand();
                command.Connection = connection;
                command.CommandText = "SELECT idSN_Register_PCBs, idReferences_PCBs, SN, Date, Status, Lote FROM SN_Register_PCBs " +
                                        "WHERE SN = @snNumber";
                command.Parameters.AddWithValue("@snNumber", SN_Number);
                command.ExecuteNonQuery();

                MySqlDataReader Read_data = command.ExecuteReader();
                status = ReadDataFromStoredProcess(Read_data, out _resul);
                status = _resul.Count > 0;
                Read_data.Close();
            }
            catch (Exception ex)
            {
                err_msg = ex.Message;
                if ((DDBB_MySQL.connection.State != ConnectionState.Open || DDBB_MySQL.connection.State != ConnectionState.Executing) && firstTime)
                {
                    if (ResetConecctionDDBB())
                    {
                        return ExistsSNFromRegPCB(SN_Number, out err_msg, false);
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    DDBB_MySQL.CloseDDBB();
                }
            }
            return status;
        }
        public static bool Insert_PCB(string user, string SN_Number, int idReference, int layout, int bom, string lote, out string err_msg, bool firstTime = true)
        {
            bool status = false;
            List<string> _resul = new List<string>();
            err_msg = "";

            try
            {
                MySqlCommand command = new MySqlCommand();
                command.Connection = connection;
                command.CommandText = "InsertarAltaPCB";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@ordenadorName", user);
                command.Parameters["@ordenadorName"].Direction = ParameterDirection.Input;

                command.Parameters.AddWithValue("@snNumber", SN_Number);
                command.Parameters["@snNumber"].Direction = ParameterDirection.Input;

                command.Parameters.AddWithValue("@idReference", idReference);
                command.Parameters["@idReference"].Direction = ParameterDirection.Input;

                command.Parameters.AddWithValue("@layout", layout);
                command.Parameters["@layout"].Direction = ParameterDirection.Input;

                command.Parameters.AddWithValue("@bom", bom);
                command.Parameters["@bom"].Direction = ParameterDirection.Input;

                command.Parameters.AddWithValue("@lote", lote);
                command.Parameters["@lote"].Direction = ParameterDirection.Input;

                //command.ExecuteNonQuery();
                MySqlDataReader Read_data = command.ExecuteReader();
                status = ReadDataFromStoredProcess(Read_data, out _resul);
                Read_data.Close();
                status = true;
            }
            catch (Exception ex)
            {
                err_msg = ex.Message;
                if ((DDBB_MySQL.connection.State != ConnectionState.Open || DDBB_MySQL.connection.State != ConnectionState.Executing) && firstTime)
                {
                    if (ResetConecctionDDBB())
                    {
                        return Insert_PCB(user, SN_Number, idReference, layout, bom, lote, out err_msg, false);
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    DDBB_MySQL.CloseDDBB();
                }
            }
            return status;
        }

        public static bool ExistsSNFromRegHousing(string SN_Number, out string err_msg, bool firstTime = true)
        {
            bool status = false;
            List<string> _resul = new List<string>();
            err_msg = "";

            try
            {
                MySqlCommand command = new MySqlCommand();
                command.Connection = connection;
                command.CommandText = "SELECT idSN_Register_Housing, idRolesOrdenadores, SN, Status, Date FROM SN_Register_Housing " +
                                        "WHERE SN = @snNumber";
                command.Parameters.AddWithValue("@snNumber", SN_Number);
                command.ExecuteNonQuery();

                MySqlDataReader Read_data = command.ExecuteReader();
                status = ReadDataFromStoredProcess(Read_data, out _resul);
                status = _resul.Count > 0;
                Read_data.Close();
            }
            catch (Exception ex)
            {
                err_msg = ex.Message;
                if ((DDBB_MySQL.connection.State != ConnectionState.Open || DDBB_MySQL.connection.State != ConnectionState.Executing) && firstTime)
                {
                    if (ResetConecctionDDBB())
                    {
                        return ExistsSNFromRegHousing(SN_Number, out err_msg, false);
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    DDBB_MySQL.CloseDDBB();
                }
            }
            return status;
        }


        public static bool GetMechanicalVersionCodes(int idPlataform, out ObservableCollection<MechanicalVersion> codesVersion, out string err_msg, bool firstTime = true)
        {
            bool status = false;
            List<string> _resul = new List<string>();
            err_msg = "";
            codesVersion = new ObservableCollection<MechanicalVersion>();
            String[] datas;

            try
            {
                MySqlCommand command = new MySqlCommand();
                command.Connection = connection;
                command.CommandText = "SELECT idMechanical_WindchillCode_Housing, Code FROM Mechanical_WindchillCode_Housing " +
                                        "WHERE idPlataforma = @idPlataforma;";
                command.Parameters.AddWithValue("@idPlataforma", idPlataform);
                command.ExecuteNonQuery();

                MySqlDataReader Read_data = command.ExecuteReader();
                status = ReadDataFromStoredProcess(Read_data, out _resul);
                Read_data.Close();

                foreach (var data in _resul)
                {
                    datas = data.Split(new string[] { ColumSpliter }, StringSplitOptions.None);
                    if (datas.Length == 2)
                    {
                        codesVersion.Add(new MechanicalVersion(int.Parse(datas[0]), datas[1],"Housing"));
                    }
                }
                
            }
            catch (Exception ex)
            {
                err_msg = ex.Message;
                if ((DDBB_MySQL.connection.State != ConnectionState.Open || DDBB_MySQL.connection.State != ConnectionState.Executing) && firstTime)
                {
                    if (ResetConecctionDDBB())
                    {
                        return GetMechanicalVersionCodes(idPlataform, out codesVersion, out err_msg, false);
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    DDBB_MySQL.CloseDDBB();
                }
            }

            return status;
        }

        public static bool Insert_Housing(string user, string snNumber, int idMechVers, out string err_msg, bool firstTime = true)
        {
            bool status = false;
            List<string> _resul = new List<string>();
            err_msg = "";

            try
            {
                MySqlCommand command = new MySqlCommand();
                command.Connection = connection;
                command.CommandText = "InsertarAltaHousing";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@ordenadorName", user);
                command.Parameters["@ordenadorName"].Direction = ParameterDirection.Input;

                command.Parameters.AddWithValue("@snNumber", snNumber);
                command.Parameters["@snNumber"].Direction = ParameterDirection.Input;

                command.Parameters.AddWithValue("@idMechanicalVersion", idMechVers);
                command.Parameters["@idMechanicalVersion"].Direction = ParameterDirection.Input;

                MySqlDataReader Read_data = command.ExecuteReader();
                status = ReadDataFromStoredProcess(Read_data, out _resul);
                Read_data.Close();
                status = true;
            }
            catch (Exception ex)
            {
                err_msg = ex.Message;
                if ((DDBB_MySQL.connection.State != ConnectionState.Open || DDBB_MySQL.connection.State != ConnectionState.Executing) && firstTime)
                {
                    if (ResetConecctionDDBB())
                    {
                        return Insert_Housing(user, snNumber, idMechVers, out err_msg, false);
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    DDBB_MySQL.CloseDDBB();
                }
            }

            return status;
        }

        public static bool SN_ReferenceLayoutBom(string SN_Number,out PCBDefinition lastPCBDefinition, out string err_msg, bool firstTime = true)
        {
            bool status = false;
            List<string> _resul = new List<string>();
            lastPCBDefinition = new PCBDefinition();
            err_msg = "";

            try
            {
                MySqlCommand command = new MySqlCommand();
                command.Connection = connection;
                command.CommandText = "SELECT idSN_Register_PCBs, SN, Reference, Layout, BOM, Rework, Date FROM SN_PCBReferenceLayoutBomFrom " +
                                        "WHERE SN = @snNumber " +
                                        "ORDER BY Rework DESC " +
                                        "LIMIT 1;";
                command.Parameters.AddWithValue("@snNumber", SN_Number);
                command.ExecuteNonQuery();

                MySqlDataReader Read_data = command.ExecuteReader();
                status = ReadDataFromStoredProcess(Read_data, out _resul);
                Read_data.Close();
                status = _resul.Count > 0;

                if (status)
                {
                    string[] rowData = _resul[0].Split(new string[] { ColumSpliter }, StringSplitOptions.None);
                }

            }
            catch (Exception ex)
            {
                err_msg = ex.Message;
                if ((DDBB_MySQL.connection.State != ConnectionState.Open || DDBB_MySQL.connection.State != ConnectionState.Executing) && firstTime)
                {
                    if (ResetConecctionDDBB())
                    {
                        return SN_ReferenceLayoutBom(SN_Number, out lastPCBDefinition, out err_msg, false);
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    DDBB_MySQL.CloseDDBB();
                }
            }
            return status;
        }

        public static string LastReworkFromIdSN(int idSN, out string err_msg, bool firstTime = true)
        {
            bool status = false;
            string lastRework = "";
            List<string> _resul = new List<string>();
            err_msg = "";

            try
            {
                MySqlCommand command = new MySqlCommand();
                command.Connection = connection;
                command.CommandText = "SELECT MAX(Value) FROM Components_Register_PCB " +
                                        "WHERE idSN_Register_PCBs = @idSN AND idParameters_Components = 3;";
                command.Parameters.AddWithValue("@idSN", idSN);
                command.ExecuteNonQuery();

                MySqlDataReader Read_data = command.ExecuteReader();
                status = ReadDataFromStoredProcess(Read_data, out _resul);
                if (_resul.Count == 1)
                {
                    lastRework = _resul.First();
                    status = true;
                }
                else
                {
                    err_msg = "IncorrectData";
                }
                Read_data.Close();
            }
            catch (Exception ex)
            {
                err_msg = ex.Message;
                if ((DDBB_MySQL.connection.State != ConnectionState.Open || DDBB_MySQL.connection.State != ConnectionState.Executing) && firstTime)
                {
                    if (ResetConecctionDDBB())
                    {
                        return LastReworkFromIdSN(idSN, out err_msg, false);
                    }
                    else
                    {
                        return lastRework;
                    }
                }
                else
                {
                    DDBB_MySQL.CloseDDBB();
                }
            }
            return lastRework;
        }

        public static bool InsertSNRework(string user, int idSN, int rework, out string err_msg, bool firstTime = true)
        {
            bool status = false;
            List<string> _resul = new List<string>();
            err_msg = "";

            try
            {
                MySqlCommand command = new MySqlCommand();
                command.Connection = connection;
                //Plantar procedimiento
                command.CommandText = "INSERT INTO Components_Register_PCB(idRolesOrdenadores, idSN_Register_PCBs, idParameters_Components, Value, Date) " +
                                        "VALUES ((SELECT idRoles_Ordenadores FROM Roles_Ordenadores WHERE INSTR(@user, Ordenador) != 0),@idSN, 3, @rework, current_timestamp());";
                command.Parameters.AddWithValue("@user", user);
                command.Parameters.AddWithValue("@idSN", idSN);
                command.Parameters.AddWithValue("@rework", rework);

                MySqlDataReader Read_data = command.ExecuteReader();
                status = ReadDataFromStoredProcess(Read_data, out _resul);
                
                Read_data.Close();
            }
            catch (Exception ex)
            {
                err_msg = ex.Message;
                if ((DDBB_MySQL.connection.State != ConnectionState.Open || DDBB_MySQL.connection.State != ConnectionState.Executing) && firstTime)
                {
                    if (ResetConecctionDDBB())
                    {
                        return InsertSNRework(user, idSN, rework, out err_msg, false);
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    DDBB_MySQL.CloseDDBB();
                }
            }
            return status;
        }

        public static int InsertInternallVersion(string plataform, bool isDummy, string sample, string partNumber, string internalVersion, string registerDate, out string err_msg, bool firstTime = true)
        {
            int newInternalVersion = -1;
            List<string> _resul = new List<string>();
            err_msg = "";

            try
            {
                MySqlCommand command = new MySqlCommand();
                command.Connection = connection;
                command.CommandText = "InsertOrGetVersionInterna";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@plataforma", plataform);
                command.Parameters["@plataforma"].Direction = ParameterDirection.Input;

                command.Parameters.AddWithValue("@isDummy", isDummy ? 1 : 0);
                command.Parameters["@isDummy"].Direction = ParameterDirection.Input;

                command.Parameters.AddWithValue("@sampleCode", sample);
                command.Parameters["@sampleCode"].Direction = ParameterDirection.Input;

                command.Parameters.AddWithValue("@partNumber", partNumber);
                command.Parameters["@partNumber"].Direction = ParameterDirection.Input;

                command.Parameters.AddWithValue("@internalName", internalVersion);
                command.Parameters["@internalName"].Direction = ParameterDirection.Input;

                command.Parameters.AddWithValue("@registerDateString", registerDate.Equals("") ? null : registerDate);
                command.Parameters["@registerDateString"].Direction = ParameterDirection.Input;

                command.Parameters.Add(new MySqlParameter("@newId", MySqlDbType.Int64));
                command.Parameters["@newId"].Direction = ParameterDirection.Output;

                command.ExecuteNonQuery();

                MySqlDataReader Read_data = command.ExecuteReader();
                ReadDataFromStoredProcess(Read_data, out _resul);

                Read_data.Close();
                foreach (var data in _resul)
                {
                    if (data.Contains(ColumSpliter))
                    {
                        err_msg += data.Split(new string[] { ColumSpliter }, StringSplitOptions.None)[2];
                        err_msg += " ";
                    }
                    else
                    {
                        err_msg += data;
                    }

                }

                if (!(command.Parameters["@newId"].Value is DBNull))
                {
                    newInternalVersion = Convert.ToInt32(command.Parameters["@newId"].Value);
                    //newInternalVersion = (int)command.Parameters["@newId"].Value;
                }
            }
            catch (Exception ex)
            {
                err_msg = ex.Message;
                if ((DDBB_MySQL.connection.State != ConnectionState.Open || DDBB_MySQL.connection.State != ConnectionState.Executing) && firstTime)
                {
                    if (ResetConecctionDDBB())
                    {
                        return InsertInternallVersion(plataform, isDummy, sample, partNumber, internalVersion, registerDate, out err_msg, false);
                    }
                    else
                    {
                        return -1;
                    }
                }
                else
                {
                    DDBB_MySQL.CloseDDBB();
                }
            }

            return newInternalVersion;
        }

        public static bool CheckInternallHardware(ElectronicHardware electronicHardware, out string err_msg, bool firstTime = true)
        {
            bool valid = true;
            ObservableCollection<PCBReference> rows_data = new ObservableCollection<PCBReference>();
            err_msg = "";
            List<string> _resul = new List<string>();
            List<string> resultReferences = new List<string>();
            String[] datas;
            PCBReference pcb;

            try
            {
                MySqlCommand command = new MySqlCommand();
                command.Connection = connection;
                command.CommandText = "SELECT idReferences_PCBs, Description_Reference, Reference, idPlataforma  FROM References_PCBs;";

                command.ExecuteNonQuery();
                MySqlDataReader Read_data = command.ExecuteReader();
                ReadDataFromStoredProcess(Read_data, out _resul);
                Read_data.Close();
                foreach (var data in _resul)
                {
                    datas = data.Split(new string[] { ColumSpliter }, StringSplitOptions.None);
                    if (datas.Length == 4)
                    {
                        rows_data.Add(new PCBReference(datas));
                        resultReferences.Add(datas[2]);
                    }
                }
                foreach(var hardware in electronicHardware.Hardwares)
                {
                    valid = false;
                    //pcb = (PCBReference)rows_data.Where(thisRef => hardware.Value.Contains(thisRef.Reference));
                    foreach (var refer in resultReferences)
                    {
                        if (hardware.Value.Contains(refer))
                        {
                            valid = true; 
                            break;
                        }
                    }
                    if (!valid)
                    {
                        err_msg = "";
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                err_msg = ex.Message;
                valid = false;
                if ((DDBB_MySQL.connection.State != ConnectionState.Open || DDBB_MySQL.connection.State != ConnectionState.Executing) && firstTime)
                {
                    if (ResetConecctionDDBB())
                    {
                        return CheckInternallHardware(electronicHardware, out err_msg, false);
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    DDBB_MySQL.CloseDDBB();
                }
            }

            return valid;
        }
        public static void InsertInternallVersionDefinition(int idInternalVersion, ReleaseDefinition releaseDefinition, out string err_msg, bool firstTime = true)
        {
            List<string> _resul = new List<string>();
            err_msg = "";
            /*
            bool mechanicalBlockEmpty =  releaseDefinition.Windchill.Equals("") || releaseDefinition.Lb.Equals("");
            bool softwareBlockEmpty =    releaseDefinition.Software.Hw_snr.Equals("") || 
                                    releaseDefinition.Software.HwStatus.Equals("") || 
                                    releaseDefinition.Software.Sw_snr.Equals("") || 
                                    releaseDefinition.Software.SwStatus.Equals("") || 
                                    releaseDefinition.Software.ReleaseTest.Equals("") || 
                                    releaseDefinition.Software.ReleaseFinal.Equals("");
            bool processBlockEmpty = releaseDefinition.Process.Equals("") || releaseDefinition.Trazability.Equals("");

            try
            {
                MySqlCommand command = new MySqlCommand();
                command.Connection = connection;
                command.CommandText = "AltaVersionInterna";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@idInternalVersionDef", idInternalVersion);
                command.Parameters["@idInternalVersionDef"].Direction = ParameterDirection.Input;

                command.Parameters.AddWithValue("@ehw1", releaseDefinition.ElectronicHardware.Value1.Equals("") ? null : releaseDefinition.ElectronicHardware.Value1);
                command.Parameters["@ehw1"].Direction = ParameterDirection.Input;
                command.Parameters.AddWithValue("@ehw2", releaseDefinition.ElectronicHardware.Value2.Equals("") ? null : releaseDefinition.ElectronicHardware.Value2);
                command.Parameters["@ehw2"].Direction = ParameterDirection.Input;
                command.Parameters.AddWithValue("@ehw3", releaseDefinition.ElectronicHardware.Value3.Equals("") ? null : releaseDefinition.ElectronicHardware.Value3);
                command.Parameters["@ehw3"].Direction = ParameterDirection.Input;
                command.Parameters.AddWithValue("@ehw4", releaseDefinition.ElectronicHardware.Value4.Equals("") ? null : releaseDefinition.ElectronicHardware.Value4);
                command.Parameters["@ehw4"].Direction = ParameterDirection.Input;
                command.Parameters.AddWithValue("@ehw5", releaseDefinition.ElectronicHardware.Value5.Equals("") ? null : releaseDefinition.ElectronicHardware.Value5);
                command.Parameters["@ehw5"].Direction = ParameterDirection.Input;
                command.Parameters.AddWithValue("@ehw6", releaseDefinition.ElectronicHardware.Value6.Equals("") ? null : releaseDefinition.ElectronicHardware.Value6);
                command.Parameters["@ehw6"].Direction = ParameterDirection.Input;
                command.Parameters.AddWithValue("@ehw7", releaseDefinition.ElectronicHardware.Value7.Equals("") ? null : releaseDefinition.ElectronicHardware.Value7);
                command.Parameters["@ehw7"].Direction = ParameterDirection.Input;
                command.Parameters.AddWithValue("@ehw8", releaseDefinition.ElectronicHardware.Value8.Equals("") ? null : releaseDefinition.ElectronicHardware.Value8);
                command.Parameters["@ehw8"].Direction = ParameterDirection.Input;

                command.Parameters.AddWithValue("@plataforma", releaseDefinition.ProjectName);
                command.Parameters["@plataforma"].Direction = ParameterDirection.Input;

                command.Parameters.AddWithValue("@windchill", mechanicalBlockEmpty ? null : releaseDefinition.Windchill);
                command.Parameters["@windchill"].Direction = ParameterDirection.Input;
                command.Parameters.AddWithValue("@lb", releaseDefinition.Lb);
                command.Parameters["@lb"].Direction = ParameterDirection.Input;

                command.Parameters.AddWithValue("@hw_snr", softwareBlockEmpty ? null : releaseDefinition.Software.Hw_snr);
                command.Parameters["@hw_snr"].Direction = ParameterDirection.Input;
                command.Parameters.AddWithValue("@hw_status", releaseDefinition.Software.HwStatus);
                command.Parameters["@hw_status"].Direction = ParameterDirection.Input;
                command.Parameters.AddWithValue("@sw_snr", releaseDefinition.Software.Sw_snr);
                command.Parameters["@sw_snr"].Direction = ParameterDirection.Input;
                command.Parameters.AddWithValue("@sw_status", releaseDefinition.Software.SwStatus);
                command.Parameters["@sw_status"].Direction = ParameterDirection.Input;
                command.Parameters.AddWithValue("@releaseTest", releaseDefinition.Software.ReleaseTest);
                command.Parameters["@releaseTest"].Direction = ParameterDirection.Input;
                command.Parameters.AddWithValue("@releaseFinal", releaseDefinition.Software.ReleaseFinal);
                command.Parameters["@releaseFinal"].Direction = ParameterDirection.Input;

                command.Parameters.AddWithValue("@processName", processBlockEmpty ? null : releaseDefinition.Process);
                command.Parameters["@processName"].Direction = ParameterDirection.Input;
                command.Parameters.AddWithValue("@trazability", releaseDefinition.Trazability ? 1 : 0);
                command.Parameters["@trazability"].Direction = ParameterDirection.Input;

                command.Parameters.AddWithValue("@snrValue", softwareBlockEmpty ? null : releaseDefinition.Labeling.Snr);
                command.Parameters["@snrValue"].Direction = ParameterDirection.Input;
                command.Parameters.AddWithValue("@zgsValue", releaseDefinition.Labeling.Zgs);
                command.Parameters["@zgsValue"].Direction = ParameterDirection.Input;
                command.Parameters.AddWithValue("@e_q", releaseDefinition.Labeling.Eq);
                command.Parameters["@e_q"].Direction = ParameterDirection.Input;
                command.Parameters.AddWithValue("@luStatus", releaseDefinition.Labeling.LuStatus);
                command.Parameters["@luStatus"].Direction = ParameterDirection.Input;
                command.Parameters.AddWithValue("@prodLoc", releaseDefinition.Labeling.ProdLoc);
                command.Parameters["@prodLoc"].Direction = ParameterDirection.Input;
                command.Parameters.AddWithValue("@daiPlant", releaseDefinition.Labeling.DaiPlant);
                command.Parameters["@daiPlant"].Direction = ParameterDirection.Input;
                command.Parameters.AddWithValue("@br_bl", releaseDefinition.Labeling.Brbl);
                command.Parameters["@br_bl"].Direction = ParameterDirection.Input;
                command.Parameters.AddWithValue("@lftNo", releaseDefinition.Labeling.Lft);
                command.Parameters["@lftNo"].Direction = ParameterDirection.Input;
                command.Parameters.AddWithValue("@p_n", releaseDefinition.Labeling.PnMahle);
                command.Parameters["@p_n"].Direction = ParameterDirection.Input;

                command.Parameters.AddWithValue("@test1", releaseDefinition.Testing.Key1.Equals("") ? null : releaseDefinition.Testing.Key1);
                command.Parameters["@test1"].Direction = ParameterDirection.Input;
                command.Parameters.AddWithValue("@test2", releaseDefinition.Testing.Key2.Equals("") ? null : releaseDefinition.Testing.Key2);
                command.Parameters["@test2"].Direction = ParameterDirection.Input;
                command.Parameters.AddWithValue("@test3", releaseDefinition.Testing.Key3.Equals("") ? null : releaseDefinition.Testing.Key3);
                command.Parameters["@test3"].Direction = ParameterDirection.Input;
                command.Parameters.AddWithValue("@test4", releaseDefinition.Testing.Key4.Equals("") ? null : releaseDefinition.Testing.Key4);
                command.Parameters["@test4"].Direction = ParameterDirection.Input;
                command.Parameters.AddWithValue("@test5", releaseDefinition.Testing.Key5.Equals("") ? null : releaseDefinition.Testing.Key5);
                command.Parameters["@test5"].Direction = ParameterDirection.Input;
                command.Parameters.AddWithValue("@test6", releaseDefinition.Testing.Key6.Equals("") ? null : releaseDefinition.Testing.Key6);
                command.Parameters["@test6"].Direction = ParameterDirection.Input;
                command.Parameters.AddWithValue("@test7", releaseDefinition.Testing.Key7.Equals("") ? null : releaseDefinition.Testing.Key7);
                command.Parameters["@test7"].Direction = ParameterDirection.Input;
                command.Parameters.AddWithValue("@test8", releaseDefinition.Testing.Key8.Equals("") ? null : releaseDefinition.Testing.Key8);
                command.Parameters["@test8"].Direction = ParameterDirection.Input;

                command.ExecuteNonQuery();

                MySqlDataReader Read_data = command.ExecuteReader();
                ReadDataFromStoredProcess(Read_data, out _resul);
                
                Read_data.Close();
                foreach (var data in _resul)
                {
                    if (data.Contains(ColumSpliter))
                    {
                        err_msg += data.Split(new string[] { ColumSpliter }, StringSplitOptions.None)[2];
                        err_msg += " ";
                    } else
                    {
                        err_msg += data;
                    }

                }
            }
            catch (Exception ex)
            {
                err_msg = ex.Message;
                // newInternalVersion = -1;
                if ((DDBB_MySQL.connection.State != ConnectionState.Open || DDBB_MySQL.connection.State != ConnectionState.Executing) && firstTime)
                {
                    if (ResetConecctionDDBB())
                    {
                        InsertInternallVersionDefinition(idInternalVersion, releaseDefinition, out err_msg, false);
                    }
                }
                else
                {
                    DDBB_MySQL.CloseDDBB();
                }
            }*/
        }

        public static bool GetIVList(int idPlataform, out ObservableCollection<string> internalVersions, out string err_msg, bool firstTime = true)
        {
            bool status = false;
            List<string> _resul = new List<string>();
            err_msg = "";
            internalVersions = new ObservableCollection<string>();

            try
            {
                MySqlCommand command = new MySqlCommand();
                command.Connection = connection;
                command.CommandText = "SELECT Internal_Version FROM InternalVersionsPlataformEnable " +
                                        "WHERE idPlataforma = @idPlataforma AND Enable = 1 " +
                                        "ORDER BY Fecha DESC;";
                command.Parameters.AddWithValue("@idPlataforma", idPlataform);
                command.ExecuteNonQuery();

                MySqlDataReader Read_data = command.ExecuteReader();
                status = ReadDataFromStoredProcess(Read_data, out _resul);
                Read_data.Close();

                foreach (var data in _resul)
                {
                    internalVersions.Add(data);
                }

            }
            catch (Exception ex)
            {
                err_msg = ex.Message;
                if ((DDBB_MySQL.connection.State != ConnectionState.Open || DDBB_MySQL.connection.State != ConnectionState.Executing) && firstTime)
                {
                    if (ResetConecctionDDBB())
                    {
                        return GetIVList(idPlataform, out internalVersions, out err_msg, false);
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    DDBB_MySQL.CloseDDBB();
                }
            }

            return status;
        }

        public static bool Insert_OF(int idPlataform, string ofDesc, string ivDef, int ofQuantity, out string err_msg, bool firstTime = true)
        {
            bool status = false;
            List<string> _resul = new List<string>();
            err_msg = "";

            try
            {
                MySqlCommand command = new MySqlCommand();
                command.Connection = connection;
                command.CommandText = "InsertarOF";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@ofDesc", ofDesc);
                command.Parameters["@ofDesc"].Direction = ParameterDirection.Input;

                command.Parameters.AddWithValue("@internalVersionDef", ivDef);
                command.Parameters["@internalVersionDef"].Direction = ParameterDirection.Input;

                command.Parameters.AddWithValue("@ofQuantity", ofQuantity);
                command.Parameters["@ofQuantity"].Direction = ParameterDirection.Input;

                MySqlDataReader Read_data = command.ExecuteReader();
                status = ReadDataFromStoredProcess(Read_data, out _resul);
                Read_data.Close();

                foreach (var data in _resul)
                {
                    if (data.Contains(ColumSpliter))
                    {
                        err_msg += data.Split(new string[] { ColumSpliter }, StringSplitOptions.None)[2];
                        err_msg += " ";
                    }
                    else
                    {
                        err_msg += data;
                    }
                    status = false;
                }

            }
            catch (Exception ex)
            {
                err_msg = ex.Message;
                if ((DDBB_MySQL.connection.State != ConnectionState.Open || DDBB_MySQL.connection.State != ConnectionState.Executing) && firstTime)
                {
                    if (ResetConecctionDDBB())
                    {
                        return Insert_OF(idPlataform, ofDesc, ivDef, ofQuantity, out err_msg, false);
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    DDBB_MySQL.CloseDDBB();
                }
            }

            return status;
        }
        public static bool GetSAList(out ObservableCollection<string> internalVersions, out string err_msg, bool firstTime = true)
        {
            bool status = false;
            List<string> _resul = new List<string>();
            err_msg = "";
            internalVersions = new ObservableCollection<string>();

            try
            {
                MySqlCommand command = new MySqlCommand();
                command.Connection = connection;
                command.CommandText = "SELECT Code_Referencia_SubAssembly FROM SubAssembly_Definition " +
                                        "ORDER BY Date DESC;";
                command.ExecuteNonQuery();

                MySqlDataReader Read_data = command.ExecuteReader();
                status = ReadDataFromStoredProcess(Read_data, out _resul);
                Read_data.Close();

                foreach (var data in _resul)
                {
                    internalVersions.Add(data);
                }

            }
            catch (Exception ex)
            {
                err_msg = ex.Message;
                if ((DDBB_MySQL.connection.State != ConnectionState.Open || DDBB_MySQL.connection.State != ConnectionState.Executing) && firstTime)
                {
                    if (ResetConecctionDDBB())
                    {
                        return GetSAList(out internalVersions, out err_msg, false);
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    DDBB_MySQL.CloseDDBB();
                }
            }

            return status;
        }

        public static bool Insert_SA(int idPlataform, string ofDesc, string code, int ofQuantity, out string err_msg, bool firstTime = true)
        {
            bool status = false;
            List<string> _resul = new List<string>();
            err_msg = "";

            try
            {
                MySqlCommand command = new MySqlCommand();
                command.Connection = connection;
                command.CommandText = "InsertarSA";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@idPlataform", idPlataform);
                command.Parameters["@idPlataform"].Direction = ParameterDirection.Input;

                command.Parameters.AddWithValue("@ofDesc", ofDesc);
                command.Parameters["@ofDesc"].Direction = ParameterDirection.Input;

                command.Parameters.AddWithValue("@subAssembCode", code);
                command.Parameters["@subAssembCode"].Direction = ParameterDirection.Input;

                command.Parameters.AddWithValue("@ofQuantity", ofQuantity);
                command.Parameters["@ofQuantity"].Direction = ParameterDirection.Input;

                MySqlDataReader Read_data = command.ExecuteReader();
                status = ReadDataFromStoredProcess(Read_data, out _resul);
                Read_data.Close();

                foreach (var data in _resul)
                {
                    if (data.Contains(ColumSpliter))
                    {
                        err_msg += data.Split(new string[] { ColumSpliter }, StringSplitOptions.None)[2];
                        err_msg += " ";
                    }
                    else
                    {
                        err_msg += data;
                    }
                    status = false;
                }

            }
            catch (Exception ex)
            {
                err_msg = ex.Message;
                if ((DDBB_MySQL.connection.State != ConnectionState.Open || DDBB_MySQL.connection.State != ConnectionState.Executing) && firstTime)
                {
                    if (ResetConecctionDDBB())
                    {
                        return Insert_SA(idPlataform, ofDesc, code, ofQuantity, out err_msg, false);
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    DDBB_MySQL.CloseDDBB();
                }
            }

            return status;
        }

        public static bool Insert_Manufacturing_Process(string in_Version_Manuf, int in_num, int in_process, string in_ProcessDescrip, int in_Operation, string in_Picture, string in_OperationDescrip, string in_proceso, string in_Test, string in_Input, string in_Screw, string in_AUXpcb, out string err_msg, bool firstTime = true)
        {
            bool status = false;
            List<string> _resul = new List<string>();
            err_msg = "";

            try
            {
                MySqlCommand command = new MySqlCommand();
                command.Connection = connection;
                command.CommandText = "Insert_Manufacturing_Process";
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@in_Version_Manuf", in_Version_Manuf);
                command.Parameters["@in_Version_Manuf"].Direction = ParameterDirection.Input;

                command.Parameters.AddWithValue("@inNum", in_num);
                command.Parameters["@inNum"].Direction = ParameterDirection.Input;

                command.Parameters.AddWithValue("@inProcess", in_process);
                command.Parameters["@inProcess"].Direction = ParameterDirection.Input;

                command.Parameters.AddWithValue("@in_Process_Descrip", in_ProcessDescrip);
                command.Parameters["@in_Process_Descrip"].Direction = ParameterDirection.Input;

                command.Parameters.AddWithValue("@in_Operation", in_Operation);
                command.Parameters["@in_Operation"].Direction = ParameterDirection.Input;

                command.Parameters.AddWithValue("@in_Picture", in_Picture);
                command.Parameters["@in_Picture"].Direction = ParameterDirection.Input;

                command.Parameters.AddWithValue("@in_Operation_Descript", in_OperationDescrip);
                command.Parameters["@in_Operation_Descript"].Direction = ParameterDirection.Input;

                command.Parameters.AddWithValue("@in_NomProc", in_proceso);
                command.Parameters["@in_NomProc"].Direction = ParameterDirection.Input;

                command.Parameters.AddWithValue("@in_Test", in_Test);
                command.Parameters["@in_Test"].Direction = ParameterDirection.Input;

                command.Parameters.AddWithValue("@in_input", in_Input);
                command.Parameters["@in_input"].Direction = ParameterDirection.Input;

                command.Parameters.AddWithValue("@in_Descrew", in_Screw);
                command.Parameters["@in_Descrew"].Direction = ParameterDirection.Input;

                command.Parameters.AddWithValue("@in_AUXPCB", in_AUXpcb);
                command.Parameters["@in_AUXPCB"].Direction = ParameterDirection.Input;

                MySqlDataReader Read_data = command.ExecuteReader();
                status = ReadDataFromStoredProcess(Read_data, out _resul);
                Read_data.Close();

                foreach (var data in _resul)
                {
                    if (data.Contains(ColumSpliter))
                    {
                        err_msg += data.Split(new string[] { ColumSpliter }, StringSplitOptions.None)[2];
                        err_msg += " ";
                    }
                    else
                    {
                        err_msg += data;
                    }
                }
                if (err_msg == String.Empty)
                {
                    status = true;
                }
            }
            catch (Exception ex)
            {
                err_msg = ex.Message;
                if ((DDBB_MySQL.connection.State != ConnectionState.Open || DDBB_MySQL.connection.State != ConnectionState.Executing) && firstTime)
                {
                    if (ResetConecctionDDBB())
                    {
                        status = Insert_Manufacturing_Process(in_Version_Manuf, in_num, in_process, in_ProcessDescrip, in_Operation, in_Picture, in_OperationDescrip, in_proceso, in_Test, in_Input, in_Screw, in_AUXpcb, out err_msg, false);

                        return status;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    DDBB_MySQL.CloseDDBB();
                }
            }
            return status;
        }

        public static void InsertSubassemblyDefinition(SubassemblyDefinition saDefinition, out string err_msg, bool firstTime = true)
        {
            // int newInternalVersion;
            List<string> _resul = new List<string>();
            err_msg = "";

            try
            {
                MySqlCommand command = new MySqlCommand();
                command.Connection = connection;
                command.CommandText = "Insert_SubAssembly";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@plataforma", saDefinition.Plataform);
                command.Parameters["@plataforma"].Direction = ParameterDirection.Input;
                command.Parameters.AddWithValue("@ref", saDefinition.Reference + "_Ed" + saDefinition.Version);
                command.Parameters["@ref"].Direction = ParameterDirection.Input;
                command.Parameters.AddWithValue("@descriptionRef", saDefinition.DescriptionRef);
                command.Parameters["@descriptionRef"].Direction = ParameterDirection.Input;
                command.Parameters.AddWithValue("@registerDateString", saDefinition.Date);
                command.Parameters["@registerDateString"].Direction = ParameterDirection.Input;

                command.Parameters.AddWithValue("@ehw1", saDefinition.ElectronicHardware.Value1.Equals("") ? null : saDefinition.ElectronicHardware.Value1);
                command.Parameters["@ehw1"].Direction = ParameterDirection.Input;
                command.Parameters.AddWithValue("@ehw2", saDefinition.ElectronicHardware.Value2.Equals("") ? null : saDefinition.ElectronicHardware.Value2);
                command.Parameters["@ehw2"].Direction = ParameterDirection.Input;
                command.Parameters.AddWithValue("@ehw3", saDefinition.ElectronicHardware.Value3.Equals("") ? null : saDefinition.ElectronicHardware.Value3);
                command.Parameters["@ehw3"].Direction = ParameterDirection.Input;
                command.Parameters.AddWithValue("@ehw4", saDefinition.ElectronicHardware.Value4.Equals("") ? null : saDefinition.ElectronicHardware.Value4);
                command.Parameters["@ehw4"].Direction = ParameterDirection.Input;
                command.Parameters.AddWithValue("@ehw5", saDefinition.ElectronicHardware.Value5.Equals("") ? null : saDefinition.ElectronicHardware.Value5);
                command.Parameters["@ehw5"].Direction = ParameterDirection.Input;
                command.Parameters.AddWithValue("@ehw6", saDefinition.ElectronicHardware.Value6.Equals("") ? null : saDefinition.ElectronicHardware.Value6);
                command.Parameters["@ehw6"].Direction = ParameterDirection.Input;
                command.Parameters.AddWithValue("@ehw7", saDefinition.ElectronicHardware.Value7.Equals("") ? null : saDefinition.ElectronicHardware.Value7);
                command.Parameters["@ehw7"].Direction = ParameterDirection.Input;
                command.Parameters.AddWithValue("@ehw8", saDefinition.ElectronicHardware.Value8.Equals("") ? null : saDefinition.ElectronicHardware.Value8);
                command.Parameters["@ehw8"].Direction = ParameterDirection.Input;

                command.Parameters.AddWithValue("@processName", saDefinition.Process);
                command.Parameters["@processName"].Direction = ParameterDirection.Input;
                command.Parameters.AddWithValue("@trazability", saDefinition.Trazability ? 1 : 0);
                command.Parameters["@trazability"].Direction = ParameterDirection.Input;

                command.Parameters.AddWithValue("@imageName", saDefinition.ImageName);
                command.Parameters["@imageName"].Direction = ParameterDirection.Input;
                command.Parameters.AddWithValue("@picture", saDefinition.ImagenData);
                command.Parameters["@picture"].Direction = ParameterDirection.Input;

                MySqlDataReader Read_data = command.ExecuteReader();
                ReadDataFromStoredProcess(Read_data, out _resul);

                Read_data.Close();
                foreach (var data in _resul)
                {
                    if (data.Contains(ColumSpliter))
                    {
                        err_msg += data.Split(new string[] { ColumSpliter }, StringSplitOptions.None)[2];
                        err_msg += " ";
                    }
                    else
                    {
                        err_msg += data;
                    }

                }
            }
            catch (Exception ex)
            {
                err_msg = ex.Message;
                if ((DDBB_MySQL.connection.State != ConnectionState.Open || DDBB_MySQL.connection.State != ConnectionState.Executing) && firstTime)
                {
                    if (ResetConecctionDDBB())
                    {
                        InsertSubassemblyDefinition(saDefinition, out err_msg, false);
                    }
                }
                else
                {
                    DDBB_MySQL.CloseDDBB();
                }
            }

        }

        public static bool UpPictureFromFolder(string archivo, out string err_msg, bool firstTime = true)
        {
            bool status = false;
            err_msg = "";
            String[] splited;
            String renamed;

            try
            {
                MySqlCommand command = new MySqlCommand();
                command.Connection = connection;

                string nombreArchivo = Path.GetFileName(archivo);

                splited = nombreArchivo.Split('.');
                renamed = nombreArchivo.Replace($".{splited.Last()}", "");
                if (renamed.Equals(""))
                {
                    renamed = nombreArchivo;
                }
                byte[] imagenData = File.ReadAllBytes(archivo);
                command.CommandText = "INSERT INTO Pictures_Manufacturing_Process (Picture_Description,Picture_JPG) values (@nombre, @imagen)";
                command.Parameters.AddWithValue("@nombre", renamed);
                command.Parameters.AddWithValue("@imagen", imagenData);
                command.ExecuteNonQuery();
                command.Parameters.Clear();
                status = true;
            }
            catch (Exception ex)
            {
                err_msg = ex.Message;
                if ((DDBB_MySQL.connection.State != ConnectionState.Open || DDBB_MySQL.connection.State != ConnectionState.Executing) && firstTime)
                {
                    if (ResetConecctionDDBB())
                    {
                        return UpPictureFromFolder(archivo, out err_msg, false);
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    DDBB_MySQL.CloseDDBB();
                }
            }
            return status;
        }

        public static List<string> GetPictureList(out string err_msg, bool firstTime = true)
        {
            List<String> _result = new List<String>();
            err_msg = "";

            try
            {
                MySqlCommand command = new MySqlCommand();
                command.Connection = connection;
                command.CommandText = "SELECT Picture_Description FROM Pictures_Manufacturing_Process;";
                command.ExecuteNonQuery();

                MySqlDataReader Read_data = command.ExecuteReader();
                ReadDataFromStoredProcess(Read_data, out _result);
                command.Parameters.Clear();
                Read_data.Close();
            }
            catch (Exception ex)
            {
                err_msg = ex.Message;
                if ((DDBB_MySQL.connection.State != ConnectionState.Open || DDBB_MySQL.connection.State != ConnectionState.Executing) && firstTime)
                {
                    if (ResetConecctionDDBB())
                    {
                        return GetPictureList(out err_msg, false);
                    }
                    else
                    {
                        return new List<String>(); ;
                    }
                }
                else
                {
                    DDBB_MySQL.CloseDDBB();
                }
            }
            return _result;
        }

        /// ////////////////////////////////////////////////////
        //Metodos Anteriores
        /// ////////////////////////////////////////////////////
        /// 
        #region metodos anteriores
        public static bool InsetTestStepSQL(string Proceso_name, string Platform_name,string PN_name, string test_name, int OrdenEjecucion, out int idTestStep)
        {
            bool status = false;
            List<string> resul = new List<string>();
            try
            {

                MySqlCommand command = new MySqlCommand();
                command.Connection = connection;
                command.CommandText = "InsertTestStep";
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Proceso_name", Proceso_name);
                command.Parameters["@Proceso_name"].Direction = ParameterDirection.Input;

                command.Parameters.AddWithValue("@Platform_name", Platform_name);
                command.Parameters["@Platform_name"].Direction = ParameterDirection.Input;

                command.Parameters.AddWithValue("@PN_name", PN_name);
                command.Parameters["@PN_name"].Direction = ParameterDirection.Input;

                command.Parameters.AddWithValue("@test_name", test_name);
                command.Parameters["@test_name"].Direction = ParameterDirection.Input;

                command.Parameters.AddWithValue("@OrdenEjecucion", OrdenEjecucion);
                command.Parameters["@OrdenEjecucion"].Direction = ParameterDirection.Input;

                MySqlDataReader Read_data = command.ExecuteReader();
                status = ReadDataFromStoredProcess(Read_data, out resul);
                idTestStep = int.Parse(resul[0]);
                Read_data.Close();
            }
            catch (Exception ex)
            {
                status = false;
                idTestStep = -1;
            }
            
            return status;
        }
        public static bool GetTestStepSQL(string Process_name, string Plataform_name, string PN_name, out List<string> out_rows)
        {
            bool status = false;
            List<string> resul = new List<string>();
            try
            {
                

                MySqlCommand command = new MySqlCommand();
                command.Connection = connection;
                command.CommandText = "GetTestStep";
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Process_name", Process_name);
                command.Parameters["@Process_name"].Direction = ParameterDirection.Input;

                command.Parameters.AddWithValue("@Plataform_name", Plataform_name);
                command.Parameters["@Plataform_name"].Direction = ParameterDirection.Input;

                command.Parameters.AddWithValue("@PN_name", PN_name);
                command.Parameters["@PN_name"].Direction = ParameterDirection.Input;


                MySqlDataReader Read_data = command.ExecuteReader();
                status = ReadDataFromStoredProcess(Read_data, out resul);
                Read_data.Close();

            }
            catch (Exception ex)
            {
                status = false;

            }
            out_rows = resul;
            
            return status;
        }
        public static bool GetTestSQL(int _idTest_Step,string PN_name, out List<string> out_rows)
        {
            bool status = false;
            List<string> resul = new List<string>();
            try
            {

                

                MySqlCommand command = new MySqlCommand();
                command.Connection = connection;
                command.CommandText = "GetTestandLimits";
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@_idTest_Step", _idTest_Step);
                command.Parameters["@_idTest_Step"].Direction = ParameterDirection.Input;


                command.Parameters.AddWithValue("@PN_name", PN_name);
                command.Parameters["@PN_name"].Direction = ParameterDirection.Input;


                MySqlDataReader Read_data = command.ExecuteReader();
                status = ReadDataFromStoredProcess(Read_data, out resul);
                Read_data.Close();

            }
            catch (Exception ex)
            {
                status = false;

            }
            out_rows = resul;
            
            return status;
        }
        public static bool ObetenerModeloyMuestraSQL(string OF, out List<string> rows_data)
        {
            bool status = false;
            List<string> resul = new List<string>();
            try
            {

                

                MySqlCommand command = new MySqlCommand();
                command.Connection = connection;
                command.CommandText = "ObtenerModeloyMuestra";
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@_OF", OF);
                command.Parameters["@_OF"].Direction = ParameterDirection.Input;
                
                MySqlDataReader Read_data = command.ExecuteReader();
                status = ReadDataFromStoredProcess(Read_data, out resul);
                Read_data.Dispose();
                Read_data.Close();


            }
            catch (Exception ex)
            {
                status = false;
            }
            rows_data = resul;
            return status;
        }
        public static bool InsetTestSQL(int Test_id, string TEST)
        {
            bool status = false;
            List<string> resul = new List<string>();
            try
            {

                MySqlCommand command = new MySqlCommand();
                command.Connection = connection;
                command.CommandText = "InsertarTestyLimites";
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Test_id", Test_id);
                command.Parameters["@Test_id"].Direction = ParameterDirection.Input;

                command.Parameters.AddWithValue("@TEST", TEST);
                command.Parameters["@TEST"].Direction = ParameterDirection.Input;
                command.ExecuteNonQuery();
                
            }
            catch (Exception ex)
            {
                status = false;
            }
            
            return status;
        }
        public static bool InsetRegistroProcesoSQL(string Hostname, string Process_name, string in_PN_name, string in_SN, string in_AppVersion, string in_Resultado, DateTime in_FECHA, out int idRegistro)
        {
            bool status = false;
            List<string> resul = new List<string>();
            try
            {
                

                MySqlCommand command = new MySqlCommand();
                command.Connection = connection;
                command.CommandText = "InsertarRegistroProceso";
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Hostname", Hostname);
                command.Parameters["@Hostname"].Direction = ParameterDirection.Input;

                command.Parameters.AddWithValue("@Process_name", Process_name);
                command.Parameters["@Process_name"].Direction = ParameterDirection.Input;

                command.Parameters.AddWithValue("@in_PN_name", in_PN_name);
                command.Parameters["@in_PN_name"].Direction = ParameterDirection.Input;

                command.Parameters.AddWithValue("@in_SN", in_SN);
                command.Parameters["@in_SN"].Direction = ParameterDirection.Input;

                command.Parameters.AddWithValue("@in_AppVersion", in_AppVersion);
                command.Parameters["@in_AppVersion"].Direction = ParameterDirection.Input;

                command.Parameters.AddWithValue("@in_Resultado", in_Resultado);
                command.Parameters["@in_Resultado"].Direction = ParameterDirection.Input;

                command.Parameters.AddWithValue("@in_FECHA", in_FECHA);
                command.Parameters["@in_FECHA"].Direction = ParameterDirection.Input;
                MySqlDataReader Read_data = command.ExecuteReader();
                status = ReadDataFromStoredProcess(Read_data, out resul);
                idRegistro = int.Parse(resul[0]);
                Read_data.Close();
            }
            catch (Exception ex)
            {
                status = false;
                idRegistro = -1;
            }
            
            
            return status;
        }
        public static bool CrearNuevaRegistroAssembly(string in_PN,string in_SN_Housing, out int idRegistro)
        {
            bool status = true;
            int _idRegistro = -1;
            try
            {
                MySqlCommand command = new MySqlCommand();
                command.Connection = connection;
                command.CommandText = "CrearNuevaRegistroAssembly";
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@in_PN", in_PN);
                command.Parameters["@in_PN"].Direction = ParameterDirection.Input;

                command.Parameters.AddWithValue("@in_SN_Housing", in_SN_Housing);
                command.Parameters["@in_SN_Housing"].Direction = ParameterDirection.Input;

                command.Parameters.AddWithValue("@idRegistro", _idRegistro);
                command.Parameters["@idRegistro"].Direction = ParameterDirection.Output;

                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                status = false;
            }
            idRegistro = _idRegistro;
            return status;
        }

        public static bool InsetEstadisticaSQL(int in_idRegistro, int in_idLimites, double in_Medida, string in_Resultado,DateTime in_FECHA)
        {
            bool status = false;
            List<string> resul = new List<string>();
            try
            {
                

                MySqlCommand command = new MySqlCommand();
                command.Connection = connection;
                command.CommandText = "InsertarEstadistica";
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@in_idRegistro", in_idRegistro);
                command.Parameters["@in_idRegistro"].Direction = ParameterDirection.Input;

                command.Parameters.AddWithValue("@in_idLimites", in_idLimites);
                command.Parameters["@in_idLimites"].Direction = ParameterDirection.Input;

                command.Parameters.AddWithValue("@in_Medida", in_Medida);
                command.Parameters["@in_Medida"].Direction = ParameterDirection.Input;

                command.Parameters.AddWithValue("@in_Resultado", in_Resultado);
                command.Parameters["@in_Resultado"].Direction = ParameterDirection.Input;


                command.Parameters.AddWithValue("@in_FECHA", in_FECHA);
                command.Parameters["@in_FECHA"].Direction = ParameterDirection.Input;

                command.ExecuteNonQuery();
                status = true;

            }
            catch (Exception ex)
            {
                status = false;
            }

            
            return status;
        }
        
        public static bool Get_Status_SN(string in_SN_Housing, string in_Ref, out List<string> rows_data, out string err_msg)
        {
            bool status = true;
            List<string> _resul = new List<string>();
            err_msg = "";

            try
            {
                MySqlCommand command = new MySqlCommand();
                command.Connection = connection;
                command.CommandText = "Get_Status_SN";
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@SerialN", in_SN_Housing);
                command.Parameters["@SerialN"].Direction = ParameterDirection.Input;

                command.Parameters.AddWithValue("@Ref", in_Ref);
                command.Parameters["@Ref"].Direction = ParameterDirection.Input;

                command.ExecuteNonQuery();

                MySqlDataReader Read_data = command.ExecuteReader();
                status = ReadDataFromStoredProcess(Read_data, out _resul);
                Read_data.Close();
            }
            catch (Exception ex)
            {
                err_msg = ex.Message;
                status = false;
                _resul.Add("3");
            }
            rows_data = _resul;
            return status;
        }
        public static bool GetHistory_SN_Housing(string in_SN_Housing, out List<string> rows_data, out string err_msg)
        {
            bool status = true;
            List<string> _resul = new List<string>();
            err_msg = "";

            try
            {
                MySqlCommand command = new MySqlCommand();
                command.Connection = connection;
                command.CommandText = "GetHistory_SN_Housing";
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@SerialN", in_SN_Housing);
                command.Parameters["@SerialN"].Direction = ParameterDirection.Input;
                
                command.ExecuteNonQuery();

                MySqlDataReader Read_data = command.ExecuteReader();
                status = ReadDataFromStoredProcess(Read_data, out _resul);
                Read_data.Close();
            }
            catch (Exception ex)
            {
                err_msg = ex.Message;
                status = false;
                _resul.Add("3");
            }
            rows_data = _resul;
            return status;
        }
        public static bool Get_Process( out List<string> rows_data, out string err_msg)
        {
            bool status = true;
            List<string> _resul = new List<string>();
            err_msg = "";

            try
            {
                MySqlCommand command = new MySqlCommand();
                command.Connection = connection;
                command.CommandText = "Get_Process";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.ExecuteNonQuery();

                MySqlDataReader Read_data = command.ExecuteReader();
                status = ReadDataFromStoredProcess(Read_data, out _resul);
                Read_data.Close();
            }
            catch (Exception ex)
            {
                err_msg = ex.Message;
                status = false;
                _resul.Add("3");
            }
            rows_data = _resul;
            return status;
        }
        public static bool Get_References_PCBs(out List<string> rows_data, out string err_msg)
        {
            bool status = true;
            List<string> _resul = new List<string>();
            err_msg = "";

            try
            {
                MySqlCommand command = new MySqlCommand();
                command.Connection = connection;
                command.CommandText = "Get_References_PCBs";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.ExecuteNonQuery();

                MySqlDataReader Read_data = command.ExecuteReader();
                status = ReadDataFromStoredProcess(Read_data, out _resul);
                Read_data.Close();
            }
            catch (Exception ex)
            {
                err_msg = ex.Message;
                status = false;
                _resul.Add("3");
            }
            rows_data = _resul;
            return status;
        }
    
        public static bool Check_Usuario_Manuf( string in_code_usuario, out List<string> rows_data, out string err_msg)
        {
            bool status = true;
            err_msg = "";
            List<string> _resul = new List<string>();

            try
            {
                MySqlCommand command = new MySqlCommand();
                command.Connection = connection;
                command.CommandText = "Check_Usuario_Manuf";
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Codigo", in_code_usuario);
                command.Parameters["@Codigo"].Direction = ParameterDirection.Input;
                command.ExecuteNonQuery();

                MySqlDataReader Read_data = command.ExecuteReader();
                status = ReadDataFromStoredProcess(Read_data, out _resul);
                Read_data.Close();
            }
            catch (Exception ex)
            {
                err_msg = ex.Message;
                status = false;
                _resul.Add("3");
            }
            rows_data = _resul;
            return status;
        }
        public static bool Get_Last_Num_Manufacturing(string in_SN_Housing, out List<string> rows_data, out string err_msg)

        {
            bool status = false;
            List<string> _resul = new List<string>();
            err_msg = "";

            try
            {
                MySqlCommand command = new MySqlCommand();
                command.Connection = connection;
                command.CommandText = "Get_Last_Num_Manufacturing";
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@SerialN", in_SN_Housing);
                command.Parameters["@SerialN"].Direction = ParameterDirection.Input;

                command.ExecuteNonQuery();

                MySqlDataReader Read_data = command.ExecuteReader();
                status = ReadDataFromStoredProcess(Read_data, out _resul);
                Read_data.Close();
            }
            catch (Exception ex)
            {
                err_msg = ex.Message;
                status = false;
                _resul.Add("3");
            }
            rows_data = _resul;
            return status;
        }
       
        public static bool Get_Nombre_VerProcMan(string in_Muestra_Interna, out List<string> rows_data, out string err_msg)
        {
            bool status = false;
            List<string> _resul = new List<string>();
            err_msg = "";

            try
            {
                MySqlCommand command = new MySqlCommand();
                command.Connection = connection;
                command.CommandText = "Get_Nombre_VerProcMan";
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Nom", in_Muestra_Interna);
                command.Parameters["@Nom"].Direction = ParameterDirection.Input;

                command.ExecuteNonQuery();

                MySqlDataReader Read_data = command.ExecuteReader();
                status = ReadDataFromStoredProcess(Read_data, out _resul);
                Read_data.Close();
            }
            catch (Exception ex)
            {
                err_msg = ex.Message;
                status = false;
                _resul.Add("3");
            }
            rows_data = _resul;
            return status;
        }
        public static bool Get_IV_SN(string in_SN_Housing, out List<string> rows_data, out string err_msg)
        {
            bool status = false;
            List<string> _resul = new List<string>();
            err_msg = "";

            try
            {
                MySqlCommand command = new MySqlCommand();
                command.Connection = connection;
                command.CommandText = "Get_IV_SN";
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@SerialN", in_SN_Housing);
                command.Parameters["@SerialN"].Direction = ParameterDirection.Input;
                command.ExecuteNonQuery();

                MySqlDataReader Read_data = command.ExecuteReader();
                status = ReadDataFromStoredProcess(Read_data, out _resul);
                Read_data.Close();
            }
            catch (Exception ex)
            {
                err_msg = ex.Message;
                status = false;
                _resul.Add("3");
            }
            rows_data = _resul;
            return status;
        }
        public static bool Get_Internal_Version(string in_OrdenFabricación, out List<string> rows_data, out string err_msg)
        {
            bool status = false;
            List<string> _resul = new List<string>();
            err_msg = "";

            try
            {
                MySqlCommand command = new MySqlCommand();
                command.Connection = connection;
                command.CommandText = "Get_Internal_Version";
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@_OF", in_OrdenFabricación);
                command.Parameters["@_OF"].Direction = ParameterDirection.Input;

                command.ExecuteNonQuery();

                MySqlDataReader Read_data = command.ExecuteReader();
                status = ReadDataFromStoredProcess(Read_data, out _resul);
                Read_data.Close();
            }
            catch (Exception ex)
            {
                err_msg = ex.Message;
                status = false;
                _resul.Add("3");
            }
            rows_data = _resul;
            return status;
        }
        public static bool Insert_Manufacturing_Register(string in_SN_Housing, string in_SN_AUXSubassembly, string in_InternalVersion, string in_comment, int in_num, string in_usuario, int in_resultado, string in_NomProceso, string in_screwdriver, out List<string> rows_data, out string err_msg)
        {
            bool status = true;
            List<string> _resul = new List<string>();
            err_msg = "";

            try
            {
                MySqlCommand command = new MySqlCommand();
                command.Connection = connection;
                command.CommandText = "Insert_Manufacturing_Register";
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@SerialN", in_SN_Housing);
                command.Parameters["@SerialN"].Direction = ParameterDirection.Input;

                command.Parameters.AddWithValue("@SerialNAUX", in_SN_AUXSubassembly);
                command.Parameters["@SerialNAUX"].Direction = ParameterDirection.Input;

                command.Parameters.AddWithValue("@InVer", in_InternalVersion);
                command.Parameters["@InVer"].Direction = ParameterDirection.Input;

                command.Parameters.AddWithValue("@Coment", in_comment);
                command.Parameters["@Coment"].Direction = ParameterDirection.Input;


                command.Parameters.AddWithValue("@Num", in_num);
                command.Parameters["@Num"].Direction = ParameterDirection.Input;

                command.Parameters.AddWithValue("@Usu", in_usuario);
                command.Parameters["@Usu"].Direction = ParameterDirection.Input;

                command.Parameters.AddWithValue("@Res", in_resultado);
                command.Parameters["@Res"].Direction = ParameterDirection.Input;

                command.Parameters.AddWithValue("@NomProc", in_NomProceso);
                command.Parameters["@NomProc"].Direction = ParameterDirection.Input;


                command.Parameters.AddWithValue("@Descrew", in_screwdriver);
                command.Parameters["@Descrew"].Direction = ParameterDirection.Input;

                //command.ExecuteNonQuery();

                MySqlDataReader Read_data = command.ExecuteReader();
                status = ReadDataFromStoredProcess(Read_data, out _resul);
                Read_data.Close();
            }
            catch (Exception ex)
            {
                err_msg = ex.Message;
                status = false;
                _resul.Add("3");
            }
            rows_data = _resul;
            return status;
        }
        public static bool UpdateStatusSN_Manufacturing_Register(string in_SN_Housing, string in_SN_AUXSubassembly,  string in_comment, int in_num, string in_usuario, int in_resultado, string in_NomProceso, string in_screwdriver, out List<string> rows_data, out string err_msg)
        {
            bool status = true;
            List<string> _resul = new List<string>();
            err_msg = "";

            try
            {
                MySqlCommand command = new MySqlCommand();
                command.Connection = connection;
                command.CommandText = "UpdateStatusSN_Manufacturing_Register";
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@SerialN", in_SN_Housing);
                command.Parameters["@SerialN"].Direction = ParameterDirection.Input;

                command.Parameters.AddWithValue("@SerialNAUX", in_SN_AUXSubassembly);
                command.Parameters["@SerialNAUX"].Direction = ParameterDirection.Input;

                command.Parameters.AddWithValue("@Coment", in_comment);
                command.Parameters["@Coment"].Direction = ParameterDirection.Input;


                command.Parameters.AddWithValue("@in_Num", in_num);
                command.Parameters["@in_Num"].Direction = ParameterDirection.Input;

                command.Parameters.AddWithValue("@Usu", in_usuario);
                command.Parameters["@Usu"].Direction = ParameterDirection.Input;

                command.Parameters.AddWithValue("@Res", in_resultado);
                command.Parameters["@Res"].Direction = ParameterDirection.Input;

                command.Parameters.AddWithValue("@NomProc", in_NomProceso);
                command.Parameters["@NomProc"].Direction = ParameterDirection.Input;


                command.Parameters.AddWithValue("@Descrew", in_screwdriver);
                command.Parameters["@Descrew"].Direction = ParameterDirection.Input;

                //command.ExecuteNonQuery();

                MySqlDataReader Read_data = command.ExecuteReader();
                status = ReadDataFromStoredProcess(Read_data, out _resul);
                Read_data.Close();
            }
            catch (Exception ex)
            {
                err_msg = ex.Message;
                status = false;
                _resul.Add("3");
            }
            rows_data = _resul;
            return status;
        }
        public static bool Insert_Housing_PostRework(string in_SN_Housing, string in_comment, int in_num, string in_NomProceso, int in_zocalo, string in_usuario, out List<string> rows_data, out string err_msg)
        {
            bool status = true;
            List<string> _resul = new List<string>();
            err_msg = "";

            try
            {
                MySqlCommand command = new MySqlCommand();
                command.Connection = connection;
                command.CommandText = "Insert_Housing_PostRework";
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@SerialN", in_SN_Housing);
                command.Parameters["@SerialN"].Direction = ParameterDirection.Input;

                command.Parameters.AddWithValue("@Coment", in_comment);
                command.Parameters["@Coment"].Direction = ParameterDirection.Input;


                command.Parameters.AddWithValue("@Num", in_num);
                command.Parameters["@Num"].Direction = ParameterDirection.Input;

                command.Parameters.AddWithValue("@NomCentro", in_NomProceso);
                command.Parameters["@NomCentro"].Direction = ParameterDirection.Input;

                command.Parameters.AddWithValue("@in_Zocalo", in_zocalo);
                command.Parameters["@in_Zocalo"].Direction = ParameterDirection.Input;

                command.Parameters.AddWithValue("@Usu", in_usuario);
                command.Parameters["@Usu"].Direction = ParameterDirection.Input;
                //command.ExecuteNonQuery();

                MySqlDataReader Read_data = command.ExecuteReader();
                status = ReadDataFromStoredProcess(Read_data, out _resul);
                Read_data.Close();
            }
            catch (Exception ex)
            {
                err_msg = ex.Message;
                status = false;
                _resul.Add("3");
            }
            rows_data = _resul;
            return status;
        }

        public static bool Insert_Operations(string in_SN_Housing, int in_num, int in_status_SN, string in_NomProceso, string in_usuario, out List<string> rows_data, out string err_msg)
        {
            bool status = false;
            List<string> _resul = new List<string>();
            err_msg = "";

            try
            {
                MySqlCommand command = new MySqlCommand();
                command.Connection = connection;
                command.CommandText = "Insert_Operations";
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@SerialN", in_SN_Housing);
                command.Parameters["@SerialN"].Direction = ParameterDirection.Input;


                command.Parameters.AddWithValue("@Num", in_num);
                command.Parameters["@Num"].Direction = ParameterDirection.Input;

                command.Parameters.AddWithValue("@status_SN", in_status_SN);
                command.Parameters["@status_SN"].Direction = ParameterDirection.Input;

                command.Parameters.AddWithValue("@NomProc", in_NomProceso);
                command.Parameters["@NomProc"].Direction = ParameterDirection.Input;

                command.Parameters.AddWithValue("@Usu", in_usuario);
                command.Parameters["@Usu"].Direction = ParameterDirection.Input;
                //command.ExecuteNonQuery();

                MySqlDataReader Read_data = command.ExecuteReader();
                status = ReadDataFromStoredProcess(Read_data, out _resul);
                Read_data.Close();
                status = true;
            }
            catch (Exception ex)
            {
                err_msg = ex.Message;
                status = false;
                _resul.Add("3");
            }
            rows_data = _resul;
            return status;
        }
        public static bool Check_OF_SNHousing(string in_SN_Housing, string in_OF, out List<string> rows_data, out string err_msg)
        {
            bool status = false;
            List<string> _resul = new List<string>();
            err_msg = "";

            try
            {
                MySqlCommand command = new MySqlCommand();
                command.Connection = connection;
                command.CommandText = "Check_OF_SNHousing";
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@SerialN", in_SN_Housing);
                command.Parameters["@SerialN"].Direction = ParameterDirection.Input;

                command.Parameters.AddWithValue("@in_DescriptionOF", in_OF);
                command.Parameters["@in_DescriptionOF"].Direction = ParameterDirection.Input;
                //command.ExecuteNonQuery();

                MySqlDataReader Read_data = command.ExecuteReader();
                status = ReadDataFromStoredProcess(Read_data, out _resul);
                Read_data.Close();
                status = true;
            }
            catch (Exception ex)
            {
                err_msg = ex.Message;
                status = false;
                _resul.Add("3");
            }
            rows_data = _resul;
            return status;
        }
        
        public static bool Insert_OF_SNHousing(string in_SN_Housing, string in_OF, out List<string> rows_data, out string err_msg)
        {
            bool status = false;
            List<string> _resul = new List<string>();
            err_msg = "";

            try
            {
                MySqlCommand command = new MySqlCommand();
                command.Connection = connection;
                command.CommandText = "Insert_OF_SNHousing";
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@SerialN", in_SN_Housing);
                command.Parameters["@SerialN"].Direction = ParameterDirection.Input;

                command.Parameters.AddWithValue("@in_DescriptionOF", in_OF);
                command.Parameters["@in_DescriptionOF"].Direction = ParameterDirection.Input;
                //command.ExecuteNonQuery();

                MySqlDataReader Read_data = command.ExecuteReader();
                status = ReadDataFromStoredProcess(Read_data, out _resul);
                Read_data.Close();
                status = true;
            }
            catch (Exception ex)
            {
                err_msg = ex.Message;
                status = false;
                _resul.Add("3");
            }
            rows_data = _resul;
            return status;
        }

        public static bool Insert_Version_Procesos_Manufacturing(string in_name_path_file, out List<string> rows_data, out string err_msg)
        {
            bool status = true;
            List<string> _resul = new List<string>();
            err_msg = "";

            try
            {
                MySqlCommand command = new MySqlCommand();
                command.Connection = connection;
                command.CommandText = "Insert_Version_Procesos_Manufacturing";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@Nom", in_name_path_file);
                command.Parameters["@Nom"].Direction = ParameterDirection.Input;//Por poner

                MySqlDataReader Read_data = command.ExecuteReader();
                status = ReadDataFromStoredProcess(Read_data, out _resul);
                Read_data.Close();
                status = true;
            }
            catch (Exception ex)
            {
                err_msg = ex.Message;
                status = false;
                _resul.Add("3");
            }
            rows_data = _resul;
            return status;
        }
        public static bool Check_Version_Procesos_Manufacturing(string in_name_path_file, out List<string> rows_data, out string err_msg)
        {
            bool status = true;
            List<string> _resul = new List<string>();
            err_msg = "";

            try
            {
                MySqlCommand command = new MySqlCommand();
                command.Connection = connection;
                command.CommandText = "SELECT IF( EXISTS(SELECT idVersion_Procesos_Manufacturing FROM Version_Procesos_Manufacturing WHERE Nombre ='" +in_name_path_file+ "'), 1, 0);";

                MySqlDataReader Read_data = command.ExecuteReader();
                status = ReadDataFromStoredProcess(Read_data, out _resul);
                Read_data.Close();
                status = true;
            }
            catch (Exception ex)
            {
                err_msg = ex.Message;
                status = false;
                _resul.Add("3");
            }
            rows_data = _resul;
            return status;
        }
        public static bool Get_Result_Process_SN(string in_SN_Housing, string in_NomProcesoTest, out List<string> rows_data, out string err_msg)
        {
            bool status = false;
            List<string> _resul = new List<string>();
            err_msg = "";

            try
            {
                MySqlCommand command = new MySqlCommand();
                command.Connection = connection;
                command.CommandText = "Get_Result_Process_SN";
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@SerialN", in_SN_Housing);
                command.Parameters["@SerialN"].Direction = ParameterDirection.Input;

                command.Parameters.AddWithValue("@Nom_proceso", in_NomProcesoTest);
                command.Parameters["@Nom_proceso"].Direction = ParameterDirection.Input;


                command.ExecuteNonQuery();

                MySqlDataReader Read_data = command.ExecuteReader();
                status = ReadDataFromStoredProcess(Read_data, out _resul);
                Read_data.Close();
            }
            catch (Exception ex)
            {
                err_msg = ex.Message;
                status = false;
                _resul.Add("3");
            }
            rows_data = _resul;
            return status;
        }
        public static bool Get_Definition_PCB(string in_SN_PCB, out List<string> rows_data, out string err_msg)
        {
            bool status = false;
            List<string> _resul = new List<string>();
            err_msg = "";

            try
            {
                MySqlCommand command = new MySqlCommand();
                command.Connection = connection;
                command.CommandText = "Get_Definition_PCB";
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@SerialN", in_SN_PCB);
                command.Parameters["@SerialN"].Direction = ParameterDirection.Input;


                command.ExecuteNonQuery();

                MySqlDataReader Read_data = command.ExecuteReader();
                status = ReadDataFromStoredProcess(Read_data, out _resul);
                Read_data.Close();
            }
            catch (Exception ex)
            {
                err_msg = ex.Message;
                status = false;
                _resul.Add("3");
            }
            rows_data = _resul;
            return status;
        }
        public static bool Insertar_Registro_Assembly_SN(string in_SN_Housing, string in_Part_Ref, out string err_msg)
        {
            bool status = false;
            err_msg = "";
            List<string> _resul = new List<string>();
            try
            {
                MySqlCommand command = new MySqlCommand();
                command.Connection = connection;
                command.CommandText = "Insertar_Registro_Assembly_SN";
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@SerialNumber", in_SN_Housing);
                command.Parameters["@SerialNumber"].Direction = ParameterDirection.Input;

                command.Parameters.AddWithValue("@PartRef", in_Part_Ref);
                command.Parameters["@PartRef"].Direction = ParameterDirection.Input;

                command.ExecuteNonQuery();
                status = true;
            }
            catch (Exception ex)
            {
                //int someInt = 3;
                //_resul.Add(someInt.ToString());
                err_msg = ex.Message;
                status = false;
            }
            //rows_data = _resul;
            return status;
        }
        public static bool IdenificacionEstacion(string HostName ,out List<string> rows_data, out string err_msg)
        {
            bool status = false;
            err_msg = "";
            List<string> resul = new List<string>();
            try
            {
                
                MySqlCommand command = new MySqlCommand();
                command.Connection = connection;
                command.CommandText = "IdenificacionEstacion";
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@HOSTNAME", HostName);
                command.Parameters["@HOSTNAME"].Direction = ParameterDirection.Input;
                command.ExecuteNonQuery();
                MySqlDataReader Read_data = command.ExecuteReader();
                status = ReadDataFromStoredProcess(Read_data, out resul);
                Read_data.Close();
            }
            catch(Exception ex)
            {
                err_msg = ex.Message;
                status = false;
                resul.Add("3");
            }
            rows_data = resul;
            return status;
        }
        public static bool OpenConnection()
        {
            bool status = false;
            try
            {
                
                status = true;
            }
            catch
            {
                status = false;
            }
            return status;
        }

    }
    #endregion
}
