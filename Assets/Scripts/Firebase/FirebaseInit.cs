using Firebase;
using Firebase.Auth;
using UnityEngine;

public class FirebaseInit : MonoBehaviour
{
    public static FirebaseAuth Auth;
    public static FirebaseUser User;

    private void Awake()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            if (task.Result == DependencyStatus.Available)
            {
                Auth = FirebaseAuth.DefaultInstance;
                SignInAnonymously();
            }
            else
            {
                Debug.LogError("Firebase not available: " + task.Result);
            }
        });
    }

    private void SignInAnonymously()
    {
        Auth.SignInAnonymouslyAsync().ContinueWith(task =>
        {
            if (task.IsCompleted && !task.IsFaulted)
            {
                User = task.Result.User; // ✅ Fix is here
                Debug.Log($"Signed in as: {User.UserId}");
            }
            else
            {
                Debug.LogError("Anonymous sign-in failed: " + task.Exception);
            }
        });
    }
}