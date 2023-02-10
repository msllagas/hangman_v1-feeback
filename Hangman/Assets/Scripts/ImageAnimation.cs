using UnityEngine;

// Script for programmatically animating each image in the background shop panel
public class ImageAnimation : MonoBehaviour
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
