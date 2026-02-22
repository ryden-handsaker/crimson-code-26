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
    public Machine PushTarget { get; protected set; }
    
    protected readonly System.Collections.Generic.Dictionary<string, Machine> Outputs = new();
    protected Belt Belt;

    protected void Initialize(string name, Guid? guid = null)
    {
        Name = name;
        Id = guid ?? Guid.NewGuid();
        ProcessFile = null;
        Belt = new Belt(this);
    }

    protected bool Push(Machine output)
    {
        bool success = output.Enqueue(ProcessFile);
        
        if (success) { ProcessFile = null; }
        else { PushTarget = output; }
        
        return success;
    }

    public bool Flush()
    {
        if (PushTarget == null)
            return true;

        bool success = Push(PushTarget);
        if (success)
            PushTarget = null;
        return success;
    }
    
    public abstract void Process(File file);

    public bool AddConnection(string name, Machine machine) =>  machine == null ? throw new ArgumentNullException(nameof(machine)) : Outputs.TryAdd(name, machine);

    public bool Enqueue(File file) => Belt.Enqueue(file);

    public void Tick() => Belt.Tick();
}