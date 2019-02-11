using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class BuildABEditor : Editor {


    /// <summary>
    ///  创建文件夹
    /// </summary>
    /// <param name="packageName"></param>
    /// <param name="filterName"></param>
    /// <param name="packagePaths"></param>
    private static void CreateAbData(string packageName, string filterName, params string[] packagePaths)
    {
        string[] pps = new string[packagePaths.Length];
        for (int i = 0; i < packagePaths.Length; i++)
        {
           // pps[i] = Path.Combine();
        }
    }
}
