using UnityEngine;
public class BubbleController : MonoBehaviour
{
    // JAHMI JAHMI JAHMI JAHMI JAHMI JAHMI JAHMI JAHMI JAHMI JAHMI JAHMI JAHMI JAHMI JAHMI JAHMI JAHMI JAHMI JAHMI
    bool firstInput; // savoir si c'est la première fois qu'on touche l'écran
    [Header("Physics")]
    [SerializeField] float dragMultiplier;
    [SerializeField] float maxSpeed;
    [SerializeField] float windForce;
    Vector2 velocity;
    Rigidbody2D rb;

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
        // si il y aucun input
        if (JoyStick.instance.inputWind == Vector2.zero)
        {
            // on désactive l'effet visuel
            windBlow.SetActive(false);       
        }
        else
        {
            // enlever le menu principal 
            if (!firstInput)
            {
                UImanager.Instance.StartGame();
                firstInput = true;
            }
        }

        // suive la bulle avec la caméra
        camera.transform.position = new Vector3(0,transform.position.y,-10f);
    }
    private void FixedUpdate()
    {
        BlowThisWay(JoyStick.instance.inputWind * windForce);
        AirDrag();
    }

    // crée une force de vent dans cette direction
    void BlowThisWay(Vector2 dir)
    {
        // on ajoute une force
        velocity += dir * Time.fixedDeltaTime;

        // si on est trop rapide on clamp
        if (velocity.magnitude > maxSpeed)
        {
            velocity = velocity.normalized * maxSpeed;
        }

        // on applique la velocity
        rb.linearVelocity = velocity;
        
        // on oriente l'effet visuel
        windBlow.SetActive(true);
        windBlow.transform.right = dir;
    }

    // fait rebondir la bulle selon la direction actuelle et la normale de la surface qu'on touche
    void BounceThisWay(Vector2 dir, Vector2 normal)
    {
        // on trouve la directipon du rebond
        dir = dir - (2 * Vector2.Dot(dir, normal) * normal);
        
        velocity = dir * velocity.magnitude;

        if (velocity.magnitude > maxSpeed)
        {
            velocity = velocity.normalized * maxSpeed;
        }

        rb.linearVelocity = velocity;
    }

    // simule le frottement de l'air (même dans l'espace)
    void AirDrag()
    {
        velocity *= dragMultiplier;
    }        

    // défaite
    void PopBubble()
    {
        popticles.SetActive(true);
        Debug.Log("POP");
    }

    // on guètte les collisions
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
