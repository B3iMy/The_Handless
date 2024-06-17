using UnityEngine;

public class UserInfo : MonoBehaviour
{
    public string userId { get; private set; }
    private string UserName, UserPassword, Level, Coins;

    public void SetCredential(string user_name, string user_password)
    {
        UserName = user_name;
        UserPassword = user_password;
        //Debug.Log($"Credentials set - UserName: {UserName}, UserPassword: {UserPassword}");
    }

    public void SetID(string id)
    {
        userId = id;
        //Debug.Log($"UserID set - UserID: {UserID}");
    }
}