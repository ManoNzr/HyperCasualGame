using UnityEngine;
public class BubbleController : MonoBehaviour
{
    bool firstInput;
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
    [SerializeField] LayerMask moneyLayer;

    [Header("Visuals")]
    [SerializeField] GameObject popticles;
    [SerializeField] GameObject windBlow;

    [Header("Camera")]
    [SerializeField] GameObject camera;
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
            if (!firstInput)
            {
                UImanager.Instance.StartGame();
                firstInput = true;
            }
        }
        camera.transform.position = new Vector3(0,transform.position.y,-10f);
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


    void BounceThisWay(Vector2 dir, Vector2 normal)
    {

        dir = dir - (2 * Vector2.Dot(dir, normal) * normal);
        
        velocity = dir * velocity.magnitude;
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
        Debug.Log("POP");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "dangerTag")
        {
            PopBubble();
        }
        else if (collision.gameObject.tag == "bounceTag")
        {
            BounceThisWay(velocity.normalized,(new Vector2(transform.position.x, transform.position.y) - collision.contacts[0].point).normalized);
        }
    }
}
