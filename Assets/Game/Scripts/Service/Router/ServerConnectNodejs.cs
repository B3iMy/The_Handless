using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

public class HttpClient : MonoBehaviour
{
    private string serverUrl = "https://127.0.0.2:8000";

    void Start()
    {
        StartServer();
    }

    public void StartServer()
    {
        StartCoroutine(ConnectToServerCoroutine());
    }

    private IEnumerator ConnectToServerCoroutine()
    {
        var getRequestTask = GetRequestAsync(serverUrl);
        yield return new WaitUntil(() => getRequestTask.IsCompleted);

        if (getRequestTask.IsFaulted || getRequestTask.IsCanceled)
        {
            Debug.LogError("GET request failed.");
        }
        else
        {
            Debug.Log("GET request completed: " + getRequestTask.Result);
        }
    }

    private async Task<string> GetRequestAsync(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            webRequest.certificateHandler = new AcceptAllCertificates();
            var operation = webRequest.SendWebRequest();

            while (!operation.isDone)
                await Task.Yield();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError ||
                webRequest.result == UnityWebRequest.Result.DataProcessingError ||
                webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error: " + webRequest.error + " URL: " + uri);
                return null;
            }
            else
            {
                Debug.Log("Received from " + uri + ": " + webRequest.downloadHandler.text);
                return webRequest.downloadHandler.text;
            }
        }
    }

    private async Task<string> PostRequestAsync(string uri, string jsonData)
    {
        using (UnityWebRequest webRequest = new UnityWebRequest(uri, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
            webRequest.certificateHandler = new AcceptAllCertificates();
            webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            webRequest.SetRequestHeader("Content-Type", "application/json");
            var operation = webRequest.SendWebRequest();

            while (!operation.isDone)
                await Task.Yield();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError ||
                webRequest.result == UnityWebRequest.Result.DataProcessingError ||
                webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error: " + webRequest.error + " URL: " + uri);
                return null;
            }
            else
            {
                Debug.Log("Received from " + uri + ": " + webRequest.downloadHandler.text);
                SceneManager.LoadSceneAsync("Maps");
                return webRequest.downloadHandler.text;
            }
        }
    }

    public async Task<string> RegisterUserDataAsync(string name, string password, string passwordConfirm, string email)
    {
        string dataRegister = JsonUtility.ToJson(new { name, password, passwordConfirm, email });
        return await PostRequestAsync(serverUrl + "/users/addUser", dataRegister);
    }

    public async Task<string> DeleteUserDataAsync()
    {
        return await PostRequestAsync(serverUrl + "/users/delete", "{}");
    }

    public async Task<string> LoadAllUserDataAsync()
    {
        return await GetRequestAsync(serverUrl + "/users");
    }

    public async Task<string> OnLoginAsync(string name, string password)
    {
        string dataRequest = $"?username={UnityWebRequest.EscapeURL(name)}&password={UnityWebRequest.EscapeURL(password)}";
        return await GetRequestAsync(serverUrl + "/users/getUser"+ dataRequest);
    }

    public class AcceptAllCertificates : CertificateHandler
    {
        protected override bool ValidateCertificate(byte[] certificateData)
        {
            return true;
        }
    }
}
