// ************ @autor: Álvaro Repiso Romero *************
using System.IO;
using UnityEngine;

public class SaveGame
{
    private string saveDirectoryPath = Application.dataPath + "/Save Data";
    private string optionsSaveFile = "/OptionsSave.json";
    //private string gameSavefile = "GameSave.json";

    public SaveGame Get()
    {
        return this;
    }

    public void SaveOptions( OptionsSave optionsSave )
    {
        if ( !Directory.Exists( saveDirectoryPath ) )
            Directory.CreateDirectory( saveDirectoryPath );

        // Save data to Json
        StreamWriter writer = new StreamWriter( saveDirectoryPath + optionsSaveFile );
        string optionsSaveJson = JsonUtility.ToJson( optionsSave );

        writer.Write( optionsSaveJson );
        writer.Close();

        Debug.Log( "Options Saved" );
    }

    public OptionsSave LoadOptions()
    {
        Debug.Log( Application.systemLanguage );
        if ( !Directory.Exists( saveDirectoryPath ) )
            Directory.CreateDirectory( saveDirectoryPath );

        if ( !File.Exists( saveDirectoryPath + optionsSaveFile ) )
        {
            // Default Options Settings
            OptionsSave newOptionsSave = new OptionsSave();
            newOptionsSave.lenguageDropdownValue = GetSystemLenguage();
            newOptionsSave.isVibrationActive = true;
            Application.targetFrameRate = Screen.currentResolution.refreshRate;
            return newOptionsSave;
        }

        // Load Data from Json
        StreamReader reader = new StreamReader( saveDirectoryPath + optionsSaveFile );

        OptionsSave optionsSave = JsonUtility.FromJson<OptionsSave>( reader.ReadToEnd() );

        reader.Close();

        return optionsSave;
    }

    private static int GetSystemLenguage()
    {
        switch ( Application.systemLanguage )
        {
            case SystemLanguage.English:
                return 0;
            case SystemLanguage.Spanish:
                return 1;
        }
        return 0;
    }
}