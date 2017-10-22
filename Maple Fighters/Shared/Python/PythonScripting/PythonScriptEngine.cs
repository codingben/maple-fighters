using System;
using ComponentModel.Common;
using Game.InterestManagement;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using ServerApplication.Common.ApplicationBase;

namespace PythonScripting
{
    public class PythonScriptEngine : Component<IServerEntity>, IPythonScriptEngine
    {
        private readonly ScriptEngine scriptEngine;

        public PythonScriptEngine()
        {
            scriptEngine = Python.CreateEngine();

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                scriptEngine.Runtime.LoadAssembly(assembly);
            }

            scriptEngine.Runtime.LoadAssembly(typeof(SceneObject).Assembly);
        }

        public ScriptEngine GetScriptEngine()
        {
            return scriptEngine;
        }
    }
}