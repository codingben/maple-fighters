using System;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using ServerApplication.Common.ApplicationBase;
using ServerApplication.Common.ComponentModel;

namespace PythonScripting
{
    public class PythonScriptEngine : Component<IServerEntity>
    {
        private readonly ScriptEngine scriptEngine;

        public PythonScriptEngine()
        {
            scriptEngine = Python.CreateEngine();

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                scriptEngine.Runtime.LoadAssembly(assembly);
            }
        }

        public ScriptEngine GetScriptEngine()
        {
            return scriptEngine;
        }
    }
}