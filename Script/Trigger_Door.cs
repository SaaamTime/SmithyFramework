using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger_Door : Trigger_ByDistance
{

    public BoxCollider collider;

    protected override void Init()
    {
        base.Init();
        collider = GetComponent<BoxCollider>();
    }

    public override void Event_Do()
    {
        if (collider.enabled)
        {
            animation.Play_Forward(null,0,delegate { collider.enabled = false; });
        }
        else
        {
            animation.Play_Back(null,0.3f,delegate { collider.enabled = true; });
        }
    }

    public override void Event_InitState()
    {
        //transform.localEulerAngles = Vector3.zero
        animation.PlayStopAtStart();
    }

    public override void Event_InRangeState()
    {
        //弹出UI提示
        Debug.Log("进入范围");
    }

    public override void Event_OutRangeState()
    {
        //去掉UI提示
        Debug.Log("走出范围");
    }

}
