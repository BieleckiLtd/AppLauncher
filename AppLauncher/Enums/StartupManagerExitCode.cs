namespace AppLauncher.Enums
{
    public enum StartupManagerExitCode
    {
        Unknown,
        Success, // can run
        UpdateFailedButNotRequired, // can run
        UpdateRequiredButFailed, // can run, but user did not allow
        InstallationFailed, // cannot run
    }
}
