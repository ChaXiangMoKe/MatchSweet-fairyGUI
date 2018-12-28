

public class RGLog {

    public static void Debug(object format, params object[] args)
    {
        UnityEngine.Debug.LogFormat(string.Format("<color=red>[Debug]</color>") + format.ToString(),args);
    }

    public static void DebugError(object format, params object[] args)
    {
        UnityEngine.Debug.LogErrorFormat(string.Format("<color=red>[ERROR]</color>") + format.ToString(), args);
    }

    public static void Log(object format,params object[] args)
    {
        UnityEngine.Debug.LogFormat("<color=green>[LOG]</color>" + format.ToString(), args);
    }

    public static void Warn(object format,params object[] args)
    {
        UnityEngine.Debug.LogWarningFormat("<color=yellow>[WARN]</color>" + format.ToString(), args);
    }

    public static void Error(object format,params object[] args)
    {
        UnityEngine.Debug.LogErrorFormat("<color=red>[ERROR]</color>" + format.ToString(), args);
    }
}
