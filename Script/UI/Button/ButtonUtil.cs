using UnityEngine.UI;
using UnityEngine;
using System;
using UnityEngine.Events;

namespace DIY.UI
{
    public  class ButtonUtil:DIY.Base.SingleClassBase<ButtonUtil>
    {
        public static Button Bind(Transform _buttonTrans, UnityAction _buttonEvent)
        {
            Button button = _buttonTrans.GetComponent<Button>();
            if (_buttonEvent != null)
            {
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(_buttonEvent);
            }
            return button;
        }
    }
}
