using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UImanager : MonoBehaviour
{
    
    [SerializeField] private GameObject volumeButton;

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

}
