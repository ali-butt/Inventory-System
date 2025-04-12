using System.Collections.Generic;
using UnityEngine;

public class ShopUI : MonoBehaviour
{
    public GameObject animalCardPrefab;         // UI prefab for each shop animal
    public Transform contentParent;             // ScrollView Content object

    private void Start()
    {
        PopulateShop();
    }

    public void PopulateShop()
    {
        // Clear previous UI
        foreach (Transform child in contentParent)
        {
            Destroy(child.gameObject);
        }

        // Loop through all shop animals
        foreach (AnimalDataSO animalSO in ShopManager.Instance.allAnimals)
        {
            if (InventoryManager.Instance.IsOwned(animalSO.id))
                continue; // Skip owned animals

            GameObject card = Instantiate(animalCardPrefab, contentParent);
            ShopAnimalCardUI cardUI = card.GetComponent<ShopAnimalCardUI>();
            cardUI.Setup(animalSO);
        }
    }
}