using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DIY.CSharp;
using System.ComponentModel;
using DIY.Base;
using System;
using DIY.Anim;
using System.IO;

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
        public GameObject mainMask;
        public Dictionary<string, UIPanel> dic_panels;
        public Dictionary<string, UIPanel> dic_panels_always;
        public Stack<UIPanel> stack_activePanel;
        public override void Reset()
        {
            //TODO:可能需要各个界面的清空与中断
            dic_panels.Clear();
            stack_activePanel.Clear();
        }

        public void InitAlwaysPanels() {
            InitPanel<UIPanel_SlideTip>(UIPanel_SlideTip.config);
        }

        protected override void Init()
        {
            dic_panels = new Dictionary<string, UIPanel>();
            dic_panels_always = new Dictionary<string, UIPanel>();
            stack_activePanel = new Stack<UIPanel>();
            GameObject UIRoot = GameObject.Find("UI_Root");
            mainCanvas = UIRoot.GetComponent<Canvas>();
            mainMask = mainCanvas.transform.Find("MainMask").gameObject;
            GameObject.DontDestroyOnLoad(mainCanvas.gameObject);
            InitAlwaysPanels();
        }

        public UIPanel InitPanel<T>(UIPanelConfig target) where T : UIPanel
        {
            Dictionary<string, UIPanel> targetPanels;
            switch (target.panelType)
            {
                case UIPanelType.Normal:
                    targetPanels = dic_panels;
                    break;
                case UIPanelType.Hang:
                    targetPanels = dic_panels;
                    break;
                case UIPanelType.Pop:
                    targetPanels = dic_panels;
                    break;
                case UIPanelType.Always:
                    targetPanels = dic_panels_always;
                    break;
                default:
                    targetPanels = dic_panels;
                    break;
            }
            if (!targetPanels.ContainsKey(target.name))
            {
                //首次打开界面，需要加载
                GameObject panelPrefab = Asset.AssetUtil.AutoLoad<GameObject>(target.path);
                GameObject panelObj = GameObject.Instantiate(panelPrefab);
                panelObj.transform.SetParent(mainCanvas.transform);
                panelObj.transform.localScale = Vector3.one;
                UIUtil.SetArchor_MaxView(panelObj.transform);
                UIPanel targetPanel = panelObj.AddComponent<T>();
                //如果该界面有主动画，需要直接跳至帧动画的最后一帧，为打开待命
                if (targetPanel.Check_HasAnim())
                {
                    targetPanel.gameObject.SetActive(true);
                    targetPanel.panelMainAnim.PlayJumpToEnd(true);
                    targetPanel.gameObject.SetActive(false);
                }
                targetPanels.Add(target.name, targetPanel);
                return targetPanel;
            }
            return null;
        }

        public void OpenPanel<T>(UIPanelConfig target, PanelParam panelParam = null) where T : UIPanel
        {
            Dictionary<string, UIPanel> targetPanels;
            switch (target.panelType)
            {
                case UIPanelType.Normal:
                    targetPanels = dic_panels;
                    break;
                case UIPanelType.Hang:
                    targetPanels = dic_panels;
                    break;
                case UIPanelType.Pop:
                    targetPanels = dic_panels;
                    break;
                case UIPanelType.Always:
                    targetPanels = dic_panels_always;
                    break;
                default:
                    targetPanels = dic_panels;
                    break;
            }
            UIPanel targetPanel;
            if (!targetPanels.ContainsKey(target.name))
            {
                targetPanel = InitPanel<T>(target);
            }
            else {
                targetPanel = targetPanels[target.name];
            }

            targetPanel.gameObject.SetActive(true);
            stack_activePanel.Push(targetPanel);
            if (targetPanel.Check_HasAnim())
            {
                SwitchMainMask(true);
                targetPanel.Open(panelParam);
                targetPanel.panelMainAnim.Play_Forward(null,0f,()=> {
                    SwitchMainMask(false);
                });
            }
        }

        public void ClosePanel(string panelName) {
            if (stack_activePanel.Count < 1)
            {
                return;
            }
            UIPanel topPanel = stack_activePanel.Peek();
            if (null != topPanel)
            {
                topPanel = stack_activePanel.Pop();
                if (topPanel.Check_HasAnim())
                {
                    SwitchMainMask(true);
                    topPanel.panelMainAnim.Play_Back(null,1f,() =>{
                        topPanel.Close();
                        SwitchMainMask(false);
                    });
                }
            }
        }

        public void SwitchMainMask(bool active) {
            mainMask.transform.SetAsLastSibling();
            mainMask.SetActive(active);
        }


    }
}
