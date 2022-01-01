using LitJson;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace DIY.Anim
{

    /// <summary>
    /// 一条曲线的所有关键帧数据（一个动画片段中有数条曲线）
    /// </summary>
    public class AnimationCurveData
    {
        public string animName;
        public string pathName;
        public string ctlFieldName;
        //public Dictionary<string,string> keyframes;
        public List<Keyframe> keyframes;
        /// <summary>
        /// 曲线数据初始化
        /// 注： 不采用曲线直接解析成数据的原因是：曲线解析仅在Editor下才可以使用，该曲线数据需要在游戏中生效，故要脱离曲线进行初始化
        /// </summary>
        /// <param name="_animName"></param>
        /// <param name="_transPath"></param>
        /// <param name="_propertyName"></param>
        public AnimationCurveData(string _animName, string _transPath, string _propertyName, Keyframe[] _keys = null)
        {
            this.animName = _animName;
            this.pathName = _transPath;
            this.ctlFieldName = _propertyName;
            keyframes = new List<Keyframe>();
            if (null != _keys)
            {
                for (int i = 0; i < _keys.Length; i++)
                {
                    AddKeyframe(_keys[i]);
                }
            }
        }

        public void AddKeyframe(Keyframe _keyframe)
        {
            keyframes.Add(_keyframe);
        }

        /// <summary>
        /// 通过字符串参数，增加曲线关键帧
        /// </summary>
        /// <param name="_keyfameParam"></param>
        public void AddKeyframeByParamStr(string _keyfameParam)
        {
            Keyframe newKeyframe = AnimExtension.StringParamToKeyframe(_keyfameParam);
            AddKeyframe(newKeyframe);
        }

        /// <summary>
        /// 获取该曲线的所有帧time-value
        /// </summary>
        /// <returns></returns>
        public Dictionary<float, float> GetAllKeyframe_KeyValue()
        {
            Dictionary<float, float> result = new Dictionary<float, float>();
            for (int i = 0; i < keyframes.Count; i++)
            {
                Keyframe keyframe = keyframes[i];
                result.Add(keyframe.time, keyframe.value);
            }
            return result;
        }

        /// <summary>
        /// 关键帧的时间，由小到大排列
        /// </summary>
        public void ReorderKeyframes()
        {
            keyframes.Sort((_a, _b) => { return _a.time.CompareTo(_b.time); });
        }

        public void SetTargetKeyframeValue(float _time, float _value)
        {
            for (int i = 0; i < keyframes.Count; i++)
            {
                Keyframe keyframe = keyframes[i];
                if (Mathf.Abs(keyframe.time - _time) <= 0.03)//相差在0.033之内，就判定为相同
                {
                    keyframe.value = _value;
                }
            }
        }

        public Keyframe? GetTargetKeyframe(float _time)
        {
            foreach (Keyframe item in keyframes)
            {
                if (item.time == _time)
                {
                    return item;
                }
            }
            return null;
        }

        public int GetKeyframeCount()
        {
            return keyframes.Count;
        }

        /// <summary>
        /// 核心方法（自行判断条件，修改该曲线关键帧的值，仅修改value，其他不推荐修改）
        /// </summary>
        /// <param name="_findFunc">关键帧time，关键帧value，关键帧排序下标，关键帧总数量</param>
        /// <param name="_changeFunc">找到关键帧后，将对应关键帧的值改为返回的值</param>
        public void WalkForChangeKeyframe(Func<float, float, int, int, bool> _findFunc, Func<int, float> _changeFunc)
        {
            int totalCount = keyframes.Count;
            for (int i = 0; i < totalCount; i++)
            {
                Keyframe keyframe = keyframes[i];
                if (_findFunc(keyframe.time, keyframe.value, i, totalCount))
                {
                    Keyframe newKeyframe = keyframe.CloneSelfWithNewValue(_changeFunc(i));
                    keyframes[i] = newKeyframe;
                }
            }
        }

        /// <summary>
        /// 将所有参数转为一列字符串
        /// </summary>
        /// <returns>"Clip名称？控制对象路径?关键帧节点名？time,value,inTangent,outTangent,inWeight,outWeight,weightedMode|。。。"</returns>
        public string ToParam()
        {
            StringBuilder param = new StringBuilder();
            param.AppendFormat("{0}?{1}?{2}?", animName, pathName, ctlFieldName);
            param.Append(keyframes[0].ToParams());
            for (int i = 1; i < keyframes.Count; i++)
            {
                param.AppendFormat("|{0}", keyframes[i].ToParams());
            }
            return param.ToString();
        }

        public JsonData ToJsonLine()
        {
            JsonData keyframesJson = new JsonData();
            for (int i = 0; i < keyframes.Count; i++)
            {
                Keyframe keyframe = keyframes[i];
                keyframesJson[keyframe.time.ToString()] = keyframe.ToParams();
            }
            return keyframesJson;
        }

        /// <summary>
        /// 将曲线数据转化为json单元,并携带json的Key值
        /// </summary>
        /// <returns>[0]:TransPath,[1]:ctlFieldName</returns>
        public string[] ToJson(out JsonData _paramJson)
        {
            string[] jsonKeys = new string[2];
            jsonKeys[0] = pathName;
            jsonKeys[1] = ctlFieldName;
            _paramJson = ToJsonLine();
            return jsonKeys;
        }

        public AnimationCurve ToAnimationCurve()
        {
            AnimationCurve curve = new AnimationCurve();
            for (int i = 0; i < keyframes.Count; i++)
            {
                curve.AddKey(keyframes[i]);
            }
            return curve;
        }
    }

}
