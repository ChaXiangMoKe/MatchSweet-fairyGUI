using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Data;

public enum PoolType
{
    GameSweet,
    Count,
}
public class PoolsManager
{

    public Dictionary<PoolType, Pool> poolDict;

    private PoolsManager() { }
    private static PoolsManager instance;
    public static PoolsManager Instance
    {
        get
        {
            if(instance== null)
            {
                instance = new PoolsManager();
                instance.Init();
            }
            return instance;
        }
    }

    public void Init()
    {
        poolDict = new Dictionary<PoolType, Pool>();
        poolDict[PoolType.GameSweet] = new ResetPool<GameSweet>(10);
    }

    public object GetObj(PoolType type)
    {
        switch (type)
        {
            case PoolType.GameSweet:
                return GetSweetObj();
            default:
                return null;
        }

    }

    public GameSweet GetSweetObj()
    {
        return poolDict[PoolType.GameSweet].Create<GameSweet>();
    }

    public void HideObj(PoolType type, object obj)
    {
        poolDict[type].Store(obj);
    }

    public void GetPoolNum(PoolType type)
    {
        RGLog.Log( "type :"+type + " Num :" + poolDict[type].GetPoolNum());
    }
}
