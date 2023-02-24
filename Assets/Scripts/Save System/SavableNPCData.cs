using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavableNPCData
{
    public string id;
    public NPCConfig config;
    public Vector3 position;
    public Quaternion rotation;
    public NPCStateEnum actionState;
    public int health;
}
