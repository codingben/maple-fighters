using System;
using ComponentModel.Common;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;

namespace PythonScripting
{
    using ArrayModule = IronPython.Modules.ArrayModule;

    public class PythonScriptEngine : Component, IPythonScriptEngine
    {
        private const string ENVIRONMENT_PYTHON_LIB_NAME = "IRON_PYTHON_LIB_DIR";
        private readonly ScriptEngine scriptEngine;

        public PythonScriptEngine()
        {
            scriptEngine = Python.CreateEngine();
            scriptEngine.Runtime.LoadAssembly(typeof(ArrayModule).Assembly);

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                scriptEngine.Runtime.LoadAssembly(assembly);
            }

            var ironPythonLibPath = Environment.GetEnvironmentVariable(ENVIRONMENT_PYTHON_LIB_NAME, EnvironmentVariableTarget.Machine);
            var paths = scriptEngine.GetSearchPaths();
            paths.Add(ironPythonLibPath);

            scriptEngine.SetSearchPaths(paths);
        }

        public ScriptEngine GetScriptEngine()
        {
            return scriptEngine;
        }
    }
}