using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Symbol : MonoBehaviour
{
    [SerializeField] private int value;
    public int Value => value;
    [SerializeField] private int position;
    public int Position => position;
}
