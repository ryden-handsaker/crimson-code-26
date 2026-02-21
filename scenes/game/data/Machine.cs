using System;
using System.Collections.Generic;
using CrimsonCode26.scenes.game.data;

namespace CrimsonCode26.scenes.game.blocks;

public abstract class Machine
{
    private readonly Dictionary<string, Belt> _outputs = new();

    public bool AddConnection(string name, Belt belt) =>  belt == null ? throw new ArgumentNullException(nameof(belt)) : _outputs.TryAdd(name, belt);
}