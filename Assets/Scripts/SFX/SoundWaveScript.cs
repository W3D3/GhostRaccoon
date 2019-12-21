using UnityEngine;

public class SoundWaveScript : MonoBehaviour
{
    public float Radius = 5f;
    public float Speed = 20f;
    public AnimationCurve Curve;

    private Material _material;

    void Start()
    {
        _material = GetComponent<Renderer>().material;

        _material.SetFloat("_Fade", 0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.localScale.x / 2 < Radius)
        {
            transform.localScale += Vector3.one * Time.deltaTime * Speed;

            var remappedFade = Remap(transform.localScale.x / 2, 0, Radius, 0, 5);

            _material.SetFloat("_Fade", 1 - Curve.Evaluate(remappedFade));
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private float Remap(float s, float a1, float a2, float b1, float b2)
    {
        return b1 + (s - a1) * (b2 - b1) / (a2 - a1);
    }
}
