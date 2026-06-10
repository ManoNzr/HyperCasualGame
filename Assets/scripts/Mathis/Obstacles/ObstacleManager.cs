using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static ObstacleData;
using static UnityEngine.GraphicsBuffer;

public class ObstacleManager : MonoBehaviour
{
    [Header("Références")]
    [SerializeField] MoneyManager moneyManager;
    [SerializeField] private GameObject player;          // Référence à la bulle
    private BubbleController bubbleController;
    [SerializeField] private ObstacleData obstacleData; // Le ScriptableObject de configuration
    [SerializeField] private ObstaclePool obsPool;      // Référence au pool d'obstacles


    [Header("Paramètres de Génération")]
    [SerializeField] private float spawnAheadDistance = 15f;    // Distance au-dessus du joueur où spawner
    [SerializeField] private float inbetweenSpacing = 5f; // Distance verticale fixe entre chaque obstacle
   // [SerializeField] private bool hasStarted = false;
    [SerializeField] private bool mustGenerate = true;
    [SerializeField]private int returnedObstacles;
    [SerializeField] private int limit = 30;

    private float playerBoundaryX;
    private float nextSpawnY;


    private List<GameObject> activeObstacles = new List<GameObject>();



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bubbleController = player.GetComponent<BubbleController>();
        var bubbleBoundary = player.GetComponent<BubbleBoundary>();
        if (bubbleBoundary != null)
        {
            playerBoundaryX = bubbleBoundary.BoundaryX;
        }
        else
        {
            playerBoundaryX = 6f; // Valeur par défaut
        }

        limit = 10;
        nextSpawnY = player.transform.position.y + 35f;
        mustGenerate = true;

    }
    private void Update()
    {
        if (!bubbleController.FirstInput) return;

        SpawnHigher();
        ReturnObstacleToPool();

    }

    private void SpawnHigher()
    {
        if(player.transform.position.y +  spawnAheadDistance >= nextSpawnY && mustGenerate)
        {
            SpawnObstacle(nextSpawnY);
            nextSpawnY += inbetweenSpacing;
        }

    }

    public void SpawnObstacle(float targetY)
    {
        ObstacleData.ObstacleType obsType = GetRandomObsType();

        GameObject obstacle = obsPool.GetObstacle(obsType.prefab);

        if(obstacle != null)
        {
            // Calcul de la position horizontale en fonction de 'isCentered'
            float spawnX = 0f;
            if (!obsType.isCentered)
            {
                if(obsType.spawnSide <=0) spawnX = -playerBoundaryX;
                spawnX = playerBoundaryX;
                Debug.Log(spawnX);
            }

            obstacle.transform.position = new Vector3(spawnX, targetY, 0f);
            moneyManager.CreateACoin(new Vector3(UnityEngine.Random.Range(-playerBoundaryX, playerBoundaryX), targetY + UnityEngine.Random.Range(5,15), 0f));

            ObstacleScript obstacleScript = obstacle.GetComponent<ObstacleScript>();
            if (obstacleScript != null) obstacleScript.Setup(obsType.dir,UnityEngine.Random.Range(30,60));

            activeObstacles.Add(obstacle);

        }

    }


    private void ReturnObstacleToPool()
    {
        // Seuil en dessous duquel un obstacle est considéré comme invisible et dépassé
        float threshold = player.transform.position.y - 60f;


        for (int i = activeObstacles.Count - 1; i >= 0; i--)
        {
            GameObject obs = activeObstacles[i];

            if (obs != null)
            {
                // Si l'obstacle est descendu trop bas par rapport à la bulle, on le recycle
                if (obs.transform.position.y < threshold)
                {
                    activeObstacles.RemoveAt(i);

                    // Retrouver le prefab d'origine pour appeler ton ReturnToPool
                    GameObject originalPrefab = InstanceToPrefab(obs.name);
                    if (originalPrefab != null)
                    {
                        obsPool.ReturnToPool(obs, originalPrefab);
                    }
                    else
                    {
                        obs.SetActive(false); // Sécurité si le prefab n'est pas retrouvé
                    }
                    returnedObstacles++;
                }
            }
        }
        if(returnedObstacles > limit) mustGenerate = false;
    }

    private void NextLevel()
    {
        if (!mustGenerate && returnedObstacles > limit)
        {
            ClearObstacles();   
            GameManager.instance.ResetGame();
            mustGenerate = true;
            returnedObstacles = 0;
            
        }
    }

    public void ClearObstacles()
    {
        for (int i = activeObstacles.Count - 1; i >= 0; i--)
        {
            GameObject obs = activeObstacles[i];
            if (obs != null)
            {
                GameObject originalPrefab = InstanceToPrefab(obs.name);
                if (originalPrefab != null)
                {
                    obsPool.ReturnToPool(obs, originalPrefab);
                }
                else
                {
                    obs.SetActive(false);
                }
            }
        }
        activeObstacles.Clear();
    }


    private ObstacleData.ObstacleType GetRandomObsType()
    {
        var obsTypes = obstacleData.obsTypes;
        if (obsTypes == null || obsTypes.Count == 0) return null;

        float totalRate = 0f;
        foreach(var obsType in obsTypes)
        {
            totalRate += obsType.spawnRate;
        }

        float rand = UnityEngine.Random.Range(0, totalRate);


        float currentRate = 0f;
        foreach (var obsType in obsTypes)
        {
            currentRate += obsType.spawnRate;
            if (rand <= currentRate)
            {
                return obsType;
            }
        }

        return obsTypes[0]; 
    }

    private GameObject InstanceToPrefab(string instanceName)
    {
        if (obstacleData == null || obstacleData.obsTypes == null) return null;

        foreach (var type in obstacleData.obsTypes)
        {
            if (type.prefab != null && type.prefab.name == instanceName)
            {
                return type.prefab;
            }
        }
        return null;
    }

}

