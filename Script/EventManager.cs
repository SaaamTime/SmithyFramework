using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DIY.Base;

namespace DIY.Events
{
    public class EventManager:SingleClassBase<EventManager>
    {

        private Dictionary<string, Action<string>> actions_string;
        private Dictionary<string, Action> actions_empty;

        protected override void Init()
        {
            actions_string = new Dictionary<string, Action<string>>();
            actions_empty = new Dictionary<string, Action>();
        }

        public void Register_ParamsString(string eventID, Action<string> action) {
            if (!actions_string.ContainsKey(eventID))
            {
                actions_string.Add(eventID, action);
            }
        }
        public void Remove_ParamString(string eventID) {
            if (actions_string.ContainsKey(eventID))
            {
                actions_string.Remove(eventID);
            }
        }

        public void Dispatch_ParamString(string eventID,string param) {
            if (actions_string.ContainsKey(eventID))
            {
                actions_string[eventID].Invoke(param);
            }
        }

        public void Register_ParamsEmpty(string eventID, Action action)
        {
            if (!actions_empty.ContainsKey(eventID))
            {
                actions_empty.Add(eventID, action);
            }
        }
        public void Remove_ParamEmpty(string eventID)
        {
            if (actions_empty.ContainsKey(eventID))
            {
                actions_empty.Remove(eventID);
            }
        }

        public void Dispatch_ParamEmpty(string eventID)
        {
            if (actions_empty.ContainsKey(eventID))
            {
                actions_empty[eventID].Invoke();
            }
        }

        public void Clear() {
            actions_string.Clear();
            actions_empty.Clear();
        }

        public void Destory() {
            Clear();
        }
    }

}
