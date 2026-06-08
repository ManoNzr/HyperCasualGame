using TMPro;
using UnityEngine;

public class AnimationTapToPlay : MonoBehaviour
{
    [SerializeField] private float animSpeed = 1f;

    // un effet de pulsation et de changement d'alpha du text en boucle.

    private void Update()
    {
        float scale = 1f + Mathf.PingPong(Time.time * animSpeed, 0.5f);
        transform.localScale = new Vector3(scale, scale, 1f);

        TMP_Text text = GetComponent<TextMeshProUGUI>();
        if (text != null)
        {
            Color color = text.color;
            color.a = 0.5f + Mathf.PingPong(Time.time * animSpeed, 0.5f);
            text.color = color;
        }
    }




}
