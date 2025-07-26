using UnityEngine;
using UnityEditor;
using System.Collections;
 
public class TransformCopier : ScriptableObject
{
	private static Vector3 position;
	private static Quaternion rotation;
	private static Vector3 scale;
	
	private static string objName;
	private static GameObject srcObj;

	[MenuItem ("Object Copier/Copy Global Transform")]
	static void CopyPRS()
	{
		position = Selection.activeTransform.position;
		rotation = Selection.activeTransform.rotation;
		
		// Not copying scale
		//scale = Selection.activeTransform.localScale;
		
		objName = Selection.activeTransform.name;       

		Debug.Log("PRS of " + objName + " is copied");
	}

	[MenuItem ("Object Copier/Paste Global Transform")]
	static void PastePRS()
	{
		Selection.activeTransform.position = position;
		Selection.activeTransform.rotation = rotation;
		
		// Not pasteing scale
		//Selection.activeTransform.scale = localScale;

		Debug.Log("PRS of " + objName + " is pasted");        
	}

	[MenuItem ("Object Copier/Copy Local Transform")]
	static void CopyLocalPRS()
	{
		position = Selection.activeTransform.localPosition;
		rotation = Selection.activeTransform.localRotation;
		scale = Selection.activeTransform.localScale;
		objName = Selection.activeTransform.name;       

		Debug.Log("Local PRS of " + objName + " is copied");
	}

	[MenuItem ("Object Copier/Paste Local Transform")]
	static void PasteLocalPRS()
	{
		Selection.activeTransform.localPosition = position;
		Selection.activeTransform.localRotation = rotation;
		Selection.activeTransform.localScale = scale;      

		Debug.Log("Local PRS of " + objName + " is pasted");        
	}
	
	[MenuItem ("Object Copier/Copy Object with Local Transform")]
	static void CopyLocalObj()
	{
		position = Selection.activeTransform.localPosition;
		rotation = Selection.activeTransform.localRotation;
		scale = Selection.activeTransform.localScale;
		objName = Selection.activeTransform.name;
		srcObj = Selection.activeGameObject;

		Debug.Log("Object " + objName + " is copied");
	}
	
	[MenuItem ("Object Copier/Paste Object with Local Transform")]
	static void PasteLocalObj()
	{
		GameObject newObj = (GameObject)Object.Instantiate(srcObj);	
		newObj.name = objName;
	
		newObj.transform.parent = Selection.activeGameObject.transform;

		newObj.transform.localPosition = position;
		newObj.transform.localRotation = rotation;		
		newObj.transform.localScale = scale;				
			
		Debug.Log("Object " + objName + " is pasted");  
	}

	
}