using System;
using ScriptableObjects;
using UnityEngine;

public class SymbolsDataHolder : MonoBehaviour
{
    public static SymbolsDataHolder Instance => _instance;
    private static SymbolsDataHolder _instance;
    public SymbolsData[] symbolsData;

    private void Awake()
    {
        if (_instance == null)
        {
            DontDestroyOnLoad(gameObject);
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        symbolsData = Resources.LoadAll<SymbolsData>("SymbolsData");
    }
}
