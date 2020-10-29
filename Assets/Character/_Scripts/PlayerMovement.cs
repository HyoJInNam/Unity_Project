using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    Vector3 velocity;      
    Vector3 LookPoint;     
    Rigidbody myRigidbody;
    Animator animator;

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }
    
    public void Move(Vector3 _velocity)
    {
        velocity = _velocity;
    }
    
    public void LookAt(Vector3 RayPoint)
    {
        LookPoint = RayPoint;
        LookPoint.y = transform.position.y;
        transform.LookAt(LookPoint);
    }
    
    public void FixedUpdate()
    {
        myRigidbody.MovePosition(myRigidbody.position + velocity * Time.fixedDeltaTime);
    }

    public void SetAnimatorParameters(float speed, Vector3 veloctity)
    {
        animator.SetFloat("speed", speed);
        animator.SetFloat("velocityX", veloctity.x);
        animator.SetFloat("velocityZ", veloctity.y);
    }
}
