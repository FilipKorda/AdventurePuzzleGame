using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingCircleInPuzzle : MonoBehaviour
{
    private enum Slot
    {
        Up = 0,
        Right = 1,
        Down = 2,
        Left = 3
    }

    public enum CircleType
    {
        TopCircle,
        BottomCircle
    }

    private struct SymbolMoveData
    {
        public CirclePuzzleSymbol symbol;
        public int fromSlot;
        public int toSlot;
    }

    [Header("Circle")]
    [SerializeField] private CircleType circleType;
    [SerializeField] private Transform rotatingVisual;
    [SerializeField] private Transform symbolOrbitCenter;
    [SerializeField] private float rotateDuration = 0.3f;

    [Header("Shared Slots")]
    [SerializeField] private CirclePuzzleSlot upSlot;
    [SerializeField] private CirclePuzzleSlot rightSlot;
    [SerializeField] private CirclePuzzleSlot downSlot;
    [SerializeField] private CirclePuzzleSlot leftSlot;

    private static bool anyCircleIsRotating = false;

    private Transform[] slotPoints;
    private bool isRotating;

    [SerializeField] private CirclePuzzleWinManager winManager;

    public event Action<RotatingCircleInPuzzle, bool> OnActiveStateChanged;
    [SerializeField] private bool isActive;
    public bool IsActive => isActive;

    private void OnEnable()
    {
        SetActiveState(true);
    }


    private void Awake()
    {
        slotPoints = new Transform[4];
        slotPoints[(int)Slot.Up] = upSlot != null ? upSlot.transform : null;
        slotPoints[(int)Slot.Right] = rightSlot != null ? rightSlot.transform : null;
        slotPoints[(int)Slot.Down] = downSlot != null ? downSlot.transform : null;
        slotPoints[(int)Slot.Left] = leftSlot != null ? leftSlot.transform : null;

        SnapSymbolsToSlots();
    }

    public void SetActiveState(bool value)
    {
        if (isActive == value) return;

        isActive = value;
        OnActiveStateChanged?.Invoke(this, isActive);
    }

    public void TryRotate()
    {
        if (isRotating) return;
        if (anyCircleIsRotating) return;

        StartCoroutine(CourutineRotate());
    }

    private IEnumerator CourutineRotate()
    {
        isRotating = true;
        anyCircleIsRotating = true;

        List<SymbolMoveData> moves = BuildClockwiseMoves();

        Quaternion startRotation = rotatingVisual != null ? rotatingVisual.localRotation : transform.localRotation;
        Quaternion targetRotation = startRotation * Quaternion.Euler(0f, 0f, 90f);

        float time = 0f;

        while (time < rotateDuration)
        {
            time += Time.deltaTime;
            float t = Mathf.Clamp01(time / rotateDuration);

            if (rotatingVisual != null)
                rotatingVisual.localRotation = Quaternion.Lerp(startRotation, targetRotation, t);
            else
                transform.localRotation = Quaternion.Lerp(startRotation, targetRotation, t);

            for (int i = 0; i < moves.Count; i++)
            {
                MoveSymbolAlongClockwisePath(moves[i], t);
            }

            yield return null;
        }

        if (rotatingVisual != null)
            rotatingVisual.localRotation = targetRotation;
        else
            transform.localRotation = targetRotation;

        ApplyNewSymbolLayout(moves);
        SnapSymbolsToSlots();

        winManager.CheckWinPuzzle();

        isRotating = false;
        anyCircleIsRotating = false;
      
    }

    private List<SymbolMoveData> BuildClockwiseMoves()
    {
        List<SymbolMoveData> moves = new List<SymbolMoveData>();

        for (int fromSlot = 0; fromSlot < 4; fromSlot++)
        {
            if (!IsSlotUsable(fromSlot)) continue;

            CirclePuzzleSlot sourceSlot = GetSlotByIndex(fromSlot);
            if (sourceSlot == null) continue;
            if (!sourceSlot.HasSymbol) continue;

            int toSlot = GetNextUsableClockwiseSlot(fromSlot);

            moves.Add(new SymbolMoveData
            {
                symbol = sourceSlot.CurrentSymbol,
                fromSlot = fromSlot,
                toSlot = toSlot
            });
        }

        return moves;
    }

    private void ApplyNewSymbolLayout(List<SymbolMoveData> moves)
    {
        HashSet<CirclePuzzleSlot> uniqueSlots = new HashSet<CirclePuzzleSlot>();

        for (int i = 0; i < 4; i++)
        {
            if (!IsSlotUsable(i)) continue;

            CirclePuzzleSlot slot = GetSlotByIndex(i);
            if (slot == null) continue;

            if (uniqueSlots.Add(slot))
            {
                slot.ClearSymbol();
            }
        }

        for (int i = 0; i < moves.Count; i++)
        {
            CirclePuzzleSlot targetSlot = GetSlotByIndex(moves[i].toSlot);
            if (targetSlot == null) continue;

            targetSlot.SetSymbol(moves[i].symbol);
        }
    }

    private void SnapSymbolsToSlots()
    {
        HashSet<CirclePuzzleSymbol> snappedSymbols = new HashSet<CirclePuzzleSymbol>();

        for (int i = 0; i < 4; i++)
        {
            if (!IsSlotUsable(i)) continue;

            CirclePuzzleSlot slot = GetSlotByIndex(i);
            if (slot == null) continue;
            if (!slot.HasSymbol) continue;

            CirclePuzzleSymbol symbol = slot.CurrentSymbol;
            if (!snappedSymbols.Add(symbol)) continue;

            symbol.transform.position = slot.transform.position;
        }
    }

    private CirclePuzzleSlot GetSlotByIndex(int slotIndex)
    {
        switch ((Slot)slotIndex)
        {
            case Slot.Up: return upSlot;
            case Slot.Right: return rightSlot;
            case Slot.Down: return downSlot;
            case Slot.Left: return leftSlot;
            default: return null;
        }
    }

    private void MoveSymbolAlongClockwisePath(SymbolMoveData moveData, float normalizedTime)
    {
        if (moveData.symbol == null) return;
        if (symbolOrbitCenter == null) return;
        if (slotPoints[moveData.fromSlot] == null) return;

        Vector3 centerPosition = symbolOrbitCenter.position;
        Vector3 fromPosition = slotPoints[moveData.fromSlot].position;

        Vector3 fromOffset = fromPosition - centerPosition;
        float radius = new Vector2(fromOffset.x, fromOffset.z).magnitude;

        float startAngle = Mathf.Atan2(fromOffset.z, fromOffset.x);
        int stepCount = GetClockwiseStepCount(moveData.fromSlot, moveData.toSlot);
        float currentAngle = startAngle - Mathf.Deg2Rad * 90f * stepCount * normalizedTime;

        float x = centerPosition.x + Mathf.Cos(currentAngle) * radius;
        float z = centerPosition.z + Mathf.Sin(currentAngle) * radius;
        float y = fromPosition.y;

        moveData.symbol.transform.position = new Vector3(x, y, z);
    }

    private int GetClockwiseStepCount(int fromSlot, int toSlot)
    {
        int steps = 0;
        int currentSlot = fromSlot;

        while (currentSlot != toSlot)
        {
            currentSlot = (currentSlot + 1) % 4;
            steps++;
        }

        return steps;
    }

    private int GetNextUsableClockwiseSlot(int currentSlot)
    {
        int nextSlot = currentSlot;

        do
        {
            nextSlot = (nextSlot + 1) % 4;
        }
        while (!IsSlotUsable(nextSlot));

        return nextSlot;
    }

    private bool IsSlotUsable(int slot)
    {
        CirclePuzzleSlot slotRef = GetSlotByIndex(slot);
        if (slotRef == null) return false;

        if (circleType == CircleType.TopCircle && slot == (int)Slot.Up)
            return false;

        if (circleType == CircleType.BottomCircle && slot == (int)Slot.Down)
            return false;

        return true;
    }
}
