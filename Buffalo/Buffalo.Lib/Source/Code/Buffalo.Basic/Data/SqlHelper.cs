﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;


namespace Buffalo.Basic.Data
{
    public class Data_SqlHelper
    {

        public static bool Action_AutoCreateSPS(SqlConnection ActiveConnection)
        {
            if (ActiveConnection != null)
            {
                string sql_getALLTables = "select * from sys.all_objects where (type = 'U')";
                Data_SqlDataHelper obj_dataHelper = new Data_SqlDataHelper();
                obj_dataHelper.ActiveConnection = ActiveConnection;
                DataTable TablesInfo = new DataTable();
                DataTable ColumnInfo = new DataTable();
                DataTable TypesInfo = new DataTable();
                if (obj_dataHelper.Action_ExecuteForDT(sql_getALLTables, out TablesInfo))
                {
                    if (TablesInfo.Rows.Count > 0)
                    {
                        foreach (DataRow activeDR_1 in TablesInfo.Rows)
                        {
                            string sql_getALLColumns = "select * from sys.syscolumns";
                            string tableName = "";
                            obj_dataHelper.Static_GetColumnData(activeDR_1, "name", out tableName);
                            string objectID = "";
                            obj_dataHelper.Static_GetColumnData(activeDR_1, "object_id", out objectID);
                            if (tableName != "")
                            {
                                if (objectID != "")
                                {
                                    sql_getALLColumns = sql_getALLColumns + " where id='" + objectID + "'";
                                    List<string> activeColumn = new List<string>();
                                    List<string> activeKeyColumn = new List<string>();
                                    List<string> filterColumn = new List<string>();
                                    if (obj_dataHelper.Action_ExecuteForDT(sql_getALLColumns, out ColumnInfo))
                                    {
                                        StringBuilder sql_CreateNewSp = new StringBuilder("IF OBJECTPROPERTY(OBJECT_ID(N'SPA_Operation_" + tableName + "'), N'IsProcedure') = 1");
                                        sql_CreateNewSp.AppendLine();
                                        sql_CreateNewSp.AppendLine("DROP PROCEDURE SPA_Operation_" + tableName);
                                        obj_dataHelper.Action_ExecuteForNonQuery(sql_CreateNewSp.ToString());
                                        sql_CreateNewSp.Clear();
                                        sql_CreateNewSp.AppendLine("CREATE PROCEDURE {SPNAME}");
                                        StringBuilder sql_insertSourceColumns = new StringBuilder();
                                        StringBuilder sql_insertValueColumns = new StringBuilder();
                                        sql_CreateNewSp.Replace("{SPNAME}", "SPA_Operation_" + tableName);
                                        sql_CreateNewSp.AppendLine("(");
                                        sql_CreateNewSp.AppendLine("@operation nvarchar(20) = '',");
                                        foreach (DataRow activeDR_2 in ColumnInfo.Rows)
                                        {
                                            string sql_getALLTypes = "select * from sys.types";
                                            string columnname = "";
                                            obj_dataHelper.Static_GetColumnData(activeDR_2, "name", out columnname);
                                            string typeid = "";
                                            string length = "";
                                            string status = "";
                                            obj_dataHelper.Static_GetColumnData(activeDR_2, "xtype", out typeid);
                                            obj_dataHelper.Static_GetColumnData(activeDR_2, "status", out status);
                                            obj_dataHelper.Static_GetColumnData(activeDR_2, "prec", out length);
                                            sql_getALLTypes = sql_getALLTypes + " where system_type_id=" + typeid;
                                            obj_dataHelper.Action_ExecuteForDT(sql_getALLTypes, out TypesInfo);
                                            string typename = "";
                                            obj_dataHelper.Static_GetColumnData(TypesInfo.Rows[0], "name", out typename);
                                            if (typename.Contains("nvarchar"))
                                                sql_CreateNewSp.AppendLine("@" + columnname + " nvarchar(" + length + ") = null ,");
                                            else if (typename.Contains("char"))
                                                sql_CreateNewSp.AppendLine("@" + columnname + " char(" + length + ") = null ,");
                                            else if (typename.Contains("varcahr"))
                                                sql_CreateNewSp.AppendLine("@" + columnname + " varchar(" + length + ") = null ,");
                                            else if (typename.Contains("binary"))
                                            {
                                                sql_CreateNewSp.AppendLine("@" + columnname + " binary(" + length + ") = null ,");
                                                filterColumn.Add(columnname);
                                            }
                                            else if (typename.Contains("varbinary"))
                                            {
                                                sql_CreateNewSp.AppendLine("@" + columnname + " varbinary(" + length + ") = null ,");
                                                filterColumn.Add(columnname);
                                            }
                                            else if (typename.Contains("nchar"))
                                                sql_CreateNewSp.AppendLine("@" + columnname + " nchar(" + length + ") = null ,");
                                            else if (typename.Contains("decimal"))
                                                sql_CreateNewSp.AppendLine("@" + columnname + " decimal" + " = null ,");
                                            else
                                            {
                                                sql_CreateNewSp.AppendLine("@" + columnname + " " + typename + " = null ,");
                                                filterColumn.Add(columnname);
                                            }
                                            activeColumn.Add(columnname);
                                            if (status == "128")
                                                activeKeyColumn.Add(columnname);
                                            if (status != "128")
                                            {
                                                sql_insertSourceColumns.Append("[" + columnname + "],");
                                                sql_insertValueColumns.Append("@" + columnname + ",");
                                            }
                                        }
                                        sql_CreateNewSp = sql_CreateNewSp.Remove(sql_CreateNewSp.Length - 3, 3);
                                        sql_insertSourceColumns = sql_insertSourceColumns.Remove(sql_insertSourceColumns.Length - 1, 1);
                                        sql_insertValueColumns = sql_insertValueColumns.Remove(sql_insertValueColumns.Length - 1, 1);
                                        sql_CreateNewSp.AppendLine(")");
                                        sql_CreateNewSp.AppendLine("AS");
                                        sql_CreateNewSp.AppendLine("if @operation='get'");
                                        sql_CreateNewSp.AppendLine("begin");
                                        sql_CreateNewSp.AppendLine("select * from [" + tableName + "]");
                                        sql_CreateNewSp.AppendLine("end");
                                        sql_CreateNewSp.AppendLine("else if @operation='insert'");
                                        sql_CreateNewSp.AppendLine("begin");
                                        sql_CreateNewSp.AppendLine("insert into " + tableName + "(" + sql_insertSourceColumns + ") values(" + sql_insertValueColumns + ")");
                                        sql_CreateNewSp.AppendLine("end");
                                        foreach (string activeCommonColumn in activeColumn)
                                        {
                                            if (!activeKeyColumn.Contains(activeCommonColumn))
                                            {
                                                if (!filterColumn.Contains(activeCommonColumn))
                                                    sql_CreateNewSp.AppendLine("if @operation='update' and @" + activeCommonColumn + " is not null");
                                                else
                                                    sql_CreateNewSp.AppendLine("if @operation='update'");
                                                sql_CreateNewSp.AppendLine("begin");
                                                sql_CreateNewSp.AppendLine("update " + tableName);
                                                sql_CreateNewSp.AppendLine("set " + activeCommonColumn + "=@" + activeCommonColumn);
                                                if (activeKeyColumn.Count > 0)
                                                {
                                                    sql_CreateNewSp.AppendLine("where ");
                                                    foreach (string keyColumn in activeKeyColumn)
                                                    {
                                                        sql_CreateNewSp.Append(keyColumn + "=@" + keyColumn + " and");
                                                    }
                                                    sql_CreateNewSp = sql_CreateNewSp.Remove(sql_CreateNewSp.Length - 4, 4);
                                                    sql_CreateNewSp.AppendLine();
                                                }
                                                sql_CreateNewSp.AppendLine("end");
                                            }
                                        }
                                        sql_CreateNewSp.AppendLine("else if @operation='delete'");
                                        sql_CreateNewSp.AppendLine("begin");
                                        /*if (activeKeyColumn.Count > 0)
                                        {
                                            sql_CreateNewSp.AppendLine("if ");
                                            foreach (string keyColumn in activeKeyColumn)
                                            {
                                                sql_CreateNewSp.Append("@" + keyColumn + "<>'' and");
                                            }
                                            sql_CreateNewSp = sql_CreateNewSp.Remove(sql_CreateNewSp.Length - 4, 4);
                                            sql_CreateNewSp.AppendLine("begin");
                                            sql_CreateNewSp.AppendLine("delete from " + tableName);
                                            sql_CreateNewSp.AppendLine("where ");
                                            foreach (string keyColumn in activeKeyColumn)
                                            {
                                                sql_CreateNewSp.Append(keyColumn + "=@" + keyColumn + " and");
                                            }
                                            sql_CreateNewSp = sql_CreateNewSp.Remove(sql_CreateNewSp.Length - 4, 4);
                                            sql_CreateNewSp.AppendLine();
                                            sql_CreateNewSp.AppendLine("end");
                                        }
                                        else
                                        {*/
                                        sql_CreateNewSp.AppendLine("delete from " + tableName);
                                        sql_CreateNewSp.AppendLine(" where ");
                                        foreach (string keyColumn in activeKeyColumn)
                                        {
                                            sql_CreateNewSp.Append(keyColumn + "=@" + keyColumn + " and");
                                        }
                                        sql_CreateNewSp = sql_CreateNewSp.Remove(sql_CreateNewSp.Length - 4, 4);
                                        sql_CreateNewSp.AppendLine("");
                                        //}
                                        sql_CreateNewSp.AppendLine("end");
                                        obj_dataHelper.Action_ExecuteForNonQuery(sql_CreateNewSp.ToString());

                                    }
                                }
                            }
                        }
                    }
                }
                return true;
            }
            else
                return false;
        }

