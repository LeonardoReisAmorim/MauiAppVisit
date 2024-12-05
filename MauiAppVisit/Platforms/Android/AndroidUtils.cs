#if ANDROID
using Android;
using Android.Content.PM;
using AndroidX.Core.App;
using AndroidX.Core.Content;
using Android.Content;
using Application = Android.App.Application;
using FileProvider = AndroidX.Core.Content.FileProvider;
using Android.OS;
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

        public static bool VerifyAppInstaled(string filename)
        {
            var filePath = Path.Combine(Application.Context.GetExternalFilesDir(null).AbsolutePath, filename);
            var packageManager = Application.Context.PackageManager;

            try
            {
                var packageInfo = packageManager.GetPackageArchiveInfo(filePath, PackageInfoFlags.Activities) ?? throw new NullReferenceException();
                InicializeAPK(packageInfo?.PackageName, packageInfo);
                return true;
            }
            catch (ActivityNotFoundException)
            {
                return false;
            }
            catch (NullReferenceException)
            {
                return false;
            }
        }

        public static bool VerifyAppInstaledButton(string filename)
        {
            var filePath = Path.Combine(Application.Context.GetExternalFilesDir(null).AbsolutePath, filename);
            var packageManager = Application.Context.PackageManager;

            try
            {
                var packageInfo = packageManager.GetPackageArchiveInfo(filePath, PackageInfoFlags.MetaData);
                //var p = packageManager.GetPackageInfo(packageInfo?.PackageName, PackageInfoFlags.MetaData);

                return true;
            }
            catch (ActivityNotFoundException)
            {
                return false;
            }
            catch (NullReferenceException)
            {
                return false;
            }
        }

        public static async Task VerifyAppDownload(Stream stream, string filename)
        {
            var filePath = Path.Combine(Application.Context.GetExternalFilesDir(null).AbsolutePath, filename);

            if (!File.Exists(filePath))
            {
                await SaveApkFromStreamAsync(stream, filePath);
                InstallApk(filePath);
            }
            else
            {
                InstallApk(filePath);
            }
        }

        private static async Task SaveApkFromStreamAsync(Stream stream, string filePath)
        {
            await using var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None);
            await stream.CopyToAsync(fileStream);
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

        private static void InicializeAPK(string packageName, PackageInfo packageInfo)
        {
            if (packageInfo?.Activities != null && packageInfo.Activities.Any())
            {
                var activityInfo = packageInfo.Activities.First();
                var componentName = new ComponentName(packageName, activityInfo.Name);

                var intent = new Intent(Intent.ActionMain);
                intent.AddCategory(Intent.CategoryLauncher);
                intent.SetComponent(componentName);
                intent.SetFlags(ActivityFlags.NewTask);

                Application.Context.StartActivity(intent);
            }
        }
    }
}
