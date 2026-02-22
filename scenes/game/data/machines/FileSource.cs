using System.ComponentModel;
using System.IO;
using Godot;

namespace CrimsonCode26.scenes.game.data.machines;

public class FileSource : Machine
{
    public override void Process(File file)
    {
        if (ProcessFile != null) throw new InvalidAsynchronousStateException(); // eh
        
        ProcessFile = file;
        Push(Outputs["Output"]);
    }

    private void FileCreated(object sender, FileSystemEventArgs e)
    {
        GD.Print("File added: {0}", e.Name);
        Enqueue(new File(e.FullPath));
    }

    public FileSource(string path)
    {
        Initialize("File Source");
        Belt = new Belt(this, 100);

        foreach (string file in Directory.GetFiles(path))
        {
            if (!Enqueue(new File(file)))
            {
                GD.PrintErr("Maximum file count exceeded"); // TODO: just a current limitation of the architecture
            }
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

}