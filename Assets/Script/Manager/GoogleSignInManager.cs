using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using UnityEngine.UI;
using Google;
using System;
using System.Threading.Tasks;
using WebSocketSharp;

public class GoogleSignInManager : MonoBehaviour
{
    public static GoogleSignInManager Instance;

    public string infoText;
    public string webClientId = "<your client id here>";

    private FirebaseAuth auth;
    private FirebaseUser user;
    private GoogleSignInConfiguration configuration;

    public string userName;
    public string userEmail;

    // Phone Authentication fields
    public InputField phoneNumber;
    public InputField countyCode;
    public InputField otp;
    private uint phoneAuthTimeoutMs = 60 * 1000;
    private string verificationID;
    private PhoneAuthProvider provider;

    private bool firebaseInitialized = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        configuration = new GoogleSignInConfiguration { WebClientId = webClientId, RequestEmail = true, RequestIdToken = true };
        CheckFirebaseDependencies();
    }

    private void Update()
    {
        if (user != null && userEmail.IsNullOrEmpty())
        {
            userEmail = user.Email;
            userName = user.DisplayName;

            print("User Email : " + userEmail);
            print("User Name : " + userName);

            LoginManager.Instance.googleUserEmail = userEmail;
            LoginManager.Instance.googleUserName = userName;
        }
    }

    private void CheckFirebaseDependencies()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                if (task.Result == DependencyStatus.Available)
                {
                    auth = FirebaseAuth.DefaultInstance;
                    provider = PhoneAuthProvider.GetInstance(auth);
                    firebaseInitialized = true;
                }
                else
                {
                    AddToInformation("Could not resolve all Firebase dependencies: " + task.Result.ToString());
                }
            }
            else
            {
                AddToInformation("Dependency check was not completed. Error : " + task.Exception.Message);
            }
        });
    }

    // Google Sign-In methods
    public void SignInWithGoogle()
    {
        SoundManager.Instance.ButtonClick();
        if (Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.WindowsEditor)
        {
            string imageUrl = "https://lh3.googleusercontent.com/ogw/AOh-ky3pVzuI2Z1jwSVkU2JGgCT22_4YtGST8d9h3uI=s64-c-mo";
            PlayerPrefs.SetString("ProfileURL", LoginManager.Instance.testImageUrl);

            LoginManager.Instance.googleUserEmail = LoginManager.Instance.testEmail;
            LoginManager.Instance.googleUserName = LoginManager.Instance.testUserName;
            LoginManager.Instance.email_Main(LoginManager.Instance.testFirebaseToken, LoginManager.Instance.googleUserName, LoginManager.Instance.testImageUrl);
        }
        else
        {
            print("Sign in Button Click");
            OnSignIn();
        }
    }

    public void SignOutFromGoogle()
    {
        AddToInformation("Calling SignOut");
        GoogleSignIn.DefaultInstance.SignOut();
    }

    private void OnSignIn()
    {
        GoogleSignIn.Configuration = configuration;
        GoogleSignIn.Configuration.UseGameSignIn = false;
        GoogleSignIn.Configuration.RequestIdToken = true;
        GoogleSignIn.Configuration.RequestEmail = true;
        AddToInformation("Calling SignIn");

        GoogleSignIn.DefaultInstance.SignIn().ContinueWith(OnAuthenticationFinished);
    }

    private void OnSignOut()
    {
        AddToInformation("Calling SignOut");
        GoogleSignIn.DefaultInstance.SignOut();
    }

    public void OnDisconnect()
    {
        AddToInformation("Calling Disconnect");
        GoogleSignIn.DefaultInstance.Disconnect();
    }

    internal void OnAuthenticationFinished(Task<GoogleSignInUser> task)
    {
        if (task.IsFaulted)
        {
            using (IEnumerator<Exception> enumerator = task.Exception.InnerExceptions.GetEnumerator())
            {
                if (enumerator.MoveNext())
                {
                    GoogleSignIn.SignInException error = (GoogleSignIn.SignInException)enumerator.Current;
                    AddToInformation("Got Error: " + error.Status + " " + error.Message);
                }
                else
                {
                    AddToInformation("Got Unexpected Exception?!?" + task.Exception);
                }
            }
        }
        else if (task.IsCanceled)
        {
            AddToInformation("Canceled");
        }
        else
        {
            print("Google Sign-In succeeded");

            print("IdToken: " + task.Result.IdToken);
            string firebaseToken = task.Result.IdToken;
            print("ImageUrl: " + task.Result.ImageUrl.ToString());

            PlayerPrefs.SetString("ProfileURL", task.Result.ImageUrl.ToString());

            Firebase.Auth.Credential credential = Firebase.Auth.GoogleAuthProvider.GetCredential(task.Result.IdToken, null);

            auth.SignInWithCredentialAsync(credential).ContinueWith(t =>
            {
                if (t.IsCanceled)
                {
                    print("SignInWithCredentialAsync was canceled.");
                    return;
                }

                if (t.IsFaulted)
                {
                    print("SignInWithCredentialAsync encountered an error: " + t.Exception);
                    return;
                }

                user = auth.CurrentUser;
                LoginManager.Instance.googleUserEmail = user.Email.ToString();
                LoginManager.Instance.googleUserName = user.DisplayName.ToString();

                LoginManager.Instance.email_Main(firebaseToken, LoginManager.Instance.googleUserName, PlayerPrefs.GetString("ProfileURL"));

                Debug.Log("LoadImage(CheckImageUrl)");
            });
        }
    }

    private void SignInWithGoogleOnFirebase(string idToken)
    {
        Credential credential = GoogleAuthProvider.GetCredential(idToken, null);

        auth.SignInWithCredentialAsync(credential).ContinueWith(task =>
        {
            AggregateException ex = task.Exception;
            if (ex != null)
            {
                if (ex.InnerExceptions[0] is FirebaseException inner && (inner.ErrorCode != 0))
                    AddToInformation("\nError code = " + inner.ErrorCode + " Message = " + inner.Message);
            }
            else
            {
                AddToInformation("Sign In Successful.");
            }
        });
    }

    public void OnSignInSilently()
    {
        GoogleSignIn.Configuration = configuration;
        GoogleSignIn.Configuration.UseGameSignIn = false;
        GoogleSignIn.Configuration.RequestIdToken = true;
        AddToInformation("Calling SignIn Silently");

        GoogleSignIn.DefaultInstance.SignInSilently().ContinueWith(OnAuthenticationFinished);
    }

    public void OnGamesSignIn()
    {
        GoogleSignIn.Configuration = configuration;
        GoogleSignIn.Configuration.UseGameSignIn = true;
        GoogleSignIn.Configuration.RequestIdToken = false;

        AddToInformation("Calling Games SignIn");

        GoogleSignIn.DefaultInstance.SignIn().ContinueWith(OnAuthenticationFinished);
    }

    private void AddToInformation(string str)
    {
        infoText += "\n" + str;
        Debug.Log("Info : " + str);
    }

    // Phone Authentication methods

    #region PhoneOtp Methods Firebase
    public void LogInWithPhoneNumber()
    {
        if (!firebaseInitialized)
        {
            Debug.LogError("Firebase not initialized.");
            return;
        }

        // Verify the phone number
        provider.VerifyPhoneNumber(countyCode.text + phoneNumber.text, phoneAuthTimeoutMs, null,
            verificationCompleted: (credential) =>
            {
                Debug.Log("Verification completed automatically");
                SignInWithPhoneAuthCredential(credential); // Automatically sign in if verification completes
        },
            verificationFailed: (error) =>
            {
                Debug.LogError("Verification failed: " + error);
            },
            codeSent: (id, token) =>
            {
                verificationID = id;
                Debug.Log("Code sent. Verification ID: " + verificationID);
            // You can automatically request user to input OTP
            // Show a UI element to input the OTP
        },
            codeAutoRetrievalTimeOut: (id) =>
            {
                Debug.LogWarning("Code auto-retrieval timeout. Verification ID: " + id);
            });
    }

    public void VerifyOTP()
    {
        if (string.IsNullOrEmpty(verificationID))
        {
            Debug.LogError("Verification ID is null or empty.");
            return;
        }

        if (provider == null)
        {
            Debug.LogError("Provider not initialized.");
            return;
        }

        // Get the credential using the verification ID and OTP entered by the user
        Credential credential = provider.GetCredential(verificationID, otp.text);
        SignInWithPhoneAuthCredential(credential);
    }

    private void SignInWithPhoneAuthCredential(Credential credential)
    {
        auth.SignInWithCredentialAsync(credential).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInWithCredentialAsync was canceled.");
                return;
            }

            if (task.IsFaulted)
            {
                Debug.LogError("SignInWithCredentialAsync encountered an error: " + task.Exception);
                return;
            }

            FirebaseUser newUser = task.Result;
            Debug.Log("User signed in successfully");
            Debug.Log("Phone number: " + newUser.PhoneNumber);
            Debug.Log("Phone provider ID: " + newUser.ProviderId);
            Debug.Log("Phone provider ID: " + newUser.UserId);
        });
    }

}
#endregion
