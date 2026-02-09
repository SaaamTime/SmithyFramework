using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DIY.Anim{
[RequireComponent(typeof(Animation))]
public class AnimationPlayCtrl : MonoBehaviour
{
    public Animation animation;
    private void Awake() {
        //默认手动控制播放，不自动控制
        animation = GetComponent<Animation>();
        animation.playAutomatically = false;
    }

    public void Play_Back()
    {
        animation.Play_Back();
    }

    public void Play_Forward()
    {
        animation.Play_Forward();
    }



}

}
