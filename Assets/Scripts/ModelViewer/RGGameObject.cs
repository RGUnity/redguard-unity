using System;
using System.Collections.Generic;
using UnityEngine;
using RGFileImport;

public class RGGameObject
{
    GameObject gameObject;
    List<Mesh> meshes;
    List<Material> materials;
    Vector3 position;
    Vector3 rotation;

//    List<RGAnim> animations;
    RGGameObject(string name)
    {
        gameObject = new GameObject(name);
    }
}
