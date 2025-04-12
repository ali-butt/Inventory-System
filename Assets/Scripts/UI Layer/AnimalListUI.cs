using UnityEngine;

public class AnimalListUI : MonoBehaviour
{
    public Transform contentParent;
    public GameObject animalCardPrefab;

    private void Start()
    {
        PopulateAnimalList();
    }

    void PopulateAnimalList()
    {
        if (animalCardPrefab == null || contentParent == null)
        {
            Debug.LogError("AnimalListUI: Prefab or content parent is not assigned!");
            return;
        }

        foreach (Transform child in contentParent)
            Destroy(child.gameObject);

        foreach (var animal in InventoryManager.Instance.inventory.animals)
        {
            GameObject card = Instantiate(animalCardPrefab, contentParent);
            card.GetComponent<AnimalCardUI>().Setup(animal);
        }
    }

}