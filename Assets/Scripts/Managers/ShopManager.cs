using UnityEngine;
using System.Collections.Generic;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance;

    public List<AnimalDataSO> allAnimals = new List<AnimalDataSO>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadAnimalsFromResources();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void LoadAnimalsFromResources()
    {
        allAnimals.Clear();
        AnimalDataSO[] loaded = Resources.LoadAll<AnimalDataSO>("Animals");
        allAnimals.AddRange(loaded);
        Debug.Log($"[ShopManager] Loaded {allAnimals.Count} animals into shop.");
    }

    public AnimalDataSO GetAnimalById(string id)
    {
        return allAnimals.Find(a => a.id == id);
    }
}