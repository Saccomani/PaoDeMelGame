using Android.App;
using Android.Widget;
using Android.OS;
using Android.Content.PM;
using Microsoft.Xna.Framework;
using Android.Views;

namespace myGame
{

    [Activity(Label = "myGame",
		MainLauncher = true,
		AlwaysRetainTaskState = true,
		LaunchMode = LaunchMode.SingleInstance,
		ScreenOrientation = ScreenOrientation.Landscape,
		ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.KeyboardHidden | ConfigChanges.Keyboard)]
	public class MainActivity : AndroidGameActivity
	{

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			var g = new Inicio();
			SetContentView((View)g.Services.GetService(typeof(View)));
			g.Run();

		}
	}
}

