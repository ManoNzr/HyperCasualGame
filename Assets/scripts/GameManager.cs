using System.Collections;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //  JAHMI JAHMI JAHMI JAHMI JAHMI JAHMI JAHMI JAHMI JAHMI JAHMI JAHMI JAHMI JAHMI JAHMI JAHMI JAHMI JAHMI JAHMI
    public static GameManager instance;
    [SerializeField] int currentLevel;
    [SerializeField] int bestLevel;
    [SerializeField] Transform player;

    [SerializeField] GameObject bubbleBurstedMenu;

    [SerializeField] TMP_Text levelText;
    [SerializeField] TMP_Text bestLevelText;
    [SerializeField] TMP_Text warningText;
    private BubbleController bubbleController;

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
        UpdateBestLevelUI();
        bubbleController = FindFirstObjectByType<BubbleController>().GetComponent<BubbleController>();
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

    public void UpdateLevelUI()
    {
        levelText.text = currentLevel.ToString();
    }

    public void UpdateBestLevelUI()
    {
        bestLevelText.text = "BEST " + bestLevel.ToString();
    }

    public void ResetGame()
    {
        ResetScore();
        player.position = Vector3.zero;
        ObstacleManager.Instance.NextSpawnY = player.position.y + 35f;
        ObstacleManager.Instance.ClearObstacles();
        MoneyManager.instance.ClearCoinsInLevel();
        bubbleBurstedMenu.SetActive(false);
        UImanager.instance.setPause();
        UpdateLevelUI();
        UpdateBestLevelUI();
        bubbleController.AsHit = false;
    }

    public void TryBuyLife()
    {
        if (MoneyManager.instance.SpendCoin(100))
        {
            ContinueGame();
        }
        else
        {
            StartCoroutine(WarningMessage());
        }
    }

    private IEnumerator WarningMessage()
    {
        warningText.gameObject.SetActive(true);
        warningText.text = "Not Enough Money\n" + "Press Restart";
        yield return new WaitForSecondsRealtime(2);
        warningText.text = null;
        warningText.gameObject.SetActive(false);
        yield return null;
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
        UpdateLevelUI();
        UpdateBestLevelUI();

    }

    public void ResetScore()
    {
        if (currentLevel > PlayerPrefs.GetInt("bestLevel", 0))
        {
            PlayerPrefs.SetInt("bestLevel", currentLevel);
            UpdateBestLevelUI();
        }
        currentLevel = 0;
        PlayerPrefs.SetInt("level", currentLevel);
        PlayerPrefs.Save();
        UpdateLevelUI();
        UpdateBestLevelUI();
    }
}
