using UnityEngine;

public class GarbageTruck : MonoBehaviour
{
    [SerializeField] private GameObject player;

    private void Update()
    {
        Vector3 spawnPosition = new Vector3(0, player.transform.position.y - 36f, 0f);
        transform.position = spawnPosition;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Coin")
            Destroy(other.gameObject);
        if (other.gameObject.tag == "dangerTag")
            other.gameObject.SetActive(false);
    }
}
