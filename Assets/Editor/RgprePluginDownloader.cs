using System;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

[InitializeOnLoad]
public static class RgprePluginDownloader
{
    private const string RepoOwner = "michidk";
    private const string RepoName = "redguard-preservation";
    private const string LatestReleaseApiUrl = "https://api.github.com/repos/" + RepoOwner + "/" + RepoName + "/releases/latest";
    private const string TagReleaseApiUrl = "https://api.github.com/repos/" + RepoOwner + "/" + RepoName + "/releases/tags/";
    private const string PluginsDirectoryAssetPath = "Assets/Plugins/rgpre";
    private const string VersionAssetPath = "Assets/Plugins/rgpre/rgpre.version.txt";

    [Serializable]
    private sealed class GitHubRelease
    {
        public string tag_name;
        public GitHubAsset[] assets;
    }

    [Serializable]
    private sealed class GitHubAsset
    {
        public string name;
        public string browser_download_url;
    }

    private sealed class PlatformTarget
    {
        public string Label;
        public string AssetPattern;
        public string BinaryFileName;
        public string TargetAssetPath;
        public BuildTarget BuildTarget;
        public string EditorCpu;
        public string PlatformCpu;
    }

    private static readonly PlatformTarget[] AllPlatforms = new[]
    {
        new PlatformTarget
        {
            Label = "Windows x64",
            AssetPattern = "librgpre-x86_64-pc-windows-msvc.zip",
            BinaryFileName = "rgpre.dll",
            TargetAssetPath = "Assets/Plugins/rgpre/Windows/x86_64/rgpre.dll",
            BuildTarget = BuildTarget.StandaloneWindows64,
            EditorCpu = "x86_64",
            PlatformCpu = "x86_64"
        },
        new PlatformTarget
        {
            Label = "macOS x64",
            AssetPattern = "librgpre-x86_64-apple-darwin.tar.gz",
            BinaryFileName = "librgpre.dylib",
            TargetAssetPath = "Assets/Plugins/rgpre/macOS/x86_64/librgpre.dylib",
            BuildTarget = BuildTarget.StandaloneOSX,
            EditorCpu = "x86_64",
            PlatformCpu = "x86_64"
        },
        new PlatformTarget
        {
            Label = "macOS ARM64",
            AssetPattern = "librgpre-aarch64-apple-darwin.tar.gz",
            BinaryFileName = "librgpre.dylib",
            TargetAssetPath = "Assets/Plugins/rgpre/macOS/arm64/librgpre.dylib",
            BuildTarget = BuildTarget.StandaloneOSX,
            EditorCpu = "ARM64",
            PlatformCpu = "ARM64"
        },
        new PlatformTarget
        {
            Label = "Linux x64",
            AssetPattern = "librgpre-x86_64-unknown-linux-gnu.tar.gz",
            BinaryFileName = "librgpre.so",
            TargetAssetPath = "Assets/Plugins/rgpre/Linux/x86_64/librgpre.so",
            BuildTarget = BuildTarget.StandaloneLinux64,
            EditorCpu = "x86_64",
            PlatformCpu = "x86_64"
        }
    };

    // ========== Startup check ==========

    static RgprePluginDownloader()
    {
        bool anyFound = false;
        foreach (var platform in AllPlatforms)
        {
            if (File.Exists(AssetPathToAbsolutePath(platform.TargetAssetPath)))
            {
                anyFound = true;
                break;
            }
        }

        if (!anyFound)
        {
            Debug.LogWarning("[rgpre] No native plugins found. Use Tools > rgpre > Update to Latest to download them.");
        }
    }

    // ========== Menu items ==========

    [MenuItem("Tools/rgpre/Update to Latest")]
    public static async void UpdateToLatest()
    {
        await DownloadReleaseAsync(null);
    }

    [MenuItem("Tools/rgpre/Download Specific Version...")]
    public static void DownloadSpecificVersion()
    {
        RgpreVersionWindow.Show();
    }

