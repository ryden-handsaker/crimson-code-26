using System;
using System.ComponentModel;
using System.IO;
using System.Text.Json.Nodes;
using Godot;

namespace CrimsonCode26.scenes.game.data.machines;

public class FolderSource : Machine, ISerializable<FolderSource>
{
    public override void Process(File file)
    {
        if (ProcessFile != null) throw new InvalidAsynchronousStateException(); // eh
        
        ProcessFile = file;
        Outputs["Output"].Enqueue(file);
        ProcessFile = null;
    }

    private void FileCreated(object sender, FileSystemEventArgs e)
    {
        GD.Print("File changed: {0}", e.Name);
        Enqueue(new File(e.FullPath));
    }

    public FolderSource(Guid guid)
    {
        Initialize("File Source", guid);
        Belt = new Belt(this, 100);
    }
    
    private void SetPath(string path) // allows you to change but is it necessary?
    {
        foreach (var file in Directory.GetFiles(path))
        {
            if (!Enqueue(new File(file)))
                GD.PrintErr("Maximum file count exceeded"); // TODO: just a current limitation of the architecture
        }

        // https://learn.microsoft.com/en-us/dotnet/api/system.io.filesystemwatcher?view=net-10.0 
        using var watcher = new FileSystemWatcher(path);

        // some of these filters are definitely unnecessary
        watcher.NotifyFilter = NotifyFilters.Attributes
                               | NotifyFilters.CreationTime
                               | NotifyFilters.DirectoryName
                               | NotifyFilters.FileName
                               | NotifyFilters.LastAccess
                               | NotifyFilters.LastWrite
                               | NotifyFilters.Security
                               | NotifyFilters.Size;

        // watcher.Changed += OnChanged; // maybe someday
        watcher.Created += FileCreated;

        // watcher.IncludeSubdirectories = true;
        watcher.EnableRaisingEvents = true;
    }
    
    public static FolderSource CreateFromJSON(Guid guid, JsonObject json)
    {
        var machine = new FolderSource(guid);
        
        machine.SetPath(json["path"]?.GetValue<string>());

        return machine;
    }
}