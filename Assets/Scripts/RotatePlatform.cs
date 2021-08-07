using UnityEngine;

public class RotatePlatform : MonoBehaviour
{
    [SerializeField] private float rotationSpeed;

    private void Update()
    {
        var finalRotation = transform.rotation.eulerAngles.SetDirectionalValue(y: transform.rotation.eulerAngles.y + rotationSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Euler(finalRotation);
    }
}