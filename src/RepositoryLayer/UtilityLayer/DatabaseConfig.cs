using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace UtilityLayer
{
    public class DatabaseConfig
    {
        private static string _connectionString = string.Empty;
        private static string _repositoryType = string.Empty;

        static DatabaseConfig()
        {
            LoadConfiguration();
        }

        private static void LoadConfiguration()
        {
            try
            {
                var configPath = ResolveConfigPath();
                var json = File.ReadAllText(configPath);
                var config = JsonSerializer.Deserialize<ConfigModel>(json)
                    ?? throw new InvalidOperationException("Configuration file is empty or invalid JSON.");

                _connectionString = config.ConnectionStrings?.DefaultConnection
                    ?? throw new InvalidOperationException("Missing ConnectionStrings.DefaultConnection in appsettings.json.");

                _repositoryType = config.RepositoryType
                    ?? throw new InvalidOperationException("Missing RepositoryType in appsettings.json.");
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to load configuration.", ex);
            }
        }

        private static string ResolveConfigPath()
        {
            var probeRoots = new List<string>();
            var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            AddProbeChain(AppContext.BaseDirectory, probeRoots, seen);
            AddProbeChain(Directory.GetCurrentDirectory(), probeRoots, seen);

            var checkedPaths = new List<string>();
            foreach (var root in probeRoots)
            {
                var srcResourcesPath = Path.GetFullPath(Path.Combine(root, "src", "Resources", "appsettings.json"));
                checkedPaths.Add(srcResourcesPath);
                if (File.Exists(srcResourcesPath))
                {
                    return srcResourcesPath;
                }

                var srcRepositoryLayerPath = Path.GetFullPath(Path.Combine(root, "src", "RepositoryLayer", "appsettings.json"));
                checkedPaths.Add(srcRepositoryLayerPath);
                if (File.Exists(srcRepositoryLayerPath))
                {
                    return srcRepositoryLayerPath;
                }

                var repositoryLayerPath = Path.GetFullPath(Path.Combine(root, "RepositoryLayer", "appsettings.json"));
                checkedPaths.Add(repositoryLayerPath);
                if (File.Exists(repositoryLayerPath))
                {
                    return repositoryLayerPath;
                }

                var resourcesPath = Path.GetFullPath(Path.Combine(root, "Resources", "appsettings.json"));
                checkedPaths.Add(resourcesPath);
                if (File.Exists(resourcesPath))
                {
                    return resourcesPath;
                }
            }

            throw new FileNotFoundException(
                "Could not locate appsettings.json. Checked paths: " + string.Join("; ", checkedPaths));
        }

        private static void AddProbeChain(string startPath, List<string> probeRoots, HashSet<string> seen)
        {
            if (string.IsNullOrWhiteSpace(startPath))
            {
                return;
            }

            var current = new DirectoryInfo(startPath);
            for (int i = 0; i < 8 && current != null; i++)
            {
                var fullName = current.FullName;
                if (seen.Add(fullName))
                {
                    probeRoots.Add(fullName);
                }

                current = current.Parent;
            }
        }

        public static string GetConnectionString()
        {
            return _connectionString;
        }

        public static string GetRepositoryType()
        {
            return _repositoryType;
        }

        private class ConfigModel
        {
            public string? RepositoryType { get; set; }
            public ConnectionStringsModel? ConnectionStrings { get; set; }
        }

        private class ConnectionStringsModel
        {
            public string? DefaultConnection { get; set; }
        }
    }
}