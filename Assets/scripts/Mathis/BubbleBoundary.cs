using UnityEngine;

public class BubbleBoundary : MonoBehaviour
{

    [SerializeField] private float boundaryX;
    [SerializeField] private float boundaryY;
    [SerializeField] [Range(0f, 1f)] private float boundaryPercentage = 0.8f;

    public float BoundaryX { get => boundaryX; set => boundaryX = value; }
    public float BoundaryY { get => boundaryY; set => boundaryY = value; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        CalculateBoundaryX();
        CalculateBoundaryY();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void CalculateBoundaryX()
    {
        // Obtenir la largeur visible de l'écran en coordonnées du monde
        Vector3 screenBounds = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0));

        // Calculer boundary en fonction du pourcentage de l'écran
        boundaryX = screenBounds.x * boundaryPercentage;

        Debug.Log($"😊 Boundary calculé : {boundaryX}. Parce que j'ai calculé avec le pourcentage {boundaryPercentage}");
    }
    private void CalculateBoundaryY()
    {
        // Obtenir la largeur visible de l'écran en coordonnées du monde
        Vector3 screenBounds = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0));

        // Calculer boundary en fonction du pourcentage de l'écran
        boundaryY = screenBounds.y * boundaryPercentage;

        Debug.Log($"😊 Boundary calculé : {boundaryY}. Parce que j'ai calculé avec le pourcentage {boundaryPercentage}");
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        // Gizmo.DrawLine(startposition, endposition)
        Gizmos.DrawLine(new Vector3(-boundaryX, -10, 0), new Vector3(-boundaryX, 10, 0));
        Gizmos.DrawLine(new Vector3(boundaryX, -10, 0), new Vector3(boundaryX, 10, 0));
    }
}
