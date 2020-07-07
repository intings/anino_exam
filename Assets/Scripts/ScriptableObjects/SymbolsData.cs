using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(menuName = "Symbols Data")]
    public class SymbolsData : ScriptableObject
    {
        [SerializeField] private int id;
        [SerializeField] private int[] payOut;
        [SerializeField] private string symbolName;
        [SerializeField] private Sprite sprite;
        public int Id => id;
        public int[] PayOut => payOut;
        public string Name => symbolName;
        public Sprite Sprite => sprite;
    }
}
