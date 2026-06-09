using UnityEngine;

public class BubbleInertia : MonoBehaviour
{
    [Tooltip("Le Transform du parent qui se déplace")]
    [SerializeField] private Transform parentTransform;

    [Tooltip("Plus la valeur est basse, plus l'inertie de la bulle est 'molle'")]
    [SerializeField] private float smoothSpeed = 10f;

    [Tooltip("Vélocité maximale transmise au shader pour éviter l'explosion du mesh")]
    [SerializeField] private float maxDeformationVelocity = 5f;

    private Material bubbleMaterial;
    private Vector3 lastPosition;
    private Vector3 smoothedVelocity;

    void Start()
    {
        // On récupère le matériau instancié sur c't objet
        Renderer rend = GetComponent<Renderer>();
        if (rend != null)
        {
            bubbleMaterial = rend.material;
        }
        
        if (parentTransform != null)
        {
            lastPosition = parentTransform.position;
        }
    }

    void Update()
    {
        if (bubbleMaterial == null || parentTransform == null) return;

        // calcul de la vélocité brute du parent
        Vector3 rawVelocity = (parentTransform.position - lastPosition) / Time.deltaTime;
        lastPosition = parentTransform.position;

        // lssage de la vélocité pour amortir les variations de framerate
        smoothedVelocity = Vector3.Lerp(smoothedVelocity, rawVelocity, Time.deltaTime * smoothSpeed);

        //Limitation de la vitesse pour empêcher les vertices de traverser le centre de la sphère
        Vector3 safeVelocity = Vector3.ClampMagnitude(smoothedVelocity, maxDeformationVelocity);

        //transmet la donnée propre au paramètre _Velocity du shader
        bubbleMaterial.SetVector("_Velocity", safeVelocity);
    }
}