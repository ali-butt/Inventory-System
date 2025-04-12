[System.Serializable]
public class AnimalInstance
{
    public string id;
    public string species;
    public string rarity;
    public int age;
    public string health;
    public string specialAbility;

    public AnimalInstance(AnimalDataSO data)
    {
        id = data.id;
        species = data.species;
        rarity = data.rarity;
        age = data.age;
        health = data.health;
        specialAbility = data.specialAbility;
    }
}