using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DoorsToNextFloor : MonoBehaviour
{
    [SerializeField] private CompositeCollider2D colliderForCamera;
    private SceneController controller;
    [SerializeField] private GameObject loaderCanvas;
    private GameObject player;
    [SerializeField] private Slider slider;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        controller = FindObjectOfType<SceneController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(ShowLoaderAndLoadNextFloor());
        }
    }

    private IEnumerator ShowLoaderAndLoadNextFloor()
    {
        loaderCanvas.SetActive(true);
        slider.value = 0f;

        // Przesuni�cie gracza
        player.transform.position = transform.position + new Vector3(26, 0, 0);

        // Rozpocz�cie �adowania nast�pnego poziomu
        var loadOperation = StartCoroutine(controller.AfterBossDeathAsync());
        float loadTime = 5f;  // Czas �adowania
        float elapsedTime = 0f;

        // P�tla for zamiast while do monitorowania post�pu �adowania
        for (elapsedTime = 0f; elapsedTime < loadTime && !IsLoadOperationComplete(loadOperation); elapsedTime += Time.deltaTime)
        {
            slider.value = Mathf.Clamp01(elapsedTime / loadTime);
            yield return null;
        }

        // Upewnienie si�, �e ekran �adowania zostaje wy��czony
        loaderCanvas.SetActive(false);
    }

    // Sprawdzenie, czy operacja �adowania jest zako�czona
    private bool IsLoadOperationComplete(Coroutine loadOperation)
    {
        return loadOperation == null;
    }
}
