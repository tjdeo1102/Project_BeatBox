using UnityEngine;
using UnityEngine.UI;

public class PrototypeLobbyUI : MonoBehaviour
{
    public void OnLogin()
    {
        FirebaseManager.Instance.GoogleLogin();
    }

    public void OnLogout()
    {
        FirebaseManager.Instance.Logout();
    }
}
