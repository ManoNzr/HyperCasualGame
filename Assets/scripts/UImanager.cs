using Unity.VisualScripting;
using UnityEngine;

public class UImanager : MonoBehaviour
{
    
    [SerializeField] private GameObject volumeButton;

    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setSounds()
    {
        if (AudioListener.volume == 1)
        {
            AudioListener.volume = 0;
            volumeButton.GetComponent<Spriterender>().fillamount = 0;
        }
        else
        {
            AudioListener.volume = 1;
        }
    }

}
