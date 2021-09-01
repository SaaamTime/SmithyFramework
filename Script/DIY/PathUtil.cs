using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.IO;

namespace DIY
{
    public static class PathUtil
    {
        public static string GetDataPath(string _localPath,string _name,bool _inLaunch = false)
        {
            if (_inLaunch)
            {
                return Path.Combine("Assets", _localPath, _name);
            }
            return Path.Combine(Application.dataPath, _localPath, _name);
            //return Application.dataPath + "/" + _localPath + "/" + _name;
        }

        public static string GetPath_Json(string _localPath, string _name,bool _inLaunch = true) {
            return GetDataPath(_localPath, _name + ".json", _inLaunch);
        }
    }
}
