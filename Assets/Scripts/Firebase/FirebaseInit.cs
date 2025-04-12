using Firebase;
using Firebase.Auth;
using UnityEngine;
using Firebase.Extensions;

public class FirebaseInit : MonoBehaviour
{
    public static FirebaseAuth Auth;
    public static FirebaseUser User;

    public static bool IsFirebaseReady = false; // ✅ Add this flag

    private void Awake()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            var status = task.Result;

            if (status == DependencyStatus.Available)
            {
                Debug.Log("✅ Firebase dependencies resolved.");
                Auth = FirebaseAuth.DefaultInstance;
                SignInAnonymously();
            }
            else
            {
                Debug.LogError("❌ Could not resolve Firebase dependencies: " + status);
            }
        });
    }

    private void SignInAnonymously()
    {
        Auth.SignInAnonymouslyAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompletedSuccessfully)
            {
                User = task.Result.User;
                IsFirebaseReady = true; // ✅ Set true once signed in
                Debug.Log("✅ Signed in anonymously as: " + User.UserId);
            }
            else
            {
                Debug.LogError("❌ Anonymous sign-in failed: " + task.Exception);
            }
        });
    }
}
