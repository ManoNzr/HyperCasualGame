using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    public static MoneyManager instance;
    int money;
    private void Awake()
    {
        if (instance == null) instance = this;
        else { Destroy(gameObject); }
    }
    private void Start()
    {
        money = PlayerPrefs.GetInt("money", 0);
    }
    public void AddCoin()
    {
        money++;
        PlayerPrefs.SetInt("money", money);
        PlayerPrefs.Save();
    }
    public bool SpendCoin(int amount)
    {
        if (money >= amount)
        {
            money -= amount;
            PlayerPrefs.SetInt("money", money);
            PlayerPrefs.Save();
            return true;
        }
        return false;
    }
}
