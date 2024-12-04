#if ANDROID
using Android;
using Android.Content.PM;
using AndroidX.Core.App;
using AndroidX.Core.Content;
using Android.Content;
using Application = Android.App.Application;
using FileProvider = AndroidX.Core.Content.FileProvider;
#endif

namespace MauiAppVisit.Platforms.Android
{
    public static class AndroidUtils
    {
        public static void GrantedPermission()
        {
            // this will run for Android 33 and greater
            if (DeviceInfo.Platform == DevicePlatform.Android && OperatingSystem.IsAndroidVersionAtLeast(33))
            {
            #if ANDROID
                var activity = Platform.CurrentActivity ?? throw new NullReferenceException("Current activity is null");
                if (ContextCompat.CheckSelfPermission(activity, Manifest.Permission.ReadExternalStorage) != Permission.Granted)
                {
                    ActivityCompat.RequestPermissions(activity, new[] { Manifest.Permission.ReadExternalStorage }, 1);
                }
            #endif
            }
        }

        public static async Task SaveApkFromStreamAsync(Stream stream)
        {
            var filePath = Path.Combine(Application.Context.CacheDir.Path, "update.apk");

            await using var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None);
            await stream.CopyToAsync(fileStream);

            InstallApk(filePath);
        }

        private static void InstallApk(string filePath)
        {
            var file = new Java.IO.File(filePath);
            var fileUri = FileProvider.GetUriForFile(Application.Context, $"{Application.Context.ApplicationContext.PackageName}.fileprovider", file);

            var intent = new Intent(Intent.ActionView);
            intent.SetDataAndType(fileUri, "application/vnd.android.package-archive");
            intent.AddFlags(ActivityFlags.GrantReadUriPermission | ActivityFlags.NewTask);
            Application.Context.StartActivity(intent);
        }
    }
}
