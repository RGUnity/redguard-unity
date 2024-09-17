using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPlayerMovementConfig", menuName = "ScriptableObjects/PlayerMovementConfig")]
public class PlayerMovementConfig : ScriptableObject
{
    [Header("Main Settings")]
    public LayerMask groundLayers;
    public float gravity = -6.5f;
    public float groundMagnet = -0.1f;

    [Header("Basic Movement")]
    public float runSpeed = 5f;
    public float walkSpeed = 2;
    public float velocitySmoothing = 100;
    public float turnSpeed = 1.8f;

    [Header("Step Handling (Corner-Jumps Fix)")]
    public float stepOffset = 0.9f;
    public float rayDistance = 1f;

    [Header("Jumping")]
    public float jumpHeight = 1.2f;
    public float longJumpDistance = 1.3f;
    public float shortJumpDistance = 0.7f;
    public float longJumpThreshold = 3.5f;
    public float shortJumpThreshold = 0.1f;

    [Header("Slope Sliding")]
    public float slopeSlideSpeed = 0.1f;
    public float slopeAlignmentSpeed = 100f;
    
    [Header("Ledge Climbing")]
    public Vector3 centerLedgeRaycastOrigin = new (0, 2.25f, 0.4f);
    public Vector3 leftLedgeRaycastOrigin = new (-0.3f, 2.25f, 0.5f);
    public Vector3 rightLedgeRaycastOrigin = new (0.3f, 2.25f, 0.5f);
    public float highLedgeStart = 2.1f;
    public float lowLedgeStart = 0.9f;
    public float ledgeStrafeSpeed = 1f;
}
