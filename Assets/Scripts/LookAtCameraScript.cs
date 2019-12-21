using UnityEngine;

public class LookAtCameraScript : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        var position = Camera.main.transform;
        transform.LookAt(position, Vector3.up);
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
    }
}
