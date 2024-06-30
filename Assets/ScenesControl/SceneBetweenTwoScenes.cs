using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneBetweenTwoScenes : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] GameObject loaderCanvas;
    [SerializeField] GameObject player;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(ShowLoaderAndLoadNextFloor());
    }
    public IEnumerator ShowLoaderAndLoadNextFloor()
    {
        slider.value = 0f;

        float loadTime = 5f;  // Czas �adowania
        float elapsedTime = 0f;

        // P�tla oczekuj�ca na zako�czenie �adowania
        while (elapsedTime < loadTime)
        {
            elapsedTime += Time.deltaTime;
            slider.value = Mathf.Clamp01(elapsedTime / loadTime);
            yield return null;
        }
    }
}
