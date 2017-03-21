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
			targetDir = System.Environment.GetFolderPath (System.Environment.SpecialFolder.MyDocuments) + "/Parkitect/pnmods";	
		
		if (!System.IO.Directory.Exists (targetDir)) 
			targetDir = EditorUtility.OpenFolderPanel( "Select pnmods folder", "", "" );

		if (targetDir.Length != 0 && (targetDir.EndsWith ("pnmods/")||targetDir.EndsWith ("pnmods"))) {

			String modPath = targetDir + "/" + payload.modName;
			System.IO.Directory.CreateDirectory (modPath);
			System.IO.Directory.CreateDirectory (modPath + "/data");
		

			String sourcePath = Application.dataPath + "/Mods/" + payload.modName;
			UnityEngine.Debug.Log (sourcePath);

			foreach (string newPath in Directory.GetFiles(sourcePath, "*.*",SearchOption.AllDirectories))
				File.Copy(newPath,modPath + "/data/" + Path.GetFileName(newPath), true);
			
		}

	}
}

