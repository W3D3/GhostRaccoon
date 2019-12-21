using UnityEngine;

public class GrassScript : MonoBehaviour
{
    void Update()
    {
        var position = Camera.main.transform;
        transform.LookAt(position, Vector3.up);
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
    }
}
