using System;
using UnityEditor;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using System.IO;
using MiniJSON;

public class Exporter
{

	public static void Export(ModPayload payload)
	{
		string path = "Assets/Mods/" + payload.ModName;
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
		//payload.GetAssetbundlePaths(paths);
		//transform assets to pk-refs
		if (AssetDatabase.IsValidFolder("Assets/Resources/Packs"))
		{
			AssetDatabase.DeleteAsset("Assets/Resources/Packs");
		}
		AssetDatabase.CreateFolder("Assets/Resources","Packs");
		
		for (int x = 0; x <	payload.ParkitectObjs.Count; x++)
		{
			ParkitectObj parkitectObj = payload.ParkitectObjs[x];
			parkitectObj.UpdatePrefab();
			String p = "Assets/Resources/Packs/" + parkitectObj.Key + ".prefab";
			AssetDatabase.CopyAsset(AssetDatabase.GetAssetPath(parkitectObj.Prefab),p);
			paths.Add(p);
			
			paths.AddRange(parkitectObj.getAssetPaths());
		}
		
		bundle.assetNames = paths.ToArray ();
		AssetDatabase.CreateFolder ("Assets/Mods", payload.ModName);
		
#if UNITY_STANDALONE_OSX
		BuildPipeline.BuildAssetBundles (path ,new AssetBundleBuild[]{bundle}, BuildAssetBundleOptions.ForceRebuildAssetBundle | BuildAssetBundleOptions.DeterministicAssetBundle | BuildAssetBundleOptions.UncompressedAssetBundle | BuildAssetBundleOptions.StrictMode, BuildTarget.StandaloneOSX);
#elif UNITY_STANDALONE_WIN
		BuildPipeline.BuildAssetBundles (path ,new AssetBundleBuild[]{bundle}, BuildAssetBundleOptions.ForceRebuildAssetBundle | BuildAssetBundleOptions.DeterministicAssetBundle | BuildAssetBundleOptions.UncompressedAssetBundle | BuildAssetBundleOptions.StrictMode, BuildTarget.StandaloneWindows);
#elif UNITY_STANDALONE_LINUX
		BuildPipeline.BuildAssetBundles (path ,new AssetBundleBuild[]{bundle},BuildAssetBundleOptions.ForceRebuildAssetBundle | BuildAssetBundleOptions.DeterministicAssetBundle | BuildAssetBundleOptions.UncompressedAssetBundle | BuildAssetBundleOptions.StrictMode, BuildTarget.StandaloneLinux);
#endif
		
		File.WriteAllText(path + "/" + payload.ModName + ".spark", MiniJSON.Json.Serialize(ModPayload.Instance.Serialize ()));

		
		string targetDir = ""; 

		if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer) 
			targetDir = Environment.GetFolderPath (Environment.SpecialFolder.MyDocuments) + "/Parkitect/Mods";	
		
		if (!Directory.Exists (targetDir)) 
			targetDir = EditorUtility.OpenFolderPanel( "Select Parkitect Mods folder", "", "" );

		if (targetDir.Length != 0 && (targetDir.EndsWith ("Mods/")||targetDir.EndsWith ("Mods"))) {

			String modPath = targetDir + "/" + payload.ModName + "/";
			Directory.CreateDirectory (modPath);

			String sourcePath = Application.dataPath + "/Mods/" + payload.ModName;
			Debug.Log (sourcePath);

			foreach (string newPath in Directory.GetFiles(sourcePath, "*.*",SearchOption.AllDirectories))
				File.Copy(newPath,modPath + Path.GetFileName(newPath), true);
			
			File.Copy(Application.dataPath+"/Parkitect-Mod-Spark/ParkitectModMiniBoot.dll",modPath +"/ParkitectModMiniBoot.dll", true);
			
		}

	}
}

