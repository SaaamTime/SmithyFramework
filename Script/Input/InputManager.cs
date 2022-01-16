
using UnityEngine;
using System;
using System.Collections;
using DIY;
using DIY.Base;
using System.Collections.Generic;

public class InputManager  : BaseManager<InputManager>
{
    private Dictionary<KeyCode, List<Action>> m_dic_keycodeEvents;
    private List<Action> m_list_noKeycodeEvent;
    protected override void Init()
    {
        m_dic_keycodeEvents = new Dictionary<KeyCode, List<Action>>();
        m_list_noKeycodeEvent = new List<Action>();
    }

    public void AddKeyCodeEvent(KeyCode keycode,Action keycodeEvent) {
        if (!m_dic_keycodeEvents.ContainsKey(keycode))
        {
            m_dic_keycodeEvents[keycode] = new List<Action>();
        }
        List<Action> keycodeList = m_dic_keycodeEvents[keycode];
        keycodeList.Add(keycodeEvent);
    }

    public void RemoveKeyCodeEvent(KeyCode keycode,Action keycodeEvent) {
        List<Action> keycodeList = m_dic_keycodeEvents[keycode];
        if (keycodeList!=null && keycodeList.Contains(keycodeEvent))
        {
            keycodeList.Remove(keycodeEvent);
        }
    }

    public void ClearKeyCodeEvent(KeyCode keycode) {
        if (m_dic_keycodeEvents.ContainsKey(keycode))
        {
            m_dic_keycodeEvents[keycode].Clear();
        }
    }

    public void ClearAllKeyCode() {
        m_dic_keycodeEvents.Clear();
    }

    public override void Reset()
    {
        Init();
    }

    private void Update()
    {
        //按鍵监听
        if (Input.anyKey)
        {
            foreach (var item in m_dic_keycodeEvents)
            {
                if (Input.GetKey(item.Key))
                {
                    foreach (var keycodeEvent in item.Value)
                    {
                        keycodeEvent.Invoke();
                    }
                }
            }
        }
        else {
            foreach (var item in m_list_noKeycodeEvent)
            {
                item.Invoke();
            }
        }
        
        //鼠标监听
    }

    
}
