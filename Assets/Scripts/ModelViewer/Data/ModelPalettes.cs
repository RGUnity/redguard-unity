using System.Collections.Generic;
using UnityEngine;

public static class ModelPalettes
{
    public static readonly Dictionary<string, string> Dict = new()
    {
        // Necromancer Isle, City Jail
        ["DEAD.3DC"] = "NECRO",
        ["JAILINT.ROB"] = "NECRO",
        ["NECRISLE.ROB"] = "NECRO",
        ["NECRTOWR.ROB"] = "NECRO",
        ["NCROCK.3D"] = "NECRO",
        ["SKELA001.3DC"] = "NECRO",
        ["SKELA002.3DC"] = "NECRO",
        ["SKELA003.3DC"] = "NECRO",
        ["SKELA004.3DC"] = "NECRO",
        ["VERMA001.3DC"] = "NECRO",
        ["VERMA002.3DC"] = "NECRO",
        ["VULTA001.3DC"] = "NECRO",
        ["ZOMBA001.3DC"] = "NECRO",
        ["ZOMBA002.3DC"] = "NECRO",

        // Goblin Caves, Mages Guild
        ["CAVERNS.ROB"] = "REDCAVE",
        ["CV_BOOM.3D"] = "REDCAVE",
        ["CV_BOOM.3DC"] = "REDCAVE",
        ["CV_EXPL1.3DC"] = "REDCAVE",
        ["CV_MUSH2.3DC"] = "REDCAVE",
        ["MGUILD.ROB"] = "REDCAVE",

        // Observatory, Dwemer Caves
        ["DGOLA001.3DC"] = "OBSERVAT",
        ["DRINT.ROB"] = "OBSERVAT",
        ["ERASA001.3DC"] = "OBSERVAT",
        ["GOLMA001.3DC"] = "OBSERVAT",
        ["GOLMA002.3DC"] = "OBSERVAT",
        ["OBSERVE.ROB"] = "OBSERVAT",

        // Restless League Hideout
        ["FLAG_RL.3DC"] = "HIDEOUT",
        ["HIDEINT.ROB"] = "HIDEOUT",
        ["HIDEOUT.ROB"] = "HIDEOUT",

        // Imperial Palace
        ["PALACE.ROB"] = "PALACE00",
        ["PALATEST.ROB"] = "PALACE00",

        // Catacombs
        ["CATACOMB.ROB"] = "CATACOMB"
    };
}
