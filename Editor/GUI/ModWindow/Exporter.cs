using System;
using UnityEditor;
using System.Collections.Generic;
using System.Xml.Linq;

public class Exporter
{
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

	}
}

