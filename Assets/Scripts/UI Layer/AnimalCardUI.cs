using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AnimalCardUI : MonoBehaviour
{
    public TextMeshProUGUI speciesText;
    public TextMeshProUGUI rarityText;
    public TextMeshProUGUI ageText;
    public TextMeshProUGUI healthText;

    public Button buyButton;
    public Button removeButton;

    private Animal currentAnimal;

    public void Setup(Animal animal)
    {
        currentAnimal = animal;

        speciesText.text = $"Species: {animal.species}";
        rarityText.text = $"Rarity: {animal.rarity}";
        ageText.text = $"Age: {animal.age}";
        healthText.text = $"Health: {animal.health}";

        buyButton.gameObject.SetActive(!animal.isOwned);
        removeButton.gameObject.SetActive(animal.isOwned);

        buyButton.onClick.RemoveAllListeners();
        removeButton.onClick.RemoveAllListeners();

        buyButton.onClick.AddListener(() =>
        {
            InventoryManager.Instance.BuyAnimal(animal.id);
            Setup(InventoryManager.Instance.inventory.animals.Find(a => a.id == animal.id)); // Refresh
        });

        removeButton.onClick.AddListener(() =>
        {
            InventoryManager.Instance.RemoveAnimal(animal.id);
            Setup(InventoryManager.Instance.inventory.animals.Find(a => a.id == animal.id)); // Refresh
        });
    }
}