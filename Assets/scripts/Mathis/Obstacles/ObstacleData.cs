using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ObsCfg", menuName = "Blowbble2/ObstacleConfiguration")]

public class ObstacleData : ScriptableObject
{
    [System.Serializable]
    public class ObstacleType
    {
        public string name;
        public string dir;
        [Range(0f, 1f)] public float spawnRate;
        public bool isCentered;
        public Color color;
        public GameObject prefab;
    }

    public List<ObstacleType> obsTypes;

}
