using UnityEngine;
using ProximityChat;

public class VoiceChatTest : MonoBehaviour
{
    [SerializeField] private VoiceNetworker voiceNetworker;
    // void Start()
    // {
    //     voiceNetworker.SetOutputVolume(10f);
    //     // voiceNetworker.StartRecording();
    //     Debug.Log("Started");
    // }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            Debug.Log("On");
            voiceNetworker.StartRecording();
        }
        else if (Input.GetKeyUp(KeyCode.V))
        {
            Debug.Log("Off");
            voiceNetworker.StopRecording();
        }
            
    }
}
