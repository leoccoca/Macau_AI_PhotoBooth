using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

class MyCustomBuildProcessor : IPostprocessBuildWithReport
{
    public int callbackOrder { get { return 0; } }
    public void OnPostprocessBuild(BuildReport report)
    {
		CopyConfigFile("ClientConfig.json", report);
    }
	
	private void CopyConfigFile(string filename, BuildReport report)
	{

		//Debug.Log("MyCustomBuildProcessor.OnPostprocessBuild for target " + report.summary.platform + " at path " + report.summary.outputPath);

		string destDirectoryName = Path.GetDirectoryName(report.summary.outputPath);

		string sourcePath = Application.dataPath + "/..";
		if (Application.platform == RuntimePlatform.OSXPlayer)
			sourcePath += "/../..";

		sourcePath += "/" + filename;

		string destPath = destDirectoryName + "\\" + filename;

		//Debug.Log("destDirectoryName:" + destDirectoryName);
		//Debug.Log("sourcePath:" + sourcePath);
		//Debug.Log("destPath:" + destPath);

		if (!File.Exists(destPath))
		{
			Debug.Log("====== Copy config file: " + destPath);

			System.IO.File.Copy(sourcePath, destPath, false);
		}	
	}
}
