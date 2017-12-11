using System.Collections;
using System.Collections.Generic;
using HoloToolkit.Unity;
using UnityEngine;

public class VoiceManager : MonoBehaviour
{
    private void Awake()
    {
        Debug.Log("Voice manager is awake");

    }
    // Use this for initialization
    void Start ()
    {
        if (TransitionManager.Instance == null)
        {
            Debug.LogWarning("VoiceManger: No TransitionManager was found, transition manager is needed to identify when new content has loaded.");
        }
        // it would be nice if we had callbacks for registering voice commands, but it requires finding game objects that are not active; this
        // bypasses that issue by getting all of the children gaze selection targets and manually registering them in the tool panel which is active
        GazeSelectionTarget[] selectionTargets = GetComponentsInChildren<GazeSelectionTarget>(true);
        foreach (GazeSelectionTarget selectionTarget in selectionTargets)
        {
            Debug.Log(selectionTarget.VoiceCommands[0]);
            selectionTarget.RegisterVoiceCommands();
            Debug.Log("Planet voice command registered");

        }
        Debug.Log("Commands should be registered");
    }

    ////// Update is called once per frame
    //void Update()
    //{
    //    GazeSelectionTarget[] selectionTargets = GetComponentsInChildren<GazeSelectionTarget>(true);
    //    foreach (GazeSelectionTarget selectionTarget in selectionTargets)
    //    {
    //        Debug.Log(selectionTarget.VoiceCommands[0]);
    //        selectionTarget.RegisterVoiceCommands();
    //        Debug.Log("voice command registered");

    //    }
    //    //Debug.LogWarning("VoiceManger: is working.");
    //}
}
