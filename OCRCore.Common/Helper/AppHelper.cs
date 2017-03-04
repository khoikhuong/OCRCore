using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;

namespace OCRCore.Common.Helper
{
    public static class AppHelper
    {
        
        private static string _CURRENT_IP = null;
        public static string GetServerIP()
        {
            try
            {
                if (string.IsNullOrEmpty(_CURRENT_IP))
                {
                    //This returns the first IP4 address or null                
                    IPAddress oIPAddress = Dns.GetHostEntry(Dns.GetHostName()).AddressList.FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork);
                    if (oIPAddress != null) _CURRENT_IP = oIPAddress.ToString();
                    else _CURRENT_IP = Dns.GetHostName();
                }
            }
            catch (Exception ex) { Console.Write(ex.Message); }

            return _CURRENT_IP;
        }

        public static string GetExceptionMessage(Exception ex)
        {
            if (ex == null) return string.Empty;

            string error = string.Empty;
            if (ex.InnerException != null)
            {
                error = ToString(ex.InnerException.Message);
                
            }
            else
            {
                error = ToString(ex.Message);
            }
            return error;
        }

        /// <summary>
        /// Convert the list to DataTable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <returns></returns>
        public static DataTable ToDataTable<T>(IList<T> items)
        {
            if (items == null) return null;
            DataTable dataTable = new DataTable();

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.DeclaredOnly
                                                | BindingFlags.Instance
                                                | BindingFlags.Public
                                                | BindingFlags.GetField
                                                | BindingFlags.SetField);
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }

        public static string ToString(string value)
        {
            string result = string.Empty;
            try
            {
                if (value != null) result = Convert.ToString(value);
            }
            catch (Exception ex) { Console.Write(ex); }
            return result.Trim();
        }

        public static string ToString(object value)
        {
            string result = string.Empty;
            try
            {
                if (value != null) result = Convert.ToString(value);
            }
            catch (Exception ex) { Console.Write(ex); }
            return result.Trim();
        }

        public static int ToInt(int? input)
        {
            if (input.HasValue) return input.Value;
            return 0;
        }

        public static int ToInt(object value)
        {
            int result = 0;
            try
            {
                if (value != null) result = Convert.ToInt32(value);
            }
            catch (Exception ex) { Console.Write(ex); }
            return result;
        }

        public static bool IsNumeric(object Expression)
        {
            double retNum = 0.0;
            return IsNumeric(Expression, ref retNum);
        }

        public static bool IsNumeric(object Expression, ref double retNum)
        {
            bool isNum = false;
            try
            {
                string expressionValue = ToString(Expression);
                if (!"NaN".Equals(expressionValue, StringComparison.OrdinalIgnoreCase))
                {
                    isNum = Double.TryParse(Convert.ToString(Expression), System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out retNum);
                }
            }
            catch (Exception ex) { Console.WriteLine(ex); }

            return isNum;
        }

        public static bool IsNumeric(object Expression, ref decimal retNum)
        {
            bool isNum = false;
            try
            {
                isNum = Decimal.TryParse(Convert.ToString(Expression), System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out retNum);
            }
            catch (Exception ex) { Console.WriteLine(ex); }

            return isNum;
        }

        public static void Main()
        {
            //string s = "                                                                     30539";
            //Console.WriteLine("TEST=" + s.GetText(68, 6));
            //Console.WriteLine("TEST=" + s.GetText(68, 6, "Test Field"));

            //IList<string> returnErrors = null;
            //Console.WriteLine("TEST=" + s.GetText(68, 16, "Test Field", returnErrors));
            //Console.WriteLine(string.Join(";", returnErrors));
        }
    }
}