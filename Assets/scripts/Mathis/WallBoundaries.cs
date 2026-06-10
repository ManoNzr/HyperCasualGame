using Unity.VisualScripting;
using UnityEngine;

public class WallBoundaries : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject leftWall;
    [SerializeField] private GameObject rightWall;
    private float playerBoundaryX;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        var bubbleBoundary = player.GetComponent<BubbleBoundary>();
        if (bubbleBoundary != null)
        {
            playerBoundaryX = bubbleBoundary.BoundaryX;
        }
        else
        {
            playerBoundaryX = 6f; // Valeur par défaut
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        leftWall.transform.position = new Vector3(playerBoundaryX - 2, player.transform.position.y, 0);
        rightWall.transform.position = new Vector3(-playerBoundaryX + 2, player.transform.position.y, 0);
    }
}
