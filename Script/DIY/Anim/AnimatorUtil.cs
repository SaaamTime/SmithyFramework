using System;
using UnityEngine;

namespace DIY.Anim
{
    public static class AnimatorUtil
    {
        public static Animator Bind(Transform _animatorTrans) {
            Animator animator = _animatorTrans.GetComponent<Animator>();
            return animator;
        } 
    }
}
