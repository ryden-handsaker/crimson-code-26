using System.ComponentModel;

namespace CrimsonCode26.scenes.game.data.machines;

public class OpenDestination : Machine
{
    public override void Process(File file)
    {
        if (ProcessFile != null) throw new InvalidAsynchronousStateException(); // eh

        ProcessFile = file;
        System.Diagnostics.Process.Start(file.Path);
        ProcessFile = null;
    }
}