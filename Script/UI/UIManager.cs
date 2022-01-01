using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DIY.CSharp;
using System.ComponentModel;

namespace DIY.UI
{
    public enum UIPanelType
    {
        [Description("普通界面")]
        Normal = 2,
        [Description("依附普通界面，辅助显示")]
        Hang = 10,
        [Description("弹窗界面")]
        Pop = 20,
        [Description("常用界面，一般时间隐藏，不会销毁")]
        Always = 100,

        
    }
    public class UIManager : BaseManager<UIManager>
    {
        public Canvas mainCanvas;
        public Dictionary<string, UIPanel> dic_panels;
        public Dictionary<string, UIPanel> dic_panels_always;
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
