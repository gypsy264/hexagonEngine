using System.IO;

public class ScriptWatcher
{
    private FileSystemWatcher _watcher;
    private HotReloadService _hotReloadService;

    public ScriptWatcher(string path, HotReloadService hotReloadService)
    {
        _hotReloadService = hotReloadService;
        _watcher = new FileSystemWatcher(path)
        {
            NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName,
            Filter = "*.cs",
            IncludeSubdirectories = true,
            EnableRaisingEvents = true
        };

        _watcher.Changed += OnChanged;
        _watcher.Created += OnChanged;
        _watcher.Deleted += OnChanged;
        _watcher.Renamed += OnRenamed;
    }

    private void OnChanged(object sender, FileSystemEventArgs e)
    {
        Console.WriteLine($"File {e.ChangeType}: {e.FullPath}");
        ReloadScript(e.FullPath);
    }

    private void OnRenamed(object sender, RenamedEventArgs e)
    {
        Console.WriteLine($"File renamed from {e.OldFullPath} to {e.FullPath}");
        ReloadScript(e.FullPath);
    }

    private void ReloadScript(string path)
    {
        var code = File.ReadAllText(path);
        _hotReloadService.RunScriptAsync(code).Wait();
    }
}
