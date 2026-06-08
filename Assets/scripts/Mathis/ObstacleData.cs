using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ObstacleCfg", menuName = "Blowbble2/ObstacleConfiguration")]

public class ObstacleData : ScriptableObject
{
    [System.Serializable]
    public class ObstacleType
    {
        public string name;
        public Color color;
        public float spacing;

        public bool canCenter;
        public GameObject prefab;
    }

    public List<ObstacleType> obsTypes;

}
