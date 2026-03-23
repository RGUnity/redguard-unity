using System;
using System.IO;
using System.IO.Compression;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

[InitializeOnLoad]
public static class RgprePluginDownloader
{
    private const string ReleaseApiUrl = "https://api.github.com/repos/michidk/redguard-preservation/releases/latest";
    private const string PluginsDirectoryAssetPath = "Assets/Plugins";
    private const string VersionAssetPath = "Assets/Plugins/rgpre.version.txt";
    private const string MenuPath = "Tools/Update rgpre Native Plugin";

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

    private sealed class PlatformInfo
    {
        public string AssetPattern;
        public string BinaryFileName;
        public string TargetAssetPath;
        public BuildTarget BuildTarget;
    }

    static RgprePluginDownloader()
    {
        if (!TryGetPlatformInfo(out PlatformInfo platformInfo, out _))
        {
            return;
        }

        string pluginAbsolutePath = AssetPathToAbsolutePath(platformInfo.TargetAssetPath);
        if (!File.Exists(pluginAbsolutePath))
        {
            Debug.LogWarning("[rgpre] Native plugin not found. Run Tools > Update rgpre Native Plugin to download it.");
        }
    }

    [MenuItem(MenuPath)]
    public static async void UpdateRgpreNativePlugin()
    {
        await UpdateRgpreNativePluginAsync();
    }

