using UnityEngine;

public abstract class ObstacleScript : MonoBehaviour
{
    [SerializeField] protected float speed;
    protected string dir;

    private void Awake()
    {
        speed = Random.Range(30, 60);
    }

    protected abstract void MoveObstacle();

    private void Update()
    {
        MoveObstacle();
    }
}
