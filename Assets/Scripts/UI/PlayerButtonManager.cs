using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using UnityEngine.SceneManagement;


public class PlayerButtonManager : NetworkBehaviour
{
    [SerializeField] private GameObject canvasMenu;

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            canvasMenu.SetActive(!canvasMenu.activeSelf);
        }
    }

    public void OpenGameMenu()
    {
        canvasMenu.SetActive(true);
    }

    public void ResumeGame()
    {
        canvasMenu.SetActive(false);
    }

    public void OpenSettings() 
    {
        canvasMenu.SetActive(false);
    }

    public void QuitServer()
    {
        NetworkManager.Singleton.Shutdown();
        SceneManager.LoadScene("");
        Debug.Log("Everything is working master");
    }
}
