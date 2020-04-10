using AppLauncher.Enums;
using AppLauncher.Models;
using AppLauncher.Utils;
using AppLauncher.ViewModels;
using System;
using System.Diagnostics;
//using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Threading;
using System.Threading.Tasks;

namespace AppLauncher.Utils
{
    public class StartupManager
    {
        private IConfigurationProvider ConfigurationProvider { get; }
        private IStatusNotifier StatusNotifier { get; }
        public StartupManager(IConfigurationProvider configurationProvider, IStatusNotifier statusNotifier)
        {
            ConfigurationProvider = configurationProvider ?? throw new ArgumentNullException(nameof(configurationProvider));
            StatusNotifier = statusNotifier ?? throw new ArgumentNullException(nameof(statusNotifier));
        }

        readonly bool isSlowModeEnabled = false; // true for debugging

        /// <summary>
        /// Asynchronously controls flow of a startup
        /// </summary>
        public async Task<StartupManagerExitCode> ManageStartupAsync()
        {
            StartupManagerExitCode exitCode;
            var cp = ConfigurationProvider;
            var sn = StatusNotifier;

            sn.IsWorking = true;
            sn.Print("Checking current installation...");
            var isInstalled = await Task.Run(() =>
            {
                if (isSlowModeEnabled) Thread.Sleep(2000);
                return File.Exists(Path.Combine(cp.InstallationDirectory, cp.ExecutableFile));
            });

            if (isInstalled)
            {
                sn.Print("Checking for updates...");
            }
            else
            {
                sn.Print("Installing...");
            }

            var isUpToDate = await Task.Run(() => { return GetLatest(cp); });

            if (!isUpToDate)
            {
                if (isInstalled)
                {
                    if (cp.RequireLatestVersion)
                    {
                        exitCode = StartupManagerExitCode.UpdateRequiredButFailed;
                    }
                    else
                    {
                        exitCode = StartupManagerExitCode.UpdateFailedButNotRequired;
                    }
                }
                else
                {
                    exitCode = StartupManagerExitCode.InstallationFailed;
                }
                
            }
            else
            {
                exitCode = StartupManagerExitCode.Success;
            }
            sn.IsWorking = false;
            return exitCode;
        }

        private bool GetLatest(IConfigurationProvider configurationProvider)
        {
            var isSuccess = false;

            Logger.Log("Confirming if local is up to date");
            var isLocalUpToDate = IsLocalUpToDate(configurationProvider);

            if (isLocalUpToDate) // Confirmed that local is up to date
            {
                Logger.Log("Confirmed that local is up to date");
                isSuccess = true;
            }
            else // Could not confirm that local is up to date
            {
                Logger.Log("Could not confirm that local is up to date, checking if remote package exists...");
                if (File.Exists(configurationProvider.RemotePackage))
                {
                    Logger.Log("...Remote package exists, attempting to update");
                    // prepare empty localDir, backup current files
                    (var isLocalDirReady, var backupDir) = BackupLocalDir(configurationProvider);
                    if (isLocalDirReady)
                    {
                        // prepare local package
                        (var isLocalPackageReady, var localPackage) = CopyPackageLocally(configurationProvider);
                        if (isLocalPackageReady)
                        {
                            // extract local package
                            var isExtracted = ExtractPackage(localPackage);
                            if (isExtracted)
                            {
                                isSuccess = true;
                            }
                            else
                            {
                                // clean local dir and revert from backup
                                RevertFromBackup(configurationProvider, backupDir);
                            }
                        }
                    }
                }
                else
                {
                    Logger.Log($"...Remote package ({configurationProvider.RemotePackage}) does not exists.");
                }

            }


            if (isSlowModeEnabled) Thread.Sleep(2000);
            return isSuccess;
        }


