public bool RunCommand()
{
    try
    {
        var procStartInfo = new ProcessStartInfo("exec", "cal")
        {
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = false
        };

        var proc = new Process { StartInfo = procStartInfo };
        proc.Start();

        // Get the output into a string
        output = proc.StandardOutput.ReadToEnd();

        return proc.ExitCode == decimal.Zero ? true : false;
    }
    finally
    {
        // do something
    }
}