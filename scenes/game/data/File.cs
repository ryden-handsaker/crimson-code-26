using System;
using Environment = System.Environment;

namespace CrimsonCode26.scenes.game.data;

public class File
{
    public string Path { get; }

    public string Name { get; set; }
    public string Extension { get; set; }
    public DateTime DateModified { get; }
    public DateTime DateCreated { get; }

    public static string TildeToHome(string path)
    {
        if (path.StartsWith('~'))
        {
            // GD.Print($"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}");
            return System.IO.Path.Combine([
                Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                path.Remove(0, 2) // cut off ~
            ]);
        }
        return System.IO.Path.GetFullPath(path);
    }

    public File(string path)
    {
        Path = TildeToHome(path);
        Name = System.IO.Path.GetFileNameWithoutExtension(Path);
        Extension = System.IO.Path.GetExtension(Path);
        DateCreated = System.IO.File.GetCreationTime(Path);
        DateModified = System.IO.File.GetLastWriteTime(Path);
    }
}