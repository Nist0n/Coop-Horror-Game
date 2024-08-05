using System;
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

    private void Update()
    {
        while (Input.GetKeyDown(KeyCode.V))
        {
            voiceNetworker.StartRecording();
        }
    }
}
