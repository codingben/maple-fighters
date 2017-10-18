using System;
using ComponentModel.Common;
using IronPython.Hosting;
using MathematicsHelper;
using Microsoft.Scripting.Hosting;
using ServerApplication.Common.ApplicationBase;

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
                scriptEngine.Runtime.LoadAssembly(typeof(Vector2).Assembly);
            }
        }

        public ScriptEngine GetScriptEngine()
        {
            return scriptEngine;
        }
    }
}