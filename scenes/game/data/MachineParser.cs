using System.Collections.Generic;
using System.Text.Json.Nodes;
using Godot;

namespace CrimsonCode26.scenes.game.data;

public class MachineParser
{
    public enum Type
    {
        FileSource, FileDestination, TrashDestination, ExtensionFilter
    }

    public static readonly Dictionary<Type, Machine> MachineMap = new()
    {
        { Type.FileSource, typeof(FileSource) },
        { Type.FileDestination, typeof(FileDestination) },
        { Type.TrashDestination, typeof(TrashDestination) },
        { Type.ExtensionFilter, typeof(ExtensionFilter) }
    };
    
    

    /*
        {
         "1b03b706-4667-47ad-8970-8449b66a8fdf" : {
           "type" : "FileSource",
           "outputs" : {
             "stream" : "639fe468-6dfc-401e-936f-27d26d4317dd"
           }
         },
         "639fe468-6dfc-401e-936f-27d26d4317dd" : {
           "type" : "ExtensionFilter",
           "outputs" : {
             "pass" : "e07fef47-851b-426c-861a-edf134782631",
             "fail" : "e202be1f-bfb8-466d-b9b4-60cc9f8a1f28"
           }
         },
         "e07fef47-851b-426c-861a-edf134782631" : {
           "type" : "FileDestination"
         },
         "e202be1f-bfb8-466d-b9b4-60cc9f8a1f28" : {
           "type" : "TrashDestination"
         }
       }
     */
}