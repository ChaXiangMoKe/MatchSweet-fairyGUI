using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using UnityEngine;
using Object = UnityEngine.Object;

public class RGPackage
{

    private string _packageName;

    // 已经加载到内存
    public bool IsLoadPackage
    {
        get { return _bundle != null; }
    }

    public string PackagerName
    {
        get { return _packageName; }
    }

    // 资源包点相对路径
    private string _packagePath;

    public string PackagePath
    {
        get
        {
            if (string.IsNullOrEmpty(_packagePath))
            {
                _packagePath = PackageManager.GetPackagePath(_packageName);

            }

            return _packagePath;
        }
    }

    public bool IsUI
    {
        get { return _packageName.StartsWith("ui"); }
    }
    // 正在加载
    private bool IsLoading = false;

    // 对应bundle资源
    private AssetBundle _bundle;

    // 加载完成回调函数
    private Dictionary<string,List<Action<UnityEngine.Object,LoadEventData>>> _loadCompleteDict = new Dictionary<string, List<Action<Object, LoadEventData>>>();
    private Dictionary<string,List<LoadEventData>> _loadEvDataDic = new Dictionary<string, List<LoadEventData>>();

    // 正在加载点时候缓存列表
    private List<string> _cacheAssetNameList = new List<string>();
    private List<Action<UnityEngine.Object,LoadEventData>> _cacheCompleteList = new List<Action<Object, LoadEventData>>();
    private List<LoadEventData> _cacheEvDataList = new List<LoadEventData>();

    // bundle里点资源缓存
    private List<RGRes> _cacheRes = new List<RGRes>(); 

    public RGPackage(string packageName)
    {
        IsLoading = false;
        _packageName = packageName;
        _loadCompleteDict.Clear();
        _loadEvDataDic.Clear();
        _cacheRes.Clear();

        _cacheAssetNameList.Clear();
        _cacheCompleteList.Clear();
        _cacheEvDataList.Clear();
    }

    public static RGPackage Create(string packageName)
    {
        var rg = new RGPackage(packageName);
        return rg;
    }

    public UnityEngine.Object GetAsset(string assetName)
    {
        var res = FindRes(assetName);
        if (res != null)
        {
            return res.GetRes();
        }

        return null;
    }

    private RGRes AddRes(string assetName, UnityEngine.Object obj)
    {
        var res = new RGRes(assetName,obj);
        _cacheRes.Add(res);

        return res;
    }

    private RGRes FindRes(string resName)
    {
        RGRes res = null;
        for (int i = 0; i < _cacheRes.Count; i++)
        {
            res = _cacheRes[i];
            if (res != null && res.ResName == resName)
            {
                return res;
            }
        }

        return null;
    }

    public AssetBundle GetBundle()
    {
        return _bundle;
    }
    #region 异步

    public void LoadBundleAsync(string assetName, Action<UnityEngine.Object, LoadEventData> loadComplete,
        LoadEventData evData)
    {
        if (!File.Exists(PackagePath))
        {
            if (loadComplete != null)
            {
                loadComplete(null, evData);
            }

            return;
        }
        
        if (IsLoading)
        {
            AddCache(assetName, loadComplete, evData);

        }
        else
        {
            AddCallback(assetName,loadComplete,evData);

            CoroutineManager.Instance.StartCoroutine(IELoadBundleAsync(assetName, loadComplete, evData));
        }
    }

    public void LoadAssetAsync(string assetName, Action<UnityEngine.Object, LoadEventData> loadComplete,
        LoadEventData evData)
    {
        RGRes res = FindRes(assetName);
        if (res != null)
        {
            var asset = GetAsset(assetName);
            loadComplete(asset, evData);
            return;
        }
        AddCallback(assetName,loadComplete,evData);

        CoroutineManager.Instance.StartCoroutine(IELoadAssetAsync(assetName, loadComplete, evData));
    }

