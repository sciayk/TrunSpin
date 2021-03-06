﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyImage : MonoBehaviour {
	private delegate void ImageRun();

	public List<Sprite> MyTurnImage=new List<Sprite>();
	private float TurnSpeed=30.0f;
	private float RemberTurnSpeed;
	//轉
	private float DownSpeed=0.0015f;
	public float DownSpeedTime=2.0f;
	private float RunTime=5.0f;
	public float StopTime=0;
	public Vector3 StopPos;
	// Use this for initialization

	//getImage
	//private GameObject Buttom;
	private Sprite NowImg;
	public string NowImageName="";
	public string NowImageis{set{NowImageName = value; } get{return NowImageName;}}

	//圖片使用
	//private List<MyImage> AllImages2 = new List<MyImage>();
	private Dictionary<string,Vector3> RemberTurnImagePos=new Dictionary<string, Vector3>();
	private Dictionary<Vector3,GameObject> RemberTurnImageGameObject=new Dictionary<Vector3,GameObject>();
	//	int TimeStop=17;
	//int ImageShowNumber1,ImageShowNumber2;
	private bool CanTurn=false;
	public bool ChangeCanTurn{set{CanTurn = value;}}
	public List<Sprite> Imagequeue;
	public int TurnTime;
	private bool Turn = false;
	public bool bStop=false;
	public bool ChangeStop{get{return bStop; } set{bStop = value;}}
	private Vector3 StartYpos;
	//private Vector3 RectY;
	void Start () {

		RandImage ();
		//RandImage ();
	//	Buttom = GameObject.FindWithTag ("GameControal");
		RemberTurnSpeed = TurnSpeed;
		StartYpos = new Vector3(-13.0f,1269.75f,0.0f);
		TurnTime = MyTurnImage.Count;
		//RectY = this.GetComponent<RectTransform> ().localPosition;
		NewAgain ();
	}
	// Update is called once per frame
	void Update () {
		if (CanTurn || Input.GetKeyDown(KeyCode.A)) {
			Turn = true;
			StopTime=0.0f;
			CanTurn = false;
			//AddDelegateImage ();
		}

		if (Turn) {
			//AddDelegateImage ();
			StartCoroutine (IRunImage ());
			if ((RunTime < StopTime) || (bStop==true) ) {
				bStop = false;
				TurnSpeed = RemberTurnSpeed;
				Turn = false;
				//NowStop();
				GameControal.getControal ().StopOneTurn ();
				//DelDelegate ();
				//Buttom.GetComponent<GameControal> ().StopOneTurn ();
			}
	
			StopTime = StopTime +Time.deltaTime;

		}
	}
	public void AddDelegateImage(){
		//ImageRun+=RandImage();
	}

	void DelDelegate(){
		//ImageRun -= RandImage;
	}

	public void NowStop(){
		//StopImagePos ();
		//RemberTurnImageGameObject[StopPos].SetActive(true);
		//Debug.Log (RemberTurnImageGameObject[StopPos].name);
		this.transform.localPosition = StopPos -new Vector3(13.0f,-39.75f,0f);
	} 



	public void StopImagePos(string SpriteName){
		NowImageName = SpriteName;
		StopPos = RemberTurnImagePos [NowImageName];
//		for (int i = 0; i < this.transform.childCount; i++) {
//			if ((this.transform.GetChild (i).GetComponent<Image> ().sprite.name) == (NowImageName)) {
//				StopPos = new Vector3 (0.0f, (1670.0f-(17.0f-i)*100.0f ),0.0f);
//				break;
//			}
//		}
		//this.GetComponent<RectTransform> ().localPosition = (new Vector3(0.0f,((int)(this.transform.localPosition.y / 100) + 1)*100 +10.0f,0.0f) -new Vector3(13.0f,0.0f,0f));
	}

	IEnumerator IRandImage(){
		RandImage ();
		yield return 0;
	}

	void RandImage(){
		RemberTurnImagePos.Clear ();
		for (int i = 0; i < TurnTime; i++) {
			int RemoveSprint;
			RemoveSprint = Random.Range (0, MyTurnImage.Count);
			this.transform.GetChild (TurnTime - i-1).gameObject.GetComponent<Image> ().sprite = MyTurnImage [RemoveSprint];
			RemberTurnImageGameObject.Add (new Vector3 (0.0f, (-30.0f + (17 - i) * 100.0f), 0.0f), this.transform.GetChild (TurnTime - i - 1).gameObject);
			RemberTurnImagePos.Add (this.transform.GetChild (TurnTime - i-1).gameObject.GetComponent<Image> ().sprite.name,new Vector3 (0.0f, (-30.0f+(17-i)*100.0f ),0.0f));
			Imagequeue.Add(MyTurnImage [RemoveSprint]);
			MyTurnImage.RemoveAt (RemoveSprint);
		}
//		Debug.Log ("RemberSuccess");
	}

	void NewAgain(){
		MyTurnImage.AddRange (this.GetComponent<MyImage>().Imagequeue);
		Imagequeue.Clear ();
	}

	IEnumerator IRunImage(){
		RunImage();
		yield return 0;
	}

	void RunImage(){
		
		if (this.transform.localPosition.y <= (TurnSpeed)) {
			//Debug.Log ("重新跑");
			this.transform.localPosition = StartYpos-new Vector3(0.0f,10.0f,0.0f);
		}
		if ((RunTime-StopTime) < DownSpeedTime) {
			TurnSpeed = TurnSpeed - DownSpeed;
		}
		this.gameObject.transform.Translate (-new Vector3 (0, TurnSpeed, 0) * Time.deltaTime * 25.6f);
		//this.gameObject.transform.localPosition = this.gameObject.transform.localPosition - new Vector3( 0,TurnSpeed,0);
	}
}
