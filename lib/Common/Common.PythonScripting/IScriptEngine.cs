using Microsoft.Scripting.Hosting;

namespace Common.PythonScripting
{
    public interface IScriptEngine
    {
        ScriptEngine GetScriptEngine();
    }
}