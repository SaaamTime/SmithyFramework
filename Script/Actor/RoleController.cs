using System;
using UnityEngine;
using DIY.Anim;
namespace DIY.Actor
{
    /// <summary>
    /// 第三人称主角控制
    /// </summary>
    public class RoleController:MonoBehaviour
    {
        public float speed_A;
        public float speed_W;
        public float speed_S;
        public float speed_D;

        public Animator animator;
        private void Start()
        {
            animator = AnimatorUtil.Bind(transform);
            //移动事件绑定
            InputManager.Instance.AddKeyCodeEvent(KeyCode.A, delegate () {
                float moveDelta = -Time.deltaTime * speed_A;

            });
            InputManager.Instance.AddKeyCodeEvent(KeyCode.W, delegate () {
                float moveDelta = -Time.deltaTime * speed_W;

            });
            InputManager.Instance.AddKeyCodeEvent(KeyCode.S, delegate () {
                float moveDelta = -Time.deltaTime * speed_S;

            });
            InputManager.Instance.AddKeyCodeEvent(KeyCode.D, delegate () {
                float moveDelta = -Time.deltaTime * speed_D;

            });
            //交互事件绑定
            InputManager.Instance.AddKeyCodeEvent(KeyCode.E, delegate () {

            });
        }
    }
}
