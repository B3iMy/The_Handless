using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using static UnityEditor.Progress;

public class Web : MonoBehaviour
{
    void Start()
    {
        //StartCoroutine(GetDate());
        //StartCoroutine(GetUser());
        //StartCoroutine(Login("testuser2","123"));
        //StartCoroutine(Register("testuser3", "123"));
    }
    public void ShowUserItem()
    {
        //StartCoroutine(GetItemID(Main.Instance.userInfo.UserID));
    }
    public IEnumerator GetDate()
    {
        using (UnityWebRequest www = UnityWebRequest.Get("http://localhost:8080/UnityBackend/getDate.php"))
        {
            yield return www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                // Show results as text
                Debug.Log(www.downloadHandler.text);
                // or retrieve results as binary data
                byte[] results = www.downloadHandler.data;

            }
        }
    }
    public IEnumerator GetUser()
    {
        using (UnityWebRequest www = UnityWebRequest.Get("http://localhost:8080/UnityBackend/getUser.php"))
        {
            yield return www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                // Show results as text
                Debug.Log(www.downloadHandler.text);
                // or retrieve results as binary data
                byte[] results = www.downloadHandler.data;

            }
        }
    }
    public IEnumerator Login(string username, string password)
    {
        WWWForm form = new WWWForm();
        form.AddField("loginUser", username);
        form.AddField("loginPass", password);

        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost:8080/UnityBackend/login.php", form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(www.error);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
                Main.Instance.userInfo.SetCredential(username, password);
                Main.Instance.userInfo.SetID(www.downloadHandler.text);
                Debug.Log("userID: '" + www.downloadHandler.text + "'");
                if (www.downloadHandler.text.Contains("Wrong Credentials") || www.downloadHandler.text.Contains("No user found with the username: "))
                {
                    Debug.Log("Try Again");
                }
                else
                {
                    //If we logged in correctly
                    Main.Instance.userProfile.SetActive(true);
                    Main.Instance.login.gameObject.SetActive(false);
                }
            }
        }
    }
    public IEnumerator Register(string username, string password, string confirmPassword)
    {
        if (password != confirmPassword)
        {
            Debug.LogError("Password and Confirm Password do not match.");
            yield break; // Exit coroutine early
        }
        WWWForm form = new WWWForm();
        form.AddField("loginUser", username);
        form.AddField("loginPass", password);
        form.AddField("confirmPass", confirmPassword);
        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost:8080/UnityBackend/register.php", form))
        {
            yield return www.SendWebRequest();
            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(www.error);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
            }
        }
    }
    public IEnumerator GetItemID(string userId, System.Action<string> callback)
    {
        WWWForm form = new WWWForm();
        form.AddField("userID", userId);
        Debug.Log("GetItemID - userID: '" + userId + "'");
        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost:8080/UnityBackend/getItemID.php", form))
        {
            yield return www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                // Show results as text
                Debug.Log(www.downloadHandler.text);
                string JsonArray = www.downloadHandler.text;
                callback(JsonArray);
            }
        }
    }
    public IEnumerator GetItem(string itemId, System.Action<string> callback)
    {
        WWWForm form = new WWWForm();
        form.AddField("itemID", itemId);
        Debug.Log("GetItem - itemID: '" + itemId + "'");
        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost:8080/UnityBackend/getItem.php", form))
        {
            yield return www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                // Show results as text
                Debug.Log(www.downloadHandler.text);
                string JsonArray = www.downloadHandler.text;
                callback(JsonArray);
            }
        }
    }
    public IEnumerator SellItem(string itemId, string userId)
    {
        WWWForm form = new WWWForm();
        form.AddField("itemID", itemId);
        form.AddField("userID", userId);
        Debug.Log("GetItem - itemID: '" + itemId + "'");
        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost:8080/UnityBackend/sellItem.php", form))
        {
            yield return www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                // Show results as text
                Debug.Log(www.downloadHandler.text);
            }
        }
    }
    public IEnumerator GetItemIcon(string itemId, System.Action<byte[]> callback)
    {
        WWWForm form = new WWWForm();
        form.AddField("itemID", itemId);
        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost:8080/UnityBackend/getItemIcon.php", form))
        {
            yield return www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Downloading icon: " + itemId);
                //Results as byte array
                byte[] bytes = www.downloadHandler.data;
                callback(bytes);
            }
        }
    }
}