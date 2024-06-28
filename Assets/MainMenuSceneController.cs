using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuSceneController : MonoBehaviour
{

    // Metoda wirtualna do rozpocz�cia gry, kt�r� mo�na nadpisa� w klasach pochodnych
    public virtual void StartGameButton()
    {
        // Domy�lna implementacja (je�li jaka� jest)
        Debug.Log("StartGameButton pressed - base implementation");
    }

    public virtual void EndGameButton()
    {
        Application.Quit();
    }

    public virtual void SettingsGameButton()
    {
        // �adowanie ustawie� - do nadpisania w klasach pochodnych
        Debug.Log("SettingsGameButton pressed - base implementation");
    }

    public virtual void ContinueGameButton()
    {
        // Kontynuowanie gry - do nadpisania w klasach pochodnych
        Debug.Log("ContinueGameButton pressed - base implementation");
    }
}