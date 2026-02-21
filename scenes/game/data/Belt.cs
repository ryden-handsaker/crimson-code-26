using System.Collections.Generic;
using System.Linq;

namespace CrimsonCode26.scenes.game.data;

public class Belt
{
    private static int _maxSize = 10;
    
    private readonly Queue<File> _queue = [];
    private readonly Machine _machine;

    public Belt(Machine machine)
    {
        _machine = machine;
    }

    public bool Enqueue(File file)
    {
        if (_queue.Count >= _maxSize)
            return false;

        if (_machine.ProcessFile == null)
            _machine.Process(file);
        else
            _queue.Enqueue(file);

        return true;
    }

    /*
     * Get the top file from a queue.
     */
    public File Dequeue() => _queue.Dequeue();
}