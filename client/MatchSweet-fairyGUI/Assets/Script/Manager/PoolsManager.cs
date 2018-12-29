using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Data;

public class PoolsManager  {

    public List<PoolBase> pools;
    public void Init()
    {
        pools.Add(new Pool<GameSweet>(10));
    }
}
