# AppLauncher
## Office365-style desktop application launcher (splash screen)

If you develop and/or maintain desktop applications in corporate environment this small app will help you distribute your applications and updates to your work colleagues. This is an alternative to ClickOnce and Squirrel.

## Main advantages
- If your application does not require elevated permissions to run nor should the installer. AppLauncher extracts package with your app to `<LocalFolder>DeveloperName\ActualApplication</LocalFolder>` in C:\Users\<User>\ folder.
- When you update your app you want users to run new version as soon as possible, not an old version and wait for pull to happen when they close it. AppLauncher checks for updates upon launch and if the new package is found, it is pulled, updated and then launched.
- You don't have to make big ceremony when you want to push a quick update. No need to prepare nuget package etc., just zip your release and place it in the shared network drive.
- You can configure everything from attached config.xml, without recompiling launcher itself. For example `<ExecutableFile>ActualApplication.exe</ExecutableFile>` is exe from your package root that is being launched after extraction. Prior that AppLauncher checks for new `<RemotePackage>C:\remote\ActualApplication.zip</RemotePackage>` and pulls it if necessary.
- You can also configure what happens if it's not been possible to check for updates (for example when remote folder is not available). You can allow to run current version user has installed or display error message - `<RequireLatestVersion>true</RequireLatestVersion>`

## So how does it works
AppLauncher runs on .Net Framework 4.7 and WPF for nice animation and does not use any 3rd party libraries. This allows solution to builds only one, small executable and config.xml file. Both files should be placed on the network drive and users should be pointed to that executable. Optionally you can also add .ico of your choice and create shortcut to launcher's exe file, use your own icon, then share just shortcut instead.

You can easily change target framework version depending on what is installed on your corporate computers.

## To get started
- Build this solution,
- set up config.xml to suit your needs,
- place both files on the network drive,
- create shortcut on your colleagues desktops.

Because launcher files are very small it smoothly starts off of network drive.

When you want to update your actual application build it, zip it, copy to network drive and optionally update config.xml. Launcher ensures users of your application will get latest version before it will be opened.

![Screenshot](/Docs/images/screen1.png)

## Visual Appearance 
You only need to change couple lines of code to achive different looks. No need to recompile the launcher.

![Screenshot](/Docs/images/screen3.png)

This gives you 

![Screenshot](/Docs/images/screen2.png)

Note, this program is not associated with Microsoft in any way. Microsoft logo is only used for demonstration purposes. Microsoft logo svg source: [Wikipedia](https://upload.wikimedia.org/wikipedia/commons/9/96/Microsoft_logo_%282012%29.svg)
