using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEditor;//TODO:这个地方有问题，这个核心脚本可能不会编译到资源包中
namespace DIY.Asset
{
    public static class AssetUtil
    {
        public static T AutoLoad<T>(string _path) where T:Object {
#if UNITY_EDITOR
            //AssetBundle ab = AssetBundle.LoadFromFile(_path);
            //return ab.
            //FileStream fs = File.OpenRead(_path);
            //fs.Read()
            return (T)AssetDatabase.LoadAssetAtPath<Object>(_path);
#else
            //Debug.Log("Lua的Android路径（更改前）：" + path);
            //path = path.Replace(Application.streamingAssetsPath, Application.persistentDataPath);
            //Debug.Log("Lua的Android路径（更改后）：" + path);
#endif
        }

        public static StreamWriter GetOrCreateTextWriter(string _path, bool _append = false)
        {
            return new StreamWriter(_path, _append, Encoding.UTF8);
        }

        public static StreamReader GetOrCreateTextReader(string _path)
        {
            if (!File.Exists(_path))
            {
                GetOrCreateTextWriter(_path).Dispose();
            }
            return File.OpenText(_path);
        }

        public static bool SafeDelete(string _pathWithName) {
            if (!File.Exists(_pathWithName))
            {
                return false;
            }
            File.Delete(_pathWithName);
            return true;
        }
    }
}
