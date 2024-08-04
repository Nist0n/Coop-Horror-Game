using UnityEngine;
using ProximityChat;

public class VoiceChatTest : MonoBehaviour
{
    [SerializeField] private VoiceNetworker voiceNetworker;
    void Start()
    {
        voiceNetworker.StartRecording();
        Debug.Log("Started");
    }
}
