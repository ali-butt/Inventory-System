using UnityEngine;
using Firebase.Firestore;
using Firebase.Auth;
using System.Threading.Tasks;
using System.Collections.Generic;
using Firebase.Extensions;



public class FirebaseInventorySync : MonoBehaviour
{
    FirebaseFirestore db;
    FirebaseUser user;

    private void Start()
    {
        user = FirebaseAuth.DefaultInstance.CurrentUser;
        db = FirebaseFirestore.DefaultInstance;
    }

    // Upload local inventory to Firestore
    public async void SaveInventoryToCloud(string json, int stars)
    {
        if (user == null) return;

        DocumentReference docRef = db.Collection("users").Document(user.UserId);
        Dictionary<string, object> data = new Dictionary<string, object>
        {
            { "inventory", json },
            { "stars", stars }
        };

        await docRef.SetAsync(data).ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
                Debug.Log("✅ Inventory and stars saved to Firebase!");
            else
                Debug.LogError("❌ Error saving to Firebase: " + task.Exception);
        });
    }

    // Load inventory from Firestore
    public async Task<(string json, int stars)> LoadInventoryFromCloud()
    {
        if (user == null) return ("", 0);

        DocumentReference docRef = db.Collection("users").Document(user.UserId);
        var snapshot = await docRef.GetSnapshotAsync();

        if (snapshot.Exists)
        {
            string json = snapshot.ContainsField("inventory") ? snapshot.GetValue<string>("inventory") : "";
            int stars = snapshot.ContainsField("stars") ? snapshot.GetValue<int>("stars") : 0;
            return (json, stars);
        }
        else
        {
            Debug.Log("No cloud save found for this user.");
            return ("", 0);
        }
    }
}