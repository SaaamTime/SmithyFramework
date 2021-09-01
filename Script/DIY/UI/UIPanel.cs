using UnityEngine;
using DIY.Json;
using System.IO;
using LitJson;
namespace DIY.UI
{
    public class UIPanel:MonoBehaviour
    {
        public string name;
        private void Awake()
        {
            JsonData jd = JsonUtil.ReadJson("Script", "UIMapTest");
        }
    }
}
