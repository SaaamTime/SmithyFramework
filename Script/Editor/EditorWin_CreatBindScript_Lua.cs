using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using System;
using UnityEngine.UI;

namespace DIY.Editor
{
    public class EditorWin_CreatBindScript_Lua : EditorWindow
    {
        //需要生成曲线的动画片段索引
        public GameObject targetPrefab;
        public string bindScriptString;
        public Dictionary<Type, string> dic_bindScriptMap;

        private void AutoAddComponent(Type _type) {
            string className = _type.Name;
            dic_bindScriptMap.Add(_type, $"this.{className.ToLower()}_" + "{0}" + $" = UIUtil.{className}.Bind(transform.Find("+"\"{1}\"))");
        }

        private void AutoAddComponent(Type _type,string _utilScript)
        {
            string className = _type.Name;
            dic_bindScriptMap.Add(_type, $"this.{className.ToLower()}_" + "{0}" + $" = {_utilScript}.Bind(transform.Find(" + "\"{1}\"))");
        }

        [MenuItem("Tools/组件工具/自动生成绑定脚本语言_Lua")]
        static void main()
        {
            EditorWindow.GetWindow<EditorWin_CreatBindScript_Lua>("自动生成绑定脚本语言");
            
            // Debug.Log(Application.dataPath);
        }

        #region 拖拽、结果显示、结果处理
        private Vector2 WindowView_AddSearchRangeLabel()
        {
            //GUIStyle searchRangeShow = GUI.skin.customStyles[4];
            //展示曲线保存的重要参数：保存路径，json文件名
            EditorGUILayout.BeginVertical();
            GUILayout.Space(50);
            //动态拖拽需要保存曲线的动画文件
            Rect pathRect = EditorGUILayout.GetControlRect(GUILayout.Width(300), GUILayout.Height(30));
            EditorGUI.TextField(pathRect, "需要生成绑定脚本的预设体(拖拽)");
            EditorGUILayout.EndVertical();
            UnityEngine.Object[] dragClips = EditorTool.GetDragObjects(pathRect);
            if (dragClips == null)
            {
                return pathRect.position;
            }
            if (dragClips[0].GetType() != typeof(GameObject))
            {
                EditorUtility.DisplayDialog("预设体拖动异常", "请拖动预设体文件，后缀名是prefab", "确定");
                dragClips = null;
                return pathRect.position;
            }

            targetPrefab = (GameObject)dragClips[0];
            AutoCreateScriptString(targetPrefab.transform);
            return pathRect.position;
        }

        void AutoCreateScriptString(Transform _transform)
        {
            string[] objNameSplit = _transform.name.Split('_');
            if (objNameSplit.Length>1)
            {
                string name_script = objNameSplit[1];
                string name_component = objNameSplit[0];
                foreach (var item in dic_bindScriptMap)
                {
                    if (item.Key.Name == name_component)
                    {
                        Type componentType = item.Key;
                        if (componentType != null && dic_bindScriptMap.ContainsKey(componentType))
                        {
                            string bind_script = string.Format(dic_bindScriptMap[componentType], name_script, AutoGetPathToRoot(_transform));
                            if (string.IsNullOrEmpty(bindScriptString))
                            {
                                bindScriptString = bind_script;
                            }
                            else
                            {
                                bindScriptString += ("\n" + bind_script);
                            }
                            break;
                        }
                    }
                }
                
            }
            if (_transform.childCount>0)
            {
                for (int i = 0; i < _transform.childCount; i++)
                {
                    Transform child = _transform.GetChild(i);
                    AutoCreateScriptString(child);
                }
            }
        }

        string AutoGetPathToRoot(Transform _childTrans) {
            string path = _childTrans.name;
            Transform targetTrans = _childTrans;
            while (targetTrans.parent!=targetPrefab.transform)
            {
                targetTrans = targetTrans.parent;
                path = targetTrans.name + "/" + path;
            }
            return path;
        }

        #endregion


        #region GUI生命周期
        void OnEnable()
        {
            dic_bindScriptMap = new Dictionary<Type, string>();
            AutoAddComponent(typeof(Image));
            AutoAddComponent(typeof(Text));
            AutoAddComponent(typeof(Button));
            AutoAddComponent(typeof(Animator),"AnimatorUtil");
            AutoAddComponent(typeof(Animation),"AnimationUtil");
        }
        void OnGUI()
        {
            if (!string.IsNullOrEmpty(bindScriptString))
            {
                GUILayout.BeginVertical();
                if (GUILayout.Button("清除生成语言"))
                {
                    bindScriptString = string.Empty;
                }
                GUI.color = Color.yellow;
                GUILayout.Box("绑定:");
                GUI.color = Color.white;
                GUILayout.TextArea(bindScriptString);
                GUILayout.EndVertical();
            }
            else {
                WindowView_AddSearchRangeLabel();
            }
        }
        void OnDestroy()
        {
            targetPrefab = null;
            bindScriptString = string.Empty;
        }
        #endregion
    }
}