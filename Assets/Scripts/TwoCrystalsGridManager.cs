using System.Collections;
using UnityEngine;

public class TwoCrystalsGridManager : MonoBehaviour
{
    public enum CrystalOrientation
    {
        Vertical,
        Horizontal
    }

    public enum OccupancyType
    {
        Empty,
        Player,
        Enemy,
        Wall
    }

    [System.Serializable]
    public class CrystalGridData
    {
        public MovingCrystal crystal;
        public bool isPlayer;
        public int startY;
        public int startX;
        public int length = 3;
        public CrystalOrientation orientation = CrystalOrientation.Vertical;
    }

    [System.Serializable]
    public class WallGridData
    {
        public int startY;
        public int startX;
        public int length = 1;
        public CrystalOrientation orientation = CrystalOrientation.Vertical;
    }

    [Header("Grid Settings")]
    [SerializeField] private int gridHeight = 13;
    [SerializeField] private int gridWidth = 6;
    [SerializeField] private float cellSize = 1f;
    [SerializeField] private Transform gridOrigin;

    [Header("Crystal Rotations")]
    [SerializeField] private Vector3 verticalRotationEuler;
    [SerializeField] private Vector3 horizontalRotationEuler;

    [Header("Crystals")]
    [SerializeField] private CrystalGridData[] crystals = new CrystalGridData[10];

    [Header("Walls")]
    [SerializeField] private WallGridData[] walls;

    [Header("Gizmos")]
    [SerializeField] private bool drawGridGizmos = true;
    [SerializeField] private Color gridColor = Color.white;
    [SerializeField] private Color playerColor = Color.green;
    [SerializeField] private Color enemyColor = Color.red;
    [SerializeField] private Color wallColor = Color.blue;
    [SerializeField] private Color originColor = Color.yellow;

    private OccupancyType[,] occupancyGrid;

    [Header("Players")]
    [SerializeField] private MovingCrystal firstPlayer;
    [SerializeField] private MovingCrystal secondPlayer;

    [Header("Win Positions")]
    [SerializeField] private int firstPlayerWinY = 13;
    [SerializeField] private int firstPlayerWinX = 3;
    [SerializeField] private int secondPlayerWinY = 1;
    [SerializeField] private int secondPlayerWinX = 4;

    [SerializeField] private TwoCrystalsPuzzle twoCrystalsPuzzle;
    [SerializeField] private BoxCollider enterPuzzleBoxCollider;
    [SerializeField] private TrapDoorVerticalManager trapDoorVerticalManager;
    [SerializeField] private OpenTrapDoorsManager openTrapDoorsManager;

    [Header("Pillar")]
    [SerializeField] private GameObject pillar;
    [SerializeField] private float pillarOffset;


    [SerializeField] private RotateAllGearsRoomTen rotateAllGearsRoomTen;

    private void Awake()
    {
        BuildGrid();
        DisableAllColliders();
    }

    private void OnEnable()
    {
        if (firstPlayer != null)
            firstPlayer.OnActiveStateChanged += HandleCrystalActiveChanged;

        if (secondPlayer != null)
            secondPlayer.OnActiveStateChanged += HandleCrystalActiveChanged;
    }

    private void OnDisable()
    {
        if (firstPlayer != null)
            firstPlayer.OnActiveStateChanged -= HandleCrystalActiveChanged;

        if (secondPlayer != null)
            secondPlayer.OnActiveStateChanged -= HandleCrystalActiveChanged;
    }


    private void HandleCrystalActiveChanged(MovingCrystal crystal, bool isActive)
    {
        CheckPlayersActiveState();
    }

    private void CheckPlayersActiveState()
    {
        if (firstPlayer == null || secondPlayer == null) return;

        if (firstPlayer.IsActive && secondPlayer.IsActive)
        {
            trapDoorVerticalManager.ActiveAnimation();
            ActiveAllColliders();
        }
    }


