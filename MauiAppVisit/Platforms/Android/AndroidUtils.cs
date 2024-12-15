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

namespace MauiAppVisit.Platforms.Android
{
    public static class AndroidUtils
    {
        private static readonly string _basePath = Application.Context.GetExternalFilesDir(null).AbsolutePath;
        private static readonly PackageManager _packageManager = Application.Context.PackageManager;
        private static readonly JsonSerializerOptions _jsonSerializerOptions = JsonSerializeOptionHelper.Options;
        private static string _contentFileReadJsonVrVersion = string.Empty;

        public static bool ProcessFileVR(FileVrDetails fileVrDetails)
        {
            CreateFileJsonFileVRVersion();
            if(string.IsNullOrWhiteSpace(_contentFileReadJsonVrVersion)) _contentFileReadJsonVrVersion = FileHelper.ReadJsonVRVersion(_basePath);
            string filePath = Path.Combine(_basePath, $"{StringHelper.RemoveAccents(fileVrDetails.FileName.ToLower())}.apk");
            string packageName = GetNameApkFromPackageManager(filePath);

            if(string.IsNullOrWhiteSpace(packageName)) return false;
            
            bool needUpdateApk = NeedUpdateAPK(fileVrDetails, packageName);

            if (needUpdateApk)
            {
                DeleteAppInDevice(filePath);
                return false;
            }

            bool hasInstalled = VerifyAppInstaled(filePath);

            if (hasInstalled)
            {
                return true;
            }

            bool hasAppInDevice = HasAppInDevice(filePath);

            if (hasAppInDevice)
            {
                InstallApk(filePath);
                return true;
            }

            return false;
        }

        private static void CreateFileJsonFileVRVersion()
        {
            FileHelper.CreateFileJSONInDevice(_basePath);
        }

        public static bool NeedUpdateAPK(FileVrDetails fileVrDetails, string packageName)
        {
            List<FileVRInstalled> fileVrDetailsList = JsonSerializer.Deserialize<List<FileVRInstalled>>(_contentFileReadJsonVrVersion, _jsonSerializerOptions);

            if (fileVrDetailsList.Count <= 0) return false;

            if (fileVrDetailsList.Any(filevrdevice => filevrdevice.PackageName.Equals(packageName, StringComparison.OrdinalIgnoreCase) && DateHelper.AreEquals(fileVrDetails.UpdatedAt, filevrdevice.UpdatedAt)))
            {
                return false;
            }

            return true;
        }

        private static void CreateJsonVRVersion(FileVrDetails fileVrDetails, string packageName)
        {
            List<FileVRInstalled> fileVrDetailsList = [];
            if (!string.IsNullOrWhiteSpace(_contentFileReadJsonVrVersion))
            {
                fileVrDetailsList = JsonSerializer.Deserialize<List<FileVRInstalled>>(_contentFileReadJsonVrVersion, _jsonSerializerOptions);

                var itemRemoveFileVR = fileVrDetailsList.FirstOrDefault(filevrdevice => filevrdevice.PackageName.Equals(packageName, StringComparison.OrdinalIgnoreCase) && !DateHelper.AreEquals(fileVrDetails.UpdatedAt, filevrdevice.UpdatedAt));
                if (itemRemoveFileVR is not null)
                {
                    fileVrDetailsList.Remove(itemRemoveFileVR);
                }

                fileVrDetailsList.Add(new FileVRInstalled
                {
                    FileName = fileVrDetails.FileName,
                    UpdatedAt = fileVrDetails.UpdatedAt,
                    PackageName = packageName
                });
            }
            else
            {
                fileVrDetailsList.Add(new FileVRInstalled
                {
                    FileName = fileVrDetails.FileName,
                    UpdatedAt = fileVrDetails.UpdatedAt,
                    PackageName = packageName
                });
            }
            FileHelper.WriteJsonVRVersion(_basePath, JsonSerializer.Serialize(fileVrDetailsList));
        }

        public static void GrantedPermission()
        {
            if (DeviceInfo.Platform == DevicePlatform.Android && OperatingSystem.IsAndroidVersionAtLeast(33))
            {
                var activity = Platform.CurrentActivity ?? throw new NullReferenceException("Current activity is null");
                if (ContextCompat.CheckSelfPermission(activity, Manifest.Permission.ReadExternalStorage) != Permission.Granted)
                {
                    ActivityCompat.RequestPermissions(activity, new[] { Manifest.Permission.ReadExternalStorage }, 1);
                }
            }
        }

        public static bool VerifyAppInstaled(string filePath)
        {
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
            var packages = _packageManager.GetInstalledApplications(PackageInfoFlags.MetaData).AsParallel();
            return packages.Any(p => p.LoadLabel(_packageManager).ToString().Equals(filename, StringComparison.OrdinalIgnoreCase));
        }

        public static async Task DownloadApk(Stream stream, string filename, FileVrDetails fileVrDetails)
        {
            var filePath = Path.Combine(_basePath, filename);
            await SaveApkFromStreamAsync(stream, filePath);
            string packageName = InstallApk(filePath);
            CreateJsonVRVersion(fileVrDetails, packageName);
        }

        public static bool HasAppInDevice(string filePath)
        {
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

        public static string InstallApk(string filepathDownloadApk)
        {
            var file = new Java.IO.File(filepathDownloadApk);
            var fileUri = FileProvider.GetUriForFile(Application.Context, $"{Application.Context.ApplicationContext.PackageName}.fileprovider", file);

            var intent = new Intent(Intent.ActionView);
            intent.SetDataAndType(fileUri, "application/vnd.android.package-archive");
            intent.AddFlags(ActivityFlags.GrantReadUriPermission | ActivityFlags.NewTask);
            Application.Context.StartActivity(intent);

            return GetNameApkFromPackageManager(filepathDownloadApk);
        }

        private static string GetNameApkFromPackageManager(string filePath)
        {
            var packageInfo = _packageManager.GetPackageArchiveInfo(filePath, PackageInfoFlags.Activities);
            return packageInfo?.PackageName;
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
