using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts;
using HoloToolkit.Unity;

public class EarthDataView : MonoBehaviour {

	public TextMesh planetTitle;
	public TextMesh descriptionText;

    private TextToSpeech textToSpeech;
    private Planet planet;
    private Transform originalParent;

    public string url;


    private void Awake()
    {
        var textToSpeechManager = GameObject.Find("TextToSpeechManager");
        textToSpeech = textToSpeechManager.GetComponent<TextToSpeech>();
        textToSpeech.Voice = TextToSpeechVoice.Zira;
        //textToSpeech.runInEditMode = true;
    }

    // Use this for initialization
    IEnumerator Start () {


        if (IntroductionFlow.Instance != null)
        {
            planetTitle.text = "";
            descriptionText.text = "";

        }
        else
        {
            WWW www = new WWW(url);

            yield return www;

            planet = JsonUtility.FromJson<Planet>(www.text);



            planetTitle.text = planet.displaytitle;
            descriptionText.text = planet.extract_html;
        }

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
	void Update () {

       
    }


}
