using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager2 : MonoBehaviour
{
    [Header("Références")]
    [SerializeField] private GameObject player;          // Référence à la bulle
    [SerializeField] private ObstacleData obstacleData; // Ton ScriptableObject de configuration
    [SerializeField] private ObstaclePool obsPool;      // Référence à ton pool d'obstacles

    [Header("Paramètres de Génération")]
    [SerializeField] private float spawnAheadDistance = 15f;    // Distance au-dessus du joueur où spawner
    [SerializeField] private float distanceBetweenObstacles = 5f; // Distance verticale fixe entre chaque obstacle
    [SerializeField] private bool hasStarted = false;

    private float playerBoundaryX;
    private float nextSpawnY;

    // Remplacement du tableau 2D par une liste dynamique pour le défilement infini
    private List<GameObject> activeObstacles = new List<GameObject>();

    void Start()
    {
        // Récupération des limites horizontales calculées par ton BubbleBoundary
        var bubbleBoundary = player.GetComponent<BubbleBoundary>();
        if (bubbleBoundary != null)
        {
            playerBoundaryX = bubbleBoundary.BoundaryX;
        }
        else
        {
            playerBoundaryX = 6f; // Valeur de secours par défaut
        }

        // On initialise la première hauteur de spawn un peu au-dessus du joueur
        nextSpawnY = player.transform.position.y + 5f;
    }

    void Update()
    {
        if (!hasStarted) return;

        // 1. Gestion de la génération en continu selon la hauteur de la bulle
        HandleProceduralSpawn();

        // 2. Nettoyage et retour au pool des obstacles dépassés
        RecyclePastObstacles();
    }

    private void HandleProceduralSpawn()
    {
        // Si le joueur s'approche de la dernière hauteur planifiée, on génère le prochain obstacle
        if (player.transform.position.y + spawnAheadDistance > nextSpawnY)
        {
            SpawnSingleObstacle(nextSpawnY);

            // On incrémente la hauteur pour le prochain obstacle
            nextSpawnY += distanceBetweenObstacles;
        }
    }

    private void SpawnSingleObstacle(float targetY)
    {
        // Sélection d'un type d'obstacle via ton algorithme ou tirage au sort pondéré
        ObstacleData.ObstacleType selectedType = GetWeightedRandomObstacle();

        if (selectedType == null || selectedType.prefab == null) return;

        // Récupération de l'obstacle dans ton dictionnaire de files (Pool)
        GameObject obstacle = obsPool.GetObstacle(selectedType.prefab);

        if (obstacle != null)
        {
            // Calcul de la position horizontale en fonction de ta variable 'isCentered'
            float spawnX = 0f;
            if (!selectedType.isCentered)
            {
                // On utilise playerBoundaryX pour ne pas dépasser des limites de l'écran
                spawnX = Random.Range(-playerBoundaryX, playerBoundaryX);
            }

            obstacle.transform.position = new Vector3(spawnX, targetY, 0f);

            // Gestion de la rotation autonome en fonction de ta variable 'canSpin'
            if (selectedType.canSpin)
            {
                obstacle.transform.rotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));
            }
            else
            {
                obstacle.transform.rotation = Quaternion.identity;
            }

            // --- Note sur ta variable 'selectedType.dir' ---
            // Si tes obstacles possèdent un script de comportement de mouvement (ex: patrouille), 
            // c'est ici qu'il faut leur transmettre la chaîne de caractères (ex: "X" ou "Y") :
            //
            // ObstacleBehavior behavior = obstacle.GetComponent<ObstacleBehavior>();
            // if (behavior != null) behavior.InitializeBehavior(selectedType.dir);

            // Ajout à la liste de suivi pour le nettoyage futur
            activeObstacles.Add(obstacle);
        }
    }

    private void RecyclePastObstacles()
    {
        // Seuil en dessous duquel un obstacle est considéré comme invisible et dépassé
        float recycleThresholdY = player.transform.position.y - 10f;

        // Parcours inversé de la liste pour pouvoir supprimer des éléments en toute sécurité
        for (int i = activeObstacles.Count - 1; i >= 0; i--)
        {
            GameObject obs = activeObstacles[i];

            if (obs != null)
            {
                // Si l'obstacle est descendu trop bas par rapport à la bulle, on le recycle
                if (obs.transform.position.y < recycleThresholdY)
                {
                    activeObstacles.RemoveAt(i);

                    // Retrouver le prefab d'origine pour appeler ton ReturnToPool
                    GameObject originalPrefab = FindOriginalPrefab(obs.name);
                    if (originalPrefab != null)
                    {
                        obsPool.ReturnToPool(obs, originalPrefab);
                    }
                    else
                    {
                        obs.SetActive(false); // Sécurité si le prefab n'est pas retrouvé
                    }
                }
            }
        }
    }

    // Méthode de nettoyage manuel utilisable lors d'un Game Over ou d'un reset de niveau
    public void ClearAllActiveObstacles()
    {
        for (int i = activeObstacles.Count - 1; i >= 0; i--)
        {
            GameObject obs = activeObstacles[i];
            if (obs != null)
            {
                GameObject originalPrefab = FindOriginalPrefab(obs.name);
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

    // Sélection probabiliste (tirage pondéré) exploitant ton 'spawnRate' de manière robuste
    private ObstacleData.ObstacleType GetWeightedRandomObstacle()
    {
        var obsTypes = obstacleData.obsTypes;
        if (obsTypes == null || obsTypes.Count == 0) return null;

        // 1. Cumul de la somme de tous les spawnRates de ta liste
        float totalWeight = 0f;
        foreach (var type in obsTypes)
        {
            totalWeight += type.spawnRate;
        }

        // 2. Tirage d'une valeur aléatoire entre 0 et le poids total
        float randomValue = Random.Range(0f, totalWeight);

        // 3. Parcours pour déterminer dans quel intervalle se situe la valeur
        float currentWeightSum = 0f;
        foreach (var type in obsTypes)
        {
            currentWeightSum += type.spawnRate;
            if (randomValue <= currentWeightSum)
            {
                return type;
            }
        }

        return obsTypes[0]; // Renvoi par défaut en cas d'anomalie
    }

    // Permet de faire la correspondance entre le nom de l'instance clonée et son prefab d'origine
    private GameObject FindOriginalPrefab(string instanceName)
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