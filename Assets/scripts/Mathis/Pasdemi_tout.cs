using UnityEngine;

public class Pasdemi_tout : MonoBehaviour
{

    /// <summary>
    /// Ca sert a faire Pas Demi Tout
    /// </summary>
    /// 

    [SerializeField] GameObject bleblut;

    void Start()
    {
        
    }

    void Update()
    {
        transform.position += Vector3.up * Time.deltaTime * 5;

        transform.position = new Vector3(transform.position.x, Mathf.Max(transform.position.y, bleblut.transform.position.y - 35), transform.position.z);

        if(transform.position.y >= bleblut.transform.position.y)
        {
            transform.position = new Vector3 (transform.position.x, bleblut.transform.position.y - 35, transform.position.z);
        }
    }
}
