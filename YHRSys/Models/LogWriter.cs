using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;

namespace YHRSys.Models
{
    public class LogWriter
    {
        private string m_exePath = string.Empty;
        String today = DateTime.Now.ToString("dd_MM_yyyy");
        private string p;

        public LogWriter(string logMessage, string obj)
        {
            LogWrite(logMessage, obj);
        }

        public void LogWrite(string logMessage, string obj)
        {
            m_exePath = AppDomain.CurrentDomain.BaseDirectory;
            try
            {
                using (StreamWriter w = File.AppendText(m_exePath + "\\SysLog\\" + obj + "_" + today + "_log.txt"))
                {
                    Log(logMessage, w);
                }
            }
            catch (Exception ex)
            {
                StreamWriter w = File.AppendText(m_exePath + "\\SysLog\\" + obj + "_" + today + "_log.txt");
                Log(ex.Message, w);
            }
        }

        public void Log(string logMessage, TextWriter txtWriter)
        {
            try
            {
                txtWriter.Write("\r\nLog Entry : ");
                txtWriter.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(),
                    DateTime.Now.ToLongDateString());
                txtWriter.WriteLine("  :");
                txtWriter.WriteLine("  :{0}", logMessage);
                txtWriter.WriteLine("-------------------------------");
            }
            catch (Exception ex)
            {
                txtWriter.Write("\r\nLog Entry : ");
                txtWriter.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(),
                    DateTime.Now.ToLongDateString());
                txtWriter.WriteLine("  :");
                txtWriter.WriteLine("  :{0}", ex.Message);
                txtWriter.WriteLine("-------------------------------");
            }
        }
    }
}