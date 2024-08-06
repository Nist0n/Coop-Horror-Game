using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonTest : MonoBehaviour
{
    public void CloseScene()
    {
        Application.Quit();
        Debug.Log("Quit");
    }
}