    // ========== Core download logic ==========

    internal static async Task DownloadReleaseAsync(string versionTag)
    {
        bool isLatest = string.IsNullOrWhiteSpace(versionTag);
        string description = isLatest ? "latest" : versionTag;

        try
        {
            EditorUtility.DisplayProgressBar("rgpre Plugin Update", $"Fetching {description} release...", 0.02f);

            string apiUrl = isLatest ? LatestReleaseApiUrl : TagReleaseApiUrl + versionTag;
            GitHubRelease release = await FetchReleaseAsync(apiUrl);
            if (release == null)
            {
                Debug.LogError($"[rgpre] Failed to fetch release ({description}). Check the version tag.");
                return;
            }

            string resolvedVersion = string.IsNullOrWhiteSpace(release.tag_name) ? "unknown" : release.tag_name.Trim();
            string currentVersion = ReadCurrentVersion();

            if (!string.IsNullOrEmpty(currentVersion) && string.Equals(currentVersion, resolvedVersion, StringComparison.Ordinal))
            {
                Debug.Log($"[rgpre] Already at {resolvedVersion}.");
            }
            else
            {
                Debug.Log($"[rgpre] Current: {currentVersion ?? "none"} → Target: {resolvedVersion}");
            }

            int succeeded = 0;
            int failed = 0;

            for (int i = 0; i < AllPlatforms.Length; i++)
            {
                PlatformTarget target = AllPlatforms[i];
                float baseProgress = 0.05f + (0.9f * i / AllPlatforms.Length);
                float sliceSize = 0.9f / AllPlatforms.Length;

                EditorUtility.DisplayProgressBar("rgpre Plugin Update", $"[{i + 1}/{AllPlatforms.Length}] {target.Label}: finding asset...", baseProgress);

                GitHubAsset matchingAsset = FindMatchingAsset(release.assets, target.AssetPattern);
                if (matchingAsset == null || string.IsNullOrWhiteSpace(matchingAsset.browser_download_url))
                {
                    Debug.LogWarning($"[rgpre] No release asset for {target.Label} (expected '{target.AssetPattern}').");
                    failed++;
                    continue;
                }

                EditorUtility.DisplayProgressBar("rgpre Plugin Update", $"[{i + 1}/{AllPlatforms.Length}] {target.Label}: downloading...", baseProgress + sliceSize * 0.1f);

                byte[] archiveBytes = await DownloadBytesAsync(
                    matchingAsset.browser_download_url,
                    progress => EditorUtility.DisplayProgressBar(
                        "rgpre Plugin Update",
                        $"[{i + 1}/{AllPlatforms.Length}] {target.Label}: downloading...",
                        baseProgress + sliceSize * (0.1f + 0.7f * Mathf.Clamp01(progress))));

                EditorUtility.DisplayProgressBar("rgpre Plugin Update", $"[{i + 1}/{AllPlatforms.Length}] {target.Label}: extracting...", baseProgress + sliceSize * 0.85f);

                byte[] binaryBytes = ExtractExpectedBinary(archiveBytes, matchingAsset.name, target.BinaryFileName);
                if (binaryBytes == null || binaryBytes.Length == 0)
                {
                    Debug.LogError($"[rgpre] Failed to extract '{target.BinaryFileName}' from '{matchingAsset.name}'.");
                    failed++;
                    continue;
                }

                string targetAbsoluteDir = Path.GetDirectoryName(AssetPathToAbsolutePath(target.TargetAssetPath));
                Directory.CreateDirectory(targetAbsoluteDir);
                File.WriteAllBytes(AssetPathToAbsolutePath(target.TargetAssetPath), binaryBytes);
                AssetDatabase.ImportAsset(target.TargetAssetPath, ImportAssetOptions.ForceUpdate);
                ConfigurePluginImporter(target);

                Debug.Log($"[rgpre] {target.Label}: {target.TargetAssetPath} ({binaryBytes.LongLength:N0} bytes)");
                succeeded++;
            }

            // Write version file
            string pluginsAbsolute = AssetPathToAbsolutePath(PluginsDirectoryAssetPath);
            Directory.CreateDirectory(pluginsAbsolute);
            File.WriteAllText(AssetPathToAbsolutePath(VersionAssetPath), resolvedVersion + "\n", Encoding.UTF8);
            AssetDatabase.ImportAsset(VersionAssetPath, ImportAssetOptions.ForceUpdate);
            AssetDatabase.Refresh();

            Debug.Log($"[rgpre] Updated to {resolvedVersion}: {succeeded} platforms succeeded, {failed} failed.");
        }
        catch (Exception ex)
        {
            Debug.LogError($"[rgpre] Update failed: {ex.Message}");
        }
        finally
        {
            EditorUtility.ClearProgressBar();
        }
    }

