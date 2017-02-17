using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetMySelfWH : MonoBehaviour {
	private GameObject ParentCanvas;
	public bool SetWidthAndHeight=true;
	[Range(0.0f,900.0f)]
	public float reduceWidth=0.0f;
	[Range(0.0f,300.0f)]
	public float reduceHeight=0.0f;
	private bool AutoSetSize=true;
	public int Row = 5;
	public int Column=3;
	private int TurntableCount;
	public GameObject Prefab;
	// Use this for initialization
	void Start () {
		if (SetWidthAndHeight) {
			ParentCanvas = this.transform.parent.gameObject;
			this.GetComponent<RectTransform> ().sizeDelta = new Vector2 ((ParentCanvas.GetComponent<RectTransform> ().rect.width - reduceWidth), (ParentCanvas.GetComponent<RectTransform> ().rect.height - reduceHeight));
		}
		if (AutoSetSize) {
			this.GetComponent<GridLayoutGroup> ().cellSize = new Vector2 ((ParentCanvas.GetComponent<RectTransform> ().rect.width/(Row+1)), (ParentCanvas.GetComponent<RectTransform> ().rect.height/(Column+1)));
		}
		TurntableCount = Row*Column;
//		Debug.Log (TurntableCount - this.transform.childCount);
		if (this.transform.childCount != TurntableCount) {
			Debug.Log ("數量錯誤");
//			AddMask ();
		}
//
		if(this.GetComponent<GridLayoutGroup> ().cellSize.x < this.GetComponent<GridLayoutGroup> ().cellSize.y){ 
			ChangeTurntableWidthHeight (this.GetComponent<GridLayoutGroup> ().cellSize.x);
		}else{ 
			ChangeTurntableWidthHeight (this.GetComponent<GridLayoutGroup> ().cellSize.y);
		}; 
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void AddMask(){
		for(int i=0;i<(TurntableCount - this.transform.childCount);i++){
			Debug.Log ("Add");
			GameObject NewMask = Instantiate (Prefab);
			NewMask.transform.SetParent (this.transform);
		}
	}

	void ChangeTurntableWidthHeight(float Size){
		for (int i = 0; i < TurntableCount; i++) {
			this.transform.GetChild (i).GetChild (0).gameObject.GetComponent<RectTransform> ().sizeDelta = new Vector2 (Size, Size);
		}
	}
}
