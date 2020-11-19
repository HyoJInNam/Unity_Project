using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swim : MotionState
{
    [SerializeField]
    private float m_HeightOffset = -7.5f;
    [SerializeField]
    private float m_OffsetSmoothing = 0.1f;

    private SwimTrigger m_Trigger;
    private float m_SmoothOffset;
    private float m_SmoothVelocity;

    private float m_HeightAdjustment = -0.3f;

    public override void OnStart()
    {
        this.m_Controller.IsSwimming = true;
        this.m_Rigidbody.useGravity = false;
        this.m_Rigidbody.velocity = Vector3.zero;
        this.m_Controller.Velocity = Vector3.zero;

        Vector3 position = transform.position;
        position.y = m_Trigger.transform.position.y + this.m_HeightOffset ;
        transform.position = position;
    }

    public override void OnStop()
    {
        this.m_Controller.IsSwimming = false;
        this.m_Rigidbody.useGravity = true;
    }

    public override bool CanStart()
    {
        return (this.m_Trigger != null && transform.position.y - this.m_Trigger.transform.position.y < this.m_HeightOffset - 0.1f) ? true : false;
    }

    public override bool CanStop()
    {
        return (this.m_Trigger == null || transform.position.y - this.m_Trigger.transform.position.y > this.m_HeightOffset + 0.1f) ? true : false;
    }

    public override bool UpdateVelocity(ref Vector3 velocity)
    {
        if (!this.m_Controller.IsStepping)
        {
            Vector3 position = transform.position;
            this.m_SmoothOffset = Mathf.SmoothDamp(position.y, m_Trigger.transform.position.y + this.m_HeightOffset, ref this.m_SmoothVelocity, this.m_OffsetSmoothing);
            position.y = this.m_SmoothOffset;
            transform.position = position;
        }
        return true;
    }
        
    public override bool CheckGround()
    {
        return false;
    }

    private void OnTriggerEnter(Collider other)
    {
        SwimTrigger trigger = other.GetComponent<SwimTrigger>();
        if (m_StartType == StartType.Automatic && trigger != null)
        {
            this.m_Trigger = trigger;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        this.m_Trigger = null;
    }
}