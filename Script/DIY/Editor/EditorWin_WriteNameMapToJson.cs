using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using System.IO;
using LitJson;
using System.Collections;
using System.Text;
using DIY.Json;
using DIY.Asset;

namespace DIY.Editor
{
    public class EditorWin_WriteNameMapToJson : EditorWindow
    {
        //需要生成曲线的动画片段索引
        public List<GameObject> prefabs;
        
        [MenuItem("Tools/组件工具/整个预设体记录NameMap为Json")]
        static void main()
        {
            EditorWindow.GetWindow<EditorWin_WriteNameMapToJson>("NameMap记录为Json");
            // Debug.Log(Application.dataPath);
        }

        #region 拖拽、结果显示、结果处理
        private Vector2 WindowView_AddSearchRangeLabel()
        {
            //GUIStyle searchRangeShow = GUI.skin.customStyles[4];
            //展示曲线保存的重要参数：保存路径，json文件名
            EditorGUILayout.BeginVertical();
            GUILayout.Space(50);
            //if (GUILayout.Button("断点测试游戏内读取曲线"))
            //{
            //    //AnimationCurveChanger.Instance.ReadAnimationCurveDataJson();
            //    AnimationCurveChanger.Instance.Reinit();
            //}
            //动态拖拽需要保存曲线的动画文件
            Rect pathRect = EditorGUILayout.GetControlRect(GUILayout.Width(300), GUILayout.Height(30));
            EditorGUI.TextField(pathRect, "需要生成NameMap的预设体(拖拽)");
            EditorGUILayout.EndVertical();
            Object[] dragClips = EditorTool.GetDragObjects(pathRect);
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

            foreach (Object _prefab in dragClips)
            {
                prefabs.Add((GameObject)_prefab);
            }
            return pathRect.position;
        }

        /// <summary>
        /// 展示已经拖拽好的动画片段名单
        /// </summary>
        private void WindowView_AutoShowPrefabs()
        {
            GUILayout.BeginVertical();
            Transform removeClick = null;
            foreach (GameObject _prefab in prefabs)
            {
                GUILayout.BeginHorizontal("HelpBox");
                GUILayout.Label("预设体名称：" + _prefab.name, GUILayout.ExpandWidth(false));
                if (GUILayout.Button("除名", GUILayout.Width(100)))
                {
                    removeClick = _prefab.transform;
                    break;
                }
                //TODO:到这了，这里应该是直接找Json，看有么有这个文件，如果有的话：显示覆盖，如果没有：显示创建
                string prefabPath = AssetDatabase.GetAssetPath(_prefab);
                string jsonPath = prefabPath.Replace(".prefab",".json");
                Object jsonAsset = AssetDatabase.LoadAssetAtPath<Object>(jsonPath);
                if (jsonAsset == null)
                {
                    if (GUILayout.Button("创建", GUILayout.Width(100)))
                    {
                        
                    }
                }
                else {
                    if (GUILayout.Button("覆盖", GUILayout.Width(100)))
                    {
                        
                    }
                    if (GUILayout.Button("删除", GUILayout.Width(100)))
                    {

                    }
                }
                GUILayout.EndHorizontal();
            }
            if (removeClick != null)
            {
                prefabs.Remove(removeClick.gameObject);
            }
            GUILayout.EndVertical();
        }

        #endregion

        /// <summary>
        /// 展示一下本地的动画曲线数据json文件
        /// </summary>
        public void WindowView_ShowJsonCacheData(JsonData _showJson)
        {
            //展示一下曲线保存结果
            GUILayout.Label("曲线数据已经存在本地的动画片段：");
            GUILayout.BeginVertical("HelpBox");
            dragPoint = EditorGUILayout.BeginScrollView(dragPoint, false, true, GUILayout.Height(150));
            if (_showJson == null || _showJson.IsObject == false)
            {
                GUILayout.Label("本地还没有动画片段");
            }
            else
            {
                string deleteKey = string.Empty;
                foreach (KeyValuePair<string, JsonData> _curveJson in _showJson)
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Label(string.Format("动画片段：{0} ", _curveJson.Key), GUILayout.ExpandWidth(false));
                    if (GUILayout.Button("删除", GUILayout.Width(50)))
                    {
                        //从json文件中删除该条记录
                        deleteKey = _curveJson.Key;

                    }
                    GUILayout.EndHorizontal();
                }
                if (!string.IsNullOrEmpty(deleteKey))
                {
                    //((IDictionary)allCurveData_json).Remove(deleteKey);
                    //CurveJsonSaveToLocal(allCurveData_json);
                    ////拖动列表中也删除一下
                    //AnimationClip dragAnim = animClips.Find((_animClip) =>
                    //{
                    //    return _animClip.name == deleteKey;
                    //});
                    //if (null != dragAnim)
                    //{
                    //    animClips.Remove(dragAnim);
                    //}
                }
            }
            EditorGUILayout.EndScrollView();
            GUILayout.EndVertical();
        }
        Vector2 dragPoint = Vector2.zero;


        #region GUI生命周期
        private void OnEnable()
        {
            prefabs = new List<GameObject>();
        }
        void OnGUI()
        {
            WindowView_AddSearchRangeLabel();
            GUILayout.Space(30);
            if (prefabs.Count>0)
            {
                WindowView_AutoShowPrefabs();
            }
            GUILayout.Space(30);
            //WindowView_SaveNameMap();
            GUILayout.Space(30);
            //WindowView_ShowCurveCacheData(allCurveData_json);
        }
        #endregion
        private void OnDestroy()
        {
            prefabs.Clear();
            prefabs = null;
        }
    }
}

