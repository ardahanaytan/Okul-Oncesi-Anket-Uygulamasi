using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;

public static class IOSFileExporter
{
#if UNITY_IOS && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern void _ios_export_to_files(string path, string uti, string suggestedName);
#endif

    public static void ExportToFiles(string path, string suggestedName = null)
    {
        if (string.IsNullOrEmpty(path) || !File.Exists(path))
        {
            Debug.LogError("ExportToFiles: dosya bulunamadı: " + path);
            return;
        }

#if UNITY_IOS && !UNITY_EDITOR
        // CSV için UTI: "public.comma-separated-values-text"
        _ios_export_to_files(path, "public.comma-separated-values-text", suggestedName ?? Path.GetFileName(path));
        Debug.Log("Kaydedildi");
#else
        Debug.Log("ExportToFiles sadece iOS cihazda çalışır. Path: " + path);
#endif
    }
}
