using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts;
using HoloToolkit.Unity;

/*
 * Script managing data retrieval and presentation for a specific view
 * */
public class EarthDataView : MonoBehaviour
{

    public Text planetTitle;
    public Text descriptionText; // Text extracted from response should be put here. Linked to a billboard in Unity

    private TextToSpeech textToSpeech;
    private Planet planet;
    private Transform originalParent;

    public string url;

    public bool TextSaid = false;

    private void Awake()
    {
        if (IntroductionFlow.Instance != null) Destroy(gameObject);

        var textToSpeechManager = GameObject.Find("TextToSpeechManager");
        textToSpeech = textToSpeechManager.GetComponent<TextToSpeech>();
        textToSpeech.Voice = TextToSpeechVoice.Zira;
        //textToSpeech.runInEditMode = true;
    }

    // Use this for initialization
    void Start()
    {

        //MakeRestCall();
        

    }

    // Update is called once per frame
    void Update()
    {
        if(descriptionText.text != null && !TextSaid)
        {
            SayText();
            TextSaid = true;
        }


    }
    void OnDestroy()
    {
        Debug.Log("Destroy Text is working");
        Destroy(planetTitle);
        Destroy(descriptionText);
    }

    /*
     * Retrieve online data with a rest call to the given API url
     * Expects a JSON response
     * */
    private IEnumerator MakeRestCall()
    {
        WWW www = new WWW(url);

        yield return www;

        planet = JsonUtility.FromJson<Planet>(www.text);


        planetTitle.text = planet.displaytitle;
        descriptionText.text = planet.extract;
    }

    /*
     * Launch text to speech
     * */
    public void SayText()
    {
        textToSpeech.StartSpeaking(descriptionText.text);
    }


}
