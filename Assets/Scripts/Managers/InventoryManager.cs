using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using Firebase.Auth;
using System;
using UnityEngine.UI;

[System.Serializable]
public class InventoryData
{
    public List<AnimalInstance> ownedAnimals = new List<AnimalInstance>();
}

public class InventoryManager : MonoBehaviour
{
    [SerializeField] Button InventoryBtn;
    public static InventoryManager Instance;

    private const string InventoryKey = "AnimalInventory";

    public InventoryData inventory = new InventoryData();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            //LoadInventory();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private async void Start()
    {
        // Wait until Firebase is ready
        while (!FirebaseInit.IsFirebaseReady)
        {
            Debug.Log("⏳ Waiting for Firebase to be ready...");
            await Task.Delay(500); // wait 0.5 seconds
        }

        Debug.Log("✅ Firebase is ready, loading inventory...");
        LoadInventory();
    }

    public async void LoadInventory()
    {
        string json = "";
        int cloudStars = 0;

        var firebaseSync = FindObjectOfType<FirebaseInventorySync>();

        if (firebaseSync != null)
        {
            Debug.Log("📝 Attempting to load inventory...");
            var result = await firebaseSync.LoadInventoryFromCloud();
            json = result.json;
            cloudStars = result.stars;

            Debug.Log("Inventory JSON: " + json);  // Check if data is returned
        }

        if (!string.IsNullOrEmpty(json))
        {
            try
            {
                Debug.Log("📦 Deserializing inventory data...");
                inventory = JsonUtility.FromJson<InventoryData>(json);
                StarManager.Instance.SetStars(cloudStars);
                Debug.Log("✅ Inventory loaded from Firebase.");

                InventoryBtn.interactable = true;
            }
            catch (Exception ex)
            {
                Debug.LogWarning("❌ Firebase data invalid: " + ex.Message);
                CreateDefaultInventory();
            }
        }
        else
        {
            CreateDefaultInventory();
            SaveInventory();
            Debug.Log("📦 No saved data. Default inventory created.");
        }
    }


    public void SaveInventory()
    {
        string json = JsonUtility.ToJson(inventory);

        /*
        PlayerPrefs.SetString(InventoryKey, json);
        PlayerPrefs.Save();
        */

        var firebaseSync = FindObjectOfType<FirebaseInventorySync>();
        if (firebaseSync != null)
        {
            int stars = StarManager.Instance.GetCurrentStars();
            firebaseSync.SaveInventoryToCloud(json, stars);

            Debug.Log("💾 Inventory saved to cloud.");
        }
        else
        {
            print("saving failed");
        }

    }

    private void CreateDefaultInventory()
    {
        inventory.ownedAnimals = new List<AnimalInstance>(); // start with empty inventory
    }

    public bool IsOwned(string id)
    {
        return inventory.ownedAnimals.Exists(a => a.id == id);
    }

    public void AddAnimal(AnimalDataSO data)
    {
        if (!IsOwned(data.id))
        {
            inventory.ownedAnimals.Add(new AnimalInstance(data));
            SaveInventory();
        }
    }

    public void RemoveAnimal(string id)
    {
        var animal = inventory.ownedAnimals.Find(a => a.id == id);
        if (animal != null)
        {
            inventory.ownedAnimals.Remove(animal);
            SaveInventory();
        }
    }

    public List<AnimalInstance> GetOwnedAnimals()
    {
        return new List<AnimalInstance>(inventory.ownedAnimals);
    }

    public List<string> GetOwnedAnimalIDs()
    {
        return inventory.ownedAnimals.ConvertAll(a => a.id);
    }
}
