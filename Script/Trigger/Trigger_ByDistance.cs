using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DIY.Trigger
{

    public abstract class Trigger_ByDistance : MonoBehaviour
    {
        // Start is called before the first frame update
        public float distance = 1.5f;
        public Animation animation;//如果没有动画播放，可以不关注该组件
        public State_TriggerByDistance state;
        public abstract void Event_InRangeState();
        public abstract void Event_Do();
        public abstract void Event_OutRangeState();
        public abstract void Event_InitState();

        public void AutoUpdateState_CheckDistance(Vector3 _rolePosition)
        {
            State_TriggerByDistance currentState = Mathf.Abs(Vector3.Distance(_rolePosition, transform.position)) <= distance ? State_TriggerByDistance.InRange : State_TriggerByDistance.OutRange;
            if (currentState != state)
            {
                state = currentState;
                AutoSwitchState();
            }
        }

        public void AutoSwitchState()
        {
            switch (state)
            {
                case State_TriggerByDistance.Init:
                    Event_InitState();
                    break;
                case State_TriggerByDistance.OutRange:
                    Event_OutRangeState();
                    break;
                case State_TriggerByDistance.InRange:
                    Event_InRangeState();
                    break;
                default:
                    break;
            }
        }

        public void AutoDo()
        {
            if (state == State_TriggerByDistance.InRange)
            {
                Event_Do();
            }
        }
        protected virtual void Init()
        {
            state = State_TriggerByDistance.Init;
            if (animation == null)
            {
                animation = GetComponent<Animation>();
            }
            AutoSwitchState();
            TriggerManager.Instance.Register(this);
        }
        void Start()
        {
            Init();
        }
    }

    public enum State_TriggerByDistance
    {
        Init = 0,
        OutRange = 1,
        InRange = 2,
    }

}
