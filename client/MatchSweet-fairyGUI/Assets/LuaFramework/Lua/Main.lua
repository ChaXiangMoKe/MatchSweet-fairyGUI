--主入口函数。从这里开始lua逻辑
function Main()
	print("logic start")
    -- local go = UnityEngine.GameObject('go');
    -- go.transform.position = Vector3.one;

    local obj = LuaFramework.Util.LoadPrefab("Tank/Cube");
    print(" obj "..tostring(obj));
    local go = UnityEngine.GameObject.Instantiate(obj);
    LuaFramework.Util.Log("Finish");
end

--场景切换通知
function OnLevelWasLoaded(level)
	collectgarbage("collect")
	Time.timeSinceLevelLoad = 0
end

function OnApplicationQuit()

end

function OnLoadFinish(objs)

end