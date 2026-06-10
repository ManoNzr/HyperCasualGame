using UnityEngine;

public class ScalableObs : ObstacleScript
{
    [SerializeField] private GameObject pivot;
    protected override void MoveObstacle()
    {
        if (dir.Contains("hor"))
        {
            pivot.transform.localScale = new Vector3(Mathf.PingPong(Time.time * Mathf.Clamp(speed*0.05f, 0, 1), 2),1,1);
            Debug.Log(Mathf.Clamp(speed * 0.05f, 0, 1));

        }
        else if(dir.Contains("vert")) pivot.transform.localScale = new Vector3(3, Mathf.PingPong(Time.time * Mathf.Clamp(speed,0,5), 5), 3);
        return;
    }
}
