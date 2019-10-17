using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BetaLoader : MonoBehaviour {

    public Text Message;
    public bool IsBeta;
    public string BetaNumber;
    public Text Betaversion;

	// Use this for initialization
	void Start ()
    {
		if(IsBeta == true)
        {
            Message.gameObject.SetActive(true);
            Betaversion.text = "Build: 1.1:" + BetaNumber;
            StartCoroutine(CheckNews());
        }
        else
        {
            Message.gameObject.SetActive(true);
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}


    private IEnumerator CheckNews()
    {
        {

            WWW www = new WWW("http://trainbase.rf.gd" + "/win/" + BetaNumber + ".txt");
            yield return www;
            if (www.error != null)
            {
                Debug.Log(www.error);
            }
            else
            {
                Message.text = www.text;
            }
        }
    }
}
