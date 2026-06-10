using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using Unity.AppUI.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] int currentLevel;
    [SerializeField] int bestLevel;
    [SerializeField] Transform player;

    [SerializeField] GameObject bubbleBurstedMenu;

    //[SerializeField] TextMeshProUGUI levelUI;
    [SerializeField] TMP_Text levelText;

    private void Awake()
    {
        if (instance == null) instance = this;
        else { Destroy(gameObject); }
    }
    
    void Start()
    {
        currentLevel = PlayerPrefs.GetInt("level", 0);
        bestLevel = PlayerPrefs.GetInt("bestLevel", 0);
        UpdateLevelUI();
    }

    public void IncreaseCurrentLevel()
    {
        currentLevel++;
        PlayerPrefs.SetInt("level", currentLevel);
        PlayerPrefs.Save();
        UpdateLevelUI();
        Debug.Log(currentLevel);
    }

    public void BubbleBursted()
    {
        bubbleBurstedMenu.SetActive(true);
    }

    private void UpdateLevelUI()
    {
        levelText.text = currentLevel.ToString();
    }

    public void ResetGame()
    {
        ResetScore();
        player.position = Vector3.zero;
        ObstacleManager.Instance.NextSpawnY = player.position.y + 35f;
        ObstacleManager.Instance.ClearObstacles();
        bubbleBurstedMenu.SetActive(false);
        UImanager.instance.setPause();
        
    }

    public void TryBuyLife()
    {
        if (MoneyManager.instance.SpendCoin(100))
        {
            ContinueGame();
        }
    }

    public void GoToNextLevel()
    {
        player.gameObject.GetComponent<BubbleController>().Respawn(new Vector3(0, -5, 0));
    }

    public void ContinueGame()
    {
        player.gameObject.GetComponent<BubbleController>().Respawn(transform.position + new Vector3(0, -10, 0)); 
        bubbleBurstedMenu.SetActive(false);
        UImanager.instance.setPause();

    }

    public void ResetScore()
    {
        if (currentLevel > PlayerPrefs.GetInt("bestLevel", 0))
        {
            PlayerPrefs.SetInt("bestLevel", currentLevel);
        }
        currentLevel = 0;
        PlayerPrefs.SetInt("level", currentLevel);
        PlayerPrefs.Save();
        UpdateLevelUI();
    }
}
