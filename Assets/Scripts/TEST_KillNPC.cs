using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST_KillNPC : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] int damage;
    void Start()
    {
        GetComponent<NPC>().RemoveHealth(damage);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
