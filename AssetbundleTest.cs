using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;
public class AssetbundleTest : MonoBehaviour
{
    string assetName= "gameobject";
    string bundleName= "gameobject";
    // Start is called before the first frame update
    void Start()
    {
        LoadDependencies();
        InstantiateBundle();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //需要先加载依赖关系
    void LoadDependencies()
    {
        //AssetBundleManifest记录所有资源依赖
        AssetBundle assetBundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, "StandaloneWindows"));
        AssetBundleManifest _manifest = assetBundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
        string[] dependencies = _manifest.GetAllDependencies(bundleName);
        //先加载依赖包
        foreach (string dependency in dependencies)
        {
            var dependencyAsync = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, dependency));
        }
    }


    protected void InstantiateBundle()
    {
        //按照路径加载ab包
        AssetBundle asset = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, bundleName));
        //从ab包中直接读取资源
        var asset_gob = asset.LoadAsset<GameObject>(assetName);
        GameObject panel = Instantiate(asset_gob);//实例化资源
        panel.transform.SetParent(this.transform);
        panel.transform.localEulerAngles = Vector3.zero;
        panel.transform.localScale = Vector3.one*0.5f;
        panel.transform.localPosition = Vector3.zero;
        
    }
}
