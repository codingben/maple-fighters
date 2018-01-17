using System;
using ComponentModel.Common;
using Game.InterestManagement;
using IronPython.Hosting;
using MathematicsHelper;
using Microsoft.Scripting.Hosting;
using Physics.Box2D;

namespace PythonScripting
{
    public class PythonScriptEngine : Component, IPythonScriptEngine
    {
        private const string ENVIRONMENT_PYTHON_LIB_NAME = "IRON_PYTHON_LIB_DIR";
        private readonly ScriptEngine scriptEngine;

        public PythonScriptEngine()
        {
            scriptEngine = Python.CreateEngine();

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                scriptEngine.Runtime.LoadAssembly(assembly);
            }

            scriptEngine.Runtime.LoadAssembly(typeof(Vector2).Assembly);
            scriptEngine.Runtime.LoadAssembly(typeof(SceneObject).Assembly);
            scriptEngine.Runtime.LoadAssembly(typeof(PhysicsWorldInfo).Assembly);
            scriptEngine.Runtime.LoadAssembly(typeof(IronPython.Modules.ArrayModule).Assembly);

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