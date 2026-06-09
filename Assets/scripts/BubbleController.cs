using UnityEngine;
public class BubbleController : MonoBehaviour
{
    [Header("Physics")]
    [SerializeField] float dragMultiplier;
    [SerializeField] float maxSpeed;
    [SerializeField] float windForce;
    Vector2 velocity;
    Rigidbody2D rb;
    
    Vector2 inputWorldPos;

    [Header("Layers")]
    [SerializeField] LayerMask dangerLayer;
    [SerializeField] LayerMask bounceLayer;

    [Header("Visuals")]
    [SerializeField] GameObject popticles;
    [SerializeField] GameObject windBlow;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        GetInput();
        if (JoyStick.instance.inputWind == Vector2.zero)
        {
            windBlow.SetActive(false);
        }
    }
    private void FixedUpdate()
    {
        BlowThisWay(JoyStick.instance.inputWind * windForce);
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
        
        windBlow.SetActive(true);
        windBlow.transform.right = dir;
    }


    void BounceThisWay(Vector2 dir)
    {
        velocity += dir;
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

    void PopBubble()
    {
        popticles.SetActive(true);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == dangerLayer)
        {
            PopBubble();
        }
        else if (collision.gameObject.layer == bounceLayer)
        {
            BounceThisWay(collision.contacts[0].point - new Vector2(transform.position.x, transform.position.y));
        }
    }
}
