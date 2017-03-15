using System;
using System.Collections;
using System.Data;
using System.IO;
using System.Reflection;
using Aspose.Cells;

namespace Tmc.Util.Excel
{
    public class TemplateExcelUtil
    {
        /// <summary>
        /// 根据模板文件及相关的变量生成Excel工作薄文件
        /// </summary>
        /// <param name="templateFileStream">模板文件流</param>        
        /// <param name="variables">简单变量集合</param>
        /// <param name="arrayVariables">数组变量集合</param>
        public static Workbook CreateWorkbookByTemplate(Stream templateFileStream, Variable[] variables, ArrayVariable[] arrayVariables)
        {
            return CreateWorkbookByTemplate(new Workbook(templateFileStream), variables, arrayVariables);
        }

        /// <summary>
        /// 根据模板文件及相关的变量生成Excel工作薄文件
        /// </summary>
        /// <param name="templateFileFullName">模板文件的物理路径</param>        
        /// <param name="variables">简单变量集合</param>
        /// <param name="arrayVariables">数组变量集合</param>
        public static Workbook CreateWorkbookByTemplate(string templateFileFullName, Variable[] variables, ArrayVariable[] arrayVariables)
        {
            return CreateWorkbookByTemplate(new Workbook(templateFileFullName), variables, arrayVariables);            
        }

        private static Workbook CreateWorkbookByTemplate(Workbook templateWorkBook, Variable[] variables, ArrayVariable[] arrayVariables)
        {
            var designer = new WorkbookDesigner
            {
                Workbook = templateWorkBook
            };

            if (variables != null)
            {
                foreach (var item in variables)
                {
                    designer.SetDataSource(item.Name, item.Data);
                }
            }
            if (arrayVariables != null)
            {
                foreach (var item in arrayVariables)
                {
                    DataTable dt = ToDataTable(item.Data, item.Name);
                    designer.SetDataSource(dt);
                }
            }

            //输出到Http响应流
            designer.Process();
            return designer.Workbook;
        }

        private static DataTable ToDataTable(IEnumerable varlist, string tableName)
        {
            DataTable dtReturn = new DataTable(tableName);

            PropertyInfo[] oProps = null;
            foreach (var rec in varlist)
            {
                // Use reflection to get property names, to create table, Only first time, others will follow
                if (oProps == null)
                {
                    oProps = rec.GetType().GetProperties();
                    foreach (PropertyInfo pi in oProps)
                    {
                        Type colType = pi.PropertyType;
                        //处理DataTable不支持范型的问题
                        if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                        {
                            colType = colType.GetGenericArguments()[0];
                        }
                        dtReturn.Columns.Add(new DataColumn(pi.Name, colType));
                    }
                }
                DataRow dr = dtReturn.NewRow();
                foreach (PropertyInfo pi in oProps)
                {
                    object val = pi.GetValue(rec, null);
                    dr[pi.Name] = val ?? DBNull.Value;
                }
                dtReturn.Rows.Add(dr);
            }
            return dtReturn;
        }
    }
}
