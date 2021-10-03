using UnityEngine.UI;
using UnityEngine;
using Button_U3D = UnityEngine.UI.Button;
using System;
using UnityEngine.Events;

namespace DIY.UI.Button
{
    public static class ButtonUtil
    {
        public static Button_U3D Bind(Transform _buttonTrans, UnityAction _buttonEvent)
        {
            Button_U3D button = _buttonTrans.GetComponent<Button_U3D>();
            if (_buttonEvent != null)
            {
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(_buttonEvent);
            }
            return button;
        }
    }
}
