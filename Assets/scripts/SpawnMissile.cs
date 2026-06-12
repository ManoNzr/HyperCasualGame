using UnityEngine;

public class SpawnMissile : MonoBehaviour
{
    [SerializeField] private GameObject missilePrefab;
    [SerializeField][Range(0f, 1f)] private float boundaryPercentage = 0.8f;

    private float spawnXPosition;
    private BubbleController bubbleController;

    [SerializeField] private GameObject player;
    void Awake()
    {
        bubbleController = Object.FindFirstObjectByType<BubbleController>();

        if (bubbleController == null)
        {
            Debug.LogError("BubbleController introuvable dans la scène !");
        }

        CalculateBoundaryX();
    }

    void Start()
    {
        InvokeRepeating(nameof(SpawnPrefabMissile), 2f, 10f);
    }

    private void CalculateBoundaryX()
    {
        // Converti en World Point, screenBounds.x sera négatif (ex: -8.5).
        Vector3 screenBounds = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0));

        // Mathf.Abs permet d'avoir une valeur positive propre pour travailler nos calculs
        spawnXPosition = Mathf.Abs(screenBounds.x) * boundaryPercentage;
    }

    private void SpawnPrefabMissile()
    {
        if (bubbleController == null) return;

        if (!bubbleController.IsDead && !bubbleController.AsHit)
        {
            float randomX = Random.Range(-spawnXPosition, spawnXPosition);
            Vector3 spawnPosition = new Vector3(randomX, transform.position.y + player.transform.position.y, 0f);

            Instantiate(missilePrefab, spawnPosition, Quaternion.identity);
        }
    }
}