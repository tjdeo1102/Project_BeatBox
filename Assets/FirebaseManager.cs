using UnityEngine;
using Firebase.Auth;
using Google;
using System.Threading.Tasks;
using System.IO;

public class FirebaseManager : ManagerBase<FirebaseManager>
{
    private string googleAPI;
    private FirebaseAuth auth;
    private GoogleSignInConfiguration config;
    private FirebaseUser currentUser;

    public override void StartInit()
    {
        base.StartInit();
        googleAPI = Path.Combine(Application.streamingAssetsPath, "config.json");
        auth = FirebaseAuth.DefaultInstance;
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

            currentUser = task.Result;
            Debug.Log($"[FirebaseLogin] 로그인 성공: {currentUser.DisplayName} / {currentUser.UserId}");
        });
    }

    /// <summary>
    /// 현재 로그인된 유저 로그아웃
    /// </summary>
    public void Logout()
    {
        Debug.Log("[Logout] 로그아웃 시도");
        auth.SignOut();
        GoogleSignIn.DefaultInstance.SignOut();
        currentUser = null;
    }

    /// <summary>
    /// 토큰 재발급
    /// </summary>
    public void RefreshToken()
    {
        if (currentUser == null)
        {
            Debug.LogWarning("[RefreshToken] 현재 로그인된 유저 없음");
            return;
        }

        currentUser.TokenAsync(true).ContinueWith(task =>
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

    /// <summary>
    /// 현재 로그인 유저 정보 가져오기
    /// </summary>
    public FirebaseUser GetCurrentUser()
    {
        return currentUser;
    }
}