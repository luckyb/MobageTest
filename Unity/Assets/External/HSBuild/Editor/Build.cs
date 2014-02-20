using UnityEditor;
using System;
using System.IO;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Debug = UnityEngine.Debug;
using System.Diagnostics;


public class Build : MonoBehaviour
{

    private static string[] scenes = new string[] {
													"Assets/Scenes/Level-1.unity"
                                                  };

	private static BuildOptions prodBuildOptions = BuildOptions.None | BuildOptions.ShowBuiltPlayer | BuildOptions.AcceptExternalModificationsToPlayer;

    protected static void RunBuild(BuildTarget target, BuildOptions options)
    {
        AppInfo.CheckForDefaults();

        string buildLocation = GetBuildDirectory();
        BuildCreateFolder(buildLocation, target);

        BuildUnityProject(target, options, buildLocation);

        BuildCopyIncludes(target);
    }

    private static void BuildCreateFolder(string buildLocation, BuildTarget target)
    {
        DirectoryInfo dirBuild = new DirectoryInfo(buildLocation);

        // if the directory exists, delete it.
        if (dirBuild.Exists)
        {
            Debug.Log("[BUILD] Cleaning Directory.");

            FileTools.MakeFilesWritable(dirBuild);

            if (target == BuildTarget.Android)
            {
                if (File.Exists(AppInfo.longName))
                    File.Delete(AppInfo.longName);
            }
            else
            {
                Directory.Delete(buildLocation, true);
            }
        }

        dirBuild.Create();
        Debug.Log("[BUILD] Created: " + Path.GetFullPath(buildLocation));
    }

    private static void BuildUnityProject(BuildTarget target, BuildOptions options, string buildLocation)
    {
        PlayerSettings.companyName = AppInfo.companyName;
        PlayerSettings.productName = AppInfo.longName;

        PlayerSettings.bundleIdentifier = AppInfo.appleBundleId;
        PlayerSettings.bundleVersion = AppInfo.fullVersion;

        List<string> buildScenes = new List<string>(scenes);

        // generally want player logs enabled in case something weird happens and we can get some
        // info from it. just don't be stupid with logging debug stuff on release.
        PlayerSettings.usePlayerLog = true;

        // platform specific options
        if (target == BuildTarget.Android)
        {
            PlayerSettings.defaultInterfaceOrientation = UIOrientation.LandscapeRight;
            PlayerSettings.Android.minSdkVersion = AndroidSdkVersions.AndroidApiLevel9;

            PlayerSettings.Android.bundleVersionCode += 1;

           // PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android, defineSymbols);
        }
        string buildResult = BuildPipeline.BuildPlayer(buildScenes.ToArray(), buildLocation, target, options);

        // error logs
        if (!string.IsNullOrEmpty(buildResult))
        {
            Debug.LogError(buildResult);
        }
        else
        {
            Debug.Log("[BUILD] Unity Build Successful.");
        }
    }

    // copy everything in the BuildIncludes directory over into the proper directories, overwriting files if they already exist.
    private static void BuildCopyIncludes(BuildTarget target)
    {
        string buildLocation = GetBuildDirectory();
        string buildIncludeLocation = GetBuildIncludesDirectory(target);

        if (Directory.Exists(buildIncludeLocation))
            FileTools.CopyAll(buildIncludeLocation, buildLocation);
    }

	private static string GetBuildDirectory()
    {
        string buildLocation = Path.GetFullPath(Application.dataPath + "/../build/");
        return Path.GetFullPath(buildLocation);
    }

    private static string GetBuildIncludesDirectory(BuildTarget target)
    {
        string buildLocation = Path.GetFullPath(Application.dataPath + "/../build-includes/");
        return buildLocation;
    }

    protected static void RunNoAndroidProdBuild(AndroidBuildSubtarget textureCompression)
    {
        EditorUserBuildSettings.androidBuildSubtarget = textureCompression;

        RunBuild(BuildTarget.Android, prodBuildOptions);
    }

    [MenuItem("Tools/Build/Android", false, 113)]
    protected static void RunAndroidProdBuildTest()
    {
        //AppInfo.IncrementBuildNumber();
        RunNoAndroidProdBuild(AndroidBuildSubtarget.Generic);
		ProcessStartInfo proc = new ProcessStartInfo();

		proc.FileName = "bash";
        proc.Arguments = AppInfo.getAppPath + "/Assets/External/HSBuild/Editor/build.sh " + AppInfo.getAppPath
						+ " " + AppInfo.longName
						+ " " + AppInfo.androidHome
						+ " " + AppInfo.companyName;
		Debug.Log(proc.Arguments);
		proc.RedirectStandardOutput = true;
		proc.RedirectStandardError = true;
		proc.UseShellExecute = false;
        Process p = Process.Start(proc);
		string strOutput = p.StandardOutput.ReadToEnd();
   		p.WaitForExit();
   		Debug.Log(strOutput);
    }

}