    public void CheckWinPuzzle()
    {
        if (firstPlayer == null || secondPlayer == null) return;

        CrystalGridData firstPlayerData = GetCrystalData(firstPlayer);
        CrystalGridData secondPlayerData = GetCrystalData(secondPlayer);

        if (firstPlayerData == null || secondPlayerData == null) return;

        bool firstPlayerCorrect =
            firstPlayerData.startY == firstPlayerWinY &&
            firstPlayerData.startX == firstPlayerWinX;

        bool secondPlayerCorrect =
            secondPlayerData.startY == secondPlayerWinY &&
            secondPlayerData.startX == secondPlayerWinX;

        if (firstPlayerCorrect && secondPlayerCorrect)
        {
            WinPuzzle();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            WinPuzzle();
        }
    }

    private void WinPuzzle()
    {
        DisableAllColliders();
        twoCrystalsPuzzle.ExitAfterWin();
        openTrapDoorsManager.OpenAnimation();
        StartCoroutine(MovePillarDown());
        Services.Audio.PlaySFX("SafeWinAkaPuzzleWin");
    }

    private IEnumerator MovePillarDown()
    {
        rotateAllGearsRoomTen.RotateAllGears();

        yield return new WaitForSeconds(1f);

        Vector3 startPosition = pillar.transform.position;
        Vector3 targetPosition = startPosition + Vector3.down * pillarOffset;
        float duration = 3f;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = Mathf.Clamp01(time / duration);

            pillar.transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            yield return null;
        }

