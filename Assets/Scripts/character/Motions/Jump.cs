using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MotionState
{
    [SerializeField]
    private float m_Force = 5f;
    [SerializeField]
    private float m_RecurrenceDelay = 0.2f;
    
    private float jumpTime;
    private float lastJumpTime;

    private void Awake()
    {
        //CharacterController.OnControllerGrounded += OnControllerGrounded;
    }

    public override void OnStart()
    {
        StartJump();
    }

    private void StartJump()
    {
        if (this.IsActive)
        {
            this.jumpTime = Time.time;
            this.m_Controller.IsGrounded = false;
            this.m_Controller.IsJumping = true;
            
            Vector3 velocity = this.m_Rigidbody.velocity;
            velocity.y = m_Force;
            this.m_Rigidbody.velocity = velocity;
            this.m_Animator.SetFloat("Float Value", this.m_Rigidbody.velocity.y);

            float cycle = 0f;
            if (this.m_Controller.IsMoving)
            {
                float normalizedTime = this.m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime % 1f;
                cycle = Mathf.Sin(360f * normalizedTime);
            }
            this.m_Animator.SetFloat("Leg", cycle);

            this.m_Animator.SetBool("Bool Value", (this.m_Controller.RawInput.z < 0f? true : false));
        }
    }
    public override bool UpdateAnimator()
    {
        this.m_Animator.SetFloat("Float Value", this.m_Rigidbody.velocity.y, 0.15f, Time.deltaTime);
        return true;
    }

    public override bool CheckGround()
    {
        if (Time.time > jumpTime + 0.2f)
        {
            return true;
        }
        return false;
    }

    public void OnControllerGrounded(bool grounded)
    {
        if (!grounded) return;
        this.lastJumpTime = Time.time;
        this.m_Controller.IsJumping = false;
        this.StopMotion(true);
    }

    public override bool CanStart()
    {
        return this.m_Controller.IsGrounded && (Time.time > lastJumpTime + this.m_RecurrenceDelay);
    }

    public override bool CanStop()
    {
        return !this.m_Controller.IsGrounded && this.m_Rigidbody.velocity.y < 0.01f;
    }
}