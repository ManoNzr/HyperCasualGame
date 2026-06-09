using UnityEngine;
using Unity.Collections;
using System.Collections; // Nécessaire pour les Coroutines

public class switchBetweenSprites : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite[] sprites;
    private int currentSpriteIndex = 0;
    [SerializeField] private float switchInterval = 0.5f;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (sprites.Length > 0)
        {
            spriteRenderer.sprite = sprites[0];
            StartCoroutine(SwitchSprites());
        }
    }

    void Update()
    {
    }

    IEnumerator SwitchSprites()
    {
        while (true)
        {
            if (sprites.Length == 0) yield break;

            currentSpriteIndex = (currentSpriteIndex + 1) % sprites.Length;
            spriteRenderer.sprite = sprites[currentSpriteIndex];

            yield return new WaitForSeconds(switchInterval);
        }
    }
}
