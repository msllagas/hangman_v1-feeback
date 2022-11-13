using UnityEngine;

public class ImageAnimation : MonoBehaviour // for feedback version only
{
    [SerializeField] private Vector3 finalPosition;
    private Vector3 initialPosition;

    private void Awake()
    {
        initialPosition = transform.localPosition;
    }

    private void Update()
    {
        transform.localPosition = Vector3.Lerp(transform.localPosition, finalPosition, 0.1f);
        
    }

    private void OnDisable()
    {
        transform.localPosition = initialPosition;
    }
}
