using UnityEngine;

public class JoyStick : MonoBehaviour
{
    public static JoyStick instance;
    [SerializeField] Transform head;
    [SerializeField] float maxHeadDistance;
    float maxHeadDistanceSqr;
    public Vector2 inputWind;

    private void Awake()
    {
        if (instance == null) instance = this;
        else { Destroy(gameObject); }
    }
    private void Start()
    {
        maxHeadDistanceSqr = maxHeadDistance * maxHeadDistance;
    }

    private void Update()
    {
        GetInput();
    }
    void GetInput()
    {
        /* if (Input.GetMouseButtonDown(0))
         {
             float headCoreDist = (head.position - transform.position).magnitude;
             if (headCoreDist > maxHeadDistance)
             {
                 head.position = inputWind;
                // headCoreDist = (transform.position - head.position).magnitude;
                 transform.position = (transform.position - head.position).normalized * maxHeadDistance;
             }
         }*/

        if (Input.GetMouseButton(0))
        {
            head.position = Input.mousePosition;

            // faire que le core suit le head si il est plus loin que maxHeadDistance
            float headCoreDist = (head.position - transform.position).magnitude;
            if (headCoreDist > maxHeadDistance)
            {
                transform.position = transform.position + (head.position - transform.position).normalized * maxHeadDistance / 2f;
            }
            inputWind = -(head.position - transform.position).normalized;
        }
        else
        {
            inputWind = Vector3.zero;
        }
    }
}
