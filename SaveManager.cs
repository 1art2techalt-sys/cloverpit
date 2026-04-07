using UnityEngine;
using System.Runtime.InteropServices; // Needed for JS bridge

public class SaveManager : MonoBehaviour {

    // This links to the JavaScript function in the HTML
    [DllImport("__Internal")]
    private static extern void triggerDownload(string jsonData);

    [DllImport("__Internal")]
    private static extern void triggerUpload();

    // 1. The Export Button calls this
    public void OnClickExport() {
        SaveData data = new SaveData();
        data.score = 100; // Replace with your real variables
        
        string json = JsonUtility.ToJson(data);
        
        #if UNITY_WEBGL && !UNITY_EDITOR
            triggerDownload(json);
        #else
            Debug.Log("Exporting: " + json);
        #endif
    }

    // 2. The Import Button calls this
    public void OnClickImport() {
        #if UNITY_WEBGL && !UNITY_EDITOR
            triggerUpload();
        #else
            Debug.Log("Import only works in WebGL build");
        #endif
    }

    // 3. This is called BY JavaScript when the file is picked
    public void ApplyImportedData(string jsonFromBrowser) {
        SaveData loadedData = JsonUtility.FromJson<SaveData>(jsonFromBrowser);
        Debug.Log("Progress Imported! Score was: " + loadedData.score);
        // Apply loadedData to your game here
    }
}

[System.Serializable]
public class SaveData {
    public int score;
    public int level;
    // Add all variables you want to save here
}
