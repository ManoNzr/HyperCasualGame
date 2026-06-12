using System.Collections;
using UnityEngine;

public class Missile : MonoBehaviour
{
    private BubbleController bubbleController;

    [SerializeField] private ParticleSystem propulsion;
    [SerializeField] private ParticleSystem missileExplosion;
    [SerializeField] private Sprite explosion;
    [SerializeField] private Sprite missile;

    private bool canMove;

    private SpriteRenderer missileRenderer;
    private BubbleController gameManager;

    private void Start()
    {
        bubbleController = FindFirstObjectByType<BubbleController>().GetComponent<BubbleController>();
        missileRenderer = GetComponentInChildren<SpriteRenderer>();
        gameManager = FindFirstObjectByType<BubbleController>().GetComponentInChildren<BubbleController>();
        missileRenderer.sprite = missile;
        propulsion.Play();

        canMove = true;
    }

    private void Update()
    {
        MoveMissile();
    }

    private void MoveMissile()
    {
        if (canMove)
            transform.Translate(Vector3.down / 10, Space.World);

        Destroy(this.gameObject, 6);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "dangerTag" || collision.gameObject.tag == "Bulle")
        {
            propulsion.Stop();
            canMove = false;
            StartCoroutine(PlayMissileExplosion());

            Vector2 vector2 = transform.position;

            Collider2D[] colliders = Physics2D.OverlapCircleAll(vector2, 5);
            foreach (Collider2D collider in colliders)
            {
                if (collider.gameObject.tag == "Bulle")
                {
                    gameManager.PopBubble();
                }
            }
        }
    }

    private IEnumerator PlayMissileExplosion()
    {
        missileRenderer.sprite = explosion;
        missileExplosion.Play();
        yield return new WaitForSecondsRealtime(2);
        Destroy(this.gameObject);
        yield return null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 5);
    }
}
