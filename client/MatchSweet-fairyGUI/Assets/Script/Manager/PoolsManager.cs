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

    public Dictionary<PoolType,Pool> poolDict;
    public void Init()
    {
        poolDict = new Dictionary<PoolType, Pool>();
        for(int i = 1;  i < (int)PoolType.Count;i++)
        {
            poolDict[(PoolType)i] = new Pool(10);
        }
        
    }

    public GameSweet GetSweetObj(PoolType type)
    {
        return poolDict[type].Create<GameSweet>();
    }

    public void HideSweetObj<T>(PoolType type, T t) where T : class,IResetable,new()
    {
        poolDict[type].Store(t);
    }
}
