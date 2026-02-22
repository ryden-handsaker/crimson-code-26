using System;
using CrimsonCode26.scenes.game.data.machines;

namespace CrimsonCode26.scenes.game.data;

public abstract class Machine
{
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
    protected void Initialize(Guid guid)
    {
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
        var success = PushTarget.Enqueue(ProcessFile);

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

        var success = Push(PushTarget);
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
    
    public static string GetName(System.Type type)
    {
        return type switch
        {
            not null when type == typeof(FolderSource) => "Folder Source",
            not null when type == typeof(FolderDestination) => "Folder Destination",
            not null when type == typeof(ExtensionFilter) => "Extension Filter",
            not null when type == typeof(TrashDestination) => "Trash",
            not null when type == typeof(DoNothingDestination) => "Do Nothing",
            not null when type == typeof(OpenDestination) => "Open",
            not null when type == typeof(RenameModifier) => "Rename",
            _ => throw new ArgumentException("unknown type")
        };
    }
}