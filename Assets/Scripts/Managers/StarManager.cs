using UnityEngine;
using TMPro;

public class StarManager : MonoBehaviour
{
    public static StarManager Instance;

    [SerializeField] private int currentStars = 10000;
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
        currentStars = PlayerPrefs.GetInt(StarKey, 10000); // Default 10000 stars
        UpdateUI();
    }

    public void SaveStars()
    {
        PlayerPrefs.SetInt(StarKey, currentStars);
        PlayerPrefs.Save();
    }

    public bool SpendStars(int amount)
    {
        if (currentStars >= amount)
        {
            currentStars -= amount;
            SaveStars();
            UpdateUI();
            return true;
        }
        return false;
    }

    public void AddStars(int amount)
    {
        currentStars += amount;
        SaveStars();
        UpdateUI();
    }

    public int GetCurrentStars()
    {
        return currentStars;
    }

    private void UpdateUI()
    {
        if (starText != null)
            starText.text = $"{currentStars}";
    }
}