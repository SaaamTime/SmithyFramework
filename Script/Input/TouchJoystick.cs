using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DIY.Input
{
    /// <summary>
    /// UI触摸摇杆控制器
    /// 支持鼠标/触摸操作，可自定义摇杆尺寸，指定控制对象
    /// </summary>
    public class TouchJoystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
    {
        [Header("摇杆基础配置")]
        [Tooltip("摇杆背景（外圈）")]
        public RectTransform joystickBackground;
        [Tooltip("摇杆手柄（内圈）")]
        public RectTransform joystickHandle;
        [Tooltip("摇杆的宽度（像素）")]
        public float joystickWidth = 250f;
        [Tooltip("摇杆的高度（像素）")]
        public float joystickHeight = 250f;

        [Header("控制对象配置")]
        [Tooltip("需要控制的游戏对象")]
        public Transform targetObject;
        [Tooltip("控制对象的移动速度")]
        public float moveSpeed = 5f;
        public Vector2 moveRangeX = Vector2.zero;
        public Vector2 moveRangeY = Vector2.zero;

        // 摇杆的中心位置
        private Vector2 joystickCenter = Vector2.zero;
        // 摇杆的最大移动半径（取长宽的最小值的一半，保证在圈内）
        private float maxRadius;
        // 最终的移动方向向量（x:左右，y:前后/上下）
        [SerializeField]
        private Vector2 moveDirection = Vector2.zero;

        public Action[] onMove = new Action[5];
        public Action[] onStop = new Action[2];

        private void Start()
        {
            // 初始化摇杆尺寸
            // SetJoystickSize();//改为直接指定，UI上直接拉好，所见所得
            // 获取摇杆中心位置
            // joystickCenter = joystickBackground.anchoredPosition;//摇杆的中心位置恒定就是0点，不能动态获取
            // 计算最大移动半径（避免手柄超出背景）
            maxRadius = Mathf.Min(joystickWidth, joystickHeight) / 2f;
            // 初始化手柄位置到中心
            ResetJoystick();
        }

        private void Update()
        {
            // 如果指定了控制对象，根据方向向量移动
            if (targetObject != null)
            {
                MoveTargetObject();
            }
        }

        /// <summary>
        /// 设置摇杆的长宽尺寸
        /// </summary>
        private void SetJoystickSize()
        {
            if (joystickBackground != null)
            {
                joystickBackground.sizeDelta = new Vector2(joystickWidth, joystickHeight);
            }
            // 手柄尺寸设为背景的1/3，保证比例协调
            if (joystickHandle != null)
            {
                float handleSize = Mathf.Min(joystickWidth, joystickHeight) / 3f;
                joystickHandle.sizeDelta = new Vector2(handleSize, handleSize);
            }
        }

        /// <summary>
        /// 控制目标对象移动
        /// </summary>
        private void MoveTargetObject()
        {
            // 将2D方向向量转换为3D世界空间的移动方向（x:左右，z:前后）
            // Vector3 movement = new Vector3(moveDirection.x, 0f, moveDirection.y) * moveSpeed * Time.deltaTime;
            Vector3 movement = new Vector3(moveDirection.x, moveDirection.y, 0f) * moveSpeed * Time.deltaTime;
            // 移动目标对象
            targetObject.Translate(movement, Space.World);
        }

        /// <summary>
        /// 拖拽摇杆时触发
        /// </summary>
        public void OnDrag(PointerEventData eventData)
        {
            // 将屏幕坐标转换为摇杆本地坐标
            Vector2 touchPosition;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                joystickBackground,
                eventData.position,
                eventData.pressEventCamera,
                out touchPosition
            );

            // 计算手柄相对于中心的偏移
            Vector2 offset = touchPosition - joystickCenter;
            // 限制偏移在最大半径内（避免超出背景）
            offset = Vector2.ClampMagnitude(offset, maxRadius);
            // 更新手柄位置
            joystickHandle.anchoredPosition = joystickCenter + offset;

            // 计算归一化的移动方向（范围-1~1）
            moveDirection = offset / maxRadius;

            AutoInvokeMoveActions();
        }

        /// <summary>
        /// 按下摇杆时触发
        /// </summary>
        public void OnPointerDown(PointerEventData eventData)
        {
            OnDrag(eventData); // 按下时直接触发拖拽逻辑
        }

        /// <summary>
        /// 松开摇杆时触发
        /// </summary>
        public void OnPointerUp(PointerEventData eventData)
        {
            ResetJoystick();
            AutoInvokeStopActions();
        }

        /// <summary>
        /// 重置摇杆到初始状态
        /// </summary>
        private void ResetJoystick()
        {
            // 手柄回到中心
            joystickHandle.anchoredPosition = Vector2.zero;
            // 移动方向归零
            moveDirection = Vector2.zero;
        }

        /// <summary>
        /// 对外暴露移动方向（供其他脚本调用）
        /// </summary>
        public Vector2 GetMoveDirection()
        {
            return moveDirection;
        }

        private void AutoInvokeMoveActions()
        {
            for (int i = 0; i < onMove.Length; i++)
            {
                onMove[i]?.Invoke();
            }
        }

        private void AutoInvokeStopActions()
        {
            for (int i = 0; i < onStop.Length; i++)
            {
                onStop[i]?.Invoke();
            }
        }

        public void BindMoveAction(Action action)
        {
            for (int i = 0; i < onMove.Length; i++)
            {
                if (onMove[i] == null)
                {
                    onMove[i] = action;
                    return;
                }
            }
        }

        public void BindStopAction(Action action)
        {
            for (int i = 0; i < onStop.Length; i++)
            {
                if (onStop[i] == null)
                {
                    onStop[i] = action;
                    return;
                }
            }
        }
    }
}
