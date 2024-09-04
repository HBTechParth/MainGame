using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Events;
using System.Collections.Generic;

namespace EasyUI.PickerWheelUISAW
{
    public class SAWPickerWheel : MonoBehaviour
    {
        [Header("References :")]
        [SerializeField] private GameObject linePrefab;
        [SerializeField] private Transform linesParent;

        [Space]
        [SerializeField] private Transform PickerWheelTransform;
        [SerializeField] private Transform wheelCircle;
        [SerializeField] private GameObject wheelPiecePrefab;
        [SerializeField] private Transform wheelPiecesParent;

        [Space]
        [Header("Sounds :")]
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip tickAudioClip;
        [SerializeField] [Range(0f, 1f)] private float volume = .5f;
        [SerializeField] [Range(-3f, 3f)] private float pitch = 1f;

        [Space]
        [Header("Picker wheel settings :")]
        [Range(1, 20)] public int spinDuration = 8;
        [SerializeField] [Range(.2f, 2f)] private float wheelSize = 1f;

        [Space]
        [Header("Piece and Line Adjustment :")]
        [SerializeField] private float pieceSpacing = 1f;
        [SerializeField] private float lineSpacing = 1f;

        [Space]
        [Header("Picker wheel pieces :")]
        public SAWWheelPiece[] wheelPieces;

        private UnityAction onSpinStartEvent;
        private UnityAction<SAWWheelPiece> onSpinEndEvent;

        private bool _isSpinning = false;
        public bool IsSpinning { get { return _isSpinning; } }

        private Vector2 pieceMinSize = new Vector2(81f, 146f);
        private Vector2 pieceMaxSize = new Vector2(144f, 213f);
        private int piecesMin = 2;
        private int piecesMax = 18;

        private float pieceAngle;
        private float halfPieceAngle;
        private float halfPieceAngleWithPaddings;

        private double accumulatedWeight;
        private System.Random rand = new System.Random();

        private List<int> nonZeroChancesIndices = new List<int>();
     
        private void Start()
        {
            pieceAngle = 360 / wheelPieces.Length;
            halfPieceAngle = pieceAngle / 2f;
            halfPieceAngleWithPaddings = halfPieceAngle - (halfPieceAngle / 4f);
          
            CalculateWeightsAndIndices();
            if (nonZeroChancesIndices.Count == 0)
                Debug.LogError("You can't set all pieces chance to zero");

            SetupAudio();
        }
        public void OnSpinEnd(UnityAction<SAWWheelPiece> action)
        {
            onSpinEndEvent = action;
        }
        private void SetupAudio()
        {
            audioSource.clip = tickAudioClip;
            audioSource.volume = volume;
            audioSource.pitch = pitch;
        }
        private List<int> rIndices = new List<int>();
        private List<int> yIndices = new List<int>();
        private List<int> bIndices = new List<int>();

        public GameObject pinObject; // Assign your pin object in the Unity Inspector

        public void Spin(int labelNumber)
        {
            rIndices.Clear();
            yIndices.Clear();
            bIndices.Clear();

            for (int i = 0; i < wheelPieces.Length; i++)
            {
                if (wheelPieces[i].Label == "R")
                    rIndices.Add(i);
                else if (wheelPieces[i].Label == "Y")
                    yIndices.Add(i);
                else if (wheelPieces[i].Label == "B")
                    bIndices.Add(i);
            }
            if (!_isSpinning)
            {
                _isSpinning = true;
                if (onSpinStartEvent != null)
                    onSpinStartEvent.Invoke();

                List<int> targetIndices = null;

                // Determine which list to use based on the label number
                switch (labelNumber)
                {
                    case 1:
                        targetIndices = rIndices;
                        break;
                    case 2:
                        targetIndices = yIndices;
                        break;
                    case 3:
                        targetIndices = bIndices;
                        break;
                    default:
                        Debug.LogError("Invalid label number!");
                        return;
                }

                // Ensure there are indices available
                if (targetIndices == null || targetIndices.Count == 0)
                {
                    Debug.LogError("No matching indices found for the given label!");
                    return;
                }

                // Select a random index from the list
                int randomIndex = targetIndices[Random.Range(0, targetIndices.Count)];
                var piece = wheelPieces[randomIndex];

                // Calculate the angle to stop the selected piece under the pin
                float angle = -(pieceAngle * randomIndex);

                // Calculate the rotation required to align the selected piece with the pin
                float targetAngle = angle - (pinObject.transform.eulerAngles.z);

                var targetRotation = Vector3.back * (targetAngle + 2 * 360 * spinDuration);

                float prevAngle, currentAngle;
                prevAngle = currentAngle = wheelCircle.eulerAngles.z;

                var isIndicatorOnTheLine = false;

                wheelCircle
                    .DORotate(targetRotation, spinDuration, RotateMode.FastBeyond360)
                    .SetEase(Ease.InOutQuart)
                    .OnUpdate(() =>
                    {
                        var diff = Mathf.Abs(prevAngle - currentAngle);
                        if (diff >= halfPieceAngle)
                        {
                            if (isIndicatorOnTheLine)
                                if (DataManager.Instance.GetSound() == 0)
                                    audioSource.PlayOneShot(audioSource.clip);
                            prevAngle = currentAngle;
                            isIndicatorOnTheLine = !isIndicatorOnTheLine;
                        }

                        currentAngle = wheelCircle.eulerAngles.z;
                    })
                    .OnComplete(() =>
                    {
                        _isSpinning = false;
                        if (onSpinEndEvent != null)
                            onSpinEndEvent.Invoke(piece);

                        onSpinStartEvent = null;
                        onSpinEndEvent = null;
                    });
            }
        }








        private int GetRandomPieceIndex()
        {
            double r = rand.NextDouble() * accumulatedWeight;

            for (int i = 0; i < wheelPieces.Length; i++)
                if (wheelPieces[i]._weight >= r)
                    return i;

            return 0;
        }

        private void CalculateWeightsAndIndices()
        {
            for (int i = 0; i < wheelPieces.Length; i++)
            {
                SAWWheelPiece piece = wheelPieces[i];

                accumulatedWeight += piece.Chance;
                piece._weight = accumulatedWeight;

                piece.Index = i;

                if (piece.Chance > 0)
                    nonZeroChancesIndices.Add(i);
            }
        }

        private void OnValidate()
        {
            if (PickerWheelTransform != null)
                PickerWheelTransform.localScale = new Vector3(wheelSize, wheelSize, 1f);

            if (wheelPieces.Length > piecesMax || wheelPieces.Length < piecesMin)
                Debug.LogError("[PickerWheel] pieces length must be between " + piecesMin + " and " + piecesMax);

            ArrangePiecesAndLines();
        }

        private void ArrangePiecesAndLines()
        {
            for (int i = 0; i < wheelPieces.Length; i++)
            {
                // Adjust piece rotation
                Transform pieceTrns = wheelPiecesParent.GetChild(i);
                float pieceAngle = i * this.pieceAngle;
                pieceTrns.RotateAround(wheelPiecesParent.position, Vector3.back, pieceAngle);

                // Adjust line rotation
                Transform lineTrns = linesParent.GetChild(i);
                float lineAngle = i * this.pieceAngle;
                lineTrns.RotateAround(wheelPiecesParent.position, Vector3.back, lineAngle);
            }
        }
    }
}
