using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using UnityEngine;

[Serializable]
public class PayLine
{
    public int col1;
    public int col2;
    public int col3;
    public int col4;
    public int col5;
    public SpriteRenderer spriteRenderer;

    public bool CheckResult(int[,] results, out Win win)
    {
        win = null;
        int[] line ={
            results[col1, 0],
            results[col2, 1],
            results[col3, 2],
            results[col4, 3],
            results[col5, 4]
        };
        var query = from element in line
            group element by element
            into g
            let count = g.Count()
            where count > 2
            select new {Value = g.Key, Count = count};
        foreach (var item in query)
        {
            win = new Win(this, item.Value, item.Count);
        }

        return (win != null);
    }
}
