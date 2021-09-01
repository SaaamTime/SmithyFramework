
using System.IO;
using LitJson;
using UnityEngine;
using DIY.Asset;
namespace DIY.Json
{
    public static class JsonUtil
    {
        public static JsonData ReadJson(string _path,string _name,bool _inLaunch=true) {
            string path = PathUtil.GetPath_Json(_path, _name, _inLaunch);
            return ReadJson(path,_inLaunch);
        }

        public static JsonData ReadJson(string _pathAndName, bool _inLaunch = true)
        {
            //TODO:这个加载之后需要改一下，需要区分编辑器、运行状态和平台状态
            TextAsset jsonTxt = AssetUtil.AutoLoad<TextAsset>(_pathAndName);
            JsonData jsonData = JsonMapper.ToObject(jsonTxt.text);
            return jsonData;
        }

        public static void WriteJson(string _path) { 
        
        }

        /// <summary>
        /// 获取本地存储的json文件（编辑器下专用）
        /// </summary>
        /// <returns></returns>
        public static JsonData GetOrCreateJson(string _path) {
            StreamReader curveText = AssetUtil.GetOrCreateTextReader(_path);
            JsonData curveJson = JsonMapper.ToObject(curveText.ReadToEnd());
            curveText.Close();
            curveText.Dispose();
            return curveJson;
        }

        /// <summary>
        /// 存取（覆盖）json文件
        /// </summary>
        public static void WriteOrCreateJson(string _path,JsonData _js)
        {
            StreamWriter curveText = AssetUtil.GetOrCreateTextWriter(_path);
            string jsonStr = JsonMapper.ToJson(_js);
            curveText.Write(jsonStr);
            curveText.Close();
            curveText.Dispose();
        }
    }
}
