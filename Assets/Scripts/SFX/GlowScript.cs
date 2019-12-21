using UnityEngine;

public class GlowScript : MonoBehaviour
{
    private Light[] _lights;

    // Start is called before the first frame update
    void Start()
    {
        _lights = GetComponentsInChildren<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        var intensity = Remap(Mathf.Sin(Time.time), -1, 1, 1, 2);

        foreach (var light in _lights)
        {
            light.intensity = intensity;
        }
    }


    private float Remap(float s, float a1, float a2, float b1, float b2)
    {
        return b1 + (s - a1) * (b2 - b1) / (a2 - a1);
    }
}
