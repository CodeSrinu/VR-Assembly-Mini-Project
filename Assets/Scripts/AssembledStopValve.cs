using Unity.VisualScripting;
using UnityEngine;

public class AssembledStopValve : MonoBehaviour
{
    public float rotationSpeed = 10f;
    public float floatSpeed = 10f;

    public Transform pointA;
    public Transform pointB;
    private Transform targetPoint;

    private void Start()
    {
        targetPoint = pointB;
    }
    private void Update()
    {
        transform.Rotate(Vector3.up * rotationSpeed);
        Float();
    }

    private void Float()
    {
        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPoint.position,
            floatSpeed * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, targetPoint.position) < 0.01f)
        {
            targetPoint = targetPoint == pointA ? pointB : pointA;
        }
    }
}
