using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RGRes  {
    
    public string ResName { get; private set; }
    
    public int RefCount { get; private set; }

    private Object _resObj;

    public RGRes(string resName, Object obj)
    {
        ResName = resName;
        _resObj = obj;
    }

    public int IncRef()
    {
        RefCount++;
        return RefCount;
    }

    public int DecRef()
    {
        RefCount--;
        return RefCount;
    }

    public Object GetRes()
    {
        return _resObj;
    }

    public void Unload()
    {
        _resObj = null;
        RefCount = 0;
    }
}
