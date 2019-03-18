using System.Collections;
using System.Collections.Generic;
using System.IO;
using LuaFramework;
using UnityEngine;

public class PackageManager {

    // 缓存资源包字典
    private static Dictionary<string,RGPackage> _packageCacheDic = new Dictionary<string, RGPackage>();

    public static RGPackage CreatePackage(string packageName)
    {
        RGLog.Debug(" CreatePackage --> " + packageName);
        var package = GetPackage(packageName);
        if (package == null)
        {
            package = RGPackage.Create(packageName);
            _packageCacheDic.Add(packageName,package);
        }
        return package;
    }

    // 获得包
    public static RGPackage GetPackage(string packageName)
    {
        RGPackage package = null;
        if (_packageCacheDic.TryGetValue(packageName, out package))
        {
            return package;
        }

        return null;
    }

    /// <summary>
    /// 获得AssetBundle资源包路径
    /// </summary>
    /// <param name="packageName"></param>
    /// <returns></returns>
    public static string GetPackagePath(string packageName)
    {
        string p = string.Format("{0}.{1}",packageName,RGResource.PACKAGE_SUFFIX).ToLower();
        return Path.Combine(Util.DataPath, p);
    }

    // 获得AssetBundle资源包名
    public static string GetPackageName(string path)
    {
        string[] model = path.ToLower().Split(RGResource.PATH_SEPARATOR);

        // 包路径
        string packageUrl = "";
        for (int i = 0; i < model.Length; i++)
        {
            RGLog.Debug(model[i]);
        }
        if (model.Length > 0)
        {
            if (model[0].Equals("ui"))
            {
                // ui
                packageUrl = "ui/" + model[1];
            }
            
            return packageUrl.ToLower();
        }
        RGLog.DebugError(" GetPackagePath Error! Path is Empty");
        return string.Empty;
    }

    public static string GetAssetName(string resUrl, string suffix)
    {
        string assetName = Path.Combine(RGResource.ROOT_PATH, string.Format("{0}.{1}", resUrl, suffix));
        return assetName.ToLower();
    }
}