        public static List<string> Action_GetAllUserTables(SqlConnection ActiveConnection)
        {
            List<string> result = new List<string>();
            string sql_getALLTables = "select * from sys.all_objects where (type = 'U')";
            if (ActiveConnection != null)
            {
                Data_SqlDataHelper obj_dataHelper = new Data_SqlDataHelper();
                obj_dataHelper.ActiveConnection = ActiveConnection;
                DataTable TablesInfo = new DataTable();
                obj_dataHelper.Action_ExecuteForDT(sql_getALLTables, out TablesInfo);
                foreach (DataRow activeDR_1 in TablesInfo.Rows)
                {
                    string tableName = "";
                    obj_dataHelper.Static_GetColumnData(activeDR_1, "name", out tableName);
                    result.Add(tableName);
                }
                return result;

            }
            else
                return result;
        }

        public static List<string> Action_GetAllUserStoreProcs(SqlConnection ActiveConnection)
        {
            List<string> result = new List<string>();
            string sql_getALLTables = "select * from sys.all_objects where type='P' and is_ms_shipped=0";
            if (ActiveConnection != null)
            {
                Data_SqlDataHelper obj_dataHelper = new Data_SqlDataHelper();
                obj_dataHelper.ActiveConnection = ActiveConnection;
                DataTable TablesInfo = new DataTable();
                obj_dataHelper.Action_ExecuteForDT(sql_getALLTables, out TablesInfo);
                foreach (DataRow activeDR_1 in TablesInfo.Rows)
                {
                    string tableName = "";
                    obj_dataHelper.Static_GetColumnData(activeDR_1, "name", out tableName);
                    result.Add(tableName);
                }
                return result;

            }
            else
                return result;
        }

