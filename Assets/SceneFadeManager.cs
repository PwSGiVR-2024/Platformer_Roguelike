using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneFadeManager : MonoBehaviour
{
    public static SceneFadeManager _instance;
    [SerializeField] private Image fadeOutImage; // Obraz do zanikania
    [Range(0.1f, 10f), SerializeField] private float fadeOutSpeed = 5f; // Pr�dko�� zanikania
    [Range(0.1f, 10f), SerializeField] private float fadeInSpeed = 5f; // Pr�dko�� pojawiania si�
    [SerializeField] private Color FadeOutStartColor = Color.black; // Pocz�tkowy kolor zanikania (domy�lnie czarny)

    public bool isFadingOut { get; private set; } // Czy zanika
    public bool isFadingIn { get; private set; } // Czy si� pojawia

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
      
        fadeOutImage.color = new Color(FadeOutStartColor.r, FadeOutStartColor.g, FadeOutStartColor.b, 0f); // Ustawienie przezroczysto�ci na 0
    }

    private void Update()
    {
        if (isFadingOut)
        {
            if (fadeOutImage.color.a < 1f)
            {
                fadeOutImage.color = new Color(fadeOutImage.color.r, fadeOutImage.color.g, fadeOutImage.color.b, fadeOutImage.color.a + Time.deltaTime * fadeOutSpeed);
            }
            else
            {
                isFadingOut = false;
            }
        }
        if (isFadingIn)
        {
            if (fadeOutImage.color.a > 0f)
            {
                fadeOutImage.color = new Color(fadeOutImage.color.r, fadeOutImage.color.g, fadeOutImage.color.b, fadeOutImage.color.a - Time.deltaTime * fadeInSpeed);
            }
            else
            {
                isFadingIn = false;
            }
        }
    }

    public void StartFadeOut()
    {
        fadeOutImage.color = new Color(FadeOutStartColor.r, FadeOutStartColor.g, FadeOutStartColor.b, 0f); // Ustawienie koloru pocz�tkowego na przezroczysty
        isFadingOut = true;
    }

    public void StartFadeIn()
    {
        fadeOutImage.color = new Color(FadeOutStartColor.r, FadeOutStartColor.g, FadeOutStartColor.b, 1f); // Ustawienie koloru pocz�tkowego na nieprzezroczysty
        isFadingIn = true;
    }
}
