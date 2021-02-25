using UnityEngine;

public class Spin : MonoBehaviour
{
    [SerializeField] float spinSpeed = 100;

    private void Update()
    {
        transform.Rotate(0, spinSpeed * Time.deltaTime, 0, Space.World);
    }
}