        public static string Action_BuildCreateTableSqlString(List<string> activeTableStructs, string tableName, string activeDBName)
        {
            if (activeTableStructs.Count == 0)
                return "";
            else
            {
                StringBuilder sql_Result = new StringBuilder();
                sql_Result.AppendLine("USE [" + activeDBName + "]");
                sql_Result.AppendLine("CREATE TABLE " + tableName);
                sql_Result.AppendLine("(");
                foreach (string activeTableStruct in activeTableStructs)
                {
                    sql_Result.AppendLine(activeTableStruct);
                }
                sql_Result.AppendLine(")");
                return sql_Result.ToString();
            }
        }

        public static bool Action_ExecuteCreateSql(List<string> activeTableStructes, string activeDBName, string tableName, SqlConnection ActiveConnection)
        {
            if (activeTableStructes.Count == 0 || activeDBName == "" || ActiveConnection == null)
                return false;
            else
            {
                Data_SqlDataHelper obj_dataHelper = new Data_SqlDataHelper();
                obj_dataHelper.ActiveConnection = ActiveConnection;
                StringBuilder sql_CreateNewSp = new StringBuilder("IF OBJECTPROPERTY(OBJECT_ID(N'" + tableName + "'), N'IsTable') = 1");
                sql_CreateNewSp.AppendLine();
                sql_CreateNewSp.AppendLine("DROP TABLE " + tableName);
                obj_dataHelper.Action_ExecuteForNonQuery(sql_CreateNewSp.ToString());
                string sql = Data_SqlHelper.Action_BuildCreateTableSqlString(activeTableStructes, tableName, activeDBName);
                if (sql != "")
                {
                    if (obj_dataHelper.Action_ExecuteForNonQuery(sql))
                        return true;
                    else
                        return false;
                }
                else
                    return false;
            }
        }

