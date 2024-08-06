using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuButtons;
    [SerializeField] private GameObject guidebookButton;


    public void PlayGame()
    {
        SceneManager.LoadScene("");
    }    
    public void OpenSettings()
    {

    }

    public void CLoseGame()
    {
        Application.Quit();
    }

    public void OpenGuidebook()
    {
        mainMenuButtons.SetActive(false);
        guidebookButton.SetActive(false);
    }
}
