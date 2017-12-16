using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts;
using HoloToolkit.Unity;

public class EarthDataView : MonoBehaviour
{

    public Text planetTitle;
    public Text descriptionText;

    private TextToSpeech textToSpeech;
    private Planet planet;
    private Transform originalParent;

    public string url;


    private void Awake()
    {
        if (IntroductionFlow.Instance != null) Destroy(gameObject);

        var textToSpeechManager = GameObject.Find("TextToSpeechManager");
        textToSpeech = textToSpeechManager.GetComponent<TextToSpeech>();
        textToSpeech.Voice = TextToSpeechVoice.Zira;
        //textToSpeech.runInEditMode = true;
    }

    // Use this for initialization
    IEnumerator Start()
    {
        WWW www = new WWW(url);

        yield return www;

        planet = JsonUtility.FromJson<Planet>(www.text);


        planetTitle.text = planet.displaytitle;
        descriptionText.text = planet.extract;


        textToSpeech.StartSpeaking(descriptionText.text);

        if (textToSpeech.IsSpeaking())
        {
            Debug.Log("Speaking");
        }
        else
        {
            Debug.Log("Not speaking");
        }

    }

    // Update is called once per frame
    void Update()
    {



    }
    void OnDestroy()
    {
        Debug.Log("Destroy Text is working");
        Destroy(planetTitle);
        Destroy(descriptionText);
    }


}
