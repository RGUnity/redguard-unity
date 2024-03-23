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
    
    private Transform _target;
    
    
    private void Start()
    {
        _target = pointA;
    }
    
    private void Update()
    {
        if (rb.transform.position == pointA.position)
        {
            _target = pointB;
        }
        else
        {
            if (rb.transform.position == pointB.position)
            {
                _target = pointA;
            }
        }
        MoveTowardsTarget(_target);
    }
    
    private void MoveTowardsTarget(Transform target)
    {
        Vector3 newPosition = Vector3.MoveTowards(rb.position, target.position, speed * Time.fixedDeltaTime);

        // moveStep is used by the player to move along with platforms
        moveStep = newPosition - rb.transform.position;

        rb.MovePosition(newPosition);
    }
}
