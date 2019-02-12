using System.Collections;
using System.Collections.Generic;
using LuaFramework;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class VersionFile  {
    
    public string Path { get; set; }

    public string Hash { set; get; }

    public  string Version { get; set; }

    public long Size { get; set; }

    // data 本地路径
    public string DataLocalPath
    {
        get { return (Util.DataPath + Path).Replace("//", "/"); }
    }

    public VersionFile() { }

    public VersionFile(string data)
    {
        var dd = data.Split('|');
        if (dd.Length == 4)
        {
            Path = dd[0];
            Hash = dd[1];
            Version = dd[2];
            Size = long.Parse( dd[3]);
        }
        else
        {
            Path = dd[0];
            Hash = string.Empty;
            Version = "1";
            Size = 0;
        }
    }

    public override string ToString()
    {
        return string.Join("|", new[] {Path, Hash, Version, Size.ToString()});
    }
}
