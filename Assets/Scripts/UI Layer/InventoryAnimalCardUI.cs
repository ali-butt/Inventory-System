using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InventoryAnimalCardUI : MonoBehaviour
{
    public TextMeshProUGUI speciesText;
    public TextMeshProUGUI rarityText;
    public TextMeshProUGUI ageText;
    public TextMeshProUGUI healthText;
    public Image iconImage;
    public Button removeButton;

    private AnimalInstance currentAnimal;

    public void Setup(AnimalInstance data)
    {
        currentAnimal = data;

        speciesText.text = data.species;
        rarityText.text = $"Rarity: {data.rarity}";
        ageText.text = $"Age: {data.age}";
        healthText.text = $"Health: {data.health}";

        // Load icon from ScriptableObject by ID
        var animalSO = ShopManager.Instance.GetAnimalById(data.id);
        if (animalSO != null)
            iconImage.sprite = animalSO.icon;

        removeButton.onClick.RemoveAllListeners();
        removeButton.onClick.AddListener(() =>
        {
            InventoryManager.Instance.RemoveAnimal(data.id);
            Destroy(gameObject); // Remove from UI
        });
    }
}