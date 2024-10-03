using System.Collections.Generic;

public class BlackBoard
{
    public Dictionary<string, object> blackboard = new();

    public void AddToBlackboard(string key, object value) => blackboard[key] = value;
    public bool TryGetFromBlackboard<T>(string key, out T obj)
    {
        if(!blackboard.TryGetValue(key, out var x))
        {
            obj = default;
            return false;
        }
        obj = (T)x;
        return true;
    }
    public bool ClearData(string key) => blackboard.Remove(key);
}