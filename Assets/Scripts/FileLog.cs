using UnityEngine;
using System;
using System.IO;


public class FileLog
{
    public static readonly string FILENAME = "DebugLog.log";
    public static readonly string PREV_FILENAME = "DebugLog-prev.log";
    public static readonly string ZIP_FILENAME = "DebugLog.log.gz";
    public static readonly string MONTHLY_FILENAME_FORMAT = "DebugLogDay{0:00}.log";
    public static readonly string LAST_LOG_DAY_FILE = "LastLogDay.log";

    static string logFilePath;
    static string prevLogFilePath;
    static string zipFilePath;
    static string logMonthlyFilePath;
    static string lastLogDayFilePath;

    static readonly int TRIM_LINE_LENGTH = 255;

    private static StreamWriter sw = null;

    public static bool writeFile = true;

    public static bool trimLine = false;

    public static bool monthlyLog = false;
    protected static int curDay = 0;


    static FileLog()
    {
        string curPath = Application.dataPath + "/..";
        if (Application.platform == RuntimePlatform.OSXPlayer)
            curPath += "/../..";

        // Set curPath under Logs folder
        curPath += "/Logs";

        if (!Directory.Exists(curPath))
        {
            //if it doesn't exists, create it
            Directory.CreateDirectory(curPath);
        }

        logFilePath = curPath + "/" + FILENAME;
        prevLogFilePath = curPath + "/" + PREV_FILENAME;
        zipFilePath = curPath + "/" + ZIP_FILENAME;

        curDay = System.DateTime.Now.Day;
        logMonthlyFilePath = curPath + "/" + string.Format(MONTHLY_FILENAME_FORMAT, curDay);

        lastLogDayFilePath = curPath + "/" + LAST_LOG_DAY_FILE;
    }

    public static void Enable()
    {
        Application.logMessageReceivedThreaded += LogMessageReceivedHandler;

        UnityEngine.Debug.Log("===============================================================================");
        UnityEngine.Debug.Log("Program Start: Time: " + System.DateTime.Now.ToString());
    }

    public static void Disable()
    {
        UnityEngine.Debug.Log("FileLog: Disable");

        Application.logMessageReceivedThreaded -= LogMessageReceivedHandler;

        Close();
    }

    public static void LogMessageReceivedHandler(string logString, string stackTrace, LogType type)
    {
        if (type != LogType.Log)
            WriteLogFile("[Time] " + System.DateTime.Now.ToString());

        if (trimLine && (logString.Length > TRIM_LINE_LENGTH))
            WriteLogFile(type.ToString() + ": " + logString.Substring(0, TRIM_LINE_LENGTH) + "...[TRIMMED]");
        else
            WriteLogFile(type.ToString() + ": " + logString);

        if (type == LogType.Exception)
            WriteLogFile("CallStack: " + stackTrace);
    }

    public static void CreateNewLog()
    {
        try
        {
            sw = File.CreateText(logFilePath);
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }

    public static void OpenMonthlyLog()
    {
        // Read last log day
        string lastLogDayText = "0";
        try
        {
            lastLogDayText = File.ReadAllText(lastLogDayFilePath);
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }


        Debug.Log("lastLogDayText: " + lastLogDayText);

        int lastDay = 0;
        int.TryParse(lastLogDayText, out lastDay);

        Debug.Log("lastDay: " + lastDay);

        try
        {
            if (lastDay != curDay)
            {
                File.WriteAllText(lastLogDayFilePath, curDay.ToString());

                Debug.Log("Create new log file");

                sw = File.CreateText(logMonthlyFilePath);
            }
            else
            {
                Debug.Log("Use previous log file");

                sw = File.AppendText(logMonthlyFilePath);
            }
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }

    }

    public static void Close()
    {
        sw = null;
    }

    public static void BackupPrevLog()
    {
        Debug.LogWarning("Info: BackupPrevLog");

        try
        {
            if (File.Exists(logFilePath))
            {
                //Archiver.Compress(logFilePath, zipFilePath);

                System.IO.File.Copy(logFilePath, prevLogFilePath, true);
            }
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }

    /*
        public static void CompressPrevLog()
        {
            Debug.LogWarning("Info: CompressPrevLog");

            try
            {
                if (File.Exists(prevLogFilePath))
                {
                    Tracer tracer = new Tracer("CompressPrevLog");

                    Archiver.Compress(prevLogFilePath, zipFilePath);

                    tracer.Exit();
                    tracer = null;
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
    */

    public static string GetZipLogFilePath()
    {
        return zipFilePath;
    }

    public static bool CheckZipLogFileExists()
    {
        return File.Exists(zipFilePath);
    }

    public static void TrimLog(string msg)
    {
        if ((Application.platform == RuntimePlatform.OSXEditor) || (Application.platform == RuntimePlatform.WindowsEditor))
        {
            UnityEngine.Debug.Log(msg);
        }
        else
        {
            if (msg.Length > TRIM_LINE_LENGTH)
                UnityEngine.Debug.Log(msg.Substring(0, TRIM_LINE_LENGTH) + "...[TRIMMED]");
            else
                UnityEngine.Debug.Log(msg);
        }
    }

    public static void WriteLogFile(string line)
    {
        if (!writeFile)
            return;

        if (sw == null)
            sw = File.AppendText(logFilePath);

        try
        {
            sw.WriteLine(line);

            sw.Flush();
        }
        catch (Exception e)
        {
            UnityEngine.Debug.LogError("WriteLogFile Exception:" + e.ToString());
        }
    }

}
