using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST_MovingPlatform : MonoBehaviour
{   
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float speed = 10f;
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;

    public Vector3 moveStep;
    
    private Transform target;
    
    
    // Start is called before the first frame update
    void Start()
    {
        target = pointA;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (rb.transform.position == pointA.position)
        {
            target = pointB;
        }
        else
        {
            if (rb.transform.position == pointB.position)
            {
                target = pointA;
            }
        }
        
        MoveTowardsTarget(target);
    }
    
    private void MoveTowardsTarget(Transform target)
    {
        Vector3 newPosition = Vector3.MoveTowards(rb.position, target.position, speed * Time.fixedDeltaTime);

        // moveStep is used by the player to move along with platforms
        moveStep = newPosition - rb.transform.position;

        rb.MovePosition(newPosition);
    }
}
