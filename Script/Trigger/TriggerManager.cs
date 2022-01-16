using DIY.Base;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DIY.Trigger
{
    public class TriggerManager:BaseManager<TriggerManager>
    {

        public List<Trigger_ByDistance> triggers_byDistance;
        public List<Transform> list_checkList;
        public void Register(Trigger_ByDistance _trigger)
        {
            if (!triggers_byDistance.Contains(_trigger))
            {
                triggers_byDistance.Add(_trigger);
            }
        }
        public void Remove(Trigger_ByDistance _trigger)
        {
            triggers_byDistance.Remove(_trigger);
        }
        public void Clear()
        {
            triggers_byDistance.Clear();
        }

        public void AddTarget(Transform target) {
            if (list_checkList.Contains(target))
            {
                list_checkList.Add(target);
            }
        }

        private void Update()
        {
            foreach (Trigger_ByDistance _trigger in triggers_byDistance)
            {
                foreach (var item in list_checkList)
                {
                    _trigger.AutoUpdateState_CheckDistance(item.position);
                }
            }
        }
        public void DoTrigger()
        {
            foreach (Trigger_ByDistance _trigger in triggers_byDistance)
            {
                _trigger.AutoDo();
            }
        }

        protected override void Init()
        {
            triggers_byDistance = new List<Trigger_ByDistance>();
        }

        public override void Reset()
        {
            Init();
        }
    }
}