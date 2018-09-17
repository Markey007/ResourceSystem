using Excel;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace ResourceSystem
{
    public class ExcelStream : IResourceReader,IResourceWriter
    {
        IExcelDataReader excelReader;
        
        FileInfo fileInfo;

        FileStream stream;

        string[,] content;

        DataSet result;

        public ExcelStream()
        {

        }

        public object ReadResouce(string path)
        {
            ReadExcel(path);
            
            return result.Tables;
        }

        private void ReadExcel(string path)
        {
            fileInfo = new FileInfo(path);
            
            stream = fileInfo.Open( FileMode.Open, FileAccess.Read);
            excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
            result = excelReader.AsDataSet();
            excelReader.Close();

            //while (excelReader.Read())
            //{
            //    DebugUtils.DebugRedInfo(excelReader.FieldCount.ToString());
            //    DebugUtils.DebugRedInfo(contentlist.Count.ToString());
            //    for (int i = 0; i < excelReader.FieldCount; i++)
            //    {
            //        DebugUtils.DebugRedInfo(i.ToString()+ excelReader.GetString(i));
            //        contentlist[i].Add(excelReader.GetString(i));
            //    }
            //}
            //if(contentlist.Count>0&& contentlist[0].Count > 0)
            //{
            //    for (int i = 0; i < contentlist.Count; i++)
            //    {
            //        for (int j = 0; j < contentlist[i].Count; j++)
            //        {
            //            content[]
            //        }
            //    }
            //}
            //else
            //{
            //    DebugUtils.DebugError("数据为空");
            //}

        }
        

        public bool WriteResource(IWriteArgs data)
        {
           return WriteExcel((ExcelWriteArgs)data);
        }

        private bool WriteExcel(ExcelWriteArgs data)
        {
            FileInfo newFile = new FileInfo(data.Path);
            if (!newFile.Exists)
            {
                //创建一个新的excel文件
                newFile.Delete();
                newFile = new FileInfo(data.Path);
                using (ExcelPackage package = new ExcelPackage(newFile))
                {
                    //在excel空文件添加新sheet
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(data.SheetName);
                    //添加列名

                    int columnNum = data.Content.GetLength(1);
                    int rowNum = data.Content.Length / columnNum;
                    DebugUtils.DebugError(rowNum.ToString());
                    DebugUtils.DebugError(columnNum.ToString());
                    for (int i = 0; i < rowNum; i++)
                    {
                        for (int j = 0; j < columnNum; j++)
                        {
                            worksheet.Cells[i + 4, j + 4].Value = data.Content[i, j];
                        }
                    }
                    //保存excel
                    package.Save();
                }
            }
            else
            {
                using (ExcelPackage package = new ExcelPackage(newFile))
                {
                    ExcelWorksheet worksheet=null;
                    //在excel空文件添加新sheet
                    for (int i = 1; i <= package.Workbook.Worksheets.Count; i++)
                    {

                        if (package.Workbook.Worksheets[i].Name == data.SheetName)
                        {
                            worksheet = package.Workbook.Worksheets[i];
                            break;
                        }
                    }
                    if (worksheet == null)
                    {
                        worksheet = package.Workbook.Worksheets.Add(data.SheetName);
                    }

                    //添加列名

                    int columnNum = data.Content.GetLength(1);
                    int rowNum = data.Content.Length / columnNum;
                    for (int i = 0; i < rowNum; i++)
                    {
                        for (int j = 0; j < columnNum; j++)
                        {
                            worksheet.Cells[i + 4, j+1 ].Value = data.Content[i, j];
                        }
                    }
                    //保存excel
                    package.Save();
                }
            }
            return true;
        }
    }
}
