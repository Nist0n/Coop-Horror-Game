using UnityEngine;

public class StartGame : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;
    void Start()
    {
        Invoke("GameStart", 1);
        Invoke("GameStop", 2);
    }

    private void GameStart()
    {
        playerMovement.enabled = false;
    }
    
    private void GameStop()
    {
        playerMovement.enabled = true;
    }
}
