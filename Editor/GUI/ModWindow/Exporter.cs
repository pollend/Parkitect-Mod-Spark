using System;
using UnityEditor;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using System.IO;

public class Exporter
{
	public const string GIT_IGNORE ="";

	public static void Export(ModPayload payload)
	{
		string path = "Assets/Mods/" + payload.modName;
		if (AssetDatabase.IsValidFolder(path))
		{
			AssetDatabase.DeleteAsset(path);
		}
		else
		{
			if (!AssetDatabase.IsValidFolder("Assets/Mods"))
			{
				AssetDatabase.CreateFolder("Assets", "Mods");
			}
		}

		AssetBundleBuild bundle = new AssetBundleBuild ();
		bundle.assetBundleName = "assetbundle";



		List<string> paths = new List<string> ();
		payload.GetAssetbundlePaths(paths);
		bundle.assetNames = paths.ToArray ();
		AssetDatabase.CreateFolder ("Assets/Mods", payload.modName);
		BuildPipeline.BuildAssetBundles ("Assets/Mods/" + payload.modName ,new AssetBundleBuild[]{bundle}, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);

		var mod = new XElement ("Mod", ModPayload.Instance.Serialize ());
		mod.Save (path + "/mod.xml");

		AssetDatabase.DeleteAsset ("Assets/Mods/" + payload.modName + "/assetbundle");
		AssetDatabase.DeleteAsset ("Assets/Mods/" + payload.modName + "/assetbundle.manifest");

		string targetDir = ""; 

		if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer) 
			targetDir = Environment.GetFolderPath (Environment.SpecialFolder.MyDocuments) + "/Parkitect/Mods";	
		
		if (!Directory.Exists (targetDir)) 
			targetDir = EditorUtility.OpenFolderPanel( "Select Parkitect Mods folder", "", "" );

		if (targetDir.Length != 0 && (targetDir.EndsWith ("Mods/")||targetDir.EndsWith ("Mods"))) {

			String modPath = targetDir + "/" + payload.modName;
			Directory.CreateDirectory (modPath);
			Directory.CreateDirectory (modPath + "/data");
		

			String sourcePath = Application.dataPath + "/Mods/" + payload.modName;
			Debug.Log (sourcePath);

			foreach (string newPath in Directory.GetFiles(sourcePath, "*.*",SearchOption.AllDirectories))
				File.Copy(newPath,modPath + Path.GetFileName(newPath), true);

			/*if(!File.Exists(modPath+ "/.gitignore"))
				File.WriteAllText (modPath+ "/.gitignore",PackagedFiles.GIT_IGNORE);
			if(!File.Exists(modPath+ "/" + payload.modName + ".sln"))
				File.WriteAllText (modPath+ "/" + payload.modName + ".sln",PackagedFiles.SLN.Replace("%1",payload.modName).Replace("%2",payload.modName));
			if(!File.Exists(modPath+ "/"  + payload.modName + ".csproj"))
				File.WriteAllText (modPath+ "/"  + payload.modName + ".csproj",PackagedFiles.CSPROJ.Replace("%1",payload.modName).Replace("%2",payload.modName));

			File.WriteAllText (modPath+ "/Main.cs",PackagedFiles.MAIN.Replace("%1",payload.modName).Replace("%2",payload.description));
			File.WriteAllText (modPath+ "/mod.json",PackagedFiles.MOD_JSON.Replace("%1",payload.modName).Replace("%2",payload.modName));*/
		}

	}
}

