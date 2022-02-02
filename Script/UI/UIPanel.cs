using UnityEngine;
using DIY.Json;
using System.IO;
using LitJson;
using System.Collections.Generic;
using System;

namespace DIY.UI
{ 
    public class UIPanelConfig
    {
        public string path;
        public UIPanelType panelType;
        public string name;
    }

    public abstract class UIPanel : MonoBehaviour
    {
        protected static UIPanelConfig m_config;
        public static UIPanelConfig config
        {
            get {
                if (null==m_config)
                {
                    m_config = new UIPanelConfig();
                }
                return m_config;
            }
        }
        public PanelParam panelParam;
        public Animation panelMainAnim;
        #region UI界面核心行为
        public abstract void Init();//首次初始化
        public abstract void Open(PanelParam panelParam=null);//打开界面
        public abstract void Refresh();//刷新界面（有可能界面已经打开了，仅仅是刷新一下）
        public abstract void Close();//关闭界面
        public abstract void Destroy();//界面销毁
        public bool Check_IsActive() {
            return gameObject.activeSelf;
        }
        public bool Check_HasAnim() {
            return panelMainAnim != null;
        }

        #endregion
        private void Awake()
        {
            Init();
        }

        private void OnDestroy()
        {
            if (null!= panelParam)
            {
                panelParam.Destory();
                panelParam = null;
            }
            Destroy();
        }
    }

    public class PanelParam
    {
        private Dictionary<string, string> m_data;
        //界面回调，原则上界面内传递的回调不允许太多，仅设置几个够用就行，不再系统管理
        private Action event_OK;
        private Action event_Cancel;
        
        public PanelParam() {
            m_data = new Dictionary<string, string>();
        }

        public void AddParam(string key,string value) {
            if (m_data.ContainsKey(key))
            {
                return;
            }
            m_data.Add(key, value);
        }

        public void AddParam(string key,int value) {
            AddParam(key, value.ToString());
        }

        public void AddParam(string key, float value)
        {
            AddParam(key, value.ToString());
        }

        public void AddParam(string key,bool value) {
            AddParam(key, value.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="index">1:OK事件、2:取消事件、3:自定义回调事件</param>
        public void AddParam(Action callback,int index) {
            if (index == 1)
            {
                event_OK = callback;
            }
            else if (index == 2)
            {
                event_Cancel = callback;
            } 
            else if (index == 3) 
            { 
                
            }
        }

        public string GetString(string key) {
            if (m_data.ContainsKey(key))
            {
                return m_data[key];
            }
            else {
                return string.Empty;
            }
        }

        public int GetInt(string key) {
            return int.Parse(GetString(key));
        }

        public float GetFloat(string key) {
            return float.Parse(GetString(key));
        }

        public bool GetBool(string key) {
            return bool.Parse(GetString(key));
        }

        public void AutoDoCallback(int index) {
            if (index == 1)
            {
                if (event_OK!=null)
                {
                    event_OK.Invoke();
                }
            }
            else if (index == 2)
            {
                if (event_Cancel!=null)
                {
                    event_Cancel.Invoke();
                }
            }
            else if (index == 3)
            {

            }
            event_OK = null;
            event_Cancel = null;
        }

        public void Destory()
        {
            m_data.Clear();
            m_data = null;
            event_OK = null;
            event_Cancel = null;
        }
    
    }
}
