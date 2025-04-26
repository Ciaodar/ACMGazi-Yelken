using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    public Transform[] layers; // Array of background layers
    public float[] parallaxScales; // Proportion of the camera's movement to move the layers
    public float smoothing = 1f; // Smoothing factor

    private Vector3 previousCameraPosition;
    private Camera _camera;

    void Start()
    {
        _camera = Camera.main;
        if (Camera.main != null)
        {
            previousCameraPosition = Camera.main.transform.position;
        }
        else
        {
            Debug.LogError("Main Camera not found. Please assign a camera to the scene.");
        }
    }

    void Update()
    {
        if (_camera == null) return;

        Vector3 cameraDelta = _camera.transform.position - previousCameraPosition;

        for (int i = 0; i < layers.Length; i++)
        {
            float parallax = cameraDelta.x * parallaxScales[i];
            Vector3 targetPosition = new Vector3(layers[i].position.x + parallax, layers[i].position.y, layers[i].position.z);
            layers[i].position = Vector3.Lerp(layers[i].position, targetPosition, smoothing * Time.deltaTime);
        }

        previousCameraPosition = _camera.transform.position;
    }
}