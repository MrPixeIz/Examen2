﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerMovement : NetworkBehaviour {

    [SerializeField] public float maxSpeed = 7;
    protected Vector2 targetVelocity;
    protected Rigidbody2D playerRigidBody2D;
    protected ContactFilter2D movementContactFilter;
   
    protected const float minMoveDistance = 0.001f;
    protected const float shellRadius = 0.01f;
    private bool controlAreEnable = true;

    private void Awake() {
        movementContactFilter = BuildContactFilter2DForLayer(LayerMask.LayerToName(gameObject.layer));
        playerRigidBody2D = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate() {
        Vector2 velocityX = new Vector2(); ;
        Vector2 velocityY = new Vector2(); ;
        velocityX.x= targetVelocity.x;
        velocityY.y = targetVelocity.y;
        Vector2 deltaPositionX = velocityX * Time.deltaTime;
        Movement(deltaPositionX);
        Vector2 deltaPositionY = velocityY * Time.deltaTime;
        Movement(deltaPositionY);
    }

    void Update() {
        if (controlAreEnable && isLocalPlayer) {
            ProcessInput();
        }
    }

    public void EnableControl() {
        this.controlAreEnable = true;
    }

    public void DisableControl() {
        this.controlAreEnable = false;
        targetVelocity = Vector2.zero;
    }

    public void ProcessInput() {
        if (controlAreEnable) {
            ComputeVelocity();
        }
    }

    protected void ComputeVelocity() {
        Vector2 move = Vector2.zero;
        move.x = Input.GetAxis("Horizontal");
        move.y = Input.GetAxis("Vertical");
        targetVelocity = move.normalized * maxSpeed;
    }

    private void Movement(Vector2 move) {
        float distance = move.magnitude;
        RaycastHit2D[] hitBuffer = new RaycastHit2D[16];
        if (distance > minMoveDistance) {
            int movementColisionHitCount = playerRigidBody2D.Cast(move, movementContactFilter, hitBuffer, distance + shellRadius);
            List<RaycastHit2D> hitBufferList = BufferArrayHitToList(hitBuffer, movementColisionHitCount);
 
            for (int i = 0; i < hitBufferList.Count; i++) {
                Vector2 currentNormal = hitBufferList[i].normal;
                float modifiedDistance = hitBufferList[i].distance - shellRadius;
                distance = modifiedDistance < distance ? modifiedDistance : distance;
            }
        }
        playerRigidBody2D.position = playerRigidBody2D.position + move.normalized * distance;
    }

   



    private ContactFilter2D BuildContactFilter2DForLayer(string LayerName) {
        ContactFilter2D contactFilter2DInteraction = new ContactFilter2D();
        contactFilter2DInteraction.useTriggers = false;
        contactFilter2DInteraction.SetLayerMask(Physics2D.GetLayerCollisionMask(LayerMask.NameToLayer(LayerName)));
        contactFilter2DInteraction.useLayerMask = true;
        return contactFilter2DInteraction;
    }

    private List<RaycastHit2D> BufferArrayHitToList(RaycastHit2D[] hitBuffer, int count) {
        List<RaycastHit2D> hitBufferList = new List<RaycastHit2D>(count);
        hitBufferList.Clear();
        for (int i = 0; i < count; i++) {
            hitBufferList.Add(hitBuffer[i]);
        }
        return hitBufferList;
    }
}
