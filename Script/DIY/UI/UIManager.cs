using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DIY.UI
{
    public class UIManager : BaseManager<UIManager>
    {
        public Canvas mainCanvas;
        public Dictionary<string, UIPanel> dic_panels;

        public override void Reset()
        {
            //TODO:可能需要各个界面的清空与中断
            dic_panels.Clear();
        }

        protected override void Init()
        {
            dic_panels = new Dictionary<string, UIPanel>();
        }

        

        
    }
}
