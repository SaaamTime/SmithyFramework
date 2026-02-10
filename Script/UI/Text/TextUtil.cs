using System;
using UnityEngine;
using UnityEngine.UI;

namespace DIY.UI
{
    public class TextUtil: DIY.Base.SingleClassBase<TextUtil>
    {
        public Text Bind(Transform _textTrans, bool _openRay = false)
        {
            Text text = _textTrans.GetComponent<Text>();
            text.raycastTarget = _openRay;
            return text;
        }

        protected override void Init()
        {
            throw new NotImplementedException();
        }
    }
}
