using UnityEngine;
using UnityEngine.AI;

public abstract class ObstacleScript : MonoBehaviour
{
    [SerializeField] protected float speed;
    protected string dir;


    public void Setup(string direction, float vitesseComportement)
    {
        this.dir = direction;
        this.speed = vitesseComportement;
    }

    protected abstract void MoveObstacle();

    private void Update()
    {
        MoveObstacle();
    }
}
