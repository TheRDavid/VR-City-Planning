using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataHandler
{

    private IDataWatcher dataWatcher;
    private FileSystemWatcher dataFileWatcher;
    private Municipality municipality;
    private ConditionList conditionList;
    private string cityDataDir, dataFileName;

    public DataHandler(string cityDataDir, string dataFileName, IDataWatcher dataWatcher)
    {
        this.dataWatcher = dataWatcher;
        this.cityDataDir = cityDataDir;
        this.dataFileName = dataFileName;
        dataFileWatcher = new FileSystemWatcher(cityDataDir);
        dataFileWatcher.Changed += OnFileChanged;
        dataFileWatcher.EnableRaisingEvents = true;
        refresh();
    }

    private void OnFileChanged(object sender, FileSystemEventArgs e)
    {
        if (e.ChangeType.Equals(WatcherChangeTypes.Changed))
        {
            refresh();
        }
    }

    private void refresh()
    {
        FileStream dataStream = File.Open(cityDataDir+dataFileName+".json", FileMode.Open);
        StreamReader dataReader = new StreamReader(dataStream);

        FileStream conditionsStream = File.Open(cityDataDir+dataFileName+".conditions.json", FileMode.Open);
        StreamReader conditionsReader = new StreamReader(conditionsStream);

        string jsonData = dataReader.ReadToEnd();
        string jsonConditions = conditionsReader.ReadToEnd();

        municipality = JsonUtility.FromJson<Municipality>(jsonData);
        conditionList = JsonUtility.FromJson<ConditionList>(jsonConditions);

        dataStream.Close();
        conditionsReader.Close();

        dataWatcher.reactToChange(municipality, conditionList);
    }

}
