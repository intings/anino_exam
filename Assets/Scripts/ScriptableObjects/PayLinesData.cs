using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(menuName = "Pay Lines Data")]
    public class PayLinesData : ScriptableObject
    {
        public static PayLinesData Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = Resources.LoadAll<PayLinesData>("PayLinesData")[0];
                    return _instance;
                }
                else return _instance;
            }
        }
        private static PayLinesData _instance;
        public PayLine[] payLines;
    }
}
