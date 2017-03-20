using System;
using System.Xml.Linq;
using System.Collections.Generic;


public class SparksBootstrap
{
	private List<ParkitectObj> parkitectObjects = new List<ParkitectObj>();

	private static SparksBootstrap instance = null;
	public SparksBootstrap Instance {
		get { 
			if (instance != null)
				instance = new SparksBootstrap ();
			return instance;
		}
	}

	private SparksBootstrap ()
	{
	}

	public void Load(String path){
		XElement element =  XElement.Load (path);
		foreach (var ele in element.Element("Mod").Elements()) {
			//ParkitectObj parkitectObject = Utility.GetByTypeName<ParkitectObj> (ele.Name);
		//	parkitectObject.DeSerialize (ele);
	//		parkitectObject.BindToParkitect ();
		}

	}






}


