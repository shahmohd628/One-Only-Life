using UnityEngine;

public class AutoHideUI : MonoBehaviour
{
    [SerializeField] private float hideAfter = 3f;

    private void Start()
    {
        Invoke(nameof(Hide), hideAfter);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}