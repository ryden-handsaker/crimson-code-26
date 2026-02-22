using System;
using System.IO;
using Godot;

namespace CrimsonCode26.scenes.game.data;

public class File
{
    public string Path { get; }

    public string Name { get; set; }
    public string Extension { get; set; }
    public DateTime DateModified { get; }
    public DateTime DateCreated { get; }

    public File(string path)
    {
        Path = System.IO.Path.GetFullPath(path);
        Name = System.IO.Path.GetFileNameWithoutExtension(Path);
        Extension = System.IO.Path.GetExtension(Path);
        DateCreated = System.IO.File.GetCreationTime(Path);
        DateModified = System.IO.File.GetLastWriteTime(Path);
    }
}