using LitJson;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System;
using XAsset;
using DIY.Json;

namespace DIY.Anim
{
    /// <summary>
    /// 动画曲线修改
    /// 注：
    ///     1、路径保存文件，从txt为json格式，所有有需求的曲线都保存在一个json文件中
    ///     2、启动后，所有曲线数据（其实没几个动画片段有这个需求）载入，并等待调用
    ///     3、该脚本仅提供读取、修改曲线json、动画片段的曲线修改于重置，没有保存曲线的功能
    ///     4、保存曲线功能在AnimationCurveSaver的编辑器扩展窗口中
    ///     5、曲线文件json保存编码UTF-8，不能用中文，json结构：{动画名：{Trans路径：{控制节点名：曲线参数}}}
    ///     6、Trans路径为空，说明是该关键帧控制的游戏物体本身，换言之，控制该游戏物体的子物体才会有Trans路径
    ///     7、一条曲线控制一个节点
    /// </summary>
    public class AnimationClipCurveChanger
    {
        public string localPath = "Main/AnimationClipCurve";
        public string jsonName = "AnimationCurveData.json";
        public static AnimationClipCurveChanger Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new AnimationClipCurveChanger();
                }
                return _instance;
            }

        }
        private static AnimationClipCurveChanger _instance;

        public Dictionary<string, List<AnimationCurveData>> curveDataDic;
        private AnimationClipCurveChanger()
        {
            curveDataDic = new Dictionary<string, List<AnimationCurveData>>();
        }

        /// <summary>
        /// 重置并加载本地曲线文件
        /// </summary>
        public void Reinit()
        {
            curveDataDic.Clear();
            foreach (KeyValuePair<string, JsonData> _animCurvesJson in ReadAnimationCurveDataJson())
            {
                List<AnimationCurveData> animCurves = new List<AnimationCurveData>();
                string animName = _animCurvesJson.Key;
                foreach (KeyValuePair<string, JsonData> _pathChild in _animCurvesJson.Value)
                {
                    JsonData pathChildJson = _pathChild.Value;
                    string pathName = _pathChild.Key;
                    bool isEndLevel = false;
                    foreach (KeyValuePair<string, JsonData> _ctlField in pathChildJson)
                    {
                        JsonData ctlFieldJson = _ctlField.Value;
                        if (ctlFieldJson.IsString)//在上一层已经是最底层了，这组控制字段的曲线是直接控制父物体本身的
                        {
                            isEndLevel = true;
                            break;
                        }
                        string ctlFieldName = _ctlField.Key;
                        animCurves.Add(ConvertParamsToCurveData(animName, pathName, ctlFieldName, ctlFieldJson));
                    }
                    //该层曲线数据是直接控制父物体字段的，实际就是ctlField
                    if (isEndLevel)
                    {
                        animCurves.Add(ConvertParamsToCurveData(animName, "", pathName, pathChildJson));
                    }
                }
                curveDataDic.Add(animName, animCurves);
            }
        }

        /// <summary>
        /// 一条曲线控制一个游戏物体字段（cltField）
        /// </summary>
        /// <param name="_cltFieldJson"></param>
        /// <returns></returns>
        public AnimationCurveData ConvertParamsToCurveData(string _animName, string _pathName, string _ctlFieldName, JsonData _cltFieldJson)
        {
            AnimationCurveData curve = new AnimationCurveData(_animName, _pathName, _ctlFieldName);
            foreach (KeyValuePair<string, JsonData> _keyframKV in _cltFieldJson)
            {
                string keyframParam = _keyframKV.Value.ToString();
                curve.AddKeyframeByParamStr(keyframParam);
            }
            return curve;
        }

        /// <summary>
        /// 修改指定动画片段中，对应一条曲线的所有关键帧（仅能改value，不能改时间坐标）
        /// </summary>
        /// <param name="_animName"></param>
        /// <param name="_pathName"></param>
        /// <param name="_ctlFieldName"></param>
        /// <param name="_value"></param>
        public AnimationCurveData ChangeTargeCurveAtAllKeyframe(string _animName, string _pathName, string _ctlFieldName, float[] _newValues)
        {
            List<AnimationCurveData> animsCurves = curveDataDic[_animName];
            if (null != animsCurves)
            {
                foreach (AnimationCurveData item in animsCurves)
                {
                    if (item.pathName == _pathName && item.ctlFieldName == _ctlFieldName)
                    {
                        //定位该关键帧的下标，修改value
                        item.ReorderKeyframes();
                        int newValueLength = _newValues.Length;
                        item.WalkForChangeKeyframe((_time, _value, _index, _totalCount) => { return newValueLength > _index; },
                            (_index) => {
                                Debug.Log(string.Format("ChangeTargeCurveAtAllKeyframe----成功:{0}-{1}-{2}-{3}-{4}", _animName, _pathName, _ctlFieldName, _index, _newValues[_index]));
                                return _newValues[_index];
                            });
                        return item;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 修改指定动画片段中，对应一条曲线的第一个关键帧（仅能改value，不能改时间坐标）
        /// </summary>
        /// <param name="_animName"></param>
        /// <param name="_pathName"></param>
        /// <param name="_ctlFieldName"></param>
        /// <param name="_value"></param>
        public AnimationCurveData ChangeTargeCurveFirstValue(string _animName, string _pathName, string _ctlFieldName, float _newValue)
        {
            return ChangeTargeCurveAtTargetKeyframe(_animName, _pathName, _ctlFieldName, 1, _newValue);
        }

        /// <summary>
        /// 修改指定动画片段中，对应一条曲线的对应关键帧（仅能改value，不能改时间坐标）
        /// </summary>
        /// <param name="_animName"></param>
        /// <param name="_pathName"></param>
        /// <param name="_ctlFieldName"></param>
        /// <param name="_value"></param>
        public AnimationCurveData ChangeTargeCurveAtTargetKeyframe(string _animName, string _pathName, string _ctlFieldName, int _targetIndex, float _newValue)
        {
            List<AnimationCurveData> animsCurves = curveDataDic[_animName];
            if (null != animsCurves)
            {
                foreach (AnimationCurveData item in animsCurves)
                {
                    if (item.pathName == _pathName && item.ctlFieldName == _ctlFieldName)
                    {
                        //定位该关键帧的下标，修改value
                        Debug.Log(string.Format("ChangeTargeCurveAtTargetKeyframe----成功:{0}-{1}-{2}-{3}", _animName, _pathName, _ctlFieldName, _newValue));
                        item.ReorderKeyframes();
                        item.WalkForChangeKeyframe((_time, _value, _index, _totalCount) => { return (_index + 1) == _targetIndex; },
                            (_index) => { return _newValue; });
                        return item;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 修改指定动画片段中，对应一条曲线的最后一帧关键帧（仅能改value，不能改时间坐标）
        /// </summary>
        /// <param name="_animName"></param>
        /// <param name="_pathName"></param>
        /// <param name="_ctlFieldName"></param>
        /// <param name="_value"></param>
        public AnimationCurveData ChangeTargeCurveLastValue(string _animName, string _pathName, string _ctlFieldName, float _newValue)
        {

            List<AnimationCurveData> animsCurves = curveDataDic[_animName];
            if (null != animsCurves)
            {
                foreach (AnimationCurveData item in animsCurves)
                {
                    if (item.pathName == _pathName && item.ctlFieldName == _ctlFieldName)
                    {
                        //定位该关键帧的下标，修改value
                        Debug.Log(string.Format("ChangeTargeCurveLastValue----成功:{0}-{1}-{2}-{3}", _animName, _pathName, _ctlFieldName, _newValue));
                        item.ReorderKeyframes();
                        item.WalkForChangeKeyframe((_time, _value, _index, _totalCount) => { return (_index + 1) == _totalCount; },
                            (_index) => { return _newValue; });
                        return item;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 修改指定动画片段中，指定字段的所有关键帧的value
        /// </summary>
        /// <param name="_anim"></param>
        /// <param name="_type"></param>
        /// <param name="_pathName"></param>
        /// <param name="_ctlFieldName"></param>
        /// <param name="_value"></param>
        /// <param name="_targetIndex">-1是修改最后一帧</param>
        public void ChangeAnimationCurveValueAllKeyframe(AnimationClip _anim, Type _type, string _pathName, string _ctlFieldName, float[] _values)
        {
            Debug.Log("AnimationClipCurveChanger.ChangeAnimationCurveValueAllKeyframe----是否是老版动画 " + _anim.name + "  " + _anim.legacy);
            _anim.legacy = true;
            string animName = _anim.name;
            AnimationCurveData targetCurveData = ChangeTargeCurveAtAllKeyframe(animName, _pathName, _ctlFieldName, _values);
            if (null != targetCurveData)
            {
                _anim.SetCurve(_pathName, _type, _ctlFieldName, targetCurveData.ToAnimationCurve());
            }
        }

        /// <summary>
        /// 修改指定动画片段中，指定字段的对应关键帧的value
        /// </summary>
        /// <param name="_anim"></param>
        /// <param name="_type"></param>
        /// <param name="_pathName"></param>
        /// <param name="_ctlFieldName"></param>
        /// <param name="_value"></param>
        /// <param name="_targetIndex">-1是修改最后一帧</param>
        public void ChangeAnimationCurveValueAtTargetKeyframe(AnimationClip _anim, Type _type, string _pathName, string _ctlFieldName, float _value, int _targetIndex)
        {
            //Debug.Log("AnimationClipCurveChanger.ChangeAnimationCurveValueAllKeyframe----是否是老版动画 " + _anim.name + "  " + _anim.legacy);
            _anim.legacy = true;//不使用老版动画的时候，真机不通过！艹！
            string animName = _anim.name;
            AnimationCurveData targetCurveData = _targetIndex < 0 ? ChangeTargeCurveLastValue(animName, _pathName, _ctlFieldName, _value) : ChangeTargeCurveAtTargetKeyframe(animName, _pathName, _ctlFieldName, _targetIndex, _value);
            if (null != targetCurveData)
            {
                _anim.SetCurve(_pathName, _type, _ctlFieldName, targetCurveData.ToAnimationCurve());
            }
        }

        public void ChangeAnimationCurveAllValue_RectTransform(AnimationClip _anim, string _pathName, string _ctlFieldName, float[] _values)
        {
            ChangeAnimationCurveValueAllKeyframe(_anim, typeof(RectTransform), _pathName, _ctlFieldName, _values);
        }

        /// <summary>
        /// 修改最后一帧的值（RectTransform修改，性能考虑，这个Type不在lua中指定）
        /// </summary>
        /// <param name="_anim"></param>
        /// <param name="_pathName"></param>
        /// <param name="_ctlFieldName"></param>
        /// <param name="_value"></param>
        public void ChangeAnimationCurveValueAtLast_RectTransform(AnimationClip _anim, string _pathName, string _ctlFieldName, float _value)
        {
            ChangeAnimationCurveValue_RectTransform(_anim, _pathName, _ctlFieldName, _value, -1);
        }
        public void ChangeAnimationCurveValueAtFirst_RectTransform(AnimationClip _anim, string _pathName, string _ctlFieldName, float _value)
        {
            ChangeAnimationCurveValue_RectTransform(_anim, _pathName, _ctlFieldName, _value, 1);
        }
        /// <summary>
        /// 修改指定帧的值（RectTransform修改，性能考虑，这个Type不在lua中指定）
        /// </summary>
        /// <param name="_anim"></param>
        /// <param name="_pathName"></param>
        /// <param name="_ctlFieldName"></param>
        /// <param name="_value"></param>
        public void ChangeAnimationCurveValue_RectTransform(AnimationClip _anim, string _pathName, string _ctlFieldName, float _value, int _index)
        {
            ChangeAnimationCurveValueAtTargetKeyframe(_anim, typeof(RectTransform), _pathName, _ctlFieldName, _value, _index);
        }

        public JsonData ReadAnimationCurveDataJson()
        {
            JsonData curveJson = JsonUtil.ReadJson(localPath, jsonName, true);
            return curveJson;
        }
    }
}