        pillar.transform.position = targetPosition;
    }


    private void DisableAllColliders()
    {
        enterPuzzleBoxCollider.enabled = false;

        foreach (var coll in crystals)
        {
            coll.crystal.boxCollider.enabled = false;
        }
    }


    private void ActiveAllColliders()
    {
        enterPuzzleBoxCollider.enabled = true;

        foreach (var coll in crystals)
        {
            coll.crystal.boxCollider.enabled = true;
        }
    }


    public void BuildGrid()
    {
        occupancyGrid = new OccupancyType[gridHeight, gridWidth];
        PlaceWalls();
        PlaceAllCrystals();
    }

    private CrystalGridData GetCrystalData(MovingCrystal crystal)
    {
        for (int i = 0; i < crystals.Length; i++)
        {
            CrystalGridData data = crystals[i];

            if (data != null && data.crystal == crystal)
                return data;
        }

        return null;
    }


    public bool TryMoveCrystal(MovingCrystal crystal, int deltaY, int deltaX, out Vector3 targetWorldPosition)
    {
        targetWorldPosition = Vector3.zero;

        CrystalGridData data = GetCrystalData(crystal);
        if (data == null) return false;

        int targetStartY = data.startY + deltaY;
        int targetStartX = data.startX + deltaX;

        if (!CanMoveTo(data, targetStartY, targetStartX))
            return false;

        ClearCrystalCells(data);

        data.startY = targetStartY;
        data.startX = targetStartX;

        OccupyCrystalCells(data);
        targetWorldPosition = GetCrystalCenterWorldPosition(data);

        return true;
    }


    private void ClearCrystalCells(CrystalGridData data)
    {
        for (int i = 0; i < data.length; i++)
        {
            int y = data.startY;
            int x = data.startX;

            if (data.orientation == CrystalOrientation.Vertical)
                y += i;
            else
                x += i;

            occupancyGrid[y, x] = OccupancyType.Empty;
        }
    }


    private bool CanMoveTo(CrystalGridData data, int targetStartY, int targetStartX)
    {
        for (int i = 0; i < data.length; i++)
        {
            int y = targetStartY;
            int x = targetStartX;

            if (data.orientation == CrystalOrientation.Vertical)
                y += i;
            else
                x += i;

            if (!IsInsideGrid(y, x))
                return false;

            OccupancyType cellType = occupancyGrid[y, x];

            if (cellType == OccupancyType.Empty)
                continue;

            if (IsCurrentCrystalCell(data, y, x))
                continue;

            return false;
        }

        return true;
    }

    private bool IsCurrentCrystalCell(CrystalGridData data, int y, int x)
    {
        for (int i = 0; i < data.length; i++)
        {
            int currentY = data.startY;
            int currentX = data.startX;

            if (data.orientation == CrystalOrientation.Vertical)
                currentY += i;
            else
                currentX += i;

            if (currentY == y && currentX == x)
                return true;
        }

        return false;
    }


    private void PlaceWalls()
    {
        if (walls == null) return;

        for (int i = 0; i < walls.Length; i++)
        {
            WallGridData data = walls[i];
            if (!CanPlaceWall(data))
            {
                continue;
            }

            OccupyWallCells(data);
        }
    }

    private void PlaceAllCrystals()
    {
        for (int i = 0; i < crystals.Length; i++)
        {
            CrystalGridData data = crystals[i];

            if (data == null || data.crystal == null)
                continue;

            if (!CanPlaceCrystal(data))
            {
                continue;
            }

            OccupyCrystalCells(data);
            SetCrystalTransform(data);
        }
    }

    private bool CanPlaceCrystal(CrystalGridData data)
    {
        for (int i = 0; i < data.length; i++)
        {
            int y = data.startY;
            int x = data.startX;

            if (data.orientation == CrystalOrientation.Vertical)
                y += i;
            else
                x += i;

            if (!IsInsideGrid(y, x))
                return false;

            if (occupancyGrid[y, x] != OccupancyType.Empty)
                return false;
        }

        return true;
    }

    private bool CanPlaceWall(WallGridData data)
    {
        for (int i = 0; i < data.length; i++)
        {
            int y = data.startY;
            int x = data.startX;

            if (data.orientation == CrystalOrientation.Vertical)
                y += i;
            else
                x += i;

            if (!IsInsideGrid(y, x))
                return false;

            if (occupancyGrid[y, x] != OccupancyType.Empty)
                return false;
        }

        return true;
    }

    private void OccupyCrystalCells(CrystalGridData data)
    {
        OccupancyType occupancyType = data.isPlayer ? OccupancyType.Player : OccupancyType.Enemy;

        for (int i = 0; i < data.length; i++)
        {
            int y = data.startY;
            int x = data.startX;

            if (data.orientation == CrystalOrientation.Vertical)
                y += i;
            else
                x += i;

            occupancyGrid[y, x] = occupancyType;
        }
    }

    private void OccupyWallCells(WallGridData data)
    {
        for (int i = 0; i < data.length; i++)
        {
            int y = data.startY;
            int x = data.startX;

            if (data.orientation == CrystalOrientation.Vertical)
                y += i;
            else
                x += i;

            occupancyGrid[y, x] = OccupancyType.Wall;
        }
    }

    private void SetCrystalTransform(CrystalGridData data)
    {
        Vector3 worldPosition = GetCrystalCenterWorldPosition(data);
        data.crystal.transform.position = worldPosition;

        if (data.orientation == CrystalOrientation.Vertical)
            data.crystal.transform.rotation = Quaternion.Euler(verticalRotationEuler);
        else
            data.crystal.transform.rotation = Quaternion.Euler(horizontalRotationEuler);
    }

    private Vector3 GetCrystalCenterWorldPosition(CrystalGridData data)
    {
        Vector3 originPosition = gridOrigin != null ? gridOrigin.position : transform.position;

        float centerYOffset = 0f;
        float centerXOffset = 0f;

        if (data.orientation == CrystalOrientation.Vertical)
            centerYOffset = (data.length - 1) * 0.5f * cellSize;
        else
            centerXOffset = (data.length - 1) * 0.5f * cellSize;

        return new Vector3(
            originPosition.x + (data.startX + 0.5f) * cellSize + centerXOffset,
            originPosition.y + (data.startY + 0.5f) * cellSize + centerYOffset,
            originPosition.z
        );
    }


    private bool IsInsideGrid(int y, int x)
    {
        return y >= 0 && y < gridHeight && x >= 0 && x < gridWidth;
    }

    public bool IsCellBlocked(int y, int x)
    {
        if (!IsInsideGrid(y, x))
            return true;

        return occupancyGrid[y, x] != OccupancyType.Empty;
    }

    public OccupancyType GetCellOccupancy(int y, int x)
    {
        if (!IsInsideGrid(y, x))
            return OccupancyType.Wall;

        return occupancyGrid[y, x];
    }

    private void OnDrawGizmos()
    {
#if UNITY_EDITOR
        if (!drawGridGizmos) return;

        Vector3 originPosition = gridOrigin != null ? gridOrigin.position : transform.position;

        DrawOrigin(originPosition);
        DrawGrid(originPosition);
        DrawOccupiedCells(originPosition);
#endif
    }

    private void DrawOrigin(Vector3 originPosition)
    {
        Gizmos.color = originColor;
        Gizmos.DrawSphere(originPosition, 0.1f);
    }

    private void DrawGrid(Vector3 originPosition)
    {
        Gizmos.color = gridColor;

        for (int y = 0; y <= gridHeight; y++)
        {
            Vector3 start = new Vector3(
                originPosition.x,
                originPosition.y + y * cellSize,
                originPosition.z
            );

            Vector3 end = new Vector3(
                originPosition.x + gridWidth * cellSize,
                originPosition.y + y * cellSize,
                originPosition.z
            );

            Gizmos.DrawLine(start, end);
        }

        for (int x = 0; x <= gridWidth; x++)
        {
            Vector3 start = new Vector3(
                originPosition.x + x * cellSize,
                originPosition.y,
                originPosition.z
            );

            Vector3 end = new Vector3(
                originPosition.x + x * cellSize,
                originPosition.y + gridHeight * cellSize,
                originPosition.z
            );

            Gizmos.DrawLine(start, end);
        }
    }

    private void DrawOccupiedCells(Vector3 originPosition)
    {
        if (occupancyGrid == null)
        {
            DrawEditorPreview(originPosition);
            return;
        }

        for (int y = 0; y < gridHeight; y++)
        {
            for (int x = 0; x < gridWidth; x++)
            {
                OccupancyType type = occupancyGrid[y, x];
                if (type == OccupancyType.Empty) continue;

                Gizmos.color = GetOccupancyColor(type);

                Vector3 cellCenter = new Vector3(
                    originPosition.x + x * cellSize + cellSize * 0.5f,
                    originPosition.y + y * cellSize + cellSize * 0.5f,
                    originPosition.z
                );

                Gizmos.DrawWireCube(cellCenter, new Vector3(cellSize, cellSize, 0.05f));
            }
        }
    }

    private void DrawEditorPreview(Vector3 originPosition)
    {
        if (walls != null)
        {
            for (int i = 0; i < walls.Length; i++)
            {
                DrawPreviewCells(originPosition, walls[i].startY, walls[i].startX, walls[i].length, walls[i].orientation, OccupancyType.Wall);
            }
        }

        if (crystals != null)
        {
            for (int i = 0; i < crystals.Length; i++)
            {
                CrystalGridData data = crystals[i];
                if (data == null) continue;

                DrawPreviewCells(
                    originPosition,
                    data.startY,
                    data.startX,
                    data.length,
                    data.orientation,
                    data.isPlayer ? OccupancyType.Player : OccupancyType.Enemy
                );
            }
        }
    }

    private void DrawPreviewCells(Vector3 originPosition, int startY, int startX, int length, CrystalOrientation orientation, OccupancyType type)
    {
        Gizmos.color = GetOccupancyColor(type);

        for (int i = 0; i < length; i++)
        {
            int y = startY;
            int x = startX;

            if (orientation == CrystalOrientation.Vertical)
                y += i;
            else
                x += i;

            if (!IsInsideGrid(y, x)) continue;

            Vector3 cellCenter = new Vector3(
                originPosition.x + x * cellSize + cellSize * 0.5f,
                originPosition.y + y * cellSize + cellSize * 0.5f,
                originPosition.z
            );

            Gizmos.DrawWireCube(cellCenter, new Vector3(cellSize, cellSize, 0.05f));
        }
    }

    private Color GetOccupancyColor(OccupancyType type)
    {
        switch (type)
        {
            case OccupancyType.Player:
                return playerColor;
            case OccupancyType.Enemy:
                return enemyColor;
            case OccupancyType.Wall:
                return wallColor;
            default:
                return gridColor;
        }
    }
}
