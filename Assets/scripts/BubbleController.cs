using UnityEngine;

public class BubbleController : MonoBehaviour
{
    [SerializeField] float dragMultiplier;
    [SerializeField] float maxSpeed;
    [SerializeField] Vector2 velocity;
    Rigidbody2D rb;

    [SerializeField] Vector2 inputWorldPos;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        GetInput();
    }
    private void FixedUpdate()
    {
        BlowThisWay(-(inputWorldPos - new Vector2(transform.position.x,transform.position.y)).normalized * 3f);
        AirDrag();
    }
    void BlowThisWay(Vector2 dir)
    {
        velocity += dir * Time.fixedDeltaTime;
        if (velocity.magnitude > maxSpeed)
        {
            velocity = velocity.normalized * maxSpeed;
        }
        rb.linearVelocity = velocity;
    }

    void AirDrag()
    {
        velocity *= dragMultiplier;
    }    

    void GetInput()
    {
        if (Input.GetMouseButton(0))
        {            
            Vector3 mousePos = Input.mousePosition;
            inputWorldPos = Camera.main.ScreenToWorldPoint(new Vector3 (mousePos.x, mousePos.y, 10f));
        }
    }
}
