using UnityEngine;

public class BubbleController : MonoBehaviour
{
    [SerializeField] float dragMultiplier;
    [SerializeField] float maxSpeed;
    [SerializeField] Vector2 velocity;
    Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {
        //Vector2 pointerPos
        BlowThisWay(new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"))); //10 * Time.fixedDeltaTime,0));
        AirDrag();
    }
    void BlowThisWay(Vector2 dir)
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
}
