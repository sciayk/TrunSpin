using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
public class UIWHSize : MonoBehaviour {

	private GameObject ParentCanvas;
	[Range(0.0f,900.0f)]
	public float reduceWidth=0.0f;
	[Range(0.0f,300.0f)]
	public float reduceHeight=0.0f;
	private GameObject ChildImage;
	void Awake(){
		ParentCanvas = this.transform.parent.gameObject ;
		ChildImage = this.transform.GetChild (0).gameObject;
		this.GetComponent<RectTransform> ().sizeDelta=new Vector2((ParentCanvas.GetComponent<RectTransform> ().rect.width ),(ParentCanvas.GetComponent<RectTransform> ().rect.height ));
		ChildImage.GetComponent<RectTransform> ().sizeDelta = new Vector2 ((this.GetComponent<RectTransform> ().rect.width - reduceWidth), (this.GetComponent<RectTransform> ().rect.height - reduceHeight));
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
