using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.IO;
using Buffalo.Basic.Base;

namespace Buffalo.Basic.Data
{

    public enum ExcelConnectionType
    {
        NPOI=1,
        COMI=2
    }

    public class ExcelConnection
    {
        public string ConnectionName
        {
            set;
            get;
        }

        public string ExcelFile
        {
            set;
            get;
        }

        public bool CanWrite
        {
            set;
            get;
        }

        public ExcelConnectionType ConnectionType
        {
            set;
            get;
        }

        public virtual List<string> GetSheets()
        {
            List<string> result = new List<string>();
            return result;
        }

        public virtual string GetCellValue(int SheetIndex, int Column, int Row)
        {
            return "";
        }

        public virtual string GetCellValue(string SheetName, int Column, int Row)
        {
            return "";
        }

        public virtual void CreateWorkSheet(string SheetName)
        {

        }

        public virtual bool CreateWorkBook(string ExcelFile)
        {
            return true;
        }

        public virtual bool ConnectToExcel(string ExcelFile, bool isWrite)
        {
            return true;
        }

        public virtual bool SetCellValue(int SheetIndex, int Column, int Row,string Value)
        {
            return true;
        }

        public virtual bool SetCellValue(string SheetName, int Column, int Row,string Value)
        {
            return true;
        }

    }

    public class ExcelConnection_NPIO:ExcelConnection
    {

        private IWorkbook _iWorkBook;

        public bool ConnectionReady
        {
            set;
            get;
        }

        public FileStream ActiveFileStream
        {
            set;
            get;
        }

        public ExcelConnection_NPIO(string ConnectionName)
        {
            this.ConnectionName = ConnectionName;                     
            this.ConnectionType = ExcelConnectionType.NPOI;
            this.ConnectionReady = false;
        }

        public override bool ConnectToExcel(string ExcelFile, bool isWrite=true)
        {
            try
            {
                this.CanWrite = isWrite;
                if (CanWrite)
                    this.ActiveFileStream = File.OpenWrite(ExcelFile);
                else
                    this.ActiveFileStream = File.OpenRead(ExcelFile);
                this.ExcelFile = ExcelFile;  
                this.ConnectionReady = true;
                _iWorkBook = WorkbookFactory.Create(this.ActiveFileStream);
                return true;
            }
            catch(ExtendedExcptions err)
            {
                return false;
            }                
        }

        public override string GetCellValue(int SheetIndex, int Column, int Row)
        {
            if (SheetIndex < 0)
                SheetIndex = 0;
            ISheet activeSheet = _iWorkBook.GetSheetAt(SheetIndex);
            if (activeSheet != null)
            {
                IRow activeRow = activeSheet.GetRow(Row);
                if (activeRow != null)
                {
                    ICell activeCell = activeRow.GetCell(Column);
                    if (activeCell != null)
                        return activeCell.StringCellValue;
                    else
                        return "";
                }
                else
                    return "";
            }
            else
                return "";
        }

        public override string GetCellValue(string SheetName, int Column, int Row)
        {
            if (SheetName == "" && _iWorkBook==null)
                return "";
            ISheet activeSheet = _iWorkBook.GetSheet(SheetName);
            if (activeSheet != null)
            {
                IRow activeRow = activeSheet.GetRow(Row);
                if (activeRow != null)
                {
                    ICell activeCell = activeRow.GetCell(Column);
                    if (activeCell != null)
                        return activeCell.StringCellValue;
                    else
                        return "";
                }
                else
                    return "";
            }
            else
                return "";
        }

        public override List<string> GetSheets()
        {
            List<string> result = new List<string>();
            int countOfSheet = _iWorkBook.NumberOfSheets;
            if(countOfSheet>0)
            {
                for(int i=0;i<countOfSheet;i++)
                {
                    string name = _iWorkBook.GetSheetAt(i).SheetName;
                    result.Add(name);
                }
            }
            return result;
        }

        public override void CreateWorkSheet(string SheetName = "")
        {
            if (SheetName != "")
            {
                _iWorkBook.CreateSheet(SheetName);
            }
            else
                _iWorkBook.CreateSheet();
        } 

        public override bool CreateWorkBook(string ExcelFile)
        {
            if (ExcelFile != "")
            {
                try
                {
                    this.ActiveFileStream = File.Create(ExcelFile);
                    _iWorkBook = WorkbookFactory.Create(this.ActiveFileStream);
                    if (ConnectToExcel(ExcelFile, true))
                        return true;
                    else
                        return false;
                }
                catch(ExtendedExcptions err)
                {
                    return false;
                }
            }
            else
                return false;
        }

        public override bool SetCellValue(int SheetIndex, int Column, int Row,string Value)
        {
            if (_iWorkBook != null && SheetIndex >= 0)
            {
                ISheet activeSheet = _iWorkBook.GetSheetAt(SheetIndex);
                if (activeSheet != null)
                {
                    IRow activeRow = activeSheet.GetRow(Row);
                    if (activeRow == null)
                        activeRow = activeSheet.CreateRow(Row);
                    ICell activeCell = activeRow.GetCell(Column);
                    if (activeCell == null)
                        activeCell = activeRow.CreateCell(Column);
                    activeCell.SetCellValue(Value);
                    return true;
                }
                else
                    return false;
            }
            else
                return false;
        }

        public override bool SetCellValue(string SheetName, int Column, int Row, string Value)
        {
            if (_iWorkBook != null && SheetName != "")
            {
                ISheet activeSheet = _iWorkBook.GetSheet(SheetName);
                if (activeSheet != null)
                {
                    IRow activeRow = activeSheet.GetRow(Row);
                    if (activeRow == null)
                        activeRow = activeSheet.CreateRow(Row);
                    ICell activeCell = activeRow.GetCell(Column);
                    if (activeCell == null)
                        activeCell = activeRow.CreateCell(Column);
                    activeCell.SetCellValue(Value);
                    return true;
                }
                else
                    return false;
            }
            else
                return false;
        }

    }

    public class ExcelConnection_COMI : ExcelConnection
    {

        public bool ConnectionReady
        {
            set;
            get;
        }
    }

    public class ExcelConnectionHelper
    {
        private Dictionary<string,ExcelConnection> Pool_ExcelConnection=new Dictionary<string,ExcelConnection>();

        public void AddNewExcelConnection(string ConnectionName, string ExcelFile, ExcelConnectionType ConnectionType = ExcelConnectionType.NPOI, bool isWrite = true)
        {
            if (ConnectionType == ExcelConnectionType.NPOI)
            {
                if (!Pool_ExcelConnection.ContainsKey(ConnectionName))
                {
                    ExcelConnection_NPIO newItem = new ExcelConnection_NPIO(ConnectionName);
                    if (newItem.ConnectToExcel(ExcelFile, true))
                        Pool_ExcelConnection.Add(ConnectionName, newItem);
                }              
            }
        }

        public ExcelConnection GetConnection(string ConnectionName)
        {
            if(ConnectionName!="")
            {
                if (Pool_ExcelConnection.ContainsKey(ConnectionName))
                {
                    return Pool_ExcelConnection[ConnectionName];
                }
                else
                    return null;
            }
            else
            {
                return null;
            }
        }

    }

}
