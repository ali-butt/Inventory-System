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
            LoadInventory();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadInventory()
    {
        if (PlayerPrefs.HasKey(InventoryKey))
        {
            string json = PlayerPrefs.GetString(InventoryKey);
            try
            {
                inventory = JsonUtility.FromJson<InventoryData>(json);
                if (inventory == null || inventory.ownedAnimals == null)
                {
                    Debug.LogWarning("[InventoryManager] JSON was parsed but returned null. Creating new inventory.");
                    inventory = new InventoryData();
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError($"[InventoryManager] Failed to parse inventory JSON: {e.Message}");
                inventory = new InventoryData(); // fallback to empty inventory
            }
        }
        else
        {
            inventory = new InventoryData(); // no save found
        }

        SaveInventory(); // Save back to ensure valid structure
    }


    public void SaveInventory()
    {
        if (inventory == null)
        {
            Debug.LogWarning("[InventoryManager] Inventory is null. Creating default before saving.");
            inventory = new InventoryData();
        }

        string json = JsonUtility.ToJson(inventory);
        PlayerPrefs.SetString(InventoryKey, json);
        PlayerPrefs.Save();
    }


    public bool IsOwned(string id)
    {
        return inventory.ownedAnimals.Exists(a => a.id == id);
    }

    public void AddAnimal(AnimalDataSO data)
    {
        print(data.species);
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

    public List<string> GetOwnedAnimalIDs()
    {
        return inventory.ownedAnimals.ConvertAll(a => a.id);
    }
}