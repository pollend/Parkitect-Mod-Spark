using System;
using System.Collections.Generic;

	public class DecoratorList
	{

		private Dictionary<Type,IDecoratorView> decoratorViews = new Dictionary<Type,IDecoratorView>();
		private ModObjectsList modObjectsPresenter;

		public DecoratorList (ModObjectsList modObjectsList)
		{
			this.modObjectsPresenter = modObjectsPresenter;
			RegisterView<ColorDecorator> (new DecoratorColorView ());
			RegisterView<BaseDecorator> (new DecoratorBasicView ());
		}

		private void RegisterView<T>(IDecoratorView decoratorView) 
		{
			decoratorViews.Add (typeof(T), decoratorView);
		}


		public IDecoratorView[] GetDecorator<T>(ParkitectObj parkitecObj) where T: Decorator
		{
			Type[] types = parkitecObj.SupportedDecorators ();

		List<IDecoratorView> views = new List<IDecoratorView>();

			for (int x = 0; x < types.Length; x++) {
			IDecoratorView view;
				if(decoratorViews.TryGetValue (types [x], out view))
					views.Add(view);
			}
			return views.ToArray();
		}

		public void Render()
		{
		}

	}


