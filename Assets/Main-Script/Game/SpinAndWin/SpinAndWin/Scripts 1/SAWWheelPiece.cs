using UnityEngine ;
using System.Collections;
using System.Collections.Generic;

namespace EasyUI.PickerWheelUISAW {
   [System.Serializable]
   public class SAWWheelPiece {
      public UnityEngine.Sprite Icon ;
      public string Label ;

      [Tooltip ("Reward amount")] public string Amount ;
      [Tooltip ("isAmount")] public bool  isAmount;
        public List<Transform> lines = new List<Transform>();
        [Tooltip ("Probability in %")] 
      [Range (0f, 100f)] 
      public float Chance = 100f ;

      [HideInInspector] public int Index ;
      [HideInInspector] public double _weight = 0f ;

        public Color c;
   }
}
