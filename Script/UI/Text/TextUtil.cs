using System;
using UnityEngine;
using UnityEngine.UI;

namespace DIY.UI
{
    public  class TextUtil: SmithyFramework.Base.SingleClassBase<TextUtil>
    {
        public static Text Bind(Transform _textTrans, bool _openRay = false)
        {
            Text text = _textTrans.GetComponent<Text>();
            text.raycastTarget = _openRay;
            return text;
        }
    }
}