    private static async Task UpdateRgpreNativePluginAsync()
    {
        if (!TryGetPlatformInfo(out PlatformInfo platformInfo, out string platformError))
        {
            Debug.LogError($"[rgpre] {platformError}");
            return;
        }

        string currentVersion = ReadCurrentVersion();

        try
        {
            EditorUtility.DisplayProgressBar("rgpre Plugin Update", "Checking latest release...", 0.05f);

            GitHubRelease latestRelease = await GetLatestReleaseAsync();
            if (latestRelease == null)
            {
                Debug.LogError("[rgpre] Failed to parse latest release response.");
                return;
            }

            string latestVersion = string.IsNullOrWhiteSpace(latestRelease.tag_name) ? "unknown" : latestRelease.tag_name.Trim();
            if (string.IsNullOrWhiteSpace(currentVersion))
            {
                Debug.Log($"[rgpre] Current: none, Latest: {latestVersion}");
            }
            else if (string.Equals(currentVersion, latestVersion, StringComparison.Ordinal))
            {
                Debug.Log($"[rgpre] Already up to date ({latestVersion})");
            }
            else
            {
                Debug.Log($"[rgpre] Current: {currentVersion}, Latest: {latestVersion}");
            }

            GitHubAsset matchingAsset = FindMatchingAsset(latestRelease.assets, platformInfo.AssetPattern);
            if (matchingAsset == null)
            {
                Debug.LogError($"[rgpre] Could not find release asset matching '{platformInfo.AssetPattern}'.");
                return;
            }

            if (string.IsNullOrWhiteSpace(matchingAsset.browser_download_url))
            {
                Debug.LogError("[rgpre] Release asset is missing browser_download_url.");
                return;
            }

            EditorUtility.DisplayProgressBar("rgpre Plugin Update", $"Downloading {matchingAsset.name}...", 0.2f);
            byte[] archiveBytes = await DownloadBytesAsync(
                matchingAsset.browser_download_url,
                progress =>
                {
                    float clamped = Mathf.Clamp01(progress);
                    EditorUtility.DisplayProgressBar("rgpre Plugin Update", $"Downloading {matchingAsset.name}...", 0.2f + (0.6f * clamped));
                });

            EditorUtility.DisplayProgressBar("rgpre Plugin Update", "Extracting archive...", 0.85f);
            byte[] binaryBytes = ExtractExpectedBinary(archiveBytes, matchingAsset.name, platformInfo.BinaryFileName);
            if (binaryBytes == null || binaryBytes.Length == 0)
            {
                Debug.LogError($"[rgpre] Failed to extract '{platformInfo.BinaryFileName}' from '{matchingAsset.name}'.");
                return;
            }

            string pluginsDirectoryAbsolute = AssetPathToAbsolutePath(PluginsDirectoryAssetPath);
            Directory.CreateDirectory(pluginsDirectoryAbsolute);

            string pluginAbsolutePath = AssetPathToAbsolutePath(platformInfo.TargetAssetPath);
            File.WriteAllBytes(pluginAbsolutePath, binaryBytes);
            File.WriteAllText(AssetPathToAbsolutePath(VersionAssetPath), latestVersion + "\n", Encoding.UTF8);

            AssetDatabase.ImportAsset(platformInfo.TargetAssetPath, ImportAssetOptions.ForceUpdate);
            ConfigurePluginImporter(platformInfo.TargetAssetPath, platformInfo.BuildTarget);
            AssetDatabase.ImportAsset(VersionAssetPath, ImportAssetOptions.ForceUpdate);
            AssetDatabase.Refresh();

            Debug.Log($"[rgpre] Updated to {latestVersion}: {platformInfo.TargetAssetPath} ({binaryBytes.LongLength} bytes).");
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

    private static async Task<GitHubRelease> GetLatestReleaseAsync()
    {
        using (UnityWebRequest request = UnityWebRequest.Get(ReleaseApiUrl))
        {
            request.SetRequestHeader("Accept", "application/vnd.github+json");
            request.SetRequestHeader("User-Agent", "redguard-unity-rgpre-updater");

            await SendRequestAsync(request, null);
            string json = request.downloadHandler.text;
            return JsonUtility.FromJson<GitHubRelease>(json);
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

    private static GitHubAsset FindMatchingAsset(GitHubAsset[] assets, string expectedName)
    {
        if (assets == null)
        {
            return null;
        }

        for (int i = 0; i < assets.Length; i++)
        {
            GitHubAsset asset = assets[i];
            if (asset != null && string.Equals(asset.name, expectedName, StringComparison.Ordinal))
            {
                return asset;
            }
        }

        return null;
    }

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
                string filePath = files[i];
                if (string.Equals(Path.GetFileName(filePath), expectedBinaryFileName, StringComparison.Ordinal))
                {
                    return File.ReadAllBytes(filePath);
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

    private static bool TryGetPlatformInfo(out PlatformInfo info, out string error)
    {
        Architecture architecture = RuntimeInformation.OSArchitecture;

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) && architecture == Architecture.X64)
        {
            info = new PlatformInfo
            {
                AssetPattern = "librgpre-x86_64-pc-windows-msvc.zip",
                BinaryFileName = "rgpre.dll",
                TargetAssetPath = "Assets/Plugins/rgpre.dll",
                BuildTarget = BuildTarget.StandaloneWindows64
            };
            error = null;
            return true;
        }

        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX) && architecture == Architecture.X64)
        {
            info = new PlatformInfo
            {
                AssetPattern = "librgpre-x86_64-apple-darwin.tar.gz",
                BinaryFileName = "librgpre.dylib",
                TargetAssetPath = "Assets/Plugins/rgpre.dylib",
                BuildTarget = BuildTarget.StandaloneOSX
            };
            error = null;
            return true;
        }

        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX) && architecture == Architecture.Arm64)
        {
            info = new PlatformInfo
            {
                AssetPattern = "librgpre-aarch64-apple-darwin.tar.gz",
                BinaryFileName = "librgpre.dylib",
                TargetAssetPath = "Assets/Plugins/rgpre.dylib",
                BuildTarget = BuildTarget.StandaloneOSX
            };
            error = null;
            return true;
        }

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) && architecture == Architecture.X64)
        {
            info = new PlatformInfo
            {
                AssetPattern = "librgpre-x86_64-unknown-linux-gnu.tar.gz",
                BinaryFileName = "librgpre.so",
                TargetAssetPath = "Assets/Plugins/librgpre.so",
                BuildTarget = BuildTarget.StandaloneLinux64
            };
            error = null;
            return true;
        }

        info = null;
        error = $"Unsupported platform: {RuntimeInformation.OSDescription} ({architecture})";
        return false;
    }

    private static void ConfigurePluginImporter(string pluginAssetPath, BuildTarget targetPlatform)
    {
        PluginImporter importer = AssetImporter.GetAtPath(pluginAssetPath) as PluginImporter;
        if (importer == null)
        {
            Debug.LogWarning($"[rgpre] PluginImporter not found for {pluginAssetPath}.");
            return;
        }

        importer.SetCompatibleWithAnyPlatform(false);
        importer.SetCompatibleWithEditor(true);
        importer.SetCompatibleWithPlatform(BuildTarget.StandaloneWindows64, false);
        importer.SetCompatibleWithPlatform(BuildTarget.StandaloneOSX, false);
        importer.SetCompatibleWithPlatform(BuildTarget.StandaloneLinux64, false);
        importer.SetCompatibleWithPlatform(targetPlatform, true);
        importer.SaveAndReimport();
    }

    private static string ReadCurrentVersion()
    {
        string versionAbsolutePath = AssetPathToAbsolutePath(VersionAssetPath);
        if (!File.Exists(versionAbsolutePath))
        {
            return null;
        }

        return File.ReadAllText(versionAbsolutePath).Trim();
    }

    private static string AssetPathToAbsolutePath(string assetPath)
    {
        string projectRoot = Path.GetDirectoryName(Application.dataPath);
        return Path.Combine(projectRoot, assetPath.Replace('/', Path.DirectorySeparatorChar));
    }

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
