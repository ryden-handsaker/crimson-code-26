using Godot;
using System;
using System.Collections.Generic;

namespace CrimsonCode26;

public interface Runnable
{
	// Run in editor
	public abstract void Run(Dictionary<string, Machine> inputs, Dictionary<string, Machine> outputs);
	// Turn into bash
	public abstract void Serialize(); // really not sure yet how this should work
	
	
}
