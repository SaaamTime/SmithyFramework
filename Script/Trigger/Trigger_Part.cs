using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DIY.Events;
using DIY.UI;

namespace DIY.Trigger
{
    public class Trigger_Part : Trigger_ByDistance
    {
        public string partName;
        public override void Event_Do()
        {
            //自己需要隐藏
            gameObject.SetActive(false);
            //激活主角身上的对应部件，并默认开启
            EventManager.Instance.Dispatch_ParamEmpty(string.Format("PickUp{0}", partName));
        }

        public override void Event_InitState()
        {

        }

        public override void Event_InRangeState()
        {
            if (gameObject.activeSelf)
            {
                PanelParam param = new PanelParam();
                param.AddParam("content", string.Format("是否要捡起 <color=red>{0}</color>", partName));
                param.AddParam(delegate ()
                {
                    Event_Do();
                }, 1);
                UIManager.Instance.OpenPanel<UIPanel_SlideTip>(UIPanel_SlideTip.config, param);
            }
        }

        public override void Event_OutRangeState()
        {
            UIManager.Instance.ClosePanel(UIPanel_SlideTip.config.name);
        }
    }
}
