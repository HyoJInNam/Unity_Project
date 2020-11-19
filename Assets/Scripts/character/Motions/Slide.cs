using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slide : ChangeHeight
{
    public override bool CanStart()
    {
        bool isSlide = base.CanStart() && m_Controller.RelativeInput.z * m_Controller.SpeedMultiplier > 1f;
        this.m_Animator.SetBool("Slide", isSlide);
        return isSlide;
    }

    public override void OnStart()
    {
        this.m_Animator.SetBool("Slide", false);
    }
}