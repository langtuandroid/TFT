// ************ @autor: Álvaro Repiso Romero *************
using System.IO;
using UnityEngine;

public class SaveGame
{
    private string optionsSaveFile = "OptionsSave.json";
    private string gameSavefile = "GameSave.json";

    public void Save()
    {
        Debug.Log( "Saving..." );
        if ( File.Exists( optionsSaveFile ) )
        {
            File.Delete( optionsSaveFile );
        }

        OptionsSave optionsSave = ServiceLocator.GetService<OptionsSave>();

        // Save data to Json
        StreamWriter writer = new StreamWriter( optionsSaveFile );

        string optionsSaveJson = JsonUtility.ToJson( optionsSave );

        writer.Write( optionsSaveJson );

        writer.Close();
        Debug.Log( "Saved" );
    }

    public void Load()
    {
        //if ( !File.Exists( optionsSaveFile ) )
        //    return 0;

        //// Load Data from Json
        //StreamReader reader = new StreamReader( optionsSaveFile );

        //maxScore = JsonUtility.FromJson<MaxScore>( reader.ReadToEnd() );

        //reader.Close();

        //// Load on console all MaxScores
        //int lastScore = 0;
        //for ( int i = 0; i < maxScore.scoreList.Count; i++ )
        //{
        //    lastScore = maxScore.scoreList[i];
        //    Debug.Log( i + 1 + ": " + lastScore );
        //}

        //return lastScore;
    }
}