using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    // Hàm này sẽ được gọi để chuyển sang scene mới
    public void LoadScene(string sceneName)
    {
        // Kiểm tra xem tên scene có hợp lệ không
        if (sceneName != null && sceneName != "")
        {
            // Chuyển sang scene mới
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogWarning("Tên scene không hợp lệ.");
        }
    }

    // Hàm ví dụ để tải scene theo chỉ số (index) của nó trong build settings
    public void LoadSceneByIndex(int sceneIndex)
    {
        // Kiểm tra xem chỉ số scene có hợp lệ không
        if (sceneIndex >= 0 && sceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            // Chuyển sang scene mới
            SceneManager.LoadScene(sceneIndex);
        }
        else
        {
            Debug.LogWarning("Chỉ số scene không hợp lệ.");
        }
    }
}

