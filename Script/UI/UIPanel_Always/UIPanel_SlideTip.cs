using DIY;
using DIY.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using DIY.Anim;
public class UIPanel_SlideTip : UIPanel
{
    public Image image_bg;
    public Button button_click;
    public Text text_buttonName;
    public Text text_tip;
    public new static UIPanelConfig config
    {
        get {
            if (m_config == null)
            {
                m_config = new UIPanelConfig();
                m_config.name = "Panel_SliderTip";
                m_config.panelType = UIPanelType.Always;
                m_config.path = PathUtil.GetDataPath("SmithyFramework/AV/UI","Panel_topTip.prefab",true);
            }
            return m_config;
        }
    }

    public override void Init()
    {
        panelMainAnim = AnimationUtil.Bind(transform.Find("Animation_mainPanel"));
        image_bg = UIUtil.Image.Bind(transform.Find("Animation_mainPanel/Image_bg"));
        text_tip = UIUtil.Text.Bind(transform.Find("Animation_mainPanel/Text_tip"));
        button_click = UIUtil.Button.Bind(transform.Find("Animation_mainPanel/Button_click"),() => {
            panelParam.AutoDoCallback(1);
            UIManager.Instance.ClosePanel(config.name);
        });
        text_buttonName = UIUtil.Text.Bind(transform.Find("Animation_mainPanel/Button_click/Text_buttonName"));
    }

    public override void Open(PanelParam panelParam = null)
    {
        this.panelParam = panelParam;
        this.Refresh();
    }

    public override void Refresh()
    {
        string content = panelParam.GetString("content");
        text_tip.text = content;
        string buttonName = panelParam.GetString("buttonName");
        if (string.IsNullOrEmpty(buttonName))
        {
            text_buttonName.text = "确定";
        }
        else {
            text_buttonName.text = buttonName;
        }
    }

    public override void Close()
    {

    }

    public override void Destroy()
    {
        
    }

}

