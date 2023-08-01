using UnityEngine;
using System.IO;
using System;

namespace TheSuperShell.Utils
{
    public class MyLogger : MonoBehaviour
    {
        string filename = "";

        private void Awake()
        {
            filename = Application.dataPath + "/LogFile.text";
        }

        private void OnEnable()
        {
            Application.logMessageReceived += Log;
        }

        private void OnDisable()
        {
            Application.logMessageReceived -= Log;
        }

        private void Log(string condition, string stackTrace, LogType type)
        {
            TextWriter tw = new StreamWriter(filename, true);

            tw.WriteLine("[" + DateTime.Now + "]" + condition);

            tw.Close();
        }
    }
}
