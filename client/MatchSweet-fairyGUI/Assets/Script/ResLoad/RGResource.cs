using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RGResource
{

    public const string ROOT_PATH = "Assets/Build/";

    public const string PRESTRAIN_FLAG = "PrestrainFlag";

    public static char[] PATH_SEPARATOR = new char[] {'/'};

    public const string PACKAGE_SUFFIX = "ab";

    public static void LoadUIAsync(string path, Action<AssetBundle, LoadEventData> loadComplete, params object[] data)
    {
        if (string.IsNullOrEmpty(path))
        {
            RGLog.Error("资源为空 ---------->" + path);
            if (loadComplete != null)
            {
                loadComplete.Invoke(null, ParseFrom(data));
            }

            return;
        }
        LoadAsync<AssetBundle>(path,"",loadComplete,ParseFrom(data));
    }


    public static void LoadAsync<T>(string resUrl, string suffix, Action<T, LoadEventData> loadComplete = null, LoadEventData evData = null) where T : UnityEngine.Object
    {
        string packageName = PackageManager.GetPackageName(resUrl);
        string assetName = PackageManager.GetAssetName(resUrl,suffix);

        LoadAssetAsync(packageName,assetName, (obj, ed) =>
        {
            if (loadComplete != null)
            {
                loadComplete.Invoke(obj as T,ed);
            }
        },evData);
    }

    private static void LoadAssetAsync(string packageName, string assetName,
        Action<UnityEngine.Object, LoadEventData> loadComplete, LoadEventData evData)
    {
        var package = PackageManager.CreatePackage(packageName);
        if (evData != null)
        {
            if (evData.data.Length > 0)
            {
                if (evData.data[0].ToString().Equals(RGResource.PRESTRAIN_FLAG))
                {
                    // 暂时无用 
                }
            }
        }

        // 获取资源，如果已经缓存，就直接返回
        var asset = package.GetAsset(assetName);
        if (asset !=null)
        {
            if (loadComplete !=null)
            {
                loadComplete(asset, evData);
            }
        }

        // bundle没有加载到内存，需要先加载bundle
        if (!package.IsLoadPackage)
        {
            package.LoadBundleAsync(assetName,loadComplete,evData);
        }
        else
        {
           //bundle 已经加载了，资源还没有加载
           if (package.IsUI)
           {
               if (loadComplete != null)
               {
                   loadComplete(package.GetBundle(), evData);
                   return;
               }
           }
           package.LoadAssetAsync(assetName,loadComplete,evData);
        }
    }

    public static LoadEventData ParseFrom(object[] data)
    {
        // 创建事件数据
        var eventData = new LoadEventData();
        eventData.data = data;
        return eventData;
    }
}

public class LoadEventData
{
    public object[] data;

    public T Get<T>(int i)
    {
        return (T)data[i];
    }

    public object Get(int i)
    {
        return data[i];
    }
}