using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Security.Policy;
using LuaFramework;
using UnityEditor;

public class BuildABTools :EditorWindow
{

    public static EditorWindow window;
    public static BundleConfigData configData = new BundleConfigData();
    [MenuItem("Game/开发打包")]
    static void main()
    {
        window = EditorWindow.GetWindow(typeof(BuildABTools));
        window.titleContent.text = "开发打包";
        window.Show();
    }

    void OnGUI()
    {
        configData = new BundleConfigData();
        EditorGUILayout.BeginVertical();
        if (GUILayout.Button("打包"))
        {
           BuildAll();
        }
        EditorGUILayout.EndVertical();
    }

    public static void BuildAll()
    {
        BuildABEditor.ToolsBuildAB(configData.configList,EditorUserBuildSettings.activeBuildTarget);
        BuildDeveloper();
    }



    public static void BuildDeveloper()
    {
        var filesPath = Path.Combine(Application.streamingAssetsPath, "files.txt");
        var fileData = File.ReadAllText(filesPath);
        var vFiles = ReadFileInfo(fileData);

        var outPathFix = Util.DataPath;
        RGLog.Debug("outPath ->"+outPathFix);
        if (Directory.Exists(outPathFix))
        {
            Directory.Delete(outPathFix,true);
        }

        // copy
        for (int i = 0; i < vFiles.Length; i++)
        {
            var vfData = vFiles[i];
            var targetPath = Path.Combine(Application.streamingAssetsPath, vfData.Path).Replace("\\", "/");
            var outPath = Path.Combine(outPathFix, vfData.Path).Replace("\\", "/");

            var path = Path.GetDirectoryName(outPath);
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            RGLog.Debug("outPath 2 ->" + outPathFix);

            File.Copy(targetPath,outPath);
        }

        var outFilesPath = Path.Combine(outPathFix, "files.txt").Replace("\\", "/");
        File.Copy(filesPath,outFilesPath);

        RGLog.Debug("构建开发使用资源");
    }

    public static VersionFile[] ReadFileInfo(string data)
    {
        var list = new List<VersionFile>();
        using (var reader = new StringReader(data))
        {
            while (reader.Peek() !=-1)
            {
                var msg = reader.ReadLine();
                if(!string.IsNullOrEmpty(msg))
                    list.Add(new VersionFile(msg));
            }
        }

        return list.ToArray();
    }

    public static string GetVersionBundlePath()
    {
        var temp = Application.dataPath.Replace("\\", "/");
        var temp2 = temp.Substring(0, temp.Length - "\\Assets".Length);
        var outPathFix = Path.Combine(temp2, "build/bundle" + "0.0.1").Replace("\\", "/");
        return outPathFix;
    }

    public static void CleanStreamingAssets()
    {
        string assetPath = Replace(string.Format("{0}{1}{2}", Application.dataPath, Path.DirectorySeparatorChar,"StreamingAssets"));

        string filePath = Replace(string.Format("{0}{1}{2}", assetPath, Path.DirectorySeparatorChar, "files.txt"));

    }

    public static string Replace(string path)
    {
        return path.Replace('\\', Path.DirectorySeparatorChar).Replace('/', Path.DirectorySeparatorChar);
    }
}

public enum Group
{
    UI,
    Max,
}

public class BundleConfig
{
    /// <summary>
    /// bundle 名称
    /// </summary>
    [SerializeField]
    public string BundleName;

    /// <summary>
    /// 资源路径
    /// </summary>
    [SerializeField]
    public string ResPath;

    /// <summary>
    /// 过滤
    /// </summary>
    [SerializeField]
    public string Filter;

    /// <summary>
    /// 是否为单一文件进行打包
    /// </summary>
    [SerializeField]
    public bool ASeparateFile;

    /// <summary>
    /// 分组名称
    /// </summary>
    [SerializeField]
    public Group EGroup;

    /// <summary>
    /// 不打入正式包
    /// </summary>
    [SerializeField]
    public bool NotInPackage;
}

public class BundleConfigData
{
    public List<BundleConfig> configList = new List<BundleConfig>();
    public Dictionary<int,List<BundleConfig>> configGroupDic = new Dictionary<int, List<BundleConfig>>();

    public BundleConfigData()
    {
        string uiPath = Path.Combine(RGResource.ROOT_PATH, "UI");
        string[] uiPaths = Directory.GetDirectories(uiPath);

        for (int i = 0; i < uiPaths.Length; i++)
        {
            string abPath = uiPaths[i].Replace(RGResource.ROOT_PATH, "").Replace(Path.DirectorySeparatorChar, '/');
            string abName = abPath + ".ab";
            Add(CreateConfig(abName,"",abPath,false,Group.UI));
        }

        //设置分组
        SetGroup();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="bn"> bundle name </param>
    /// <param name="filter"> 过滤模式 </param>
    /// <param name="rp"> 资源路径 </param>
    /// <param name="asf"> 是否单个文件打包 </param>
    /// <param name="g"> 分组类型 </param>
    /// <param name="notInPackage"> 不打入正式包里 </param>
    /// <returns></returns>
    private BundleConfig CreateConfig(string bn,string filter,string rp,bool asf,Group g,bool notInPackage = false)
    {
        BundleConfig config = new BundleConfig();
        config.BundleName = bn.Trim();
        config.ResPath = rp.Trim();
        return config;
    }
    private void Add(BundleConfig config)
    {
        configList.Add(config);
    }

    private void SetGroup()
    {
        int groudCount = (int) Group.Max;

        for (int i = 0; i < groudCount; i++)
        {
            var group = (Group) i;
            List<BundleConfig> c1 = new List<BundleConfig>();
            for (int j = 0; j < configList.Count; j++)
            {
                if (group == configList[j].EGroup)
                {
                    c1.Add(configList[j]);
                }
                configGroupDic.Add(i,c1);
            }

        }
    }


}