        public static Dictionary<string, Data_SqlSPEntry> Action_AutoLoadingAllSPS(SqlConnection ActiveConnection, string SPType)
        {
            if (ActiveConnection != null)
            {
                Data_SqlDataHelper obj = new Data_SqlDataHelper();
                obj.ActiveConnection = ActiveConnection;
                string sql_getallsps = "select * from sys.all_objects where (type = 'P') AND (is_ms_shipped = 0)";
                DataTable activeSPSDT = new DataTable();
                Dictionary<string, Data_SqlSPEntry> result = new Dictionary<string, Data_SqlSPEntry>();
                if (obj.Action_ExecuteForDT(sql_getallsps, out activeSPSDT))
                {
                    foreach (DataRow activeRow in activeSPSDT.Rows)
                    {
                        Data_SqlSPEntry newSPEntry = new Data_SqlSPEntry();
                        string spName = "";
                        obj.Static_GetColumnData(activeRow, "name", out spName);
                        if (SPType != "")
                        {
                            if (SPType == Data_SqlSPEntryType.SelectAction)
                            {
                                if (!spName.StartsWith(Data_SqlSPEntryNameFiler.StartNamed_SelectAction))
                                    continue;
                            }
                            else if (SPType == Data_SqlSPEntryType.UpdateAction)
                            {
                                if (!spName.StartsWith(Data_SqlSPEntryNameFiler.StartNamed_Update))
                                    continue;
                            }
                        }
                        newSPEntry.SPName = spName;
                        newSPEntry.KeyName = spName;

                        string spObjectID = "";
                        obj.Static_GetColumnData(activeRow, "object_id", out spObjectID);
                        string sql_paramters = "select * from sys.all_parameters where object_id = " + spObjectID;
                        DataTable activeSPParametersDT = new DataTable();
                        string sql_paramstype = "select * from sys.types";
                        DataTable paramstypeDT = new DataTable();
                        if (!obj.Action_ExecuteForDT(sql_paramstype, out paramstypeDT))
                        {
                            return null;
                        }
                        if (obj.Action_ExecuteForDT(sql_paramters, out activeSPParametersDT))
                        {
                            foreach (DataRow activeParamterRow in activeSPParametersDT.Rows)
                            {
                                string activeSystemType_ID = "";
                                obj.Static_GetColumnData(activeParamterRow, "system_type_id", out activeSystemType_ID);
                                string activeUserType_ID = "";
                                obj.Static_GetColumnData(activeParamterRow, "user_type_id", out activeUserType_ID);
                                string activeParamsMaxLength = "";
                                obj.Static_GetColumnData(activeParamterRow, "max_length", out activeParamsMaxLength);
                                string activeParamsName = "";
                                obj.Static_GetColumnData(activeParamterRow, "name", out activeParamsName);
                                string activeIsOutPut = "";
                                obj.Static_GetColumnData(activeParamterRow, "is_output", out activeIsOutPut);
                                string max_length = "";
                                obj.Static_GetColumnData(activeParamterRow, "max_length", out max_length);
                                string activeDBType = "";
                                DataRow[] dbtyps = paramstypeDT.Select("system_type_id=" + activeSystemType_ID + " and user_type_id=" + activeUserType_ID);
                                if (dbtyps.Length > 0)
                                {
                                    obj.Static_GetColumnData(dbtyps[0], "name", out activeDBType);
                                    Data_SqlSPEntry.AddSPParameter(ref newSPEntry, activeParamsName, Data_SqlSPHelper.Static_GetDbTypeFromConfigStr(activeDBType), ParameterDirection.Input, int.Parse(max_length), null);
                                }
                                else
                                    continue;
                            }
                        }
                        else
                        {
                            continue;
                        }
                        result.Add(newSPEntry.KeyName, newSPEntry);
                    }
                }
                return result;
            }
            else
                return null;
        }

