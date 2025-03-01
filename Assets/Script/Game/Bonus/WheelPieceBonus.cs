using UnityEngine ;

namespace EasyUI.PickerWheelUIBonus {
   [System.Serializable]
   public class WheelPieceBonus {
      public UnityEngine.Sprite Icon ;
      public string Label ;

      [Tooltip ("Reward amount")] public string Amount ;
      [Tooltip ("isAmount")] public bool  isAmount;

        [Tooltip ("Probability in %")] 
      [Range (0f, 100f)] 
      public float Chance = 100f ;

      [HideInInspector] public int Index ;
      [HideInInspector] public double _weight = 0f ;

        public Color c;
   }
}
