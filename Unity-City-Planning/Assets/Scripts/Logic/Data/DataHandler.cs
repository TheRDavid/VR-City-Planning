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
    private string dataFilePath, conditionsFilePath;

    public DataHandler(string cityDataDir, string dataFileName, IDataWatcher dataWatcher)
    {
        this.dataWatcher = dataWatcher;
        this.dataFilePath = cityDataDir + dataFileName + ".json";
        this.conditionsFilePath = cityDataDir + dataFileName + ".conditions.json";
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
        if (!(File.Exists(dataFilePath) && File.Exists(conditionsFilePath)))
        {
            ErrorHandler.instance.reportError("Could not refresh data, make sure these files exist:\n"+dataFilePath+"\n"+conditionsFilePath);
            return;
        }

        FileStream dataStream = File.Open(dataFilePath, FileMode.Open);
        StreamReader dataReader = new StreamReader(dataStream);

        FileStream conditionsStream = File.Open(conditionsFilePath, FileMode.Open);
        StreamReader conditionsReader = new StreamReader(conditionsStream);

        string jsonData = dataReader.ReadToEnd();
        string jsonConditions = conditionsReader.ReadToEnd();
        dataStream.Close();
        conditionsReader.Close();
        try
        {
            municipality = JsonUtility.FromJson<Municipality>(jsonData);
        } catch(ArgumentException ae)
        {
            ErrorHandler.instance.reportError("Data file " + dataStream.Name + " can not be read as Municipality -> it appears to be corrupt.\nDetails:\n" + ae.ToString());
            return;
        }
        try
        {
            conditionList = JsonUtility.FromJson<ConditionList>(jsonConditions);
        } catch(ArgumentException ae)
        {
            ErrorHandler.instance.reportError("Conditions file " + conditionsStream.Name + " can not be read as list of conditions -> it appears to be corrupt.\nDetails:\n" + ae.ToString());
            return;
        }

        dataWatcher.reactToChange(municipality, conditionList);
    }

}
