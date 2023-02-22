using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Text.RegularExpressions;

namespace Skiw.JO.Services.Db
{
    //Json Class Helper
    public class JCHelper
    {
        /// <summary>
        ///  Create JSON text from Class object, obj is Class object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ConvertToJson<T>(T obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
        /// <summary>
        /// Create a known Class object from JSON text, json is a valid JSON formatted text
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public static T ConvertFromJson<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }


        public static dynamic CreateDynamicObjectFromJson(string json)
        {
            dynamic dynamicObject = JsonConvert.DeserializeObject<ExpandoObject>(json);

            return dynamicObject;
        }


        public static DataTable ToDataTable(string json)
        {
            DataTable dtUsingMethodReturn = new DataTable();
            string[] jsonStringArray = Regex.Split(json.Replace("[", "").Replace("]", ""), "},{");
            List<string> ColumnsName = new List<string>();
            foreach (string strJSONarr in jsonStringArray)
            {
                string[] jsonStringData = Regex.Split(strJSONarr.Replace("{", "").Replace("}", ""), ",");
                foreach (string ColumnsNameData in jsonStringData)
                {
                    try
                    {
                        int idx = ColumnsNameData.IndexOf(":");
                        string ColumnsNameString = ColumnsNameData.Substring(0, idx).Replace("\"", "").Trim();
                        if (!ColumnsName.Contains(ColumnsNameString))
                        {
                            ColumnsName.Add(ColumnsNameString);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(string.Format("Error Parsing Column Name : {0}", ColumnsNameData));
                    }
                }
                break;
            }
            foreach (string AddColumnName in ColumnsName)
            {
                dtUsingMethodReturn.Columns.Add(AddColumnName);
            }
            foreach (string strJSONarr in jsonStringArray)
            {
                string[] RowData = Regex.Split(strJSONarr.Replace("{", "").Replace("}", ""), ",");
                DataRow nr = dtUsingMethodReturn.NewRow();
                foreach (string rowData in RowData)
                {
                    try
                    {
                        int idx = rowData.IndexOf(":");
                        string RowColumns = rowData.Substring(0, idx).Replace("\"", "").Trim();
                        string RowDataString = rowData.Substring(idx + 1).Replace("\"", "").Trim();
                        nr[RowColumns] = RowDataString;
                    }
                    catch (Exception ex)
                    {
                        continue;
                    }
                }
                dtUsingMethodReturn.Rows.Add(nr);
            }
            return dtUsingMethodReturn;
        }


    }
}