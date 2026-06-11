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

    private void Start()
    {
        bubbleController = FindFirstObjectByType<BubbleController>().GetComponent<BubbleController>();
        missileRenderer = GetComponentInChildren<SpriteRenderer>();
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

        Destroy(this.gameObject, 5);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "dangerTag" || collision.gameObject.tag == "Bulle")
        {
            propulsion.Stop();
            canMove = false;
            StartCoroutine(PlayMissileExplosion());
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
}
