using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public GameObject animalCardPrefab;   // Reusable card prefab
    public Transform contentParent;       // Content panel of ScrollView

    private void Start()
    {
        PopulateInventory();
    }

    public void PopulateInventory()
    {
        // Clear old cards
        foreach (Transform child in contentParent)
            Destroy(child.gameObject);

        List<AnimalInstance> ownedAnimals = InventoryManager.Instance.inventory.ownedAnimals;

        foreach (var animal in ownedAnimals)
        {
            GameObject card = Instantiate(animalCardPrefab, contentParent);
            InventoryAnimalCardUI cardUI = card.GetComponent<InventoryAnimalCardUI>();
            cardUI.Setup(animal);
        }
    }
}