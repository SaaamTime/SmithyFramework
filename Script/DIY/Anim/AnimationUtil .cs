using System;
using UnityEngine;

namespace DIY.Anim
{
    public static class AnimationUtil
    {
        public static Animation Bind(Transform _animationTrans) {
            Animation animation = _animationTrans.GetComponent<Animation>();
            return animation;
        }
    }
}
