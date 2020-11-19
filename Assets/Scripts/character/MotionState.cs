using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotionState : MonoBehaviour
{
    [HideInInspector]
    [SerializeField]
    private string m_FriendlyName = string.Empty;
    public string FriendlyName
    {
        get
        {
            return this.m_FriendlyName;
        }
    }

    protected Transform m_Transform;
    protected Rigidbody m_Rigidbody;
    protected Animator m_Animator;
    protected CapsuleCollider m_CapsuleCollider;
    protected CharacterController m_Controller;

    [SerializeField]
    public string m_InputName = string.Empty;
    [SerializeField]
    public StartType m_StartType = StartType.Automatic;
    [SerializeField]
    public StopType m_StopType = StopType.Automatic;

    private void Start()
    {
        this.m_Transform = transform;
        this.m_Animator = this.m_Transform.GetComponent<Animator>();
        this.m_Rigidbody = this.m_Transform.GetComponent<Rigidbody>();
        this.m_CapsuleCollider = this.m_Transform.GetComponent<CapsuleCollider>();
        this.m_Controller = this.m_Transform.GetComponent<CharacterController>();
    }

    public bool IsActive;
    public int Index;
    public int Layer;

    [SerializeField]
    public string State;

    public virtual bool CanStart()
    {
        return true;
    }
    
    public virtual void OnStart()
    {

    }

    public virtual bool UpdateVelocity(ref Vector3 velocity)
    {
        return true;
    }

    public virtual bool UpdateRotation()
    {
        return true;
    }

    public virtual bool UpdateAnimator()
    {
        return true;
    }

    public virtual bool CheckGround()
    {
        return true;
    }

    public virtual bool CanStop()
    {
        return true;
    }
    public virtual void OnStop()
    {

    }

    public void StartMotion()
    {
        this.IsActive = true;
        OnStart();
    }

    public void StopMotion(bool force)
    {
        if (!this.IsActive || !force && !this.CanStop()) return;

        this.IsActive = false;
        OnStop();
    }

    public enum StartType
    {
        Automatic,
        Down,
        Press
    }

    public enum StopType
    {
        Automatic,
        Manual,
        Up,
        Toggle
    }
}