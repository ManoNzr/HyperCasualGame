using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObstaclePool : MonoBehaviour
{
    [SerializeField]
    public ObstacleData obsData;


    private Dictionary<GameObject, Queue<GameObject>> poolDictionary = new Dictionary<GameObject, Queue<GameObject>>();




    void Start()
    {
        // Initialiser un pool pour chaque type d'ennemi avec des quantités spécifiques
        for (int i = 0; i < obsData.obsTypes.Count; i++)
        {
            var obsType = obsData.obsTypes[i];


            if (poolDictionary.ContainsKey(obsType.prefab))
            {
                Debug.LogWarning($"Le prefab {obsType.prefab.name} pour le type {obsType.name} est déjà dans le pool");
                continue;
            }

            int poolSize = GetPoolSizeForObstacleType(i);

            Queue<GameObject> obsQueue = new Queue<GameObject>();

            for (int j = 0; j < poolSize; j++)
            {
                GameObject obstacle = Instantiate(obsType.prefab);
                obstacle.transform.name = obsType.prefab.name;
                obstacle.SetActive(false);
                obsQueue.Enqueue(obstacle);
            }

            poolDictionary.Add(obsType.prefab, obsQueue);
        }
    }

    public GameObject GetObstacle(GameObject prefab)
    {
        if (poolDictionary.TryGetValue(prefab, out Queue<GameObject> obsQueue) && obsQueue.Count > 0)
        {
            GameObject obstacle = obsQueue.Dequeue();
            obstacle.SetActive(true);

            return obstacle;
        }

        return null;
    }

    public void ReturnToPool(GameObject obstacle, GameObject prefab)
    {
        // TODO : A debugguer
        obstacle.SetActive(false);

        if (poolDictionary.TryGetValue(prefab, out var obsQueue))
        {
            obsQueue.Enqueue(obstacle);
        }
        else
        {
            Debug.Log("Tentative de retourner un obstacle à un pool inexistant !");
        }
    }

    public List<ObstacleData.ObstacleType> GetObstacleType()
    {
        return obsData.obsTypes;
    }

    private int GetPoolSizeForObstacleType(int index)
    {
        switch (index)
        {
            case 0: // Type A
                return 22;
            case 1: // Type B
                return 22;
            case 2: // Type C
                return 11;
            default:
                return 0;
        }
    }
}
