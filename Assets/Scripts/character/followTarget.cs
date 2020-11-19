using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followTarget : MonoBehaviour
{
    [SerializeField]
    public Transform target;
    private Animator m_Animator;

    [SerializeField]
    public Vector3 offset;

    private bool m_IsCrounch {
        get {
            return m_Animator.GetBool("Crouch") || m_Animator.GetBool("Slide");
        }
    }

    private void Start()
    {
        m_Animator = target.GetComponent<Animator>();
    }

    void Update()
    {
        transform.position = target.position + Offset();
    }

    private Vector3 Offset()
    {
        Vector3 newOffset = offset;
        if (m_IsCrounch) newOffset.y = offset.y * 0.5f;
        return newOffset;
    }
}
