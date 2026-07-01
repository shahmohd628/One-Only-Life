using UnityEngine;
using TMPro;

public class DeathCounterUI : MonoBehaviour
{
    TextMeshProUGUI textElement;

    void Start()
    {
        textElement = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        if (GameManager.Instance != null && textElement != null)
        {
            textElement.text = "Deaths: " + GameManager.Instance.GetDeathCount();
        }
    }
}