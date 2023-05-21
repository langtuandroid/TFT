// ************ @autor: Álvaro Repiso Romero *************
using System.IO;
using UnityEngine;

public class SaveGame
{
    private string saveDirectoryPath = Application.dataPath + "/Save Data";
    private string optionsSaveFile = "/OptionsSave.json";

    public void SavePlayerGame( int saveSlot , GameSaveData gameSaveData )
    {
        if ( !Directory.Exists( saveDirectoryPath ) )
            Directory.CreateDirectory( saveDirectoryPath );

        string gameSavefile = $"/GameSave{saveSlot}.json";

        StreamWriter writer = new StreamWriter( saveDirectoryPath + gameSavefile );
        string optionsSaveJson = JsonUtility.ToJson( gameSaveData );

        writer.Write( optionsSaveJson );
        writer.Close();
    }

    public GameSaveData LoadGameSaveData( int saveSlot )
    {
        string gameSavefile = $"/GameSave{saveSlot}.json";

        if ( !File.Exists( saveDirectoryPath + gameSavefile ) )
            return null;

        // Load Data from Json
        StreamReader reader = new StreamReader( saveDirectoryPath + gameSavefile );

        GameSaveData gameSaveData = JsonUtility.FromJson<GameSaveData>( reader.ReadToEnd() );

        reader.Close();

        return gameSaveData;
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
    }

    public OptionsSave LoadOptions()
    {
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

    private int GetSystemLenguage()
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