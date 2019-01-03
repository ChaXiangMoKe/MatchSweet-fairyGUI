using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Data;

public enum PoolType
{
    GameSweet,
    Count,
}
public class PoolsManager  {

    public static Dictionary<PoolType,Pool> poolDict;
    public void Init()
    {
        poolDict = new Dictionary<PoolType, Pool>();
        for(int i = 1;  i < (int)PoolType.Count;i++)
        {
            poolDict[(PoolType)i] = new ResetPool(10);
        }
        
    }

    public static object GetObj(PoolType type)  
    {
        switch (type)
        {
            case PoolType.GameSweet:
                return GetSweetObj();
            default:
                return null;
        }  

    }

    public static GameSweet GetSweetObj()
    {
        return poolDict[PoolType.GameSweet].Create<GameSweet>();
    }

    public static void HideObj(PoolType type,object obj) 
    {
        poolDict[type].Store(obj);
    }
}
