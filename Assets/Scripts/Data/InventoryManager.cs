using UnityEngine;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    public AnimalInventory inventory = new AnimalInventory();

    private const string InventoryKey = "AnimalInventory";

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
            inventory = JsonUtility.FromJson<AnimalInventory>(json);
        }
        else
        {
            CreateDefaultInventory();
            SaveInventory();
        }
    }

    public void SaveInventory()
    {
        string json = JsonUtility.ToJson(inventory);
        PlayerPrefs.SetString(InventoryKey, json);
        PlayerPrefs.Save();
    }

    private void CreateDefaultInventory()
    {
        inventory.animals = new List<Animal>
        {
            new Animal { id = "cat_001", species = "Cat", rarity = "Common", age = 1, health = 100, specialAbility = "Sneaky", isOwned = false },
            new Animal { id = "dog_001", species = "Dog", rarity = "Rare", age = 2, health = 120, specialAbility = "Loyal", isOwned = false },
            new Animal { id = "fox_001", species = "Fox", rarity = "Epic", age = 3, health = 140, specialAbility = "Fast", isOwned = false },
        };
    }

    public void BuyAnimal(string animalId)
    {
        var animal = inventory.animals.Find(a => a.id == animalId);
        if (animal != null && !animal.isOwned)
        {
            animal.isOwned = true;
            SaveInventory();
        }
    }

    public void RemoveAnimal(string animalId)
    {
        var animal = inventory.animals.Find(a => a.id == animalId);
        if (animal != null && animal.isOwned)
        {
            animal.isOwned = false;
            SaveInventory();
        }
    }
}
