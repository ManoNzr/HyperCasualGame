using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static ObstacleData;

public class EnemyManager : MonoBehaviour
{
    [SerializeField]
    private GameObject player;
    private float playerBoundaryX;

    [SerializeField] ObstacleData enemyData;


    public ObstaclePool obsPool;
    public int rows = 5; // Nb de rangées
    public int columns = 11; // Nb de colonnes
    public float spacing = 1.5f; // Espacement entre les ennemies
    public float _stepDistance = 0.5f; // Distance de déplacement par frame
    public float _stepDistanceVertical = 1f; // Distance de déplacement vertical par frame

    public Vector2 startPosition = new Vector2(-7.5f, 6f);


    private GameObject[,] obstacles;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        //playerBoundaryX = player.GetComponent<BubbleController>().boundary;
        obstacles = new GameObject[rows, columns];

        SpawnObstacles();




    }
    private void Update()
    {


    }



    public void SpawnObstacles()
    {
        var obsTypes = obsPool.GetObstacleType();



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

                    //   Debug.Log($"[EnemyManager] {enemy.name} est à la position X : {xPos}; Y : {yPos}");

                    obstacle.transform.position = new Vector3(xPos, yPos, 0);

                    /*
                    if (enemyScript != null)
                    {
                        enemyScript.EnemyType = obsType;
                        enemyScript.ScoreData = obsType.points;
                    }*/
                    obstacles[row, col] = obstacle;

                }
            }

        }


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

    private ObstacleData.ObstacleType GetObstacleTypeForRow(int row, List<ObstacleData.ObstacleType> obsTypes)
    {
        if (row == 0) // 1er ligne : Type C
        {
            return obsTypes[2];
        }
        else if (row <= 2) // 2e et 3e lignes : Type B
        {
            return obsTypes[1];

        }
        else // 4e et 5e lignes : Type A
        {
            return obsTypes[0];
        }
    }
}
