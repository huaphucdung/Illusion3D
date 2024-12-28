using AYellowpaper.SerializedCollections;
using System.Collections.Generic;
using UnityEngine;

public class RiverManager : MonoBehaviour
{
    [SerializedDictionary("Source", "Active")]
    [SerializeField] private SerializedDictionary<River, bool> sourceDictionary = new SerializedDictionary<River, bool>();

    public void ActiveNewRun()
    {
        foreach(KeyValuePair<River, bool> source in sourceDictionary)
        {
            source.Key.ActiveRiverWater(source.Value);
        }
    }

    public void ChangeRiverSources(Dictionary<River,bool> dictionary)
    {
        foreach (KeyValuePair<River, bool> sources in dictionary)
        {
            if(sourceDictionary.ContainsKey(sources.Key)) {
                sourceDictionary[sources.Key] = sources.Value;
            }
        }
        ActiveNewRun();
    }
}