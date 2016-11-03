using System;

public class ModObjectSingle
{
	public ModObjectSingle ()
	{
	}

	public void Render(ModObjectsList modObjectList)
	{
		ParkitectObj selected =  modObjectList.selectedParkitectObject;
		Type[] decorators =  selected.SupportedDecorators ();
		for (int x = 0; x < decorators.Length; x++) {
			Decorator decorator =  modObjectList.selectedParkitectObject.GetDecorator (decorators [x]);
			
		}
	}
}


