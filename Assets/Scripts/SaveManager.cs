using UnityEngine;
using System.IO;

public class SaveManager : MonoBehaviour
{
    // 存檔
    public static void Save<T>(T data, string fileName)
    {
        string path = Application.persistentDataPath + "/" + fileName;
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(path, json);
        Debug.Log($"存檔成功: {path}");
    }

    // 讀檔
    public static T Load<T>(string fileName) where T : new()
    {
        string path = Application.persistentDataPath + "/" + fileName;

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            T data = JsonUtility.FromJson<T>(json);
            Debug.Log($"讀檔成功: {path}"); Debug.Log(data);
            return data;
        }
        else
        {
            //Debug.Log($"找不到存檔: {path}，回傳預設值");
            //return new T();  // 回傳空的資料結構
            return default(T);  // 回傳預設值（null 或空結構）
        }
    }

    // 刪除存檔
    public static void Delete(string fileName)
    {
        string path = Application.persistentDataPath + "/" + fileName;
        if (File.Exists(path))
        {
            File.Delete(path);
            Debug.Log($"刪除存檔: {path}");
        }
    }

    // 檢查存檔是否存在
    public static bool Exists(string fileName)
    {
        string path = Application.persistentDataPath + "/" + fileName;
        return File.Exists(path);
    }
}