    // ========== GitHub API ==========

    private static async Task<GitHubRelease> FetchReleaseAsync(string apiUrl)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(apiUrl))
        {
            request.SetRequestHeader("Accept", "application/vnd.github+json");
            request.SetRequestHeader("User-Agent", "redguard-unity-rgpre-updater");

            await SendRequestAsync(request, null);
            if (request.result != UnityWebRequest.Result.Success)
            {
                return null;
            }

            return JsonUtility.FromJson<GitHubRelease>(request.downloadHandler.text);
        }
    }

    private static async Task<byte[]> DownloadBytesAsync(string url, Action<float> onProgress)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            request.SetRequestHeader("User-Agent", "redguard-unity-rgpre-updater");
            await SendRequestAsync(request, onProgress);
            return request.downloadHandler.data;
        }
    }

    private static async Task SendRequestAsync(UnityWebRequest request, Action<float> onProgress)
    {
        UnityWebRequestAsyncOperation operation = request.SendWebRequest();
        while (!operation.isDone)
        {
            onProgress?.Invoke(request.downloadProgress);
            await Task.Delay(100);
        }

        onProgress?.Invoke(1f);

        if (request.result != UnityWebRequest.Result.Success)
        {
            throw new InvalidOperationException($"HTTP {request.responseCode}: {request.error}");
        }
    }

    // ========== Asset matching ==========

    private static GitHubAsset FindMatchingAsset(GitHubAsset[] assets, string expectedName)
    {
        if (assets == null)
        {
            return null;
        }

        for (int i = 0; i < assets.Length; i++)
        {
            if (assets[i] != null && string.Equals(assets[i].name, expectedName, StringComparison.Ordinal))
            {
                return assets[i];
            }
        }

        return null;
    }

    // ========== Archive extraction ==========

    private static byte[] ExtractExpectedBinary(byte[] archiveBytes, string archiveName, string expectedBinaryFileName)
    {
        if (archiveName.EndsWith(".zip", StringComparison.OrdinalIgnoreCase))
        {
            return ExtractFromZip(archiveBytes, expectedBinaryFileName);
        }

        if (archiveName.EndsWith(".tar.gz", StringComparison.OrdinalIgnoreCase))
        {
            return ExtractFromTarGz(archiveBytes, expectedBinaryFileName);
        }

        throw new InvalidOperationException($"Unsupported archive format: {archiveName}");
    }

    private static byte[] ExtractFromZip(byte[] archiveBytes, string expectedBinaryFileName)
    {
        string tempRoot = Path.Combine(Path.GetTempPath(), "rgpre-unity-zip-" + Guid.NewGuid().ToString("N"));
        string archivePath = Path.Combine(tempRoot, "archive.zip");
        string extractDirectory = Path.Combine(tempRoot, "extract");

        Directory.CreateDirectory(extractDirectory);

        try
        {
            File.WriteAllBytes(archivePath, archiveBytes);
            ZipFile.ExtractToDirectory(archivePath, extractDirectory);

            string[] files = Directory.GetFiles(extractDirectory, "*", SearchOption.AllDirectories);
            for (int i = 0; i < files.Length; i++)
            {
                if (string.Equals(Path.GetFileName(files[i]), expectedBinaryFileName, StringComparison.Ordinal))
                {
                    return File.ReadAllBytes(files[i]);
                }
            }

            return null;
        }
        finally
        {
            if (Directory.Exists(tempRoot))
            {
                Directory.Delete(tempRoot, true);
            }
        }
    }

    private static byte[] ExtractFromTarGz(byte[] archiveBytes, string expectedBinaryFileName)
    {
        using (MemoryStream compressedStream = new MemoryStream(archiveBytes))
        using (GZipStream gzipStream = new GZipStream(compressedStream, CompressionMode.Decompress))
        {
            while (TryReadExact(gzipStream, 512, out byte[] header))
            {
                if (IsAllZeroBlock(header))
                {
                    break;
                }

                string entryName = ReadTarString(header, 0, 100);
                long size = ParseTarOctal(header, 124, 12);
                byte typeFlag = header[156];
                bool isRegularFile = typeFlag == 0 || typeFlag == (byte)'0';
                long paddedSize = AlignTo512(size);

                if (isRegularFile)
                {
                    byte[] fileData = ReadExactOrThrow(gzipStream, size);
                    long remainingPadding = paddedSize - size;
                    if (remainingPadding > 0)
                    {
                        SkipBytesOrThrow(gzipStream, remainingPadding);
                    }

                    if (string.Equals(Path.GetFileName(entryName), expectedBinaryFileName, StringComparison.Ordinal))
                    {
                        return fileData;
                    }
                }
                else
                {
                    SkipBytesOrThrow(gzipStream, paddedSize);
                }
            }
        }

        return null;
    }

    // ========== Plugin importer configuration ==========

    private static void ConfigurePluginImporter(PlatformTarget target)
    {
        PluginImporter importer = AssetImporter.GetAtPath(target.TargetAssetPath) as PluginImporter;
        if (importer == null)
        {
            Debug.LogWarning($"[rgpre] PluginImporter not found for {target.TargetAssetPath}.");
            return;
        }

        importer.SetCompatibleWithAnyPlatform(false);

        // Editor: compatible, with correct CPU
        importer.SetCompatibleWithEditor(true);
        importer.SetEditorData("CPU", target.EditorCpu);
        importer.SetEditorData("OS", GetEditorOs(target.BuildTarget));

        // Standalone platforms: only the matching one
        importer.SetCompatibleWithPlatform(BuildTarget.StandaloneWindows64, false);
        importer.SetCompatibleWithPlatform(BuildTarget.StandaloneOSX, false);
        importer.SetCompatibleWithPlatform(BuildTarget.StandaloneLinux64, false);
        importer.SetCompatibleWithPlatform(target.BuildTarget, true);
        importer.SetPlatformData(target.BuildTarget, "CPU", target.PlatformCpu);

        importer.SaveAndReimport();
    }

    private static string GetEditorOs(BuildTarget target)
    {
        switch (target)
        {
            case BuildTarget.StandaloneWindows64: return "Windows";
            case BuildTarget.StandaloneOSX: return "OSX";
            case BuildTarget.StandaloneLinux64: return "Linux";
            default: return "AnyOS";
        }
    }

    // ========== Utilities ==========

    private static string ReadCurrentVersion()
    {
        string path = AssetPathToAbsolutePath(VersionAssetPath);
        if (!File.Exists(path))
        {
            return null;
        }

        return File.ReadAllText(path).Trim();
    }

    private static string AssetPathToAbsolutePath(string assetPath)
    {
        string projectRoot = Path.GetDirectoryName(Application.dataPath);
        return Path.Combine(projectRoot, assetPath.Replace('/', Path.DirectorySeparatorChar));
    }

    // ========== Tar/stream helpers ==========

    private static bool TryReadExact(Stream stream, int count, out byte[] buffer)
    {
        buffer = new byte[count];
        int offset = 0;

        while (offset < count)
        {
            int read = stream.Read(buffer, offset, count - offset);
            if (read <= 0)
            {
                if (offset == 0)
                {
                    buffer = null;
                    return false;
                }

                throw new EndOfStreamException("Unexpected end of stream while reading tar header.");
            }

            offset += read;
        }

        return true;
    }

    private static byte[] ReadExactOrThrow(Stream stream, long count)
    {
        if (count < 0 || count > int.MaxValue)
        {
            throw new InvalidOperationException($"Invalid tar entry size: {count}");
        }

        byte[] buffer = new byte[(int)count];
        int offset = 0;
        int intCount = (int)count;

        while (offset < intCount)
        {
            int read = stream.Read(buffer, offset, intCount - offset);
            if (read <= 0)
            {
                throw new EndOfStreamException("Unexpected end of stream while reading tar entry data.");
            }

            offset += read;
        }

        return buffer;
    }

    private static void SkipBytesOrThrow(Stream stream, long bytesToSkip)
    {
        if (bytesToSkip <= 0)
        {
            return;
        }

        byte[] skipBuffer = new byte[8192];
        long remaining = bytesToSkip;
        while (remaining > 0)
        {
            int request = remaining > skipBuffer.Length ? skipBuffer.Length : (int)remaining;
            int read = stream.Read(skipBuffer, 0, request);
            if (read <= 0)
            {
                throw new EndOfStreamException("Unexpected end of stream while skipping tar entry data.");
            }

            remaining -= read;
        }
    }

    private static bool IsAllZeroBlock(byte[] block)
    {
        for (int i = 0; i < block.Length; i++)
        {
            if (block[i] != 0)
            {
                return false;
            }
        }

        return true;
    }

    private static string ReadTarString(byte[] buffer, int offset, int length)
    {
        int end = offset;
        int max = offset + length;
        while (end < max && buffer[end] != 0)
        {
            end++;
        }

        return Encoding.ASCII.GetString(buffer, offset, end - offset).Trim();
    }

    private static long ParseTarOctal(byte[] buffer, int offset, int length)
    {
        string value = ReadTarString(buffer, offset, length).Trim();
        if (string.IsNullOrEmpty(value))
        {
            return 0;
        }

        long result = 0;
        for (int i = 0; i < value.Length; i++)
        {
            char c = value[i];
            if (c < '0' || c > '7')
            {
                continue;
            }

            result = (result * 8) + (c - '0');
        }

        return result;
    }

    private static long AlignTo512(long value)
    {
        if (value <= 0)
        {
            return 0;
        }

        long remainder = value % 512;
        return remainder == 0 ? value : value + (512 - remainder);
    }
}

/// <summary>
/// Small EditorWindow for entering a specific rgpre version tag to download.
/// </summary>
public class RgpreVersionWindow : EditorWindow
{
    private string versionTag = "v";

    public static void Show()
    {
        var window = GetWindow<RgpreVersionWindow>(true, "Download rgpre Version", true);
        window.minSize = new Vector2(340, 90);
        window.maxSize = new Vector2(340, 90);
        window.ShowUtility();
    }

    private void OnGUI()
    {
        EditorGUILayout.Space(8);
        EditorGUILayout.LabelField("Enter version tag (e.g. v0.3.4):");
        versionTag = EditorGUILayout.TextField(versionTag);

        EditorGUILayout.Space(4);
        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();

        if (GUILayout.Button("Download", GUILayout.Width(100)))
        {
            string tag = versionTag?.Trim();
            if (!string.IsNullOrEmpty(tag))
            {
                Close();
                _ = RgprePluginDownloader.DownloadReleaseAsync(tag);
            }
        }

        if (GUILayout.Button("Cancel", GUILayout.Width(80)))
        {
            Close();
        }

        EditorGUILayout.EndHorizontal();
    }
}
