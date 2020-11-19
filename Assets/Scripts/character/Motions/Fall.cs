using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fall : MotionState
{
    [SerializeField]
    private float m_GravityMultiplier = 2f;
    [SerializeField]
    private float m_FallMinHeight = 0.3f;

    public override bool CanStart()
    {
        return this.m_Controller.IsFalling && this.m_Rigidbody.velocity.y < 0f;
    }

    public override bool UpdateVelocity(ref Vector3 velocity)
    {
        Vector3 extraGravityForce = (Physics.gravity * this.m_GravityMultiplier) - Physics.gravity;
        velocity += extraGravityForce * Time.deltaTime;
        return true;
    }

    public override bool UpdateAnimator()
    {
        this.m_Animator.SetFloat("Float Value", this.m_Rigidbody.velocity.y, 0.15f, Time.deltaTime);
        return true;
    }
    
    public override bool CanStop()
    {
        return this.m_Controller.IsFalling;
    }
    
    private void OnControllerLanded()
    {
        this.StopMotion(true);
    }
}