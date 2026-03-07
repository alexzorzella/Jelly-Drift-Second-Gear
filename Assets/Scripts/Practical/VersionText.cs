using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

public class VersionText : MonoBehaviour {
    static readonly Regex versionExtractorRegexp = new(@"((\d{2}.\d{2}).(\d+))(?:-(\w{4}))?");

    /// <summary>
    /// Returns a version string suitable for printing as a watermark.
    /// It adds a "v" prefix, and strips the `-<sha>`.
    /// </summary>
    /// <param name="applicationVersion"></param>
    /// <returns></returns>
    static string PrintableVersionNoSha(string applicationVersion) {
        return "v" + versionExtractorRegexp.Match(applicationVersion).Groups[1];
    }

    void Start() {
        GetComponent<TextMeshProUGUI>().text = PrintableVersionNoSha(Application.version);
    }
}