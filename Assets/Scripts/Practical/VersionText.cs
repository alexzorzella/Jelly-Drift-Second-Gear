using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

public static class VersionTextUtil {
    static readonly Regex versionExtractorRegexp = new(@"((\d{2}.\d{2}).(\d+))(?:-(\w{4}))?");

    public static string CurrentPrintableVersionNoSha() {
        return PrintableVersionNoSha(Application.version);
    }
    
    /// <summary>
    /// Returns a version string suitable for printing as a watermark.
    /// It adds a "v" prefix, and strips the `-<sha>`.
    /// </summary>
    /// <param name="applicationVersion"></param>
    /// <returns></returns>
    public static string PrintableVersionNoSha(string applicationVersion) {
        return "v" + versionExtractorRegexp.Match(applicationVersion).Groups[1];
    }
}

public class VersionText : MonoBehaviour {
    void Start() {
        GetComponent<TextMeshProUGUI>().text = VersionTextUtil.CurrentPrintableVersionNoSha();
    }
}