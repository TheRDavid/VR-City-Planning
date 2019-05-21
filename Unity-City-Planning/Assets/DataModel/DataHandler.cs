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
    private string dataFileName;

    public DataHandler(string dataFilePath, string dataFileName, IDataWatcher dataWatcher)
    {
        this.dataWatcher = dataWatcher;
        this.dataFileName = dataFileName;
        dataFileWatcher = new FileSystemWatcher(dataFilePath);
        dataFileWatcher.Changed += OnFileChanged;
        dataFileWatcher.EnableRaisingEvents = true;
        refresh();
    }

    private void OnFileChanged(object sender, FileSystemEventArgs e)
    {
        Debug.Log($"Change type: {e.ChangeType}");
        if (e.ChangeType.Equals(WatcherChangeTypes.Changed))
        {
            refresh();
        }
    }

    private void refresh()
    {
        Debug.Log("refresh");
        FileStream dataStream = File.Open(dataFileWatcher.Path+dataFileName, FileMode.Open);
        StreamReader reader = new StreamReader(dataStream);
        municipality = JsonUtility.FromJson<Municipality>(reader.ReadToEnd());
        dataStream.Close();

        dataWatcher.reactToChange(municipality);
    }

}
