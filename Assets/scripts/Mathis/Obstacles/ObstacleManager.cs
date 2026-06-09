using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static ObstacleData;

public class ObstacleManager : MonoBehaviour
{
    [Header("Références")]
    [SerializeField] private GameObject player;          // Référence à la bulle
    [SerializeField] private ObstacleData obstacleData; // Le ScriptableObject de configuration
    [SerializeField] private ObstaclePool obsPool;      // Référence au pool d'obstacles

    [Header("Paramètres de Génération")]
    [SerializeField] private float spawnAheadDistance = 15f;    // Distance au-dessus du joueur où spawner
    [SerializeField] private float inbetweenSpacing = 5f; // Distance verticale fixe entre chaque obstacle
    [SerializeField] private bool hasStarted = false;

    private float playerBoundaryX;
    private float nextSpawnY;


    private List<GameObject> activeObstacles = new List<GameObject>();



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

  
        nextSpawnY = player.transform.position.y + 5f;

    }
    private void Update()
    {
        if (!hasStarted) return;

        SpawnHigher();
   

    }

    private void SpawnHigher()
    {
        if(player.transform.position.y +  spawnAheadDistance > nextSpawnY)
        {
            SpawnObstacle();
        }

        nextSpawnY += inbetweenSpacing;
    }

    public void SpawnObstacle()
    {
        ObstacleData.ObstacleType obsType = GetRandomObsType();

        GameObject obstacle = obsPool.GetObstacle(obsType.prefab);


    }


    public void ReturnObstacle(GameObject obstacle, GameObject prefab)
    {


        

        obsPool.ReturnToPool(obstacle, prefab);





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

