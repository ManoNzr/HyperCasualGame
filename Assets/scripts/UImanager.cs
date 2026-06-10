using UnityEngine;
using UnityEngine.UI;
using System.Collections; // Nécessaire pour les Coroutines

public class UImanager : MonoBehaviour
{
    [SerializeField] private GameObject volumeButton;
    [SerializeField] private GameObject MainMenuPanel;
    
    [SerializeField] private GameObject deathPanel;

    [SerializeField] private GameObject TapToStartLabel;
    [SerializeField] private float animationDuration = 0.5f; 
    [SerializeField] private float slideDistance = 1000f;

    public static UImanager Instance { get; private set; }

    private Vector2 initialpos = Vector2.zero;
    private RectTransform mainMenuRect;
    private CanvasGroup labelCanvasGroup;

    private void Start()
    {

        if (Instance != null)
        {
            Destroy(gameObject);
        }
        Instance = this;

        moveDeathPanelOffScreen();

        mainMenuRect = MainMenuPanel.GetComponent<RectTransform>();
        initialpos = mainMenuRect.anchoredPosition;

        labelCanvasGroup = TapToStartLabel.GetComponent<CanvasGroup>();
        if (labelCanvasGroup == null)
        {
            labelCanvasGroup = TapToStartLabel.AddComponent<CanvasGroup>();
        }
        //StartGame();
    }

    public void setPause()
    {
        if (Time.timeScale == 1)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    public void setActive(GameObject obj, bool active)
    {
        obj.SetActive(active);
    }

    public void setSounds()
    {
        if (AudioListener.volume == 1)
        {
            AudioListener.volume = 0;
            volumeButton.GetComponent<Image>().fillAmount = 0.5f;
        }
        else
        {
            AudioListener.volume = 1;
            volumeButton.GetComponent<Image>().fillAmount = 1;
        }
    }
    public void StartGame()
    {
        StartCoroutine(StartGameRoutine());
    }

    public void goBackToMainMenu()
    {
        StartCoroutine(BackToMenuRoutine());
    }
    private IEnumerator StartGameRoutine()
    {
        float elapsedTime = 0f;

        while (elapsedTime < animationDuration)
        {
            labelCanvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsedTime / animationDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        labelCanvasGroup.alpha = 0f;

        elapsedTime = 0f;
        Vector2 targetPos = initialpos + new Vector2(-slideDistance, 0);
        
        while (elapsedTime < animationDuration)
        {
            mainMenuRect.anchoredPosition = Vector2.Lerp(initialpos, targetPos, elapsedTime / animationDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        mainMenuRect.anchoredPosition = targetPos;
        setActive(MainMenuPanel, false);
    }
    private IEnumerator BackToMenuRoutine()
    {
        setActive(MainMenuPanel, true);

        float elapsedTime = 0f;
        Vector2 currentPos = mainMenuRect.anchoredPosition;

        while (elapsedTime < animationDuration)
        {
            mainMenuRect.anchoredPosition = Vector2.Lerp(currentPos, initialpos, elapsedTime / animationDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        mainMenuRect.anchoredPosition = initialpos;

        elapsedTime = 0f;
        while (elapsedTime < animationDuration)
        {
            labelCanvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsedTime / animationDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        labelCanvasGroup.alpha = 1f;
    }


    // créer une fonction qui appel une coroutine pour afficher le menu quand le joueur est mort. (viens de la droite et vas vers la droite pour quitter le panel)

    public void ShowDeathPanel()
    {
        StartCoroutine(ShowDeathPanelRoutine());
    }

    public void HideDeathPanel()
    {
        StartCoroutine(HideDeathPanelRoutine());
    }

    private IEnumerator ShowDeathPanelRoutine()
    {
        deathPanel.SetActive(true);
        RectTransform deathPanelRect = deathPanel.GetComponent<RectTransform>();
        Vector2 startPos = new Vector2(Screen.width + slideDistance, 0);
        Vector2 endPos = new Vector2(Screen.width / 2, 0);
        float elapsedTime = 0f;

        while (elapsedTime < animationDuration)
        {
            deathPanelRect.anchoredPosition = Vector2.Lerp(startPos, endPos, elapsedTime / animationDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        deathPanelRect.anchoredPosition = endPos;
    }

    private IEnumerator HideDeathPanelRoutine()
    {
        RectTransform deathPanelRect = deathPanel.GetComponent<RectTransform>();
        Vector2 startPos = deathPanelRect.anchoredPosition;
        Vector2 endPos = new Vector2(Screen.width + slideDistance, 0);
        float elapsedTime = 0f;

        while (elapsedTime < animationDuration)
        {
            deathPanelRect.anchoredPosition = Vector2.Lerp(startPos, endPos, elapsedTime / animationDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        deathPanelRect.anchoredPosition = endPos;
        deathPanel.SetActive(false);
    }

    private void moveDeathPanelOffScreen()
    {
        RectTransform deathPanelRect = deathPanel.GetComponent<RectTransform>();
        deathPanelRect.anchoredPosition = new Vector2(Screen.width + slideDistance, 0);
    }

}