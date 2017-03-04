using log4net;
using log4net.Appender;
using log4net.Core;
using log4net.Layout;
using log4net.Repository.Hierarchy;
using System;
using System.IO;

namespace OCRCore.Common.Helper
{
    public class LogHelper
    {
        public static void initLogger(string ConnStringLog)
        {
            if ("localhost".Equals(ConnStringLog))
            {
                string CurrentDirectory = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                //output:localhost_yyyyMMdd.log
                string LogFilePath = Path.Combine(CurrentDirectory, "Logs\\OCRCore");
                LogHelper.onLocal(LogFilePath);
            }
            else
            {
                //LogHelper.onAzure(logConnString);
            }
        }

        //public static void onAzure(string logStorageConnectionString)
        //{
        //    Hierarchy hierarchy = (Hierarchy)LogManager.GetRepository();
        //    if (hierarchy == null)
        //    {
        //        throw new ApplicationException("Can't load log4net.");
        //    }

        //    PatternLayout patternLayout = new PatternLayout();
        //    patternLayout.ConversionPattern = "%date{yyyy-MM-dd HH:mm:ss.FFF}  [%thread] %-5level %logger:%line - %message%newline";
        //    patternLayout.ActivateOptions();

        //    AzureTableStorageAppender appender = new AzureTableStorageAppender();
        //    appender.Name = "AzureAppender";
        //    appender.Layout = patternLayout;
        //    appender.AzureStorageConnectionString = logStorageConnectionString;

        //    appender.Threshold = log4net.Core.Level.Info;
        //    appender.ActivateOptions();

        //    hierarchy.Root.AddAppender(appender);
        //    hierarchy.Root.Level = Level.Info;
        //    hierarchy.Configured = true;
        //}

        public static void onLocal(string logFilePath)
        {
            Hierarchy hierarchy = (Hierarchy)LogManager.GetRepository();
            if (hierarchy == null)
            {
                throw new ApplicationException("Can't load log4net.");
            }

            PatternLayout patternLayout = new PatternLayout();
            patternLayout.ConversionPattern = "%date{yyyy-MM-dd HH:mm:ss.FFF} [%thread] %-5level %logger:%line - %message%newline";
            patternLayout.ActivateOptions();

            //for local log
            RollingFileAppender appender = new RollingFileAppender();
            appender.Name = "LocalAppender";
            appender.File = logFilePath;
            appender.AppendToFile = true;
            appender.RollingStyle = RollingFileAppender.RollingMode.Date;
            appender.DatePattern = "'_'yyyyMMdd'.log'";
            appender.StaticLogFileName = false;
            //appender.MaxSizeRollBackups = 10;
            appender.Layout = patternLayout;

            appender.Threshold = log4net.Core.Level.Debug;
            appender.ActivateOptions();

            hierarchy.Root.AddAppender(appender);
            hierarchy.Root.Level = Level.Debug;
            hierarchy.Configured = true;
        }

        
    }
}
