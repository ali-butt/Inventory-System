using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ShopAnimalCardUI : MonoBehaviour
{
    public TextMeshProUGUI speciesText;
    public TextMeshProUGUI rarityText;
    public TextMeshProUGUI costText;
    public TextMeshProUGUI ageText;
    public TextMeshProUGUI healthText;

    public Image iconImage;
    public Button buyButton;

    private AnimalDataSO animalData;

    public void Setup(AnimalDataSO data)
    {
        animalData = data;

        speciesText.text = data.species;
        rarityText.text = $"Rarity: {data.rarity}";
        costText.text = $"Stars: {data.cost}";
        ageText.text = $"Age: {data.age}";
        healthText.text = $"Health: {data.health}";
        iconImage.sprite = data.icon;

        buyButton.onClick.RemoveAllListeners();
        buyButton.onClick.AddListener(() =>
        {
            if (StarManager.Instance.SpendStars(data.cost))
            {
                InventoryManager.Instance.AddAnimal(data);
                Destroy(gameObject); // Remove from shop UI
            }
            //Here we can add logic to display text to the user about low stars balance
        });
    }
}