using System.Collections.Generic;

namespace CrimsonCode26;

public class Machine
{
	public string Name { get; }
	public string Id { get; }
	private Dictionary<string, Machine> Inputs;
	private Dictionary<string, Machine> Outputs;
	private List<File> Queue;

	public void Run()
	{
		// await input queue
		// Do processing
		// await Outputs.get("Success").receive(file);
	}
}
