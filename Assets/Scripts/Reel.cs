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
    private int _rinseRepeat;
    public Action<int, int[]> stopReel;
    private const int DUPLICATE_COUNT = 5;
    private const float GRID_SIZE = 1.5f;
    private const float TIME_INTERVAL = 0.005f; 
    private const int TICKS_TO_MOVE_ONE_SYMBOL = 6;
    
    
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

    public void StopSpin()
    {
        _rinseRepeat = 3;
    }

    /// <summary>
    /// Will Spin and Stop accordingly to achieve target result
    /// </summary>
    /// <param name="targetResult">Positon of the symbol on the reel</param>
    /// <param name="rinseRepeat">How many times reel will do full rotation</param>
    public void StartSpin(int targetResult, int rinseRepeat)
    {
        _rinseRepeat = rinseRepeat;
        StartCoroutine(Spin(targetResult));
    }
    
    private IEnumerator Spin(int targetResult)
    {
        //Debug.Log(symbols[targetResult].name);
        for (var i = 0; i < _rinseRepeat; i++)
        {
            var targetDistance = targetResult - (((int) (transform.position.y / GRID_SIZE)) + 1);
            if (targetDistance <= 0)
                targetDistance += (symbols.Length - DUPLICATE_COUNT);
            var remainingLoops = targetDistance * TICKS_TO_MOVE_ONE_SYMBOL;
            const float tickDistance = GRID_SIZE / TICKS_TO_MOVE_ONE_SYMBOL;
            for (var j = 0; j < remainingLoops; j++)
            {
                Tick(tickDistance);
                yield return new WaitForSeconds(TIME_INTERVAL);
            }
        }
        GetReelResult();
    }
    
    private IEnumerator Spin2(int targetResult)
    {
        //Debug.Log(targetResult);
        Debug.Log(symbols[targetResult].name);
        Debug.Log(transform.position.y);
        var targetDistance = targetResult - ((int) (transform.position.y / GRID_SIZE)) - 1;
        if (targetDistance <= 0)
            targetDistance += symbols.Length - DUPLICATE_COUNT;
        var remainingLoops = targetDistance * TICKS_TO_MOVE_ONE_SYMBOL;
        const float tickDistance = GRID_SIZE / TICKS_TO_MOVE_ONE_SYMBOL;
        for (var i = 0; i < remainingLoops; i++)
        {
            Tick(tickDistance);
            yield return new WaitForSeconds(TIME_INTERVAL);
        }

        
        // _spinDuration = Random.Range(30, 50);
        // var addSpinTime = _spinDuration % TICKS_TO_MOVE_ONE_SYMBOL;
        // _spinDuration += (TICKS_TO_MOVE_ONE_SYMBOL - addSpinTime);
        //
        // for (var i = 0; i < _spinDuration; i++)
        // {
        //     float timeIntervalToSLowSpin = 0;
        //     var j = i;
        //     Tick(tick, () =>
        //     {
        //         if (j > Mathf.RoundToInt(_spinDuration * 0.25f))
        //             timeIntervalToSLowSpin = 0.05f;
        //         if (j > Mathf.RoundToInt(_spinDuration * 0.5f))
        //             timeIntervalToSLowSpin = 0.1f;
        //         if (j > Mathf.RoundToInt(_spinDuration * 0.75f))
        //             timeIntervalToSLowSpin = 0.15f;
        //         if (j > Mathf.RoundToInt(_spinDuration * 0.95f))
        //             timeIntervalToSLowSpin = 0.2f;
        //     });
        //     yield return new WaitForSeconds(timeIntervalToSLowSpin);
        // }
        GetReelResult();
    }

    private void GetReelResult()
    {
        var reelResults = new int[3];
        for (var i = 0; i < 3; i++)
        {
            reelResults[i] = symbols[(int) (transform.position.y / GRID_SIZE) + i + 1].Value;
        }
        stopReel?.Invoke(
            reelNumber-1,
            reelResults
        );
    }

    private void Tick(float tickDistance, Action reduceSpeed = null)
    {
        var position = transform.position;
        if (position.y < 0)
        {
            var yOffset = ((symbols.Length-DUPLICATE_COUNT) * GRID_SIZE) - tickDistance;
            position = transform.position = new Vector3(position.x, yOffset);
        }
        reduceSpeed?.Invoke();
        transform.position = new Vector3(position.x, position.y - tickDistance);
    }

    private void OnDestroy()
    {
        stopReel = null;
    }
}
