using UnityEngine;

public class ParallaxMaterial : MonoBehaviour
{
    public Renderer[] layers; // Array of renderers for the background layers
    public float[] parallaxSpeeds; // Speed of parallax for each layer
    public Camera mainCamera;

    private Vector3 previousCameraPosition;

    void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
        previousCameraPosition = mainCamera.transform.position;
    }

    void Update()
    {
        Vector3 cameraDelta = mainCamera.transform.position - previousCameraPosition;

        for (int i = 0; i < layers.Length; i++)
        {
            Vector2 offset = layers[i].material.mainTextureOffset;
            transform.Translate(cameraDelta.x * parallaxSpeeds[i] * Vector3.right);
        }

        previousCameraPosition = mainCamera.transform.position;
    }
}