        public DataTable Action_ExecuteGetSPSForDT(Data_SqlSPEntry activeEntry, SqlConnection ActiveConnection)
        {
            DataTable dt = new DataTable();
            if (activeEntry != null && ActiveConnection!=null)
            {
                Data_SqlSPEntry.ModifySPParameter(ref activeEntry, "@operation", SqlDbType.NVarChar, ParameterDirection.Input, "get");
                Data_SqlDataHelper activeSqlSPHelper = new Data_SqlDataHelper();
                activeSqlSPHelper.ActiveConnection = ActiveConnection;
                activeSqlSPHelper.Action_ExecuteForDT(activeEntry, out dt);
                return dt;
            }
            else
                return null;
        }

        public DataSet Action_ExecuteGetSPSForDS(Data_SqlSPEntry activeEntry, SqlConnection ActiveConnection)
        {
            DataSet ds = new DataSet();
            if (activeEntry != null && ActiveConnection != null)
            {
                Data_SqlSPEntry.ModifySPParameter(ref activeEntry, "@operation", SqlDbType.NVarChar, ParameterDirection.Input, "get");
                Data_SqlDataHelper activeSqlSPHelper = new Data_SqlDataHelper();
                activeSqlSPHelper.ActiveConnection = ActiveConnection;
                activeSqlSPHelper.Action_ExecuteForDS(activeEntry, out ds);
                return ds;
            }
            else
                return null;
        }

        public SqlDataReader Action_ExecuteGetSPSForDR(Data_SqlSPEntry activeEntry, SqlConnection ActiveConnection)
        {
            if (activeEntry != null && ActiveConnection != null)
            {
                Data_SqlSPEntry.ModifySPParameter(ref activeEntry, "@operation", SqlDbType.NVarChar, ParameterDirection.Input, "get");
                Data_SqlDataHelper activeSqlSPHelper = new Data_SqlDataHelper();
                activeSqlSPHelper.ActiveConnection = ActiveConnection;
                SqlDataReader dr;
                if (activeSqlSPHelper.Action_ExecuteForDataReader(activeEntry, out dr))
                    return dr;
                else
                    return null;
            }
            else
                return null;
        }

        public bool Action_ExecuteInsertSPS(Data_SqlSPEntry activeEntry, SqlConnection ActiveConnection, Dictionary<string, object> ValueMaping)
        {
            Dictionary<string, int> SizeMap = new Dictionary<string, int>();
            return Action_ExecuteInsertSPS(activeEntry, ActiveConnection, ValueMaping, SizeMap);
        }
        
        public bool Action_ExecuteInsertSPS(Data_SqlSPEntry activeEntry, SqlConnection ActiveConnection, Dictionary<string, object> ValueMaping, Dictionary<string, int> SizeMaping)
        {
            if (activeEntry != null && ActiveConnection!=null)
            {
                Data_SqlSPEntry.ModifySPParameter(ref activeEntry, "@operation", SqlDbType.NVarChar, ParameterDirection.Input, "insert");
                foreach (string activeParameter in ValueMaping.Keys)
                {
                    int activeParametersIndex = Data_SqlSPEntry.GetSPParameterIndex(ref activeEntry, activeParameter);
                    if (activeParametersIndex != -1)
                    {
                        SqlParameter activeExistedParamter = activeEntry.ActiveParameters[activeParametersIndex];
                        if (!SizeMaping.ContainsKey(activeParameter))
                            Data_SqlSPEntry.ModifySPParameter(ref activeEntry, activeParameter, activeExistedParamter.SqlDbType, activeExistedParamter.Direction, ValueMaping[activeParameter]);
                        else
                            Data_SqlSPEntry.ModifySPParameter(ref activeEntry, activeParameter, activeExistedParamter.SqlDbType, activeExistedParamter.Direction, ValueMaping[activeParameter], SizeMaping[activeParameter]);

                    }
                }
                Data_SqlDataHelper activeSqlSPHelper = new Data_SqlDataHelper();
                activeSqlSPHelper.ActiveConnection = ActiveConnection;
                activeSqlSPHelper.Action_ExecuteForNonQuery(activeEntry);
                return true;
            }
            else
                return false;
        }

