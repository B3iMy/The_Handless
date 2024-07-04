using UnityEngine;

public class SaveLoadManager : MonoBehaviour
{
    DataUser dataUser = new DataUser();

    public void AddDataUser(string user_id, string user_name, string user_password, string user_email)
    {
        // Set data
        dataUser.SetUserID(user_id);
        dataUser.SetUsername(user_name);
        dataUser.SetPassword(user_password);
        dataUser.SetEmail(user_email);

        // Serialize to JSON
        string json = JsonUtility.ToJson(dataUser);
        Debug.Log("Serialized JSON: " + json);

        // Save JSON to file
        SaveToFile(json);
    }

    void SaveToFile(string json)
    {
        string path = Application.persistentDataPath + "/dataUser.json";
        System.IO.File.WriteAllText(path, json);
        Debug.Log("Data saved to: " + path);
    }

    private void LoadFromFile()
    {
        string path = Application.persistentDataPath + "/dataUser.json";
        if (System.IO.File.Exists(path))
        {
            string json = System.IO.File.ReadAllText(path);
            dataUser = JsonUtility.FromJson<DataUser>(json);
            Debug.Log("Data loaded from: " + path);
        }
        else
        {
            Debug.Log("Save file not found at: " + path);
        }
    }

    public string LoadUserFromFile()
    {
        string path = Application.persistentDataPath + "/dataUser.json";
        if (System.IO.File.Exists(path))
        {
            string json = System.IO.File.ReadAllText(path);
            dataUser = JsonUtility.FromJson<DataUser>(json);
            return dataUser.GetUsername();
        }
        else
        {
            Debug.Log("Save file not found at: " + path);
            return "Save file not found.";
        }
    }
}
