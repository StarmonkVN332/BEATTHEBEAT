using Facebook.Unity;
using Firebase.Auth;
using Firebase.Extensions;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FirebaseLoginManager : MonoBehaviour
{
    [Header("Register")]
    public InputField ipRegisterEmail;
    public InputField ipRegisterPassword;
    public Button buttonRegister;

    [Header("Sign In")]
    public InputField ipLoginEmail;
    public InputField ipLoginPassword;
    public Button buttonLogin;

    [Header("Social Login")]
    public Button buttonGoogleLogin;
    public Button buttonFacebookLogin;

    [Header("Switch form")]
    public Button buttonMoveToLogin;
    public Button buttonMoveToRegister;
    public GameObject loginForm;
    public GameObject registerForm;

    private FirebaseAuth auth;
    private FirebaseDatabaseManager databaseManager;

    private void Awake()
    {
        if (!FB.IsInitialized)
        {
            FB.Init(() =>
            {
                if (FB.IsInitialized) FB.ActivateApp();
                else Debug.LogError("Failed to initialize Facebook SDK.");
            });
        }
        else
        {
            FB.ActivateApp();
        }
    }

    private void Start()
    {
        auth = FirebaseAuth.DefaultInstance;

        buttonRegister.onClick.AddListener(RegisterAccountWithFirebase);
        buttonLogin.onClick.AddListener(SignInAccountWithFirebase);
        buttonMoveToRegister.onClick.AddListener(SwitchForm);
        buttonMoveToLogin.onClick.AddListener(SwitchForm);

        buttonFacebookLogin.onClick.AddListener(SignInWithFacebook);
        buttonGoogleLogin.onClick.AddListener(SignInWithGoogle); // bạn cần tích hợp Google SDK

        databaseManager = GetComponent<FirebaseDatabaseManager>();
    }

    public void RegisterAccountWithFirebase()
    {
        string email = ipRegisterEmail.text;
        string password = ipRegisterPassword.text;

        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled || task.IsFaulted)
            {
                Debug.Log("Đăng ký thất bại");
                return;
            }

            Debug.Log("Đăng ký thành công");
            User userInGame = new User("", 100, 1000);
            FirebaseUser firebaseUser = task.Result.User;

            databaseManager.WriteDatabase("Users/" + firebaseUser.UserId, userInGame.ToString());
            SceneManager.LoadScene("Test");
        });
    }

    public void SignInAccountWithFirebase()
    {
        string email = ipLoginEmail.text;
        string password = ipLoginPassword.text;

        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled || task.IsFaulted)
            {
                Debug.Log("Đăng nhập thất bại");
                return;
            }

            Debug.Log("Đăng nhập thành công");
            SceneManager.LoadScene("Test");
        });
    }

    public void SignInWithFacebook()
    {
        var permissions = new System.Collections.Generic.List<string>() { "public_profile", "email" };
        FB.LogInWithReadPermissions(permissions, result =>
        {
            if (FB.IsLoggedIn)
            {
                var accessToken = AccessToken.CurrentAccessToken.TokenString;
                var credential = FacebookAuthProvider.GetCredential(accessToken);

                auth.SignInWithCredentialAsync(credential).ContinueWithOnMainThread(task =>
                {
                    if (task.IsCanceled || task.IsFaulted)
                    {
                        Debug.LogError("Firebase Facebook login failed: " + task.Exception);
                        return;
                    }

                    Debug.Log("Firebase Facebook login success!");
                    SceneManager.LoadScene("Test");
                });
            }
            else
            {
                Debug.Log("Facebook login cancelled or failed.");
            }
        });
    }

    public void SignInWithGoogle()
    {
        Debug.Log("Google login not yet implemented. You must integrate Google Sign-In SDK for Unity.");

        // Placeholder — You must integrate the Google Sign-In plugin for Unity first.
        // After you obtain Google ID Token, do:
        // var credential = GoogleAuthProvider.GetCredential(idToken, null);
        // auth.SignInWithCredentialAsync(credential);
    }

    public void SwitchForm()
    {
        loginForm.SetActive(!loginForm.activeSelf);
        registerForm.SetActive(!registerForm.activeSelf);
    }
}