        public bool Action_ExecuteUpdateSPS(Data_SqlSPEntry activeEntry, SqlConnection ActiveConnection, Dictionary<string, object> ValueMaping)
        {
            Dictionary<string, int> SizeMaping=new Dictionary<string,int>();
            return Action_ExecuteUpdateSPS(activeEntry, ActiveConnection, ValueMaping, SizeMaping);
        }

        public bool Action_ExecuteUpdateSPS(Data_SqlSPEntry activeEntry, SqlConnection ActiveConnection, Dictionary<string, object> ValueMaping, Dictionary<string, int> SizeMaping)
        {
            if (activeEntry != null && ActiveConnection!=null)
            {
                Data_SqlSPEntry.ModifySPParameter(ref activeEntry, "@operation", SqlDbType.NVarChar, ParameterDirection.Input, "update");
                List<string> filterExclude = new List<string>();
                filterExclude.Add("@operation");
                Data_SqlSPEntry.ModifyExcludeSPParameter(ref activeEntry, ValueMaping, filterExclude);
                foreach (string activeParameter in ValueMaping.Keys)
                {
                    int activeParametersIndex = Data_SqlSPEntry.GetSPParameterIndex(ref activeEntry, activeParameter);
                    if (activeParametersIndex != -1)
                    {
                        SqlParameter activeExistedParamter = activeEntry.ActiveParameters[activeParametersIndex];
                        if (!SizeMaping.ContainsKey(activeParameter))
                            Data_SqlSPEntry.ModifySPParameter(ref activeEntry, activeParameter, activeExistedParamter.SqlDbType, activeExistedParamter.Direction, ValueMaping[activeParameter]);
                        else
                            Data_SqlSPEntry.ModifySPParameter(ref activeEntry, activeParameter, activeExistedParamter.SqlDbType, activeExistedParamter.Direction, ValueMaping[activeParameter], SizeMaping[activeParameter]);

                    }
                }
                Data_SqlDataHelper activeSqlSPHelper = new Data_SqlDataHelper();
                activeSqlSPHelper.ActiveConnection = ActiveConnection;
                activeSqlSPHelper.Action_ExecuteForNonQuery(activeEntry);
                return true;
            }
            else
                return false;
        }

        public bool Action_ExecuteDeleteSPS(Data_SqlSPEntry activeEntry, SqlConnection ActiveConnection, Dictionary<string, object> ValueMaping)
        {
            Dictionary<string, int> SizeMaping = new Dictionary<string, int>();
            return Action_ExecuteDeleteSPS(activeEntry, ActiveConnection, ValueMaping, SizeMaping);
        }

        public bool Action_ExecuteDeleteSPS(Data_SqlSPEntry activeEntry, SqlConnection ActiveConnection, Dictionary<string, object> ValueMaping, Dictionary<string, int> SizeMaping)
        {
            if (activeEntry != null && ActiveConnection!=null)
            {

                Data_SqlSPEntry.ModifySPParameter(ref activeEntry, "@operation", SqlDbType.NVarChar, ParameterDirection.Input, "delete");
                foreach (string activeParameter in ValueMaping.Keys)
                {
                    int activeParametersIndex = Data_SqlSPEntry.GetSPParameterIndex(ref activeEntry, activeParameter);
                    if (activeParametersIndex != -1)
                    {
                        SqlParameter activeExistedParamter = activeEntry.ActiveParameters[activeParametersIndex];
                        if (!SizeMaping.ContainsKey(activeParameter))
                            Data_SqlSPEntry.ModifySPParameter(ref activeEntry, activeParameter, activeExistedParamter.SqlDbType, activeExistedParamter.Direction, ValueMaping[activeParameter]);
                        else
                            Data_SqlSPEntry.ModifySPParameter(ref activeEntry, activeParameter, activeExistedParamter.SqlDbType, activeExistedParamter.Direction, ValueMaping[activeParameter], SizeMaping[activeParameter]);

                    }
                }
                Data_SqlDataHelper activeSqlSPHelper = new Data_SqlDataHelper();
                activeSqlSPHelper.ActiveConnection = ActiveConnection;
                activeSqlSPHelper.Action_ExecuteForNonQuery(activeEntry);
                return true;
            }
            else
                return false;
        }

    }
}
