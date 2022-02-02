using System;
using UnityEngine;
using DIY.Anim;
using DIY.Trigger;
using System.Collections.Generic;
using DIY.Events;

namespace DIY.Actor
{
    /// <summary>
    /// 第三人称主角控制
    /// </summary>
    public class RoleController:MonoBehaviour
    {
        public float speed_A = 160f;
        float speed_forward = 0; //前进速度
        public float speed_W = 1f;
        public float speed_S = 0.5f;
        public float speed_D = 160f;
        public Rigidbody rig;
        public float velocity = 0f;
        public float velocity_max = 1f;
        public Animator animator;
        private Dictionary<string, Transform> m_dic_part;
        private void Start()
        {
            rig = GetComponent<Rigidbody>();
            animator = AnimatorUtil.Bind(transform);
            m_dic_part = new Dictionary<string, Transform>();
            //移动事件绑定
            InputManager.Instance.AddKeyCodeEvent(KeyCode.A, delegate () {
                float speed = -Time.deltaTime * speed_A;
                Rotate(speed);
            });
            InputManager.Instance.AddKeyCodeEvent(KeyCode.LeftShift, delegate () {
                speed_forward = speed_W + speed_W;
            });
            InputManager.Instance.AddKeyCodeEvent(KeyCode.W, delegate () {
                float moveDelta = Time.deltaTime * speed_forward;
                MoveForward(moveDelta,speed_forward == speed_W?0.3f:0.7f);
            });
            InputManager.Instance.AddKeyCodeEvent(KeyCode.S, delegate () {
                float moveDelta = -Time.deltaTime * speed_S;
                MoveBack(moveDelta);
            });
            InputManager.Instance.AddKeyCodeEvent(KeyCode.D, delegate () {
                float speed = Time.deltaTime * speed_D;
                Rotate(speed);
            });
            //交互事件绑定
            //InputManager.Instance.AddKeyCodeEvent(KeyCode.E, delegate () {
            //    TriggerManager.Instance.DoTrigger();
            //});不再让手动开门了
            InputManager.Instance.AddKeyCodeEvent(KeyCode.L, delegate () {
                SwitchPartActive("Flashlight");
            });
            EventManager.Instance.Register_ParamsEmpty("PickUpFlashlight", delegate () {
                AddPart("Flashlight");
                SwitchPartActive("Flashlight");
            });
            //不再输入按键，需要减速
            InputManager.Instance.AddEvent_EmptyKeyCode(() => { 
                MoveForward(-Time.deltaTime);
                speed_forward = speed_W;
            });
            //将自己加入检测名单
            TriggerManager.Instance.AddTarget(transform);
        }

        public void SetAnimParam_Speed(float _speed)
        {
            animator.SetFloat("Speed", Mathf.Abs(_speed));
        }
        public virtual void Rotate(float _velocity)
        {
            transform.localRotation = Quaternion.Lerp(transform.localRotation,
                Quaternion.Euler(transform.localEulerAngles.x, transform.localEulerAngles.y + _velocity, transform.localEulerAngles.z),
                Time.deltaTime * 30);
            SetAnimParam_Speed(velocity == 0 ? 0.2f : velocity);
        }
        public virtual void RunSlow()
        {
            MoveForward(Time.deltaTime, 0.6f);
        }

        public virtual void Run()
        {
            MoveForward(Time.deltaTime, 1f);
        }
        public virtual void MoveForward(float _velocity, float _maxVelocity = 0.3f)
        {
            velocity_max = Mathf.Abs(_maxVelocity);
            velocity += _velocity;
            velocity = Mathf.Min(velocity, velocity_max);
            velocity = Mathf.Max(velocity, 0);
            rig.position = Vector3.Lerp(rig.position, rig.position + transform.forward * velocity * 0.3f, 5 * Time.deltaTime);
            SetAnimParam_Speed(this.velocity);
        }
        public virtual void MoveBack(float _velocity, float _maxVelocity = 0.3f)
        {
            velocity_max = Mathf.Abs(_maxVelocity) * -1;
            velocity += _velocity;
            velocity = Mathf.Max(velocity, velocity_max);
            rig.position = Vector3.Lerp(rig.position, rig.position + transform.forward * velocity * 0.3f, 5 * Time.deltaTime);
            SetAnimParam_Speed(this.velocity);
        }

        public void AddPart(string partName) {
            Transform part = transform.Find(partName);
            m_dic_part.Add(partName, part);
        }

        public void SwitchPartActive(string name) {
            if (m_dic_part.ContainsKey(name))
            {
                Transform part = m_dic_part[name];
                part.gameObject.SetActive(!part.gameObject.activeSelf);
            }
            else { 
                //就算找不到部件，似乎没什么事情要做
            }
        }

        public void SetPartActive(Transform part, bool active)
        {
            part.gameObject.SetActive(active);
        }
    }
}


