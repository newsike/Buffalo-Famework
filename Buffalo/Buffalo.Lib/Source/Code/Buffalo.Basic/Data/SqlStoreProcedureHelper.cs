using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Buffalo.Basic.Base;

namespace Buffalo.Basic.Data
{
    public class Data_SqlSPEntryNameFiler
    {
        public const string StartNamed_SelectAction = "Get";
        public const string StartNamed_Update = "Set";
        public const string StartNamed_All = "All";
    }

    public class Data_SqlSPEntryType
    {
        public const string UpdateAction = "Update";
        public const string SelectAction = "Select";
        public const string AllAction = "All";
    }

    public class Data_SqlSPEntry : ICloneable
    {
        public static bool AddSPEntryToQueue(string SPName, SqlDbType SPType, string EntryType, ref Queue<Data_SqlSPEntry> activeQueue)
        {
            if (activeQueue == null)
                return false;
            Data_SqlSPEntry activeEntry = new Data_SqlSPEntry();
            activeEntry.SPName = SPName;
            activeEntry.SPType = SPType;
            activeEntry.EntryType = EntryType;
            activeQueue.Enqueue(activeEntry);
            return true;
        }

        public static bool AddSPEntryToList(string SPName, SqlDbType SPType, string EntryType, ref List<Data_SqlSPEntry> activeList)
        {
            if (activeList == null)
                return false;
            Data_SqlSPEntry activeEntry = new Data_SqlSPEntry();
            activeEntry.SPName = SPName;
            activeEntry.SPType = SPType;
            activeEntry.EntryType = EntryType;
            activeList.Add(activeEntry);
            return true;
        }

        public static int GetSPParameterIndex(ref Data_SqlSPEntry activeSPEntry, string Paramername)
        {
            int index = 0;
            if (activeSPEntry != null)
            {
                foreach (SqlParameter activeParameter in activeSPEntry.ActiveParameters)
                {
                    if (activeParameter.ParameterName == Paramername)
                    {
                        return index;
                    }
                    index++;
                }
                return -1;
            }
            else
                return -1;
        }

        public static bool AddSPParameter(ref Data_SqlSPEntry activeSPEntry, string Paramername, SqlDbType SPType, ParameterDirection SPDirection, int size, object SPValue)
        {
            if (activeSPEntry == null)
                return false;
            SqlParameter activeParameter = new SqlParameter();
            activeParameter.ParameterName = Paramername;
            activeParameter.Direction = SPDirection;
            activeParameter.SqlDbType = SPType;
            activeParameter.Value = SPValue;
            activeParameter.Size = size;
            activeSPEntry.ActiveParameters.Add(activeParameter);
            return true;
        }

        public static bool ModifySPParameter(ref Data_SqlSPEntry activeSPEntry, string Paramername, SqlDbType SPType, ParameterDirection SPDirection, object SPValue, int size = -1)
        {
            if (activeSPEntry == null)
                return false;
            else
            {
                SqlParameter activeParameter = null;
                foreach (SqlParameter tmpParameter in activeSPEntry.ActiveParameters)
                {
                    if (tmpParameter.ParameterName == Paramername)
                    {
                        activeParameter = tmpParameter;
                        break;
                    }
                }
                if (activeParameter != null)
                {
                    activeParameter.Direction = SPDirection;
                    activeParameter.SqlDbType = SPType;
                    activeParameter.Value = SPValue;
                    if (size != -1)
                        activeParameter.Size = size;
                    return true;
                }
                else
                    return false;
            }
        }

        public static void ModifyExcludeSPParameter(ref Data_SqlSPEntry activeSPEntry, Dictionary<string, object> parameterMaping, List<string> filterParameter)
        {
            List<SqlParameter> excluded = new List<SqlParameter>();
            foreach (SqlParameter tmpParameter in activeSPEntry.ActiveParameters)
            {
                if (filterParameter.Contains(tmpParameter.ParameterName))
                    continue;
                if (parameterMaping.ContainsKey(tmpParameter.ParameterName))
                    continue;
                else
                {
                    excluded.Add(tmpParameter);
                }
            }
            foreach (SqlParameter tmpExcludeParameter in excluded)
            {
                activeSPEntry.ActiveParameters.Remove(tmpExcludeParameter);
            }

        }

        public string SPName = "";
        public SqlDbType SPType = SqlDbType.NVarChar;
        public string KeyName = "";
        public string EntryType = "";
        public List<SqlParameter> ActiveParameters = new List<SqlParameter>();


        public object Clone()
        {
            Data_SqlSPEntry newEntry = new Data_SqlSPEntry();
            newEntry.ActiveParameters = this.ActiveParameters;
            newEntry.EntryType = this.EntryType;
            newEntry.KeyName = this.KeyName;
            newEntry.SPName = this.SPName;
            newEntry.SPType = this.SPType;
            return newEntry;
        }

        public static void CopyParams(ref Data_SqlSPEntry sourceEntry, ref Data_SqlSPEntry objectEntry)
        {
            foreach (SqlParameter activeParameter in sourceEntry.ActiveParameters)
            {
                SqlParameter cloneObj = new SqlParameter();
                cloneObj.ParameterName = activeParameter.ParameterName;
                cloneObj.Direction = activeParameter.Direction;
                cloneObj.DbType = activeParameter.DbType;
                objectEntry.ActiveParameters.Add(cloneObj);
            }
        }

        public static bool ClearAllParamersValue(ref List<SqlParameter> activeParamtersList)
        {
            if (activeParamtersList != null)
            {
                foreach (SqlParameter activeParameter in activeParamtersList)
                {
                    activeParameter.Value = null;
                }
                return true;
            }
            else
                return false;
        }

    }



    public class Data_SqlSPHelper
    {

        public static SqlDbType Static_GetDbTypeFromConfigStr(string DBType)
        {
            switch (DBType)
            {
                case "SqlDbType.NVarChar":
                case "NVarChar":
                case "12":
                default:
                    return SqlDbType.NVarChar;
                case "SqlDbType.Int":
                case "Int":
                case "8":
                    return SqlDbType.Int;
                case "SqlDbType.Decimal":
                case "Decimal":
                case "5":
                    return SqlDbType.Decimal;
                case "SqlDbType.Binary":
                case "Binary":
                case "1":
                    return SqlDbType.Binary;
                case "SqlDbType.Image":
                case "Image":
                case "7":
                    return SqlDbType.Image;
                case "SqlDbType.DateTime":
                case "DateTime":
                case "4":
                    return SqlDbType.DateTime;

            }
        }

        public static DbType Static_GetCommonDbTypeFromConfigStr(string DBType)
        {
            switch (DBType)
            {
                case "DbType.String":
                case "String":
                case "16":
                default:
                    return DbType.String;
                case "DbType.Int32":
                case "Int32":
                case "11":
                    return DbType.Int32;
                case "DbType.Decimal":
                case "Decimal":
                case "7":
                    return DbType.Decimal;
                case "DbType.Binary":
                case "Binary":
                case "1":
                    return DbType.Binary;
                case "DbType.DateTime":
                case "DateTime":
                case "6":
                    return DbType.DateTime;

            }
        }

        public static ParameterDirection Static_GetDirectionFromConfigStr(string spDirection)
        {
            switch (spDirection)
            {
                case "1":
                case "Input":
                default:
                    return ParameterDirection.Input;
                case "2":
                case "Output":
                    return ParameterDirection.Output;
                case "3":
                case "InputOutput":
                    return ParameterDirection.InputOutput;
            }
        }
    }

}