    private IEnumerator IELoadBundleAsync(string assetName, Action<UnityEngine.Object, LoadEventData> loadComplete,
        LoadEventData evData)
    {
        
        if (!IsLoadPackage)
        {
            var bRequest = AssetBundle.LoadFromFileAsync(PackagePath);

            IsLoading = true;

            yield return bRequest;
            var abRequest = bRequest.assetBundle;
            
            if (abRequest == null)
            {
                yield break;
            }

            if (!bRequest.isDone)
            {
                yield break;
            }

            _bundle = abRequest;

            IsLoading = false;
        }

        if (IsUI)
        {
            if (loadComplete != null)
            {
                Debug.Log(" loadComplete ");
                loadComplete(_bundle, evData);
            }
        }
        else
        {
            CoroutineManager.Instance.StartCoroutine(IELoadAssetAsync(assetName, loadComplete, evData));
            yield return 0;
        }
        CoroutineManager.Instance.StartCoroutine(LoadCache());

        yield return null;
    }

    private IEnumerator IELoadAssetAsync(string assetName, Action<UnityEngine.Object, LoadEventData> loadComplete,
        LoadEventData evData)
    {
        var ab = _bundle.LoadAssetAsync<UnityEngine.Object>(assetName);
        yield return ab;

        // 添加资源缓存
        AddRes(assetName, ab.asset);

        if (!_loadCompleteDict.ContainsKey(assetName))
        {
            yield break;
        }

        var callbackList = _loadCompleteDict[assetName];
        var callbackDataList = _loadEvDataDic[assetName];

        for (int i = 0; i < callbackList.Count; i++)
        {
            LoadCallback(callbackList[i], GetAsset(assetName), callbackDataList[i]);

            callbackList[i] = null;
        }

        callbackList = _loadCompleteDict[assetName];

        var needDelete = callbackList.TrueForAll(c => c == null);
        if (needDelete)
        {
            _loadCompleteDict.Remove(assetName);
            _loadEvDataDic.Remove(assetName);
        }

    }
    #endregion

    #region 正在加载bundle 相关处理

    private void AddCache(string assetName, Action<UnityEngine.Object, LoadEventData> loadComplete,
        LoadEventData evData)
    {
        _cacheAssetNameList.Add(assetName);
        _cacheCompleteList.Add(loadComplete);
        _cacheEvDataList.Add(evData);
    }

    // bundle 加载完成之后 需要检查缓存是否有资源需要加载
    IEnumerator LoadCache()
    {
        string assetName = "";
        Action<UnityEngine.Object, LoadEventData> loadComplete = null;
        LoadEventData evData = null;

        for (int i = 0; i < _cacheAssetNameList.Count; i++)
        {
            assetName = _cacheAssetNameList[i];
            loadComplete = _cacheCompleteList[i];
            evData = _cacheEvDataList[i];

            if (IsUI)
            {
                if (loadComplete != null)
                {
                    loadComplete(_bundle, evData);
                }
            }
            else
            {
                LoadAssetAsync(assetName,loadComplete,evData);
            }

            yield return 0;
        }

        _cacheAssetNameList.Clear();
        _cacheCompleteList.Clear();
        _cacheEvDataList.Clear();
    }
    #endregion

    private void AddCallback(string assetName, Action<UnityEngine.Object, LoadEventData> loadComplete,
        LoadEventData evData)
    {
        if (!_loadCompleteDict.ContainsKey(assetName))
        {
            var list = new List<Action<UnityEngine.Object,LoadEventData>>();
            _loadCompleteDict.Add(assetName,list);

            var list2 = new List<LoadEventData>();
            _loadEvDataDic.Add(assetName,list2);
        }
        _loadCompleteDict[assetName].Add(loadComplete);
        _loadEvDataDic[assetName].Add(evData);
    }


    private void LoadCallback(Action<UnityEngine.Object, LoadEventData> callback, UnityEngine.Object obj,
        LoadEventData evData)
    {
        if (callback != null)
        {
            callback(obj, evData);
        }
    }
}
