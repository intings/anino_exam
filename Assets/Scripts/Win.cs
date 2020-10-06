using System.Collections;
using ScriptableObjects;
using UnityEngine;

public class Win
{
    public int Amount;
    private readonly PayLine payLine;
    private readonly int value;
    private readonly int count;
    
    public Win(PayLine payLine, int value, int count)
    {
        this.payLine = payLine;
        this.value = value;
        this.count = count;
    }

    public IEnumerator Show(SymbolsData[] symbolsData, int betAmount)
    {
        var lastDisplayedLine = Object.Instantiate(payLine.spriteRenderer.gameObject);
        SoundController.Instance.PlaySoundByIndex(7);
        var linePayout = symbolsData[value - 1].PayOut[count - 1];
        Amount = linePayout * (betAmount / 10);
        yield return new WaitForSeconds(1);
        Object.Destroy(lastDisplayedLine);
    }
}