        private void RevertFromBackup(IConfigurationProvider configurationProvider, string backupDir)
        {
            var localDir = configurationProvider.InstallationDirectory;
            // move old files from backup dir
            try
            {
                Logger.Log("Clean local directory");
                EraseAll(localDir);
                Logger.Log("Moving old files from backup to local directory");
                MoveAll(backupDir, localDir);
                Logger.Log($"Old files are put back to local directory.");
            }
            catch (Exception e)
            {
                // Old files could not be moved back from backup dir.
                Logger.Log($"This is serious. Old files could not be moved back from backup directory:\n{e.ToString()}");
            }
        }
        private static (bool isSuccess, string localPackage) CopyPackageLocally(IConfigurationProvider configurationProvider)
        {
            var remotePackage = configurationProvider.RemotePackage;
            var localDir = configurationProvider.InstallationDirectory;
            var localPackage = Path.Combine(localDir, Path.GetFileName(remotePackage));
            try
            {
                Logger.Log("Trying to copy remote package to localDir");
                File.Copy(remotePackage, localPackage);
                return (true, localPackage);
            }
            catch (Exception e)
            {
                Logger.Log($"Remote package could not be copied to localDir:\n{e.ToString()}");
                return (false, null);
            }
        }
        private static bool ExtractPackage(string packageLocation)
        {
            var extractionDir = Path.GetDirectoryName(packageLocation);
            try
            {
                Logger.Log("Trying to extract local package");
                ZipFile.ExtractToDirectory(packageLocation, extractionDir);
                Logger.Log("Local package extracted");
                return true;
            }
            catch (Exception e)
            {
                Logger.Log($"Local package could not be extracted:\n{e.ToString()}");
                return false;
            }
        }
        private static bool IsLocalUpToDate(IConfigurationProvider configurationProvider)
        {
            var isLocalUpToDate = false; // assume local package is not up to date
            string remotePackage = configurationProvider.RemotePackage;
            string localDir = configurationProvider.InstallationDirectory;

            var remoteFileName = Path.GetFileName(remotePackage);
            var localPackage = Path.Combine(localDir, remoteFileName);

            // Check packages

            var remotePackageInfo = new FileInfo(remotePackage);
            var localPackageInfo = new FileInfo(localPackage);
            if (localPackageInfo.Exists && remotePackageInfo.Exists &&
                localPackageInfo.LastWriteTimeUtc == remotePackageInfo.LastWriteTimeUtc)
            {
                isLocalUpToDate = true;
            }

            return isLocalUpToDate;
        }

        /// <summary>
        /// Tries to backup currently installed application.
        /// </summary>
        /// <param name="configurationProvider"></param>
        /// <returns> Returns tuple with success (true or false) and if success also path to backup directory </returns>
        private static (bool isSuccess, string backupDir) BackupLocalDir(IConfigurationProvider configurationProvider)
        {
            (bool, string) result = (false, null);
            var localDir = configurationProvider.InstallationDirectory;

            // ensure local dir exists
            try
            {
                Logger.Log($"Creating local installation directory (unless it already exist) {localDir}");
                Directory.CreateDirectory(localDir);
            }
            catch (Exception e)
            {
                Logger.Log($"Could not create local installation directory:\n{e.ToString()}");
                return result;
            }

            var tempDir = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            // ensure temp dir exists
            try
            {
                Logger.Log($"Creating temp backup directory {tempDir}");
                Directory.CreateDirectory(tempDir);
            }
            catch (Exception e)
            {
                Logger.Log($"Could not create local installation directory:\n{e.ToString()}");
                return result;
            }

            // move old files to backup dir
            try
            {
                
                Logger.Log("Moving old files to temp dir");
                MoveAll(localDir, tempDir);
                Logger.Log("Old files are in temp dir");
                result = (true, tempDir);
            }
            catch (Exception e)
            {
                // Old files could not be moved to backup dir.
                Logger.Log($"Old files could not be moved to backup directory:\n{e.ToString()}");

                // Put the files back.
                MoveAll(tempDir, localDir);
                Logger.Log($"Old files are put back to local directory.");
            }
            
            return result;
        }
        
        /// <summary>
        /// Use with caution! Moves all files and folders from location to location.
        /// </summary>
        /// <param name="from">Location from which all files and folders will be moved.</param>
        /// <param name="to">Location to which all files and folders will be moved.</param>
        private static void MoveAll(string from, string to)
        {
            var fromDirectory = new DirectoryInfo(from);
            foreach (var file in fromDirectory.GetFiles())
            {
                file.MoveTo(Path.Combine(to, file.Name));
            }
            foreach (var dir in fromDirectory.GetDirectories())
            {
                dir.MoveTo(Path.Combine(to, dir.Name));
            }
        }

        /// <summary>
        /// Use with caution! Removes all files and folders from location.
        /// </summary>
        /// <param name="location">Location from which all files and folders will be deleted.</param>
        private static void EraseAll(string location)
        {
            var directory = new DirectoryInfo(location);
            foreach (var file in directory.GetFiles())
            {
                file.Delete();
            }
            foreach (var dir in directory.GetDirectories())
            {
                dir.Delete(true);
            }
        }

        /// <summary>
        /// Starts process and returns it to the caller.
        /// </summary>
        public static Process StartProcess(IConfigurationProvider cp)
        {
            var executableFilePath = Path.Combine(cp.InstallationDirectory, cp.ExecutableFile);

            Process process = new Process();
            // Configure the process using the StartInfo properties.
            process.StartInfo.FileName = executableFilePath;
            process.Start();
            return process;
        }
    }
}
