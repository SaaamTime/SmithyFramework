using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerManager 
{
    private static TriggerManager _instance;
    public static TriggerManager Instance
    {
        get {
            if (_instance==null)
            {
                _instance = new TriggerManager();
            }
            return _instance;
        }
    }
    private TriggerManager() {
        triggers_byDistance = new List<Trigger_ByDistance>();
    }

    public List<Trigger_ByDistance> triggers_byDistance;

    public void Register(Trigger_ByDistance _trigger) {
        if (!triggers_byDistance.Contains(_trigger))
        {
            triggers_byDistance.Add(_trigger);
        }
    }

    public void Remove(Trigger_ByDistance _trigger) {
        triggers_byDistance.Remove(_trigger);
    }

    public void Clear() {
        triggers_byDistance.Clear();
    }

    public void Update(Vector3 _rolePosition) {
        foreach (Trigger_ByDistance _trigger in triggers_byDistance)
        {
            _trigger.AutoUpdateState_CheckDistance(_rolePosition);
        }
    }

    public void DoTrigger() {
        foreach (Trigger_ByDistance _trigger in triggers_byDistance)
        {
            _trigger.AutoDo();
        }
    }
}
