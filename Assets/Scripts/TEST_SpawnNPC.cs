using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST_SpawnNPC : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    // Start is called before the first frame update
    void Start()
    {
        var newGuard = Instantiate(prefab, transform.position, Quaternion.identity);
        newGuard.GetComponent<NPC>().GenerateID();
        newGuard.GetComponent<NPC>().RegisterNPC();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
