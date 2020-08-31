using Android.App;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

// FontAwesome Assemblies
[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
[assembly: ExportFont("fa-solid-900.ttf", Alias = "FA-solid")]

// Geolocation Assemblies
[assembly: UsesPermission(Android.Manifest.Permission.AccessCoarseLocation)]
[assembly: UsesPermission(Android.Manifest.Permission.AccessFineLocation)]
[assembly: UsesFeature("android.hardware.location", Required = false)]
[assembly: UsesFeature("android.hardware.location.gps", Required = false)]
[assembly: UsesFeature("android.hardware.location.network", Required = false)]