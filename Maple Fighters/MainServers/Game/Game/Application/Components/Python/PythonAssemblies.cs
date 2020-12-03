using System.Collections.Generic;
using System.Reflection;
using CommonTools.Log;
using ComponentModel.Common;
using PythonScripting;
using ServerApplication.Common.ApplicationBase;

namespace Game.Application.Components.Python
{
    internal class PythonAssemblies : Component
    {
        protected override void OnAwake()
        {
            base.OnAwake();

            AddAssembliesToPytonScriptEngine();
        }

        /// <summary>
        /// Getting assemblies which can not be found by python script engine
        /// </summary>
        private void AddAssembliesToPytonScriptEngine()
        {
            var pythonScriptEngine = ServerComponents.GetComponent<IPythonScriptEngine>().AssertNotNull();
            var assemblies = GetPythonScriptEngineAssemblies();

            foreach (var assembly in assemblies)
            {
                pythonScriptEngine.GetScriptEngine().Runtime.LoadAssembly(assembly);
            }

            IEnumerable<Assembly> GetPythonScriptEngineAssemblies()
            {
                yield return typeof(MathematicsHelper.Vector2).Assembly;
                yield return typeof(InterestManagement.Components.SceneObject).Assembly;
                yield return typeof(Physics.Box2D.Core.PhysicsWorldInfo).Assembly;
            }
        }
    }
}