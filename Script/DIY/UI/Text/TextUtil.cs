using System;
using UnityEngine;
using UnityEngine.UI;
using Text_U3D = UnityEngine.UI.Text;

namespace DIY.UI.Text
{
    public static class TextUtil
    {
        public static Text_U3D Bind(Transform _textTrans, bool _openRay = false)
        {
            Text_U3D text = _textTrans.GetComponent<Text_U3D>();
            text.raycastTarget = _openRay;
            return text;
        }
    }
}
