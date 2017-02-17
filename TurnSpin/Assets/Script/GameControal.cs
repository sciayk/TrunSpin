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
			//mGameConcroal=this.gameObject;
			mGameConcroal = GameObject.Find("GameControl").GetComponent<GameControal>();
		}
		return mGameConcroal = GameObject.Find("GameControl").GetComponent<GameControal>(); ;
	}
	private GameControal(){}
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
	private List<Vector3> WorldPos=new List<Vector3>();
	public GameObject RenderLineGameObject;
	private List<LineRenderer> LineRenderAll=new List<LineRenderer>();
	public int BoneTime;
	private List<int[]> LineLibrary = new List<int[]> ();
//	private Dictionary<int,int[]> LibraryBone = new Dictionary<int, int[]> ();
	private string DataIP="http://test2.3-pts.com/unityexam/getroll.php";
	public GameObject NewGameObj;
	public GameObject ButtomText;
	//private string StarButtomtext;
	public GameObject Turntable;
	public int TimeStop=0;
	public string Spin="Spin";
	public string Stop="Stop";
	public List<string> MyData=new List<string>();
	private bool Chicka=true;
	public bool ChangeChicka{set{Chicka = value; }}
	private bool SpriteRunend = false;
	// Use this for initialization


	//Second
	public bool HaveInternet=false;

	//showfps
	public Text FPSTEXT;
	public bool FpsShow=true;
	private float updateInterval = 0.5f;
	private double lastInterval;
	private int frames = 0;
	private float fps;

	void Awake(){
	}

	void Start () {
		Screen.orientation=ScreenOrientation.LandscapeRight;
		Screen.orientation=ScreenOrientation.AutoRotation;
		Screen.autorotateToLandscapeLeft=true;
		Screen.autorotateToLandscapeRight=true;
		Screen.autorotateToPortrait=false;
		Screen.autorotateToPortraitUpsideDown=false;
		int[] CheckBoneRure = {0,1,2,3,4};
		int[] CheckBoneRure1 = {5,6,7,8,9};
		int[] CheckBoneRure2 = {10,11,12,13,14};
		int[] CheckBoneRure3 = {0,6,12,8,4};
		int[] CheckBoneRure4 = {10,6,2,8,14};
		LineLibrary.Add (CheckBoneRure);
		LineLibrary.Add (CheckBoneRure1);
		LineLibrary.Add (CheckBoneRure2);
		LineLibrary.Add (CheckBoneRure3);
		LineLibrary.Add (CheckBoneRure4);
		if (RenderLineGameObject.transform.childCount != 0) {
			for (int i = 0; i < RenderLineGameObject.transform.childCount; i++) {
				LineRenderAll.Add (RenderLineGameObject.transform.GetChild(i).GetComponent<LineRenderer>());
			}

		}
		//ButtomText = GameObject.FindWithTag ("UItag");
		//Go=GameObject.FindWithTag ("Tag1");
		lastInterval = Time.realtimeSinceStartup;
		frames = 0;
		for (int i = 0; i < Turntable.transform.childCount; i++) {
			//Debug.Log (Turntable.transform.GetChild (i).GetChild (0).position);
			WorldPos.Add (Turntable.transform.GetChild (i).transform.position);
			Debug.Log (WorldPos[i]+" "+Turntable.transform.GetChild (i).name+" "+i);
			AllImages.Add(Turntable.transform.GetChild (i).GetChild (0).GetComponent<MyImage> ());
		}

		DrawLineGo ();
		if (FpsShow == false) {
			FPSTEXT.gameObject.SetActive (false);
		} else {
			FPSTEXT.gameObject.SetActive (true);
		}
		//StartCoroutine (WaitForRequest());
		//StarButtomtext = ButtomText.text;
	}
	void Update(){
		if (FpsShow) {
			showfps ();
		}
	}
		
	public void ButtomClick(){
		if (Chicka)
		{	
			//if (!HaveInternet) {
			//	StartCoroutine(NoInternet ());
			//} else {
				StartCoroutine (WaitForPost ());
			//}
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


	void DrawLineGo(){
		int LinCount = LineLibrary.Count;
		int RenderAll = LineRenderAll.Count;
		//Debug.Log ("Start "+LineLibrary.Count+" "+LineRenderAll.Count);
		for (int i = 0; i < (LinCount-RenderAll); i++) {
			GameObject NewLine = Instantiate(NewGameObj);
			NewLine.AddComponent<LineRenderer> ();
			NewLine.transform.SetParent (RenderLineGameObject.transform);
			LineRenderAll.Add (NewLine.GetComponent<LineRenderer>());

			//Debug.Log ("C: "+i);
		}
		for (int i = 0; i < LineLibrary.Count; i++) {
			for (int s = 0; s < LineLibrary [i].Length; s++) {
				//Debug.Log ("b: "+LineLibrary [i].Length);
				LineRenderAll [i].SetVertexCount (LineLibrary [i].Length);
				int[] ar= LineLibrary[i];
				LineRenderAll[i].SetPositions(GetVec3 (ar));

				//LineRenderAll [i].SetPositions (GetVec3());

			}
			LineRenderAll [i].startColor = Color.red;
			LineRenderAll [i].endColor = Color.red;
		}
	}
	Vector3[] GetVec3(int[] Arr){
		Vector3 Office = new Vector3 (0f,0f,-10f);
		Vector3[] APos=new Vector3[Arr.Length];
		for (int i = 0; i < Arr.Length; i++) {
			APos [i] = WorldPos [i];
		}
		return APos;
	}
	IEnumerator NoInternet(){
		MyData.Clear ();
		SpriteRunend = false;
		for (int i = 0; i<AllImages.Count; i++) {
			string Randstring=AllImages [i].MyTurnImage[Random.Range(0,AllImages [i].MyTurnImage.Count-1)].name;
			AllImages [i].StopImagePos (Randstring);
			MyData.Add (Randstring);
		}
		SpriteRunend = true;
		yield return 0;
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
		Debug.Log ("ok4 "+Time.time);
		yield return www;
		Debug.Log ("ok7 "+Time.time);
		if (www.error == null) {
			Debug.Log ("Success");
			MyData.Clear ();
			//Debug.Log ("Success"+www.text);
			JsonData jsonTargets = JsonMapper.ToObject (www.text);
			//Debug.Log (jsonTargets["CURRENT_ROLL"]);
			//Debug.Log ("ok1 "+Time.time);
			for (int i = 0; i < jsonTargets ["CURRENT_ROLL"].Count; i++) {
				//Debug.Log ("add: "+jsonTargets ["CURRENT_ROLL"][i].ToString());
				//AllImages[i].NowImageis=jsonTargets ["CURRENT_ROLL"] [i].ToString ();
				//Debug.Log ("ok2 "+Time.time);
				AllImages[i].StopImagePos (jsonTargets ["CURRENT_ROLL"] [i].ToString ());
				//Go.transform.GetChild (i).GetChild (0).GetComponent<MyImage> ().NowImageis = jsonTargets ["CURRENT_ROLL"] [i].ToString ();
				//Go.transform.GetChild (i).GetChild (0).GetComponent<MyImage> ().StopImagePos ();
				//Debug.Log ("ok3 "+Time.time);
				MyData.Add(jsonTargets ["CURRENT_ROLL"][i].ToString());
				//Debug.Log ("give value");
				//Debug.Log ("ok4 "+Time.time);
			}
			//Debug.Log ("ok  "+Time.time);
			SpriteRunend = true;
		} else {
			StartCoroutine(NoInternet ());
			Debug.Log ("error"+www.error);
		}
	}
}
