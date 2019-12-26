using System;
using System.Collections.Generic;
using System.Reflection;
using IronPython.Hosting;
using IronPython.Modules;
using Microsoft.Scripting.Hosting;

namespace Common.PythonScripting
{
    public class PythonScriptEngine : IScriptEngine
    {
        private const string EnviVariable = "IRON_PYTHON_LIB";
        private ScriptEngine scriptEngine;

        public PythonScriptEngine()
        {
            SetScriptEngine();
            SetSearchPaths();
        }

        private void SetScriptEngine()
        {
            scriptEngine = Python.CreateEngine();

            var assemblies =
                new List<Assembly>
                {
                    typeof(ArrayModule).Assembly
                };
            assemblies.AddRange(AppDomain.CurrentDomain.GetAssemblies());

            foreach (var assembly in assemblies)
            {
                scriptEngine.Runtime.LoadAssembly(assembly);
            }
        }

        private void SetSearchPaths()
        {
            var target = EnvironmentVariableTarget.Machine;
            var path =
                Environment.GetEnvironmentVariable(EnviVariable, target);
            var paths = scriptEngine.GetSearchPaths();
            paths.Add(path);

            scriptEngine.SetSearchPaths(paths);
        }

        public ScriptEngine GetScriptEngine()
        {
            return scriptEngine;
        }
    }
}