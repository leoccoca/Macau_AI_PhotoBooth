using UnityEngine;
using System.Collections;
using System;
using System.Runtime.InteropServices;

public class PrinterPlugin : MonoBehaviour {

	[DllImport ("PrinterPlugin")]
	private static extern void _PrintTexture(System.IntPtr texture, bool showDialog, int printScaleMode);

	public enum PrintScaleMode : int{
		NO_SCALE = 0,
		PAGE_WIDTH,
		PAGE_HEIGHT,
		FILL_PAGE
	}


	public static void print(Texture texture,bool showDialog, PrintScaleMode printScaleMode){
		_PrintTexture(texture.GetNativeTexturePtr(),showDialog,(int) printScaleMode);
	}
	
}
