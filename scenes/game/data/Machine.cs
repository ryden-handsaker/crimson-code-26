using System;
using System.Collections.Generic;
using CrimsonCode26.scenes.game.data;

namespace CrimsonCode26.scenes.game.data;

public abstract class Machine
{
    public string Name { get; }
    public string Id { get; }
    
    private readonly Dictionary<string, Machine> _outputs = new();
    private Belt _belt;
    
    public abstract void Run();
        // await input belt
        // Do processing
        // await Outputs.get("Success").receive(file);

    public bool AddConnection(string name, Belt belt) =>  belt == null ? throw new ArgumentNullException(nameof(belt)) : _outputs.TryAdd(name, belt);
}