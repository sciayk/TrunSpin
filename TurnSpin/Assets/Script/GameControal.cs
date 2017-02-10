using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LitJson;
using System.IO;
public class GameControal : MonoBehaviour {

	private static GameControal mGameConcroal;

	public static GameControal getControal(){
		if (mGameConcroal == null) {
			//mGameConcroal = new GameControal();
			mGameConcroal = GameObject.Find("GameControl").GetComponent<GameControal>();
		}
		return mGameConcroal = GameObject.Find("GameControl").GetComponent<GameControal>(); ;
	}

//	private  static GameControal instance;
//	public 	 static GameControal Instance{
//		get{
//			if(mGameConcroal == null)
//				mGameConcroal = GameObject.Find("GameControl").GetComponent<GameControal>();
//
//			return mGameConcroal;
//		}
//	}
//
//	void Test()
//	{
//		GameControal.Instance
//	}

	private List<MyImage> AllImages = new List<MyImage>();


	private GameControal(){}

	private string DataIP="http://test2.3-pts.com/unityexam/getroll.php";

	private GameObject ButtomText;
	//private string StarButtomtext;
	private GameObject Go;
	public int TimeStop=0;
	private string Spin="Spin";
	private string Stop="Stop";
	public List<string> MyData=new List<string>();
	private bool Chicka=true;
	public bool ChangeChicka{set{Chicka = value; }}
	private bool SpriteRunend = false;
	// Use this for initialization


	//showfps
	public Text FPSTEXT;
	public bool FPSSHOW=true;
	private float updateInterval = 0.5f;
	private double lastInterval;
	private int frames = 0;
	private float fps;

	void Awake(){
	}

	void Start () {
		ButtomText = GameObject.FindWithTag ("UItag");
		Go=GameObject.FindWithTag ("Tag1");
		lastInterval = Time.realtimeSinceStartup;
		frames = 0;

		for (int i = 0; i < Go.transform.childCount; i++) {
			AllImages.Add(Go.transform.GetChild (i).GetChild (0).GetComponent<MyImage> ());
		}
		if (FPSSHOW == false) {
			FPSTEXT.gameObject.SetActive (false);
		} else {
			FPSTEXT.gameObject.SetActive (true);
		}
		//StartCoroutine (WaitForRequest());
		//StarButtomtext = ButtomText.text;
	}
	void Update(){
		if (FPSSHOW) {
			showfps ();
		}
	}
		
	public void ButtomClick(){
		if (Chicka)
		{
			StartCoroutine (WaitForPost());
			Chicka = false;
			ButtomText.GetComponent<Text>().text = Stop;
//			for (int i = 0; i < Go.transform.childCount; i++) {
//				Go.transform.GetChild (i).GetChild (0).GetComponent<MyImage> ().ChangeCanTurn = true;
//			}	
			foreach (MyImage image in AllImages) {
				image.ChangeCanTurn = true;
			}
		} else{
			if(SpriteRunend){
			Chicka = true;
			ButtomText.GetComponent<Text>().text = Spin;
//			for (int i = 0; i < Go.transform.childCount; i++) {
//				Go.transform.GetChild (i).GetChild (0).GetComponent<MyImage> ().StopTime=20.0f;
//			}
			foreach (MyImage image in AllImages) {
				image.NowStop();
				image.StopTime = 20f;
			}
			}
		}
	}


	void showfps(){
		++frames;
		float timeNow = Time.realtimeSinceStartup;
		if (timeNow > lastInterval + updateInterval)
		{
			fps = (float)(frames / (timeNow - lastInterval));
			frames = 0;
			lastInterval = timeNow;
		}
		FPSTEXT.text = fps.ToString ();
	}

	public void StopOneTurn(){
		TimeStop+=1;
		//Debug.Log (TimeStop);
		if (TimeStop >= 15 ) {
			TimeStop = 0;
			Chicka = false;
			ButtomClick ();
		}
	}

	IEnumerator WaitForPost(){
		SpriteRunend = false;
		string url = DataIP;
		WWWForm form = new WWWForm();
		form.AddField ("METHOD","spin");
		form.AddField ("PARAMS","test");
		WWW www = new WWW (url, form);
		yield return www;
		if (www.error == null) {
			MyData.Clear ();

			//Debug.Log ("Success"+www.text);
			JsonData jsonTargets = JsonMapper.ToObject (www.text);
			//Debug.Log (jsonTargets["CURRENT_ROLL"]);
			for (int i = 0; i < jsonTargets ["CURRENT_ROLL"].Count; i++) {
				//Debug.Log ("add: "+jsonTargets ["CURRENT_ROLL"][i].ToString());
				//AllImages[i].NowImageis=jsonTargets ["CURRENT_ROLL"] [i].ToString ();
				AllImages[i].StopImagePos (jsonTargets ["CURRENT_ROLL"] [i].ToString ());
				//Go.transform.GetChild (i).GetChild (0).GetComponent<MyImage> ().NowImageis = jsonTargets ["CURRENT_ROLL"] [i].ToString ();
				//Go.transform.GetChild (i).GetChild (0).GetComponent<MyImage> ().StopImagePos ();
				MyData.Add(jsonTargets ["CURRENT_ROLL"][i].ToString());
				Debug.Log ("give value");
			}
			SpriteRunend = true;
		} else {
			Debug.Log ("error"+www.error);
		}
	}
}
