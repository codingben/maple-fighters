using ComponentModel.Common;
using Microsoft.Scripting.Hosting;

namespace PythonScripting
{
    public interface IPythonScriptEngine : IExposableComponent
    {
        ScriptEngine GetScriptEngine();
    }
}