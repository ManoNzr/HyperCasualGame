using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static ObstacleData;

public class ObstacleManager : MonoBehaviour
{
    [SerializeField] private GameObject player;
    private float playerBoundaryX;
    private float playerBoundaryY;

    [SerializeField] ObstacleData enemyData;

    [SerializeField] private float spawnAheadDist;

    [SerializeField] private bool hasStarted = false;


    public ObstaclePool obsPool;
    public int rows = 5; // Nb de rangées
    public int columns = 11; // Nb de colonnes
    public float spacing = 1.5f; // Espacement entre les obstacle et la boundaryX


    [SerializeField] private Vector2 startPosition = new Vector2(-7.5f, 6f);


    private GameObject[,] obstacles;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        playerBoundaryY = player.GetComponent<BubbleBoundary>().BoundaryY;
        obstacles = new GameObject[rows, columns];

        





    }
    private void Update()
    {
        if (hasStarted)
        {
            SpawnObstacles();
        }

    }



    public void SpawnObstacles()
    {
        var obsTypes = obsPool.GetObstacleType();

        for(int i = 0; i <= obsTypes.Count; i++)
        {
            var obsType = obsTypes[i];
        }
        /*
        for (int row = 0; row < rows; row++)
        {
            var obsType = GetObstacleTypeForRow(row, obsTypes);

            for (int col = 0; col < columns; col++)
            {
                GameObject obstacle = obsPool.GetObstacle(obsType.prefab);

                if (obstacle != null)
                {
                    float xPos = startPosition.x + (col * spacing);
                    float yPos = startPosition.y - (row * spacing);


                    obstacle.transform.position = new Vector3(xPos, yPos, 0);

                    
                    if (enemyScript != null)
                    {
                        enemyScript.EnemyType = obsType;

                    }
                    obstacles[row, col] = obstacle;

                }
            }

        }*/


    }


    private List<GameObject> GetBottomObstacles()
    {
        List<GameObject> bottomobstacles = new List<GameObject>();

        for (int col = 0; col < columns; col++)
        {
            for (int row = rows - 1; row >= 0; row--)
            {
                if (obstacles[row, col] != null && obstacles[row, col].activeSelf)
                {
                    bottomobstacles.Add(obstacles[row, col]);
                    break;
                }
            }
        }



        return bottomobstacles;
    }



    public void ReturnObstacle(GameObject obstacle, GameObject prefab)
    {
        for (int row = 0; row < rows; row++)
        {

            for (int col = 0; col < columns; col++)
            {

                if (obstacles[row, col] == obstacle)
                {
                    obstacles[row, col] = null;
                }

            }


        }

        

        obsPool.ReturnToPool(obstacle, prefab);





    }





    /*private bool ReachedBoundary(GameObject obstacle)
    {
        float xPos = obstacle.transform.position.x;

        if (currentState == MoveState.MoveRight && xPos >= playerBoundaryX)
        {

            return true;
        }
        if (currentState == MoveState.MoveLeft && xPos <= -playerBoundaryX)
        {

            return true;
        }

        return false;

    }
    */

    private ObstacleData.ObstacleType GetRandomObsType(GameObject obstacle)
    {
        float rand = UnityEngine.Random.Range(0f, 1f);
        if (rand == obstacle.GetComponent<ObstacleData.ObstacleType>().spawnRate) return obstacle.GetComponent<ObstacleData.ObstacleType>();
        return null;

    }
}
