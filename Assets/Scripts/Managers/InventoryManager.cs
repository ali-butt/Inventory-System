
using System.Collections.Generic;
using UnityEngine;



[System.Serializable]
public class InventoryData
{
    public List<AnimalInstance> ownedAnimals = new List<AnimalInstance>();
}

public class InventoryManager : MonoBehaviour
{
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

    void Start()
    {
        LoadInventory();
    }

    public async void LoadInventory()
    {
        string json = "";
        int cloudStars = 0;

        var firebaseSync = FindObjectOfType<FirebaseInventorySync>();

        if (firebaseSync != null)
        {
            var result = await firebaseSync.LoadInventoryFromCloud();
            json = result.json;
            cloudStars = result.stars;
        }

        if (!string.IsNullOrEmpty(json))
        {
            try
            {
                inventory = JsonUtility.FromJson<InventoryData>(json);
                StarManager.Instance.SetStars(cloudStars);
                Debug.Log("✅ Inventory loaded from Firebase.");
            }
            catch
            {
                Debug.LogWarning("❌ Firebase data invalid. Using default inventory.");
                CreateDefaultInventory();
            }
        }
        else if (PlayerPrefs.HasKey(InventoryKey))
        {
            json = PlayerPrefs.GetString(InventoryKey);
            inventory = JsonUtility.FromJson<InventoryData>(json);
            StarManager.Instance.LoadStars(); // fallback star load
            Debug.Log("📦 Loaded from PlayerPrefs.");
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
        PlayerPrefs.SetString(InventoryKey, json);
        PlayerPrefs.Save();

        var firebaseSync = FindObjectOfType<FirebaseInventorySync>();
        if (firebaseSync != null)
        {
            int stars = StarManager.Instance.GetCurrentStars();
            firebaseSync.SaveInventoryToCloud(json, stars);
        }

        Debug.Log("💾 Inventory saved locally + to cloud.");
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
