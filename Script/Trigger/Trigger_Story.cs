using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DIY.Events;
using DIY.UI;

namespace DIY.Trigger
{
    public class Trigger_Story : Trigger_ByDistance
    {
        public string story;
        public override void Event_Do()
        {
            
        }

        public override void Event_InitState()
        {

        }

        public override void Event_InRangeState()
        {
            if (gameObject.activeSelf)
            {
                PanelParam param = new PanelParam();
                param.AddParam("content", story);
                UIManager.Instance.OpenPanel<UIPanel_SlideTip>(UIPanel_SlideTip.config, param);
            }
        }

        public override void Event_OutRangeState()
        {
            UIManager.Instance.ClosePanel(UIPanel_SlideTip.config.name);
        }
    }
}
