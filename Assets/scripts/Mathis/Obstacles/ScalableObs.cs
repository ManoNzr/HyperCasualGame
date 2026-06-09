using UnityEngine;

public class ScalableObs : ObstacleScript
{
    protected override void MoveObstacle()
    {
        if (dir.Contains("hor"))
        {
            transform.localScale = new Vector3(Mathf.PingPong(Time.time * Mathf.Clamp(speed, 0, 5), 5),3,3);

        }
        else if(dir.Contains("vert")) transform.localScale = new Vector3(3, Mathf.PingPong(Time.time * Mathf.Clamp(speed,0,5), 5), 3);
        return;
    }
}
