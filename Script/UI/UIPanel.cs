using UnityEngine;
using DIY.Json;
using System.IO;
using LitJson;
namespace DIY.UI
{
    public abstract class UIPanel:MonoBehaviour
    {
        public string path;

        #region UI界面核心行为
        protected abstract void Init();//首次初始化
        protected abstract void Open();//打开界面
        protected abstract void Refresh();//刷新界面（有可能界面已经打开了，仅仅是刷新一下）
        protected abstract void Close();//关闭界面
        protected abstract void Destroy();//界面销毁
        #endregion
        private void Awake()
        {
            Init();
        }
    }
}
