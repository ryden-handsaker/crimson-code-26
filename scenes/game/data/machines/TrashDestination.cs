using System;
using System.Text.Json.Nodes;

namespace CrimsonCode26.scenes.game.data.machines;

public class TrashDestination : Machine, ISerializable<TrashDestination>
{
    public override void Process(File file)
    {
        // TODO: delete file
        throw new NotImplementedException();
    }

    public TrashDestination(Guid guid)
    {
        Initialize(guid);
    }

    public static TrashDestination CreateFromJSON(Guid guid, JsonObject json)
    {
        return new TrashDestination(guid);
    }
}
