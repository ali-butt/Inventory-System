
using UnityEngine;
using TMPro;

public class StarManager : MonoBehaviour
{
    public static StarManager Instance;

    [SerializeField] private int currentStars = 100;
    [SerializeField] private TextMeshProUGUI starText;

    private const string StarKey = "PlayerStars";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadStars();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadStars()
    {
        currentStars = PlayerPrefs.GetInt(StarKey, 100);
        UpdateUI();
    }

    public void SaveStars()
    {
        PlayerPrefs.SetInt(StarKey, currentStars);
        PlayerPrefs.Save();

        // Also sync to Firebase (optional)
        var firebaseSync = FindObjectOfType<FirebaseInventorySync>();
        if (firebaseSync != null)
        {
            string inventoryJson = JsonUtility.ToJson(InventoryManager.Instance.inventory);
            firebaseSync.SaveInventoryToCloud(inventoryJson, currentStars);
        }
    }

    public bool SpendStars(int amount)
    {
        if (currentStars >= amount)
        {
            currentStars -= amount;
            SaveStars();
            return true;
        }
        return false;
    }

    public void AddStars(int amount)
    {
        currentStars += amount;
        SaveStars();
    }

    public void SetStars(int amount)
    {
        currentStars = amount;
        SaveStars();
    }

    public int GetCurrentStars()
    {
        return currentStars;
    }

    private void UpdateUI()
    {
        if (starText != null)
            starText.text = $"⭐ {currentStars}";
    }
}
