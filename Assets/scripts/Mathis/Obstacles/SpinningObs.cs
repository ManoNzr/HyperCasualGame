using UnityEngine;
using UnityEngine.UIElements;

public class SpinningObs : ObstacleScript
{
    protected override void MoveObstacle()
    {
        transform.Rotate(0, 0, -Time.deltaTime * speed);
    }
}
