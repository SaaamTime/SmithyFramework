using LitJson;
using UnityEngine;
using System.Collections.Generic;

namespace DIY.UI
{
    public class UIUtil
    {
        public static TextUtil Text;
        public static ButtonUtil Button;
        public static ImageUtil Image;

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

    }
}
