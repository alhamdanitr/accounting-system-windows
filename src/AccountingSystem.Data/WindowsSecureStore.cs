using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Runtime.Versioning;

namespace AccountingSystem.Data
{
    [SupportedOSPlatform("windows")]
    internal sealed class WindowsSecureStore
    {
        private readonly string _sessionPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "AccountingSystem",
            "session.bin");

        public void Save(LoginResponse response)
        {
            var directory = Path.GetDirectoryName(_sessionPath)!;
            Directory.CreateDirectory(directory);
            var plainText = JsonSerializer.SerializeToUtf8Bytes(response);
            var protectedBytes = ProtectedData.Protect(plainText, null, DataProtectionScope.CurrentUser);
            File.WriteAllBytes(_sessionPath, protectedBytes);
        }

        public LoginResponse? Load()
        {
            try
            {
                if (!File.Exists(_sessionPath)) return null;
                var protectedBytes = File.ReadAllBytes(_sessionPath);
                var plainText = ProtectedData.Unprotect(protectedBytes, null, DataProtectionScope.CurrentUser);
                return JsonSerializer.Deserialize<LoginResponse>(plainText);
            }
            catch
            {
                Clear();
                return null;
            }
        }

        public void Clear()
        {
            try
            {
                if (File.Exists(_sessionPath)) File.Delete(_sessionPath);
            }
            catch
            {
                // A stale encrypted session must never prevent the application from starting.
            }
        }
    }
}
