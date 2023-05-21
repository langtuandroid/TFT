// ************ @autor: Álvaro Repiso Romero *************
using System.IO;
using UnityEngine;

public class SaveGame
{
    private string saveDirectoryPath = Application.dataPath + "/Save Data";
    private string optionsSaveFile = "/OptionsSave.json";
    private string gameSavefile = "GameSave.json";

    public void SaveOptions( OptionsSave optionsSave )
    {
        if ( !Directory.Exists( saveDirectoryPath ) )
        {
            Directory.CreateDirectory( saveDirectoryPath );
        }

        Debug.Log( "Saving..." );
        if ( File.Exists( saveDirectoryPath + optionsSaveFile ) )
        {
            File.Delete( saveDirectoryPath + optionsSaveFile );
        }

        // Save data to Json
        StreamWriter writer = new StreamWriter( saveDirectoryPath + optionsSaveFile );

        string optionsSaveJson = JsonUtility.ToJson( optionsSave );

        writer.Write( optionsSaveJson );

        writer.Close();
        Debug.Log( "Saved" );
    }

    public OptionsSave LoadOptions()
    {
        if ( !File.Exists( optionsSaveFile ) )
        {
            return new OptionsSave();
        }

        // Load Data from Json
        StreamReader reader = new StreamReader( optionsSaveFile );

        OptionsSave optionsSave = JsonUtility.FromJson<OptionsSave>( reader.ReadToEnd() );

        reader.Close();

        return optionsSave;
    }
}