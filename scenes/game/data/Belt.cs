using System.Collections.Generic;

namespace CrimsonCode26.scenes.game.data;

public class Belt
{
    private readonly Queue<File> _queue = [];

    /*
     * Get the top file from a queue.
     */
    public File Dequeue() => _queue.Dequeue();
}