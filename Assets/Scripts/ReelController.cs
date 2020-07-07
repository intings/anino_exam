using System.Collections.Generic;
using System.Linq;
using ScriptableObjects;
using UnityEngine;

public class ReelController : MonoBehaviour
{
    [SerializeField] private Reel[] reels;
    [SerializeField] private PayLine[] payLines;
    private int _reelsStopped = 0;
    private bool _canSpin = true; //enable again when done displaying wins
    private int[,] _results;
    private List<Win> _wins;

    // private void Awake()
    // {
    //     PayLinesData.Instance.payLines = payLines;
    // }

    public void Spin()
    {
        if (!_canSpin) return;
        _canSpin = false;
        _results = new int[3, 5];
        _wins = new List<Win>();
        var i = 0;
        foreach (var reel in reels)
        {
            reel.stopReel = StopReel;
            reel.StartSpin(30 + (i * 30));
            i++;
        }
    }

    private void StopReel(int reelNumber, int[] rows)
    {
        _reelsStopped++;
        for (var i = 0; i < rows.Length; i++)
        {
            _results[i, reelNumber] = rows[i];
        }

        if (_reelsStopped != reels.Length) return;
        _reelsStopped = 0;
        CheckResults();
    }

    private void CheckResults()
    {
        foreach (var payLine in PayLinesData.Instance.payLines)
        {
            int[] line ={
                _results[payLine.col1, 0],
                _results[payLine.col2, 1],
                _results[payLine.col3, 2],
                _results[payLine.col4, 3],
                _results[payLine.col5, 4]
            };
            var query = from element in line
                group element by element
                into g
                let count = g.Count()
                where count > 2
                select new {Value = g.Key, Count = count};
            foreach (var item in query)
            {
                Debug.Log("Value: " + item.Value + " Count: " + item.Count);
                
                _wins.Add(new Win(payLine, item.Value));
            }

            _canSpin = true;
        }
    }

    private void HighLightSymbols()
    {
        foreach (var win in _wins)
        {
            
        }
    }
}
