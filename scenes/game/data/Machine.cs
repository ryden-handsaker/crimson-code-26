using System;
using CrimsonCode26.scenes.game.data;
using CrimsonCode26.scenes.game.data.machines;
using Godot.Collections;

namespace CrimsonCode26.scenes.game.data;

public abstract class Machine
{
    public string Name { get; protected set; }
    public Guid Id { get; protected set; }
    
    /// File being actively processed by the machine (null otherwise)
    public File ProcessFile { get; protected set; }
    /// Machine to push to.
    /// Set when a file is processed and waiting to be pushed (null otherwise)
    public Machine PushTarget { get; protected set; }
    
    /// Map of machines to push to
    public readonly System.Collections.Generic.Dictionary<string, Machine> Outputs = new();
    /// Input belt
    protected Belt Belt;

    /// Set default values for machine
    protected void Initialize(string name, Guid guid)
    {
        Name = name;
        Id = guid;
        ProcessFile = null;
        PushTarget = null;
        Belt = new Belt(this);
    }

    /// Push ProcessFile to `output`
    /// returns true if queued successfully, false otherwise
    protected bool Push(Machine output)
    {
        PushTarget = output;
        bool success = PushTarget.Enqueue(ProcessFile);

        if (success)
        {
            ProcessFile = null;
            PushTarget = null;
        }
        
        return success;
    }

    /// Flush ProcessFile to PushTarget if they are set
    /// returns true if ProcessFile is cleared, false otherwise
    public bool Flush()
    {
        if (PushTarget == null)
            return true;

        bool success = Push(PushTarget);
        if (success)
            PushTarget = null;
        return success;
    }
    
    /// Called by Belt
    public abstract void Process(File file);

    /// Connects the output of this machine to the provided machine
    public bool AddConnection(string name, Machine machine)
    {
        return machine == null ? throw new ArgumentNullException(nameof(machine)) : Outputs.TryAdd(name, machine);
    }

    /// Queues a file to be processed
    public bool Enqueue(File file) => Belt.Enqueue(file);

    /// Ticks the machine
    public void Tick() => Belt.Tick();
}