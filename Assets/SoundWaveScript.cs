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

        Debug.Log(GetComponent<Renderer>().materials.Length);
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.localScale.x / 2 < Radius)
        {
            transform.localScale += Vector3.one * Time.deltaTime * Speed;
            _material.SetFloat("_Fade", 1 - Curve.Evaluate(transform.localScale.x / 2));
        }
    }
}
