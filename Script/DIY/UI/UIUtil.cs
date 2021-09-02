using LitJson;
using UnityEngine;
using System.Collections.Generic;

namespace DIY.UI
{
    public static class UIUtil
    {

        public static JsonData AutoBindNameTree(Transform _transRoot) {
            JsonData nameTree = new JsonData();
            if (_transRoot.childCount > 0)
            {
                JsonData nameTree_child = new JsonData();
                for (int i = 0; i < _transRoot.childCount; i++)
                {
                    Transform child = _transRoot.GetChild(i);
                    nameTree_child[child.name] = child.childCount > 0 ? AutoBindNameTree(child) : 0;
                }
                nameTree[_transRoot.name] = nameTree_child;
            }
            return nameTree;
        }
    }
}
