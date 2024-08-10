using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuUI;
    [SerializeField] private GameObject settingsUI;
    [SerializeField] private GameObject guidebookUI;
    [SerializeField] private GameObject sessionUI;

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            mainMenuUI.SetActive(true);
            settingsUI.SetActive(false);
            guidebookUI.SetActive(false);
        }
    }

    public void CreateSession()
    {
        mainMenuUI.SetActive(false);
        sessionUI.SetActive(true);
    }
    
    public void PlayGame()
    {
        SceneManager.LoadScene("VLAD UI");
    }    

    public void OpenSettings()
    {
        mainMenuUI.SetActive(false);
        settingsUI.SetActive(true);
    }

    public void CloseGame()
    {
        Application.Quit();
        Debug.Log("Everything is working master");
    }

    public void OpenGuidebook()
    {
        mainMenuUI.SetActive(false);
        guidebookUI.SetActive(true);
    }
}
