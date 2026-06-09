using UnityEngine;

public class JoyStick : MonoBehaviour
{
    //  JAHMI JAHMI JAHMI JAHMI JAHMI JAHMI JAHMI JAHMI JAHMI JAHMI JAHMI JAHMI JAHMI JAHMI JAHMI JAHMI
    public static JoyStick instance;
    [SerializeField] Transform head;
    [SerializeField] float maxHeadDistance;
    public Vector2 inputWind;

    private void Awake()
    {
        if (instance == null) instance = this;
        else { Destroy(gameObject); }
    }

    private void Update()
    {
        GetInput();
    }
    void GetInput()
    {
        // si on touche l'ecran
        if (Input.GetMouseButton(0))
        {
            // la head vas toujours là ou on touche
            head.position = Input.mousePosition;

            // faire que le core suit le head si il est plus loin que maxHeadDistance
            float headCoreDist = (head.position - transform.position).magnitude;
            if (headCoreDist > maxHeadDistance)
            {
                transform.position = head.position + (transform.position - head.position).normalized * maxHeadDistance;
            }
            inputWind = -(head.position - transform.position).normalized;
        }
        else // si on touche rien on fait rien
        {
            inputWind = Vector2.zero;
        }
    }
}
