using UnityEngine;
using Firebase.Auth;
using Google;
using System.Threading.Tasks;
using System.IO;
using Firebase;
using Firebase.Extensions;
using Firebase.Database;
using UnityEngine.AddressableAssets;

public class FirebaseManager : ManagerBase<FirebaseManager>
{
    private string googleAPI;
    private FirebaseAuth auth;
    private GoogleSignInConfiguration config;
    public FirebaseUser CurrentUser;
    public DatabaseReference DBRef;

    public override void StartInit()
    {
        base.StartInit();
        var textAsset = Addressables.LoadAssetAsync<TextAsset>("Config").WaitForCompletion();
        var data = JsonUtility.FromJson<ConfigData>(textAsset.text);
        googleAPI = data.GoogleWebClientId;
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                Debug.Log("Firebase Ready");
                auth = FirebaseAuth.DefaultInstance;
                DBRef = FirebaseDatabase.DefaultInstance.RootReference;
            }
            else
            {
                Debug.LogError($"Error: {dependencyStatus}");
            }
        });
        config = new GoogleSignInConfiguration
        {
            WebClientId = googleAPI,
            RequestIdToken = true,
            RequestEmail = true,
        };
    }

    private void Start()
    {
        StartInit();
    }

    /// <summary>
    /// 구글 로그인 요청
    /// </summary>
    public void GoogleLogin()
    {
        GoogleSignIn.Configuration = config;
        GoogleSignIn.Configuration.UseGameSignIn = false;
        GoogleSignIn.Configuration.RequestIdToken = true;

        Debug.Log("[GoogleLogin] 로그인 시도");
        GoogleSignIn.DefaultInstance.SignIn().ContinueWith(OnGoogleAuthFinished);
    }

    /// <summary>
    /// 구글 로그인 완료 후 파이어베이스 인증 연결
    /// </summary>
    private void OnGoogleAuthFinished(Task<GoogleSignInUser> task)
    {
        TaskCompletionSource<FirebaseUser> signInCompleted = new();
        if (task.IsFaulted)
        {
            signInCompleted.SetException(task.Exception);
            Debug.LogError("[GoogleLogin] 구글 로그인 실패: " + task.Exception);
            return;
        }
        if (task.IsCanceled)
        {
            signInCompleted.SetCanceled();
            Debug.LogWarning("[GoogleLogin] 구글 로그인 취소됨");
            return;
        }

        GoogleSignInUser googleUser = task.Result;
        Debug.Log($"[GoogleLogin] 구글 로그인 성공: {googleUser.DisplayName} ({googleUser.Email})");

        Credential credential = GoogleAuthProvider.GetCredential(googleUser.IdToken, null);
        SignInWithFirebase(credential);
    }

    /// <summary>
    /// 파이어베이스 인증
    /// </summary>
    private void SignInWithFirebase(Credential credential)
    {
        if (auth == null)
        {
            Logout();
        }
        auth.SignInWithCredentialAsync(credential).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("[FirebaseLogin] 취소됨");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("[FirebaseLogin] 에러: " + task.Exception);
                return;
            }

            CurrentUser = task.Result;
            SceneLoadManager.Instance.LoadScene((int)SceneIndex.Lobby);

            Debug.Log($"[FirebaseLogin] 로그인 성공: {CurrentUser.DisplayName} / {CurrentUser.UserId}");
        });
    }

    /// <summary>
    /// 현재 로그인된 유저 로그아웃
    /// </summary>
    public void Logout()
    {
        Debug.Log("[Logout] 로그아웃 시도");
        if (auth != null)  auth.SignOut();
        GoogleSignIn.DefaultInstance.SignOut();
        CurrentUser = null;
    }

    /// <summary>
    /// 토큰 재발급
    /// </summary>
    public void RefreshToken()
    {
        if (CurrentUser == null)
        {
            Debug.LogWarning("[RefreshToken] 현재 로그인된 유저 없음");
            return;
        }

        CurrentUser.TokenAsync(true).ContinueWith(task =>
        {
            if (task.IsCanceled || task.IsFaulted)
            {
                Debug.LogError("[RefreshToken] 토큰 재발급 실패: " + task.Exception);
                return;
            }
            string newToken = task.Result;
            Debug.Log("[RefreshToken] 새 토큰 발급 완료: " + newToken);
        });
    }
}