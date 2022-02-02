using LitJson;
using UnityEngine;
using System.Collections.Generic;

namespace DIY.UI
{
    public class UIUtil
    {
        public static TextUtil Text = TextUtil.Instance;
        public static ButtonUtil Button = ButtonUtil.Instance;
        public static ImageUtil Image = ImageUtil.Instance;

        public static JsonData AutoBindNameTree(Transform _transRoot,bool _isChild=false) {
            JsonData nameTree = new JsonData();
            if (_transRoot.childCount > 0)
            {
                for (int i = 0; i < _transRoot.childCount; i++)
                {
                    Transform child = _transRoot.GetChild(i);
                    nameTree[child.name] = child.childCount > 0 ? AutoBindNameTree(child) : 0;
                }
            }
            return nameTree;
        }

        public static void SetArchor_MaxView(Transform target) {
            RectTransform rect = target as RectTransform;
            rect.anchorMin = Vector2.one;
            rect.anchorMin = Vector2.zero;
            rect.pivot = Vector2.one * 0.5f;
            rect.offsetMax = Vector2.zero;
            rect.offsetMin = Vector2.zero;
        }

    }
}
