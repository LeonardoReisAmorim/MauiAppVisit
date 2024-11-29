#if ANDROID
using Android;
using Android.Content.PM;
using AndroidX.Core.App;
using AndroidX.Core.Content;
using Android.OS;
using Android.Content;
using Android.Provider;
using System.IO;
using Environment = Android.OS.Environment;
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

        public static void ListFilesInDownloadFolder()
        {
            var activity = Platform.CurrentActivity ?? throw new NullReferenceException("Current activity is null");

            if (Build.VERSION.SdkInt >= BuildVersionCodes.R)
            {
                if (!Environment.IsExternalStorageManager)
                {
                    Intent intent = new Intent(Settings.ActionManageAllFilesAccessPermission);
                    activity.StartActivity(intent);
                }
            }
            else
            {
                if (ContextCompat.CheckSelfPermission(activity, Manifest.Permission.ReadExternalStorage) != Permission.Granted ||
                ContextCompat.CheckSelfPermission(activity, Manifest.Permission.WriteExternalStorage) != Permission.Granted)
                {
                    ActivityCompat.RequestPermissions(activity, new string[] {
                    Manifest.Permission.ReadExternalStorage,
                    Manifest.Permission.WriteExternalStorage
                }, 1);
                }
            }
            
            //if (ContextCompat.CheckSelfPermission(activity, Manifest.Permission.ReadExternalStorage) != Permission.Granted ||
            //ContextCompat.CheckSelfPermission(activity, Manifest.Permission.WriteExternalStorage) != Permission.Granted)
            //{
            //    ActivityCompat.RequestPermissions(activity, new string[] { Manifest.Permission.ReadExternalStorage, Manifest.Permission.WriteExternalStorage }, 1);
            //}

            //var docsDirectory = Application.Context.GetExternalFilesDir(Environment.DirectoryDcim);

            var downloadsPath = Platform.CurrentActivity.Application.GetExternalFilesDir(Environment.DirectoryDownloads).AbsoluteFile.Path;
            var apkFile = Path.Combine(downloadsPath, "Profile.pdf");
            var a = File.OpenRead(apkFile);
            //var files = Directory.GetFiles(downloadsPath); 
            //foreach (var file in files) 
            //{ 

            //}

            //List<string> filePaths = new List<string>();
            //using (var collection = MediaStore.Downloads.ExternalContentUri)
            //{

            //}
        }
    }
}
