using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Services.Authentication;  // Make sure Unity Services Authentication is correctly imported
using System.Threading.Tasks;

public class RegisterService : MonoBehaviour
{
    public InputField InputName;
    public InputField InputPassword;
    public InputField InputConfirmPassword;
    public InputField InputEmail;
    public Button register;
    public Text ResultText;  // Changed to use UnityEngine.UI.Text directly

    private HttpClient httpClient;

    private void Start()
    {
        try
        {
            // Initialize HttpClient (replace with your actual initialization method)
            httpClient = new HttpClient();

            // Add listener for the register button
            register.onClick.AddListener(RegisterUserData);
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Error initializing RegisterService: {ex.Message}");
        }
    }

    private async void RegisterUserData()
    {

            // Basic input validation
            if (string.IsNullOrEmpty(InputName.text) || string.IsNullOrEmpty(InputPassword.text) || string.IsNullOrEmpty(InputEmail.text))
            {
                Debug.LogWarning("Please fill in all fields.");
                return;
            }

            if (InputPassword.text != InputConfirmPassword.text)
            {
                Debug.LogWarning("Passwords do not match.");
                return;
            }

            // You might want to perform more thorough validation here (e.g., email format)

            // Call your API or service to register user data
            string result = await httpClient.RegisterUserDataAsync(InputName.text, InputPassword.text, InputEmail.text);
            Debug.Log(result);
            // Display result to user (or log it for debugging)
        
    }
}
