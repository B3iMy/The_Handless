using UnityEngine;
using UnityEngine.Networking;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using System.Collections;
using System;
using System.Text;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;

// Define a Unity MonoBehaviour class to handle HTTP requests
public class HttpClient : MonoBehaviour
{
    // Class to represent user data
    private SaveLoadManager saveLoadManager= new SaveLoadManager();
    private class DataRegister
    {
        public string username { get; set; }
        public string password { get; set; }
        public string email { get; set; }
    };
    private class DataRequestTo
    {
        public string username { get; set; }
        public string email { get; set; }
    };
    private class DataResetPassword
    {
        public string username { get; set; }
        public string resetToken { get; set; }
        public string newPassword { get; set; }
    };
    private class UserData
    {
        public string user_id { get; set; }
        public string username { get; set; }
        public string email { get; set; }
        public string password { get; set; }
    };
    // URL of the server
    private string serverUrl = "https://127.0.0.2:8000";
    

    // Called when the script instance is being loaded
    void Start()
    {
        StartServer();
    }

    // Start the server connection coroutine
    public void StartServer()
    {
        
        StartCoroutine(ConnectToServerCoroutine());
    }

    // Coroutine to handle server connection asynchronously
    private IEnumerator ConnectToServerCoroutine()
    {
        var getRequestTask = GetRequestAsync(serverUrl);
        yield return new WaitUntil(() => getRequestTask.IsCompleted);

        if (getRequestTask.IsFaulted || getRequestTask.IsCanceled)
        {
            Debug.LogError("GET request failed: " + getRequestTask.Exception?.Message);
        }
        else
        {
            Debug.Log("GET request completed: " + getRequestTask.Result);
        }
    }

    // Asynchronous method to perform a GET request
    private async Task<string> GetRequestAsync(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            webRequest.certificateHandler = new AcceptAllCertificates();
            var operation = webRequest.SendWebRequest();

            // Wait for the request to complete
            while (!operation.isDone)
                await Task.Yield();

            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error: " + webRequest.error + " URL: " + uri);
                return null;
            }

            Debug.Log("Received from " + uri + ": " + webRequest.downloadHandler.text);
            return webRequest.downloadHandler.text;
        }
    }

    // Asynchronous method to perform a POST request
    private async Task<string> PostRequestAsync(string uri, string jsonData)
    {



        using (UnityWebRequest webRequest = new UnityWebRequest(uri, "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
            webRequest.certificateHandler = new AcceptAllCertificates();
            webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            webRequest.SetRequestHeader("Content-Type", "application/json");

            var operation = webRequest.SendWebRequest();

            // Wait for the request to complete
            while (!operation.isDone)
            {
                await Task.Yield();
            }

            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error: " + webRequest.error + " URL: " + uri);
                return null;
            }

            Debug.Log("Received from " + uri + ": " + webRequest.downloadHandler.text);
            return webRequest.downloadHandler.text;
        }
    }

    // Method to register user data asynchronously
    public async Task<string> RegisterUserDataAsync(string username, string userpassword, string useremail)
    {
        DataRegister DataRegister = new DataRegister { username = username, password = userpassword, email = useremail };
        string data = JsonConvert.SerializeObject(DataRegister);
       
        return await PostRequestAsync(serverUrl + "/users/addUser", data);
        
    }

    // Method to handle user login asynchronously
    public async Task<string> OnLoginAsync(string name, string password)
    {
        string dataRequest = $"?username={UnityWebRequest.EscapeURL(name)}&password={UnityWebRequest.EscapeURL(password)}";

        string data = await GetRequestAsync(serverUrl + "/users/getUser" + dataRequest);
        UserData userData = new UserData();
        userData = JsonUtility.FromJson<UserData>(data);
        string user_id = userData.user_id;
        string user_name = userData.username;
        string user_password = userData.password;
        string user_email = userData.email;
        Debug.Log(user_id + user_name + user_password + user_email);
        saveLoadManager.AddDataUser(user_id, user_name, user_password, user_email);

        return data;
    }

    // Method to handle forgot password functionality asynchronously
    public async Task<string> ForgotPasswordAsync(string name, string email)
    {
        DataRequestTo dataRequest = new DataRequestTo {username = name, email = email};
        string data = JsonConvert.SerializeObject(dataRequest);
        saveLoadManager.AddDataUser("", name, "", "");
        try
        {
            return await PostRequestAsync(serverUrl + "/users/forgotPassword", data);
        }
        catch (Exception ex)
        {
            // Handle exceptions (e.g., logging)
            throw new InvalidOperationException("Error occurred during forgot password request.", ex);
        }
    }

    // Method to handle password reset functionality asynchronously
    public async Task<string> ResetPasswordAsync(string resetToken, string newPassword)
    {
        string dataUser = saveLoadManager.LoadUserFromFile();
        DataResetPassword dataReset = new DataResetPassword { username = dataUser, resetToken = resetToken, newPassword = newPassword };
        return await PostRequestAsync(serverUrl + "/users/resetPassword", JsonConvert.SerializeObject(dataReset));
    }

    // Custom certificate handler to accept all certificates
    public class AcceptAllCertificates : CertificateHandler
    {
        protected override bool ValidateCertificate(byte[] certificateData)
        {
            return true;  // Accept all certificates
        }
    }
}
