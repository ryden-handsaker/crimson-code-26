using System;
using System.Collections.Generic;

namespace CrimsonCode26.scenes.game.data;

public class Belt
{
    private const int DefaultMaxSize = 10;
    private readonly int _maxSize;
    
    private readonly Queue<File> _queue = [];
    private readonly Machine _machine;

    public Belt(Machine machine, int maxSize = DefaultMaxSize)
    {
        _machine = machine;
        _maxSize = maxSize;
    }

    public bool Enqueue(File file)
    {
        if (_queue.Count >= _maxSize)
            return false;
        
        _queue.Enqueue(file);

        return true;
    }

    public void Tick()
    {
        if (_machine.ProcessFile != null)
            _machine.Flush();
        else if (Dequeue() is { } file)
            _machine.Process(file);
    }

    /*
     * Get the top file from a queue.
     */
    public File Dequeue()
    {
        try { return _queue.Dequeue(); }
        catch (InvalidOperationException e) { return null; }
    } 
}