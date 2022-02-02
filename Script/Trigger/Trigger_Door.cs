using DIY.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DIY.Trigger
{
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
                animation.Play_Forward(null, 0, delegate { collider.enabled = false; },0.5f); //需要加快一下box关闭
            }
            else
            {
                animation.Play_Back(null, 0.3f, delegate { collider.enabled = true; },0.5f);
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
            //Debug.Log("进入范围");
            PanelParam param = new PanelParam();
            param.AddParam("content", string.Format(" 是否 {0} 此门 ？",collider.enabled?"开启":"关闭"));
            param.AddParam(() =>{
                Event_Do();
            },1);
            UIManager.Instance.OpenPanel<UIPanel_SlideTip>(UIPanel_SlideTip.config, param);
        }
        public override void Event_OutRangeState()
        {
            //去掉UI提示
            //Debug.Log("走出范围");
            UIManager.Instance.ClosePanel(UIPanel_SlideTip.config.name);
        }
    }
}