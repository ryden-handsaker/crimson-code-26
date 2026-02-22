using System;
using CrimsonCode26.scenes.game.data;
using CrimsonCode26.scenes.game.data.machines;
using Godot.Collections;

namespace CrimsonCode26.scenes.game.data;

public abstract class Machine
{
    public string Name { get; protected set; }
    public Guid Id { get; protected set; }
    
    public File ProcessFile { get; protected set; }
    
    protected readonly System.Collections.Generic.Dictionary<string, Machine> Outputs = new();
    protected Belt Belt;

    protected void Initialize(string name)
    {
        Name = name;
        Id = Guid.NewGuid();
        ProcessFile = null;
        Belt = new Belt(this);
    }
    
    public abstract void Process(File file);
        // await input belt
        // Do processing
        // await Outputs.get("Success").receive(file);

    public bool AddConnection(string name, Machine machine) =>  machine == null ? throw new ArgumentNullException(nameof(machine)) : Outputs.TryAdd(name, machine);

    public bool Enqueue(File file) => Belt.Enqueue(file);
}