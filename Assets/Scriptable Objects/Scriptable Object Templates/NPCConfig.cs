using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewNPCType", menuName = "ScriptableObjects/NPCType")]
public class NPCConfig : ScriptableObject
{
    public GameObject prefab;
    // NPC will attack on sight. Can not talk with player.
    public bool isAggressive;
    // NPC will attack when nearby player pulls sword. Can talk with player while not in combat.
    public bool isProtective;
    public int maxHealth = 3;
    public int damage = 1;
}
