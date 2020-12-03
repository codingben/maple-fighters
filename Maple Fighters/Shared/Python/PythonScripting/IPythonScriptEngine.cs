using Microsoft.Scripting.Hosting;

namespace PythonScripting
{
    public interface IPythonScriptEngine
    {
        ScriptEngine GetScriptEngine();
    }
}