using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public static class AnimExtension
{
    public static void Play_Forward(this Animation _animation, string _name=null,float _progress=0,Action _endEvent=null) {
        if (string.IsNullOrEmpty(_name))
        {
            _name = _animation.clip.name;
        }
        AnimationState animState = _animation[_name];
        animState.speed = 1;
        animState.normalizedTime = _progress;
        _animation.Play(_name);
        if (_endEvent!=null)
        {
            Clock.Instance.DoDelay(animState.name + _animation.gameObject.name, animState.length, _endEvent);
        }
    }
    public static void Play_Back(this Animation _animation,string _name=null,float _progress = 1, Action _endEvent = null) {
        if (string.IsNullOrEmpty(_name))
        {
            _name = _animation.clip.name;
        }
        AnimationState animState = _animation[_name];
        animState.speed = -1;
        animState.normalizedTime = _progress;
        _animation.Play(_name);
        if (_endEvent != null)
        {
            Clock.Instance.DoDelay(animState.name + _animation.gameObject.name, animState.length, _endEvent);
        }
    }
    public static void PlayJumpToEnd(this Animation _animation,bool _isRewind = false,string _name=null) {
        if (string.IsNullOrEmpty(_name))
        {
            _name = _animation.clip.name;
        }
        AnimationState animState = _animation[_name];
        animState.speed = 1;
        animState.normalizedTime = 1;
        _animation.Play(_name);
    }

    public static void PlayStopAtStart(this Animation _animation ,string _name=null) {
        if (string.IsNullOrEmpty(_name))
        {
            _name = _animation.clip.name;
        }
        AnimationState animState = _animation[_name];
        animState.speed = -1;
        animState.normalizedTime = 0;
        _animation.Play(_name);
    }

    public static string SafeCheckAndGetName(this Animation _animation,string _name = null) {
        if (string.IsNullOrEmpty(_name))
        {
            _name = _animation.clip.name;
        }
        return _name;
    }

    #region Animator
    /// <summary>
    /// 获取动画状态机animator的动画clip的播放持续时长
    /// </summary>
    /// <param name="animator">查询的动画状态机</param>
    /// <param name="clip">动画clip</param>
    /// <returns></returns>
    public static float GetClipLength(this Animator animator, string clip)
    {
        if (null == animator || string.IsNullOrEmpty(clip) || null == animator.runtimeAnimatorController)
            return 0;
        RuntimeAnimatorController ac = animator.runtimeAnimatorController;
        AnimationClip[] tAnimationClips = ac.animationClips;
        if (null == tAnimationClips || tAnimationClips.Length <= 0)
            return 0;
        AnimationClip tAnimationClip;
        for (int tCounter = 0, tLen = tAnimationClips.Length; tCounter < tLen; tCounter++)
        {
            tAnimationClip = tAnimationClips[tCounter];
            if (null != tAnimationClip && tAnimationClip.name == clip)
                return tAnimationClip.length;
        }
        return 0f;
    }

    /// <summary>
    /// 通过动画片段名称，找到Animator中动画片段 
    /// </summary>
    /// <param name="_animator"></param>
    /// <param name="_name"></param>
    /// <returns></returns>
    public static AnimationClip GetAnimationClipByName(this Animator _animator, string _name)
    {
        for (int i = 0; i < _animator.runtimeAnimatorController.animationClips.Length; ++i)
        {
            if (_animator.runtimeAnimatorController.animationClips[i].name == _name)
            {
                return _animator.runtimeAnimatorController.animationClips[i];
            }
        }
        return null;
    }

    public static void ClearEvents(this Animator _animator)
    {
        foreach (AnimationClip anim in _animator.runtimeAnimatorController.animationClips)
        {
            anim.ClearEvents();
        }
    }

    public static void ClearAnimClipEvent(this Animator _animator, string _animName)
    {
        AnimationClip targetAnimation = GetAnimationClipByName(_animator, _animName);
        if (targetAnimation)
        {
            targetAnimation.ClearEvents();
        }
    }

    public static void ClearEvents(this AnimationClip _animClip)
    {
        if (_animClip.events.Length > 0)
        {
            _animClip.events = Array.Empty<AnimationEvent>();
        }
    }

    public static void PlayOrRewind(this Animator _animator, string _animName, bool isRewind = false, string _speedKey = "Speed")
    {
        AnimationClip targetAnimation = GetAnimationClipByName(_animator, _animName);
        if (targetAnimation)
        {
            float beginPoint = isRewind ? 1f : 0f;
            _animator.SetFloat(_speedKey, isRewind ? -1f : 1f);
            _animator.Play(_animName, 0, beginPoint);
        }
    }

    /// <summary>
    /// 判断该Animator中是否有指定动画片段
    /// </summary>
    /// <param name="_animator"></param>
    /// <param name="_name"></param>
    /// <returns></returns>
    public static bool Check_HasThisAnimation(this Animator _animator, string _name)
    {
        AnimationClip targetAnimation = GetAnimationClipByName(_animator, _name);
        return targetAnimation != null;
    }
    #region Animator修改曲线，在真机上竟然不通过！艹了！
    //public static void ChangeAnimCurve_LastFrame_RectTrans(this Animator _animator,string _animName, string _pathName, string _ctlFieldName, float _value) {
    //    AnimationClip targetAnim = _animator.GetAnimationClipByName(_animName);
    //    if (null!= targetAnim)
    //    {
    //        LuaFramework.AnimationCurveChanger.Instance.ChangeAnimationCurveValueAtLast_RectTransform(targetAnim, _pathName, _ctlFieldName, _value);
    //    }
    //}

    //public static void ChangeAnimCurve_FirstFrame_RectTrans(this Animator _animator, string _animName, string _pathName, string _ctlFieldName, float _value)
    //{
    //    AnimationClip targetAnim = _animator.GetAnimationClipByName(_animName);
    //    if (null != targetAnim)
    //    {
    //        LuaFramework.AnimationCurveChanger.Instance.ChangeAnimationCurveValueAtFirst_RectTransform(targetAnim, _pathName, _ctlFieldName, _value);
    //    }
    //}
    //public static void ChangeAnimCurve_TargetFrame_RectTrans(this Animator _animator, string _animName, string _pathName, string _ctlFieldName, float _value,int _index)
    //{
    //    AnimationClip targetAnim = _animator.GetAnimationClipByName(_animName);
    //    if (null != targetAnim)
    //    {
    //        LuaFramework.AnimationCurveChanger.Instance.ChangeAnimationCurveValue_RectTransform(targetAnim, _pathName, _ctlFieldName, _value, _index);
    //    }
    //}
    //public static void ChangeAnimCurve_AllFrame_RectTrans(this Animator _animator, string _animName, string _pathName, string _ctlFieldName, float[] _values) {
    //    AnimationClip targetAnim = _animator.GetAnimationClipByName(_animName);
    //    if (null != targetAnim)
    //    {
    //        LuaFramework.AnimationCurveChanger.Instance.ChangeAnimationCurveAllValue_RectTransform(targetAnim, _pathName, _ctlFieldName, _values);
    //    }
    //}
    //public static void changeAnimCurve_FirstFram_RectTrans() { 

    //}
    #endregion

    #endregion

    #region AnimationClip
    /// <summary>
    /// 将一个关键帧的所有参数返回成字符串
    /// </summary>
    /// <param name="_keyframe"></param>
    /// <returns>time,value,inTangent,outTangent,inWeight,outWeight,weightedMode</returns>
    public static string ToParams(this Keyframe _keyframe)
    {
        StringBuilder keyframeParams = new StringBuilder();
        keyframeParams.AppendFormat("{0},{1},{2},{3},{4},{5},{6}",
            _keyframe.time,
            _keyframe.value,
            _keyframe.inTangent,
            _keyframe.outTangent,
            _keyframe.inWeight,
            _keyframe.outWeight,
            _keyframe.weightedMode);
        return keyframeParams.ToString();
    }

    /// <summary>
    /// 将字符串参数转换为一个关键帧
    /// </summary>
    /// <param name="_keyfameParam">time,value,inTangent,outTangent,inWeight,outWeight,weightedMode</param>
    /// <returns></returns>
    public static Keyframe StringParamToKeyframe(string _keyfameParam)
    {
        string[] paramArr = _keyfameParam.Split(',');
        Keyframe keyframe = new Keyframe(float.Parse(paramArr[0]), float.Parse(paramArr[1]), float.Parse(paramArr[2]),
            float.Parse(paramArr[3]), float.Parse(paramArr[4]), float.Parse(paramArr[5]));
        keyframe.weightedMode = (WeightedMode)(Enum.Parse(typeof(WeightedMode), paramArr[6]));
        return keyframe;
    }

    public static Keyframe CloneSelfWithNewValue(this Keyframe _keyframe, float _newValue)
    {
        Keyframe keyframe = new Keyframe(_keyframe.time, _newValue, _keyframe.inTangent,
            _keyframe.outTangent, _keyframe.inWeight, _keyframe.outWeight);
        keyframe.weightedMode = _keyframe.weightedMode;
        return keyframe;
    }
    #endregion

    #region Animation
    public static AnimationState GetAnimationState(this Animation animation, string animationName)
    {
        AnimationState state = animation[animationName];
        return state;
    }
    #endregion

}
