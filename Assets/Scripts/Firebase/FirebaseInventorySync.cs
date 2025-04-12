using UnityEngine;
using Firebase.Firestore;
using Firebase.Auth;
using System.Threading.Tasks;
using System.Collections.Generic;
using Firebase.Extensions;
using System.Collections;

public class FirebaseInventorySync : MonoBehaviour
{
    FirebaseFirestore db;
    FirebaseUser user;

    private void Start()
    {
        // Ensure Firebase is initialized and user is signed in before proceeding
        StartCoroutine(WaitForUser());
    }

    // Wait until Firebase is ready
    private IEnumerator WaitForUser()
    {
        while (FirebaseAuth.DefaultInstance.CurrentUser == null)
        {
            yield return new WaitForSeconds(1);
            Debug.Log("⏳ Waiting for Firebase sign-in...");
        }

        user = FirebaseAuth.DefaultInstance.CurrentUser;
        db = FirebaseFirestore.DefaultInstance;
        Debug.Log("✅ Firebase and user are ready.");
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

        // Check if the document exists
        if (snapshot.Exists)
        {
            Debug.Log("Document exists: " + snapshot.Id);

            // Retrieve inventory data and stars
            string json = snapshot.ContainsField("inventory") ? snapshot.GetValue<string>("inventory") : "";
            int stars = snapshot.ContainsField("stars") ? snapshot.GetValue<int>("stars") : 0;

            // Log the fetched data for debugging
            Debug.Log("Fetched inventory data: " + json);
            Debug.Log("Fetched stars: " + stars);

            return (json, stars);
        }
        else
        {
            Debug.LogError("❌ No document found for this user.");
            return ("", 0);
        }
    }
}
