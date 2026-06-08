using Unity.VisualScripting;
using UnityEngine;

public class UImanager : MonoBehaviour
{
    
    

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
        }
        else
        {
            AudioListener.volume = 1;
        }
    }

}
