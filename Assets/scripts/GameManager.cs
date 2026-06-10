using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    int currentLevel;
    int bestLevel;
    [SerializeField] Transform player;

    [SerializeField] GameObject bubbleBurstedMenu;
    private void Awake()
    {
        if (instance == null) instance = this;
        else { Destroy(gameObject); }
    }
    
    void Start()
    {
        currentLevel = PlayerPrefs.GetInt("level", 0);
        bestLevel = PlayerPrefs.GetInt("bestLevel", 0);
    }

    public void IncreaseCurrentLevel()
    {
        currentLevel++;
        PlayerPrefs.SetInt("level", currentLevel);
    }

    public void BubbleBursted()
    {
        bubbleBurstedMenu.SetActive(true);
    }

    public void ResetGame()
    {
        ResetScore();
        player.position = Vector3.zero;
        bubbleBurstedMenu.SetActive(false);
    }

    public void TryBuyLife()
    {
        if (MoneyManager.instance.SpendCoin(100))
        {
            ContinueGame();
        }
    }
    public void ContinueGame()
    {
        player.gameObject.GetComponent<BubbleController>().Respawn( new Vector3(0, -25, 0)); bubbleBurstedMenu.SetActive(false);
    }

    public void ResetScore()
    {
        if (currentLevel > PlayerPrefs.GetInt("bestLevel", 0))
        {
            PlayerPrefs.SetInt("bestLevel", currentLevel);
        }
        currentLevel = 0;
        PlayerPrefs.SetInt("level", currentLevel);
    }
}
