# AppLauncher - runtime configurable, Office365-style application launcher (splash screen).

Installs, keeps up to date and launches application of your choice on client PC. Does not require elevated permissions (package is extracted in Users folder in `<LocalFolder>DeveloperName\ActualApplication</LocalFolder>`).

- Before actual application `<ExecutableFile>ActualApplication.exe</ExecutableFile>` is launched program checks for updates in `<RemotePackage>C:\remote\ActualApplication.zip</RemotePackage>`. If new package is found, update happens before launch.
- You can configure what happens if it's not possible to check for updates (for example when remote folder is not available, user is working offline, etc.). You can allow to run whichever version user has currently installed or display error message - `<RequireLatestVersion>true</RequireLatestVersion>`

This solution builds one, small, .Net Framework 4.7 dependent executable file with config.xml file. Both files should be placed on network drive and users should be pointed to the executable file. Optionally you can add .ico of your choice and create shortcut to launcher's exe file but use your own icon.

## To get started
- Build this solution,
- set up config.xml to your needs,
- place both files on the network drive,
- create shortcut on your colleagues desktops.

Because launcher files are very small it starts off very fast off network drive.

When you want to update your actual application build it, zip it, copy to network drive and update config.xml. Launcher ensures users of your application will get latest version before it will be opened.

![Screenshot](/Docs/images/screen1.png)

## Visual Appearance 
You only need to change couple lines of code to achive different looks. No need to recompile the launcher.

![Screenshot](/Docs/images/screen3.png)

This gives you 

![Screenshot](/Docs/images/screen2.png)

Note, this program is not associated with Microsoft in any way. Microsoft logo used for demonstration purposes only. Microsoft logo source: [Wikipedia](https://upload.wikimedia.org/wikipedia/commons/9/96/Microsoft_logo_%282012%29.svg)
