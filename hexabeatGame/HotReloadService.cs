using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;

public class HotReloadService
{
    public async Task RunScriptAsync(string code)
    {
        var scriptOptions = ScriptOptions.Default.AddReferences(AppDomain.CurrentDomain.GetAssemblies().Where(a => !a.IsDynamic));
        await CSharpScript.RunAsync(code, scriptOptions);
    }
}
