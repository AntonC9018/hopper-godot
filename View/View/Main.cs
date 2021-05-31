using Hopper.Core;

namespace Hopper.View
{
	public class ViewRegistryExtension : IRegistryExtension
	{
		public StaticRegistry<Godot.Sprite> Sprite = new StaticRegistry<Godot.Sprite>();
	}

	public static partial class Main
	{
		public static RegistryExtensionPath<ViewRegistryExtension> RegistryExtensionPath;

		public static ViewRegistryExtension GetViewExtension(this Registry registry)
			=> RegistryExtensionPath.Get(registry);

		private static void CustomInit()
		{
			Registry.Global.Extend(new ViewRegistryExtension());
		}
	}
}
