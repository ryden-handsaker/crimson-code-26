using System;
using System.Collections.Generic; 
using CrimsonCode26.scenes.game.data;

namespace CrimsonCode26.scenes.game.data;

public abstract class Machine
{
    public string Name { get; }
    public string Id { get; }
    
    public File ProcessFile { get; protected set; }
    
    protected readonly Dictionary<string, Machine> _outputs = new();
    protected Belt _belt;
    
    public abstract void Process(File file);
        // await input belt
        // Do processing
        // await Outputs.get("Success").receive(file);

    public bool AddConnection(string name, Machine machine) =>  machine == null ? throw new ArgumentNullException(nameof(machine)) : _outputs.TryAdd(name, machine);

    public bool Enqueue(File file) => _belt.Enqueue(file);
}