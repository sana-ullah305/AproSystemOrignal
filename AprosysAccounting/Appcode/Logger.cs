using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace AprosysAccounting.Appcode
{
    public class Logger
    {
        public static readonly object lockObj = new object();
        public Logger()
        {
            //default constructor
        }
        public enum LogType
        {
            InformationLog,
            ErrorLog
        }
        static string LastLogFileName = null;
        static StreamWriter lastLogWriter = null;
        public static void Write(string Title, string Message, string stkTrace, LogType Log)
        {
            lock (lockObj)
            {


                string BaseDir = AppDomain.CurrentDomain.BaseDirectory;

                //Creating Error Directory
                string dirpath = System.IO.Path.Combine(BaseDir, "Error\\");
                if (!System.IO.Directory.Exists(dirpath))
                    System.IO.Directory.CreateDirectory(dirpath);
                //Creating Log Directory
                string dirpathlog = System.IO.Path.Combine(BaseDir, "Logs\\");
                if (!System.IO.Directory.Exists(dirpathlog))
                    System.IO.Directory.CreateDirectory(dirpathlog);

                FileStream fs = default(FileStream);
                StreamWriter logger = null;
                if (Log == LogType.ErrorLog)
                {
                    fs = new FileStream(dirpath + DateTime.Now.ToString("MM-dd-yyyy") + ".txt", FileMode.Append, FileAccess.Write, FileShare.Write);
                    logger = new StreamWriter(fs);
                }
                else
                {
                    string FilePath = dirpathlog + DateTime.Now.ToString("MM-dd-yyyy") + ".txt";
                    if (lastLogWriter != null && LastLogFileName == FilePath)
                    {
                        logger = lastLogWriter;
                    }
                    else
                    {
                        if (lastLogWriter != null)
                        {
                            lastLogWriter.Close();
                        }
                        fs = new FileStream(FilePath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
                        logger = new StreamWriter(fs);
                        logger.AutoFlush = true;
                        LastLogFileName = FilePath;
                        lastLogWriter = logger;

                    }
                }


                logger.WriteLine(DateTime.Now.ToString("MM-dd-yyyy HH:mm:ss") + " - " + Log.ToString() + " : " + Title + " - " + Message);
                Console.WriteLine(DateTime.Now.ToString("MM-dd-yyyy HH:mm:ss") + " - " + Log.ToString() + " : " + Title + " - " + Message);

                if (Log == LogType.ErrorLog)
                {
                    logger.WriteLine("Error: " + stkTrace);
                    Console.WriteLine("Error: " + stkTrace);
                    logger.Close();
                    fs.Close();

                }

            }
        }
    }
}