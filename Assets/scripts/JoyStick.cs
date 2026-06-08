using UnityEngine;

public class JoyStick : MonoBehaviour
{
    [SerializeField] Transform head;
    [SerializeField] float maxHeadDistance;
    float maxHeadDistanceSqr;
    public Vector2 inputWind;

    private void Start()
    {
        maxHeadDistanceSqr = maxHeadDistance * maxHeadDistance;
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
            Vector3 mousePos = Input.mousePosition;
            head.position = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 10f));
           


            // faire que le core suit le head si il est plus loin que maxHeadDistance
            float headCoreDist = (head.position - transform.position).magnitude;
            if (headCoreDist > maxHeadDistance)
            {
                head.position = inputWind;
                transform.position = (transform.position - head.position).normalized * maxHeadDistance;
            }
        }
    }
}
