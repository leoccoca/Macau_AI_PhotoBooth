using UnityEngine;
using System.Collections;

public class Demo : MonoBehaviour {

	public Texture2D texture;

	// Use this for initialization
	void Start () {

		//PrinterPlugin.Test();
	}

	void OnGUI () {
		// Make a background box
		GUI.Box(new Rect(10,10,220,90), "Printer Menu");
		
		// Make the first button.
		if(GUI.Button(new Rect(20,40,200,25), "Print with printer dialog")) {
			PrinterPlugin.print(texture,true,PrinterPlugin.PrintScaleMode.FILL_PAGE);
		}
		
		// Make the second button.
		if(GUI.Button(new Rect(20,70,200,25), "Print without printer dialog")) {
			PrinterPlugin.print(texture,false,PrinterPlugin.PrintScaleMode.FILL_PAGE);
		}
	}

}
