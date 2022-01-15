using System.Collections.Generic;
using UnityEngine;

namespace XAsset
{
    public class PlayerSettings : ScriptableObject
    {
        public List<string> assets = new List<string>();
        public bool offlineMode;
    }
}