using UnityEngine;
using TMPro;
using System.Collections.Generic;
public class MoneyManager : MonoBehaviour
{
    public static MoneyManager instance;
    [SerializeField] TMP_Text text;
    int money;
    [SerializeField] GameObject coinPrefab;
    List<Transform> activeCoins = new List<Transform>();
    List<Transform> inactiveCoins = new List<Transform>();
    [SerializeField] int coinsInLevel;
    float collectRangeSqr;

    [SerializeField] Transform player;
    private void Awake()
    {
        if (instance == null) instance = this;
        else { Destroy(gameObject); }
    }
    private void Start()
    {
        money = PlayerPrefs.GetInt("money", 0);
        text.text = money.ToString();
        CreateCoins();
        collectRangeSqr = transform.localScale.x * transform.localScale.x * 20f;
    }

    private void FixedUpdate()
    {
        FindCollectableCoins();
    }

    void FindCollectableCoins()
    {
        for (int i = 0; i < activeCoins.Count; i++)
        {
            if ((player.position - activeCoins[i].position).sqrMagnitude < collectRangeSqr)
            {
                Transform coin = activeCoins[i];
                activeCoins.RemoveAt(i);
                inactiveCoins.Add(coin);
                coin.gameObject.SetActive(false);
                AddCoin();
            }
        }
    }
    void CreateCoins()
    {
        for (int i = 0;i < coinsInLevel;i++)
        {
            activeCoins.Add(Instantiate(coinPrefab, transform.position + new Vector3(0, i, 0), Quaternion.identity).transform);
        }
    }
    public void AddCoin()
    {
        money++;
        PlayerPrefs.SetInt("money", money);
        PlayerPrefs.Save();
        text.text = money.ToString();
    }
    public bool SpendCoin(int amount)
    {
        if (money >= amount)
        {
            money -= amount;
            PlayerPrefs.SetInt("money", money);
            PlayerPrefs.Save();
            text.text = money.ToString();
            return true;
        }
        return false;
    }
}
