using UnityEngine;

public static class PlayerPrefsUtils
{
    /// <summary>
    /// 지정된 오브젝트의 정보를 저장합니다.
    /// </summary>
    public static void SetObject<T>(string key, T obj)
    {
        var json = JsonUtility.ToJson(obj);
        PlayerPrefs.SetString(key, json);
    }

    /// <summary>
    /// 지정된 오브젝트의 정보를 불러옵니다.
    /// </summary>
    public static T GetObject<T>(string key)
    {
        var json = PlayerPrefs.GetString(key);

        var obj = JsonUtility.FromJson<T>(json);
        return obj;
    }
}
