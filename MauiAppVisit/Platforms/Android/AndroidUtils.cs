#if ANDROID
using Android;
using Android.Content.PM;
using AndroidX.Core.App;
using AndroidX.Core.Content;
using Android.Content;
using Application = Android.App.Application;
using FileProvider = AndroidX.Core.Content.FileProvider;
using MauiAppVisit.Helpers;
using MauiAppVisit.Model;
using System.Text.Json;
#endif

namespace MauiAppVisit.Platforms.Android
{
    public static class AndroidUtils
    {
        private static readonly string _basePath = Application.Context.GetExternalFilesDir(null).AbsolutePath;
        private static readonly PackageManager _packageManager = Application.Context.PackageManager;

        public static void CreateFileJsonFileVRVersion()
        {
            FileHelper.CreateFileJSONInDevice(_basePath);
        }

        //public static bool NeedUpdateAPK(FileVrDetails fileVrDetails)
        //{
        //    var contentFile = FileHelper.ReadJsonVRVersion(_basePath);
        //    if (string.IsNullOrWhiteSpace(contentFile)) return false;

        //    var fileVrDetailsListInDevice = JsonSerializer.Deserialize<List<FileVRInstalled>>(contentFile);
        //    return fileVrDetailsListInDevice.Any(filevrdevice => filevrdevice.fileName.Equals(fileVrDetails.fileName, StringComparison.OrdinalIgnoreCase) && !DateHelper.AreEquals(fileVrDetails.updatedAt, filevrdevice.updatedAt));
        //}

        public static bool NeedUpdateAPK(FileVrDetails fileVrDetails)
        {
            List<FileVRInstalled> fileVrDetailsList = [];
            bool needUpdateAPK = false;
            var contentFile = FileHelper.ReadJsonVRVersion(_basePath);
            if (!string.IsNullOrWhiteSpace(contentFile))
            {
                fileVrDetailsList = JsonSerializer.Deserialize<List<FileVRInstalled>>(contentFile);

                if(fileVrDetailsList.Any(filevrdevice => filevrdevice.fileName.Equals(fileVrDetails.fileName, StringComparison.OrdinalIgnoreCase) && DateHelper.AreEquals(fileVrDetails.updatedAt, filevrdevice.updatedAt)))
                {
                    return needUpdateAPK;
                }

                var itemRemoveFileVR = fileVrDetailsList.FirstOrDefault(filevrdevice => filevrdevice.fileName.Equals(fileVrDetails.fileName, StringComparison.OrdinalIgnoreCase) && !DateHelper.AreEquals(fileVrDetails.updatedAt, filevrdevice.updatedAt));
                if (itemRemoveFileVR is not null)
                {
                    needUpdateAPK = true;
                    fileVrDetailsList.Remove(itemRemoveFileVR);
                }
                    
                fileVrDetailsList.Add(new FileVRInstalled
                {
                    fileName = fileVrDetails.fileName,
                    updatedAt = fileVrDetails.updatedAt
                });
            }
            else
            {
                fileVrDetailsList.Add(new FileVRInstalled
                {
                    fileName = fileVrDetails.fileName,
                    updatedAt = fileVrDetails.updatedAt
                });
            }
            FileHelper.WriteJsonVRVersion(_basePath, JsonSerializer.Serialize(fileVrDetailsList));
            return needUpdateAPK;
        }

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
            var filePath = Path.Combine(_basePath, filename);
            try
            {
                var packageInfo = _packageManager.GetPackageArchiveInfo(filePath, PackageInfoFlags.Activities) ?? throw new NullReferenceException();
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

        public static bool HasAppInstalledButton(string filename)
        {
            var packages = _packageManager.GetInstalledApplications(PackageInfoFlags.MetaData);
            return packages.Any(p => p.LoadLabel(_packageManager).ToString().Equals(filename, StringComparison.OrdinalIgnoreCase));
        }

        public static async Task DownloadApk(Stream stream, string filename)
        {
            var filePath = Path.Combine(_basePath, filename);
            await SaveApkFromStreamAsync(stream, filePath);
            InstallApk(null, filePath);
        }

        public static bool HasAppInDevice(string filename)
        {
            var filePath = Path.Combine(_basePath, filename);
            return File.Exists(filePath);
        }

        public static void DeleteAppInDevice(string filename)
        {
            var filePath = Path.Combine(_basePath, filename);
            File.Delete(filePath);
        }

        private static async Task SaveApkFromStreamAsync(Stream stream, string filePath)
        {
            await using var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None);
            await stream.CopyToAsync(fileStream);
        }

        public static void InstallApk(string filename, string filepathDownloadApk = null)
        {
            var filePathInstall = string.Empty;
            if (!string.IsNullOrWhiteSpace(filename))
            {
                filePathInstall = Path.Combine(_basePath, filename);
            }

            var file = new Java.IO.File(filepathDownloadApk ?? filePathInstall);
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
