using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ReelController : MonoBehaviour
{
    [SerializeField] private Reel[] reels;
    [SerializeField] private Button spinButton;
    [SerializeField] private TextMeshProUGUI spinButtonText;
    [SerializeField] private TextMeshProUGUI coinsText;
    [SerializeField] private TextMeshProUGUI winningsText;
    [SerializeField] private TextMeshProUGUI betText;
    private int _betAmount = 10;
    private int _coins = 100;
    private int _reelsStopped = 0;
    private bool _canSpin = true; //enable again when done displaying wins
    private bool _canStop = false;
    private int[,] _results;
    private List<Win> _wins;

    private void Spin()
    {
        if (!_canSpin || _betAmount == 0) return;
        SoundController.Instance.PlaySoundByIndex(5);
        _canSpin = false;
        _coins -= _betAmount;
        coinsText.text = "COINS : " + _coins;
        winningsText.text = "WINNINGS : ";
        _results = new int[3, 5];
        spinButton.onClick.RemoveListener(Spin);
        spinButton.onClick.AddListener(Stop);
        spinButtonText.text = "STOP";
        _wins = new List<Win>();
        var i = 0;
        var targetResults = new List<int>();
        var reelLength = SymbolsDataHolder.Instance.symbolsData.Length;
        foreach (var reel in reels)
        {
            var targetResult = SetTargetBeforeSpinning(reelLength);
            targetResults.Add(targetResult);
            reel.stopReel = StopReel;
            reel.StartSpin(targetResult, i + 4);
            i++;
        }
        _canStop = true;
    }

    private void Stop()
    {
        if (!_canStop) return;
        SoundController.Instance.PlaySoundByIndex(6);
        _canStop = false;
        spinButton.enabled = false;
        foreach (var reel in reels)
        {
            reel.StopSpin();
        }
    }

    private static int SetTargetBeforeSpinning(int reelLength)
    {
        return Random.Range(0, reelLength - 1);
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
            if (payLine.CheckResult(_results, out var win))
                _wins.Add(win);
            
        }

        StartCoroutine(DisplayWinnings());
    }

    private IEnumerator DisplayWinnings()
    {
        var symbolsData = SymbolsDataHolder.Instance.symbolsData;
        var winAmount = 0;
        GameObject lastDisplayedLine = null;
        foreach (var win in _wins)
        {
            if (lastDisplayedLine != null)
                Destroy(lastDisplayedLine);
            lastDisplayedLine = Instantiate(win.PayLine.spriteRenderer.gameObject);
            SoundController.Instance.PlaySoundByIndex(7);
            var linePayout = symbolsData[win.Value - 1].PayOut[win.Count - 1];
            linePayout *= (_betAmount / 10);
            winAmount += linePayout;
            yield return new WaitForSeconds(1);
        }
        if (lastDisplayedLine != null)
            Destroy(lastDisplayedLine);
        if (winAmount > 0)
        {
            SoundController.Instance.PlaySoundByIndex(4);
            _coins += winAmount;
            winningsText.text = "WINNINGS : " + winAmount;
            coinsText.text = "COINS : " + _coins;
        }
        CheckIfBetAllowed();
        _canSpin = true;
        spinButton.enabled = true;
        spinButton.onClick.RemoveListener(Stop);
        spinButton.onClick.AddListener(Spin);
        spinButtonText.text = "SPIN";
        StopAllCoroutines();
    }

    public void AddBet(int amount)
    {
        var soundIndex = amount > 0 ? 1 : 0;
        _betAmount += amount;
        soundIndex = CheckIfBetAllowed() ? soundIndex : 3;
        SoundController.Instance.PlaySoundByIndex(soundIndex);

    }

    private bool CheckIfBetAllowed()
    {
        var allow = true;
        if (_betAmount < 0) _betAmount = 0;
        while (_coins < _betAmount)
        {
            allow = false;
            _betAmount -= 10;
        }
        betText.text = "BET : " + _betAmount;
        return allow;
    }
}
