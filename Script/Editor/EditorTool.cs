using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

namespace DIY.Editor
{

    public class EditorTool
    {

        public static Object[] GetDragObjects(Rect _dragBound)
        {
            //如果鼠标正在拖拽中或拖拽结束时，并且鼠标所在位置在拖拽的有效范围内
            if (_dragBound.Contains(Event.current.mousePosition))
            {
                if (Event.current.type == EventType.DragUpdated)
                {
                    //改变鼠标的外表
                    DragAndDrop.visualMode = DragAndDropVisualMode.Generic;
                }
                else if (Event.current.type == EventType.DragExited)
                {
                    Event.current.Use();
                    return DragAndDrop.objectReferences;
                }
            }
            return null;
        }

        public static Object GetDragObject(Rect _dragBound)
        {
            Object[] dragObjectArray = DragAndDrop.objectReferences;
            //只能拖拽一个
            if (dragObjectArray == null || dragObjectArray.Length > 1)
            {
                return null;
            }
            //如果鼠标正在拖拽中或拖拽结束时，并且鼠标所在位置在拖拽的有效范围内
            if (_dragBound.Contains(Event.current.mousePosition))
            {
                if (Event.current.type == EventType.DragUpdated)
                {
                    //改变鼠标的外表
                    DragAndDrop.visualMode = DragAndDropVisualMode.Generic;
                }
                else if (Event.current.type == EventType.DragExited)
                {
                    Event.current.Use();
                    return dragObjectArray[0];
                }
            }
            return null;
        }

        public void DoEvent_DragObjEnd(Object _dragObj, System.Action<Object> _dragObjEndEvent)
        {
            Event currentEvent = Event.current;
            EventType type = currentEvent.type;
            if (type == EventType.MouseDown)
            {

            }
            else if (type == EventType.MouseDrag)
            {

            }
            else if (type == EventType.MouseUp)
            {

            }
            else if (type == EventType.DragUpdated)
            {

            }
            else if (type == EventType.DragPerform)
            {

            }
            else if (type == EventType.DragExited || type == EventType.Ignore)
            {

            }
        }

        public static List<string> GetAllAssetPathInTheFolder(string _folderPath)
        {
            List<string> pathList = new List<string>();
            System.Action<string> _AddPathInResultList = (_filePath) =>
            {
                if (!pathList.Contains(_filePath))
                {
                    pathList.Add(_filePath);
                }
            };
            bool isFolder = AssetDatabase.IsValidFolder(_folderPath);
            if (isFolder)//文件夹,文件夹内所有文件全部加入list
            {
                //文件夹下子文件直接添加
                string[] filePathList = Directory.GetFiles(_folderPath);
                if (null != filePathList)
                {
                    foreach (string filePath in filePathList)
                    {
                        _AddPathInResultList(filePath);
                    }
                }
                //文件夹下子目录，进入递归
                string[] subFolderList = Directory.GetDirectories(_folderPath);
                if (null != subFolderList)
                {
                    for (int i = 0; i < subFolderList.Length; i++)
                    {
                        string folderPath = subFolderList[i];
                        List<string> subFilePathList = GetAllAssetPathInTheFolder(folderPath);
                        for (int j = 0; j < subFilePathList.Count; j++)
                        {
                            string subPath = subFilePathList[j];
                            _AddPathInResultList(subPath);
                            EditorUtility.DisplayProgressBar("路径遍历中", subPath, (float)j / (float)subFilePathList.Count);
                        }
                        EditorUtility.DisplayProgressBar("路径遍历中", folderPath, (float)i / (float)subFolderList.Length);
                    }

                }
            }
            else//单独文件,文件路径直接加入list
            {
                _AddPathInResultList(_folderPath);
            }
            EditorUtility.ClearProgressBar();
            return pathList;
        }

    }

}
