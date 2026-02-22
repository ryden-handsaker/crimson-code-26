using System;
using System.ComponentModel;
using System.IO;
using System.Text.Json.Nodes;

namespace CrimsonCode26.scenes.game.data.machines;

public class FolderSource : Machine, ISerializable<FolderSource>
{
    public bool Initialized { get; private set; }
    public string Path { get; private set; }
    
    public override void Process(File file)
    {
        if (ProcessFile != null) throw new InvalidAsynchronousStateException(); // eh
        
        ProcessFile = file;
        Push(Outputs["Output"]);
    }

    private void FileCreated(object sender, FileSystemEventArgs e)
    {
        Godot.GD.Print("File added: {0}", e.Name);
        Enqueue(new File(e.FullPath));
    }

    public FolderSource(Guid guid)
    {
        Initialize(guid);
        Belt = new Belt(this, 500);
        Initialized = false;
    }
    
    private void ReadFiles()
    {
        var pathToSearch = File.TildeToHome(Path);
        if (!System.IO.Directory.Exists(pathToSearch))
        {
            Godot.GD.PrintErr($"{pathToSearch} does not exist");
            BadState = true;
            return;
        }
        
        foreach (var file in Directory.GetFiles(pathToSearch))
        {
            if (!Enqueue(new File(file)))
                // HACK: limitation of the architecture for  big folders
                Godot.GD.PrintErr("Maximum file count exceeded"); 
        }

        // https://learn.microsoft.com/en-us/dotnet/api/system.io.filesystemwatcher?view=net-10.0 
        using var watcher = new FileSystemWatcher(pathToSearch);

        // some of these filters are definitely unnecessary
        watcher.NotifyFilter = NotifyFilters.Attributes
                               | NotifyFilters.CreationTime
                               | NotifyFilters.DirectoryName
                               | NotifyFilters.FileName
                               | NotifyFilters.LastAccess
                               | NotifyFilters.LastWrite
                               | NotifyFilters.Security
                               | NotifyFilters.Size;

        watcher.Changed += FileCreated; // maybe someday
        watcher.Created += FileCreated;

        // watcher.IncludeSubdirectories = true;
        watcher.EnableRaisingEvents = true;
        Initialized = true;
    }
    
    public static FolderSource CreateFromJSON(Guid guid, JsonObject json)
    {
        var machine = new FolderSource(guid);

        machine.Path = json["path"]?.GetValue<string>();

        return machine;
    }

    public override void Tick()
    {
        if (!Initialized && !BadState)
            ReadFiles();
        Belt.Tick();
    }
}