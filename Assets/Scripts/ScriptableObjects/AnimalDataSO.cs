using UnityEngine;

[CreateAssetMenu(fileName = "AnimalData", menuName = "Animal/Animal Data")]
public class AnimalDataSO : ScriptableObject
{
    public string id;
    public string species;
    public string rarity;
    public int age;
    public string health;
    public string specialAbility;
    public int cost;
    public Sprite icon; // Image used in UI
}