using System;
using System.Text.Json.Nodes;

namespace CrimsonCode26.scenes.game.data;

public interface ISerializable<T> where T : Machine
{
    static abstract T CreateFromJSON(Guid guid, JsonObject json);
}