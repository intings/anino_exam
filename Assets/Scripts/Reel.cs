using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Reel : MonoBehaviour
{
    [SerializeField] private int reelNumber;
    [SerializeField] private Symbol[] symbols;
    private int _spinDuration;
    public Action<int, int[]> stopReel;
    private const int DUPLICATE_COUNT = 5;
    private const float GRID_SIZE = 1.5f;
    private const float TIME_INTERVAL = 0.005f; 
    private const int TICKS_TO_FULL_SPIN = 6;
    
    private void Awake()
    {
        var initialSymbols = GetComponentsInChildren<Symbol>();
        symbols = new Symbol[initialSymbols.Length + DUPLICATE_COUNT];
        foreach (var symbol in initialSymbols)
        {
            var i = symbol.Position -1;
            symbols[i] = symbol;
        }

        for (int i = initialSymbols.Length, j = 0; i < symbols.Length; i++, j++)
        {
            symbols[i] = Instantiate(initialSymbols[j], transform);
        }
    }

    public void StartSpin(int remainingLoops)
    {
        StartCoroutine(Spin(remainingLoops));
    }

    private IEnumerator Spin(int remainingLoops)
    {
        const float tick = GRID_SIZE / TICKS_TO_FULL_SPIN;
        for (var i = 0; i < remainingLoops; i++)
        {
            Tick(tick);
            yield return new WaitForSeconds(TIME_INTERVAL);
        }

        
        _spinDuration = Random.Range(30, 50);
        var addSpinTime = _spinDuration % TICKS_TO_FULL_SPIN;
        _spinDuration += (TICKS_TO_FULL_SPIN - addSpinTime);

        for (var i = 0; i < _spinDuration; i++)
        {
            float timeIntervalToSLowSpin = 0;
            var j = i;
            Tick(tick, () =>
            {
                if (j > Mathf.RoundToInt(_spinDuration * 0.25f))
                    timeIntervalToSLowSpin = 0.05f;
                if (j > Mathf.RoundToInt(_spinDuration * 0.5f))
                    timeIntervalToSLowSpin = 0.1f;
                if (j > Mathf.RoundToInt(_spinDuration * 0.75f))
                    timeIntervalToSLowSpin = 0.15f;
                if (j > Mathf.RoundToInt(_spinDuration * 0.95f))
                    timeIntervalToSLowSpin = 0.2f;
            });
            yield return new WaitForSeconds(timeIntervalToSLowSpin);
        }
        GetReelResult();
    }

    private void GetReelResult()
    {
        var reelPosition = (int) (transform.position.y / GRID_SIZE);
        var reelResults = new int[3];
        for (var i = 0; i < 3; i++)
        {
            reelResults[i] = symbols[reelPosition + i + 1].Value;
        }
        stopReel?.Invoke(
            reelNumber-1,
            reelResults
        );
    }

    private void Tick(float tick, Action reduceSpeed = null)
    {
        var position = transform.position;
        if (position.y < 0)
        {
            var yOffset = ((symbols.Length-DUPLICATE_COUNT) * GRID_SIZE) - tick;
            position = transform.position = new Vector3(position.x, yOffset);
        }
        reduceSpeed?.Invoke();
        transform.position = new Vector3(position.x, position.y - tick);
    }
}
