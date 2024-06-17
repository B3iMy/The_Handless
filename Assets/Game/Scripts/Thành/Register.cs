using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Register : MonoBehaviour
{
    public InputField UsernameInput;
    public InputField PasswordInput;
    public InputField ConfirmPasswordInput;
    public Button RegisterButton;

    void Start()
    {
        RegisterButton.onClick.AddListener(() =>
        {
            StartCoroutine(Main.Instance.web.Register(UsernameInput.text, PasswordInput.text, ConfirmPasswordInput.text));
        });
    }
}
