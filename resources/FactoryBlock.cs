// BotStats.cs
using Godot;

namespace ExampleProject
{
	[GlobalClass]
	public partial class BotStats : Resource
	{
		[Export]
		public int Health { get; set; }

		[Export]
		public Resource SubResource { get; set; }

		[Export]
		public string[] Strings { get; set; }

		// Make sure you provide a parameterless constructor.
		// In C#, a parameterless constructor is different from a
		// constructor with all default values.
		// Without a parameterless constructor, Godot will have problems
		// creating and editing your resource via the inspector.
		public BotStats() : this(0, null, null) {}

		public BotStats(int health, Resource subResource, string[] strings)
		{
			Health = health;
			SubResource = subResource;
			Strings = strings ?? System.Array.Empty<string>();
		}
	}
}
