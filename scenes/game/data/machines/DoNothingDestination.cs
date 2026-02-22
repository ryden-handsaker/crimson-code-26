using System;
using System.Text.Json.Nodes;

namespace CrimsonCode26.scenes.game.data.machines;

public class DoNothingDestination : Machine, ISerializable<DoNothingDestination>
{
    public override void Process(File file) { }

    public DoNothingDestination(Guid guid)
    {
        Initialize("Do Nothing", guid);
    }

    public static DoNothingDestination CreateFromJSON(Guid guid, JsonObject json)
    {
        return new DoNothingDestination(guid);
    }
}