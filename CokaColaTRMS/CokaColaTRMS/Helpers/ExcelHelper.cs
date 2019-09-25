using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CokaColaTRMS.Models;
using System.Data.OleDb;
using System.Data;

namespace CokaColaTRMS.Helpers
{
   public class ExcelHelper
   {

       #region constants

       private const int ContactNameColumnNumber = 1;
       private const int ContactNumberColumnNumber = 2;
       private const int MessageColumnNumber = 3;
       private const int UniqueIDColumnNumber = 0;
       #endregion


       public static List<ImportContactsModel> GetContactsFromExcel(string filePath)
       {
           try
           {
               List<ImportContactsModel> tempReturnList = new List<ImportContactsModel>();
               OleDbConnection cnn = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" +
                   filePath + "; Extended Properties=Excel 12.0;");
               OleDbCommand oconn = new OleDbCommand("select * from [Sheet1$]", cnn);
               cnn.Open();
               OleDbDataAdapter adp = new OleDbDataAdapter(oconn);
               DataTable dt = new DataTable();
               adp.Fill(dt);

               if (dt.Rows.Count.Equals(0))
               {
                   return new List<ImportContactsModel>();
               }

               for (int i = 0; i < dt.Rows.Count; i++)
               {
                   tempReturnList.Add(new ImportContactsModel
                   {
                       UniqueID = GetCellValue(dt,i,UniqueIDColumnNumber),
                       Name = GetCellValue(dt,i,ContactNameColumnNumber),
                       TelephoeNumber = GetCellValue(dt, i, ContactNumberColumnNumber),
                       Message = GetCellValue(dt, i, MessageColumnNumber),
                       MessageSent = false
                   });
               }

               return tempReturnList;
           }
           catch (Exception)
           {
               return new List<ImportContactsModel>();
           }
       }

       private static string GetCellValue(DataTable table, int rowNumber, int columnNumber)
       {
           try
           {
               if (table==null)
               {
                   return string.Empty;
               }
               object cellValue = table.Rows[rowNumber][columnNumber];
               if (cellValue != null)
               {
                   return cellValue.ToString();
               }
               else
               {
                   return string.Empty;
               }
           }
           catch (Exception)
           {
               return string.Empty;
           }
       }
    }
}
