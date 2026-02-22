using System.Collections.Generic;
using System.ComponentModel;
using CrimsonCode26.scenes.game.data;

namespace CrimsonCode26.scenes.game.data.machines;

public class ExtensionFilter : Machine
{
    public List<string> Extensions { get; private set; }
    
    public override void Process(File file)
    {
        if (ProcessFile != null) throw new InvalidAsynchronousStateException(); // eh
        
        ProcessFile = file;
        foreach (string extension in Extensions)
        {
            if (file.Extension.Equals(extension))
            {
                Outputs["Pass"].Enqueue(file);
                ProcessFile = null;
                return;
            }
        }
        Outputs["Fail"].Enqueue(file);
        ProcessFile = null;
    }

    public ExtensionFilter()
    {
        this.Initialize("Extension Filter");
    }
}