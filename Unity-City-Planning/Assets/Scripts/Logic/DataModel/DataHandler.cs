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
    private string dataFileName, ruleFileName;

    public DataHandler(string dataFilePath, string dataFileName, string ruleFileName, IDataWatcher dataWatcher)
    {
        this.dataWatcher = dataWatcher;
        this.dataFileName = dataFileName;
        this.ruleFileName = ruleFileName;
        dataFileWatcher = new FileSystemWatcher(dataFilePath);
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
        FileStream dataStream = File.Open(dataFileWatcher.Path + dataFileName, FileMode.Open);
        StreamReader dataReader = new StreamReader(dataStream);

        FileStream ruleStream = File.Open(dataFileWatcher.Path + ruleFileName, FileMode.Open);
        StreamReader ruleReader = new StreamReader(ruleStream);

        string jsonData = dataReader.ReadToEnd();
        string jsonRules = ruleReader.ReadToEnd();

        municipality = JsonUtility.FromJson<Municipality>(jsonData);

        dataStream.Close();
        ruleStream.Close();

        dataWatcher.reactToChange(municipality, jsonRules);
    }

}
