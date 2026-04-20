using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class TwelveDotsPuzzleManager : MonoBehaviour
{
    public static TwelveDotsPuzzleManager Instance { get; private set; }

    [System.Serializable]
    private class DotConnection
    {
        public DotFromDotsPuzzle fromDot;
        public DotFromDotsPuzzle toDot;
    }

    [SerializeField] private DotFromDotsPuzzle[] allDots;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private TwelveDotPuzzle twelveDotPuzzle;

    private DotFromDotsPuzzle currentSelectedDot;
    private readonly List<DotConnection> connections = new List<DotConnection>();
    private readonly List<Vector3> linePoints = new List<Vector3>();
    private readonly Dictionary<DotFromDotsPuzzle, int> dotConnectionUsage = new Dictionary<DotFromDotsPuzzle, int>();

    [SerializeField] private float lineDrawDuration = 0.2f;

    private Coroutine lineAnimationCoroutine;

    [SerializeField] private BoxCollider boxCollider;
    [SerializeField] private BoxCollider boxColliderPressurePlate;
    [SerializeField] private ChestManager chestManager;
    [SerializeField] private TrapDoorVerticalManager trapDoorVerticalManager;
    [SerializeField] private GameObject pillar;



    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        if (lineRenderer == null)
        {
            lineRenderer = GetComponent<LineRenderer>();
        }

        InitializeConnectionUsage();
        ResetLineRenderer();
    }

    private void InitializeConnectionUsage()
    {
        dotConnectionUsage.Clear();

        for (int i = 0; i < allDots.Length; i++)
        {
            DotFromDotsPuzzle dot = allDots[i];
            if (dot == null) continue;

            if (!dotConnectionUsage.ContainsKey(dot))
            {
                dotConnectionUsage.Add(dot, 0);
            }
        }
    }

    public void TrySelectDot(DotFromDotsPuzzle clickedDot)
    {
        if (clickedDot == null) return;

        if (currentSelectedDot == null)
        {
            if (!HasFreeConnectionSlot(clickedDot))
            {
                Debug.Log(clickedDot.name + " has no free connection slots.");
                return;
            }

            currentSelectedDot = clickedDot;
            currentSelectedDot.SelectFirstDot();
            AddLinePoint(clickedDot.transform.position);
            StartCurrentDotPulse();
            return;
        }


        if (clickedDot == currentSelectedDot)
        {
            Debug.Log("You clicked the same dot again.");
            return;
        }

        if (!currentSelectedDot.CanConnectTo(clickedDot))
        {
            Debug.Log(currentSelectedDot.name + " cannot connect to " + clickedDot.name);
            return;
        }

        if (!HasFreeConnectionSlot(currentSelectedDot))
        {
            Debug.Log(currentSelectedDot.name + " reached its connection limit.");
            return;
        }

        if (!HasFreeConnectionSlot(clickedDot))
        {
            Debug.Log(clickedDot.name + " reached its connection limit.");
            return;
        }

        CreateConnection(currentSelectedDot, clickedDot);

        IncreaseUsage(currentSelectedDot);
        IncreaseUsage(clickedDot);

        StopCurrentDotPulse();

        currentSelectedDot.DeselectDot();
        clickedDot.SelectNextDot();
        currentSelectedDot = clickedDot;

        AddLinePoint(clickedDot.transform.position);
        StartCurrentDotPulse();



    }

    private void CreateConnection(DotFromDotsPuzzle fromDot, DotFromDotsPuzzle toDot)
    {
        DotConnection connection = new DotConnection
        {
            fromDot = fromDot,
            toDot = toDot
        };

        connections.Add(connection);
        Debug.Log("Connected: " + fromDot.name + " -> " + toDot.name);
    }

    private bool HasFreeConnectionSlot(DotFromDotsPuzzle dot)
    {
        if (dot == null) return false;

        if (!dotConnectionUsage.ContainsKey(dot))
        {
            dotConnectionUsage.Add(dot, 0);
        }

        int currentUsage = dotConnectionUsage[dot];
        int maxUsage = dot.AvailableConnections.Length;

        return currentUsage < maxUsage;
    }

    private void IncreaseUsage(DotFromDotsPuzzle dot)
    {
        if (dot == null) return;

        if (!dotConnectionUsage.ContainsKey(dot))
        {
            dotConnectionUsage.Add(dot, 0);
        }

        dotConnectionUsage[dot]++;
    }

    private void AddLinePoint(Vector3 targetPoint)
    {
        if (linePoints.Count == 0)
        {
            linePoints.Add(targetPoint);
            lineRenderer.positionCount = linePoints.Count;
            lineRenderer.SetPositions(linePoints.ToArray());
            return;
        }

        Vector3 startPoint = linePoints[linePoints.Count - 1];

        linePoints.Add(startPoint);
        lineRenderer.positionCount = linePoints.Count;
        lineRenderer.SetPositions(linePoints.ToArray());

        if (lineAnimationCoroutine != null)
        {
            StopCoroutine(lineAnimationCoroutine);
        }

        lineAnimationCoroutine = StartCoroutine(AnimateLastLinePoint(startPoint, targetPoint));
    }

    private IEnumerator AnimateLastLinePoint(Vector3 startPoint, Vector3 targetPoint)
    {
        float time = 0f;
        int lastIndex = linePoints.Count - 1;

        while (time < lineDrawDuration)
        {
            time += Time.deltaTime;
            float t = Mathf.Clamp01(time / lineDrawDuration);

            linePoints[lastIndex] = Vector3.Lerp(startPoint, targetPoint, t);
            lineRenderer.SetPosition(lastIndex, linePoints[lastIndex]);

            yield return null;
        }

        linePoints[lastIndex] = targetPoint;
        lineRenderer.SetPosition(lastIndex, targetPoint);
        lineAnimationCoroutine = null;
        CheckWinPuzzle();
    }


    private void ResetLineRenderer()
    {
        linePoints.Clear();
        lineRenderer.positionCount = 0;
    }

    public void ResetPuzzle()
    {
        StopCurrentDotPulse();

        if (currentSelectedDot != null)
        {
            currentSelectedDot.DeselectDot();
            currentSelectedDot = null;
        }

        for (int i = 0; i < allDots.Length; i++)
        {
            if (allDots[i] == null) continue;
            allDots[i].ResetDot();
        }

        connections.Clear();
        InitializeConnectionUsage();
        ResetLineRenderer();

        Debug.Log("Dots puzzle reset.");
    }

    private bool ConnectionAlreadyExists(DotFromDotsPuzzle dotA, DotFromDotsPuzzle dotB)
    {
        for (int i = 0; i < connections.Count; i++)
        {
            DotFromDotsPuzzle fromDot = connections[i].fromDot;
            DotFromDotsPuzzle toDot = connections[i].toDot;

            if ((fromDot == dotA && toDot == dotB) || (fromDot == dotB && toDot == dotA))
            {
                return true;
            }
        }

        return false;
    }


    private void StartCurrentDotPulse()
    {
        if (currentSelectedDot == null) return;

        DotFromDotsPuzzle[] availableConnections = currentSelectedDot.AvailableConnections;

        for (int i = 0; i < availableConnections.Length; i++)
        {
            DotFromDotsPuzzle nextDot = availableConnections[i];

            if (nextDot == null) continue;
            if (!HasFreeConnectionSlot(nextDot)) continue;
            if (ConnectionAlreadyExists(currentSelectedDot, nextDot)) continue;

            nextDot.StartPulse();
        }
    }


    private void StopCurrentDotPulse()
    {
        if (currentSelectedDot == null) return;

        DotFromDotsPuzzle[] availableConnections = currentSelectedDot.AvailableConnections;

        for (int i = 0; i < availableConnections.Length; i++)
        {
            if (availableConnections[i] == null) continue;
            availableConnections[i].StopPulse();
        }
    }



    private void CheckWinPuzzle()
    {
        HashSet<string> possibleConnections = GetAllPossibleConnections();
        HashSet<string> usedConnections = GetUsedConnections();

        Debug.Log("Possible: " + possibleConnections.Count + " Used: " + usedConnections.Count + " LinePoints: " + lineRenderer.positionCount);

        if (possibleConnections.Count == 0) return;

        foreach (string connection in possibleConnections)
        {
            if (!usedConnections.Contains(connection))
            {
                Debug.Log("Missing connection: " + connection);
                return;
            }
        }

        WinPuzzle();
    }

    private void WinPuzzle()
    {
        boxCollider.enabled = false;
        twelveDotPuzzle.ExitAfterWin();
        DisableAllDots();
        StopCurrentDotPulse();
        boxColliderPressurePlate.enabled = true;
        StartEndSequence();
        chestManager.OpenChest();
        Services.Audio.PlaySFX("SafeWinAkaPuzzleWin");
    }

    private void StartEndSequence()
    {
        StartCoroutine(CloseTrapDoorsAndMovePillarDownCoroutine());
    }

    private IEnumerator CloseTrapDoorsAndMovePillarDownCoroutine()
    {
        trapDoorVerticalManager.ActiveAnimation();

        yield return new WaitForSeconds(3.1f);
        ResetPuzzle();
        Vector3 startPos = pillar.transform.position;
        Vector3 targetPos = startPos + Vector3.down * 2f;
        float time = 0f;
        float duration = 3f;
        while (time < duration)
        {
            time += Time.deltaTime;
            float t = Mathf.Clamp01(time / duration);
            pillar.transform.position = Vector3.Lerp(startPos, targetPos, t);
            yield return null;
        }
        pillar.transform.position = targetPos;
    }

    private void DisableAllDots()
    {
        foreach (var dot in allDots)
        {
            if (dot != null)
            {
                dot.GetComponent<BoxCollider>().enabled = false;
            }
        }
    }

    private string GetConnectionKey(DotFromDotsPuzzle dotA, DotFromDotsPuzzle dotB)
    {
        int indexA = dotA.DotIndex;
        int indexB = dotB.DotIndex;

        if (indexA < indexB)
            return indexA + "_" + indexB;

        return indexB + "_" + indexA;
    }

    private HashSet<string> GetAllPossibleConnections()
    {
        HashSet<string> possibleConnections = new HashSet<string>();

        for (int i = 0; i < allDots.Length; i++)
        {
            DotFromDotsPuzzle dot = allDots[i];
            if (dot == null) continue;

            DotFromDotsPuzzle[] availableConnections = dot.AvailableConnections;

            for (int j = 0; j < availableConnections.Length; j++)
            {
                DotFromDotsPuzzle targetDot = availableConnections[j];
                if (targetDot == null) continue;

                possibleConnections.Add(GetConnectionKey(dot, targetDot));
            }
        }

        return possibleConnections;
    }
    private HashSet<string> GetUsedConnections()
    {
        HashSet<string> usedConnections = new HashSet<string>();

        for (int i = 0; i < connections.Count; i++)
        {
            DotFromDotsPuzzle fromDot = connections[i].fromDot;
            DotFromDotsPuzzle toDot = connections[i].toDot;

            if (fromDot == null || toDot == null) continue;

            usedConnections.Add(GetConnectionKey(fromDot, toDot));
        }

        return usedConnections;
    }

}
