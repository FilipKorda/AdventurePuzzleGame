using System.Collections;
using UnityEngine;

public class OnePillarPuzzleManager : MonoBehaviour
{
    [SerializeField] private Collider VerticalCube;
    [SerializeField] private Collider VerticalCube1;
    [SerializeField] private Collider VerticalCube2;

    [SerializeField] private Collider HorizontalCube;
    [SerializeField] private Collider HorizontalCube1;
    [SerializeField] private Collider HorizontalCube2;

    [SerializeField] private Collider VerticalCubeWinCollider;
    [SerializeField] private Collider VerticalCube1WinCollider;
    [SerializeField] private Collider VerticalCube2WinCollider;

    [SerializeField] private Collider HorizontalCubeWinCollider;
    [SerializeField] private Collider HorizontalCube1WinCollider;
    [SerializeField] private Collider HorizontalCube2WinCollider;

    [SerializeField] private Collider[] verticalCube;
    [SerializeField] private Collider[] vertical1Cube;
    [SerializeField] private Collider[] vertical2Cube;

    [SerializeField] private Collider[] horizontalCubes;
    [SerializeField] private Collider[] horizontal1Cubes;
    [SerializeField] private Collider[] horizontal2Cubes;

    private bool verticalCubeInPlace;
    private bool verticalCube1InPlace;
    private bool verticalCube2InPlace;

    private bool horizontalCubesInPlace;
    private bool horizontalCubes1InPlace;
    private bool horizontalCubes2InPlace;

    private bool winPuzzle;

    [SerializeField] private float moveSpeed = 2f;

    [SerializeField] private GameObject movingVericalCube;
    [SerializeField] private GameObject movingVericalCube1;
    [SerializeField] private GameObject movingVericalCube2;

    [SerializeField] private GameObject movingHorizontalCube;
    [SerializeField] private GameObject movingHorizontalCube1;
    [SerializeField] private GameObject movingHorizontalCube2;

    [SerializeField] private Vector3 movingVerticalCubeFinalPosition;
    [SerializeField] private Vector3 movingVerticalCube1FinalPosition;
    [SerializeField] private Vector3 movingVerticalCube2FinalPosition;

    [SerializeField] private Vector3 movingHorizontalCubeFinalPosition;
    [SerializeField] private Vector3 movingHorizontalCube1FinalPosition;
    [SerializeField] private Vector3 movingHorizontalCube2FinalPosition;

    [SerializeField] private GameObject movingPillar;
    [SerializeField] private float movingPillarDuration = 2f;
    [SerializeField] private Vector3 movingPillarStartPosition = new Vector3(0, 10.541f, 0);
    [SerializeField] private Vector3 movingPillarFinalPosition = new Vector3(0, 13.6f, 0);

    [SerializeField] private RotatingPillarMovingBlock rotatingPillarMovingBlock;

    private Vector3 verticalStartPos;
    private Vector3 vertical1StartPos;
    private Vector3 vertical2StartPos;

    private Vector3 horizontalStartPos;
    private Vector3 horizontal1StartPos;
    private Vector3 horizontal2StartPos;

    [SerializeField] private GameObject gear;
    [SerializeField] private MoveSphereOnePillarPuzzle moveSphereOnePillarPuzzle;

    [SerializeField] private BoxCollider boxCollider;

    [SerializeField] private InteractableItem[] rotatingPillars;
    private void Start()
    {
        winPuzzle = false;
        verticalCubeInPlace = false;
        verticalCube1InPlace = false;
        verticalCube2InPlace = false;
        horizontalCubesInPlace = false;
        horizontalCubes1InPlace = false;
        horizontalCubes2InPlace = false;

        verticalStartPos = movingVericalCube.transform.localPosition;
        vertical1StartPos = movingVericalCube1.transform.localPosition;
        vertical2StartPos = movingVericalCube2.transform.localPosition;

        horizontalStartPos = movingHorizontalCube.transform.localPosition;
        horizontal1StartPos = movingHorizontalCube1.transform.localPosition;
        horizontal2StartPos = movingHorizontalCube2.transform.localPosition;

        foreach (var rotatingPillar in rotatingPillars)
        {
            rotatingPillar.canRotateMoveSphereOnePillarPuzzle = false;
        }
    }

    private void Update()
    {
        if (winPuzzle)
            return;

        if (!verticalCubeInPlace && VerticalCube.bounds.Intersects(VerticalCubeWinCollider.bounds))
        {
            foreach (var cube in verticalCube)
                cube.enabled = false;

            CursorController.Instance.currentMode.Exit();
            verticalCubeInPlace = true;

            StartCoroutine(MoveToLocalPosition(movingVericalCube, movingVerticalCubeFinalPosition));
            CheckIfWinPuzzle();
        }

        if (!verticalCube1InPlace && VerticalCube1.bounds.Intersects(VerticalCube1WinCollider.bounds))
        {
            foreach (var cube in vertical1Cube)
                cube.enabled = false;

            CursorController.Instance.currentMode.Exit();
            verticalCube1InPlace = true;

            StartCoroutine(MoveToLocalPosition(movingVericalCube1, movingVerticalCube1FinalPosition));
            CheckIfWinPuzzle();
        }

        if (!verticalCube2InPlace && VerticalCube2.bounds.Intersects(VerticalCube2WinCollider.bounds))
        {
            foreach (var cube in vertical2Cube)
                cube.enabled = false;

            CursorController.Instance.currentMode.Exit();
            verticalCube2InPlace = true;

            StartCoroutine(MoveToLocalPosition(movingVericalCube2, movingVerticalCube2FinalPosition));
            CheckIfWinPuzzle();
        }

        if (!horizontalCubesInPlace && HorizontalCube.bounds.Intersects(HorizontalCubeWinCollider.bounds))
        {
            foreach (var cube in horizontalCubes)
                cube.enabled = false;

            CursorController.Instance.currentMode.Exit();
            horizontalCubesInPlace = true;

            StartCoroutine(MoveToLocalPosition(movingHorizontalCube, movingHorizontalCubeFinalPosition));
            CheckIfWinPuzzle();
        }

        if (!horizontalCubes1InPlace && HorizontalCube1.bounds.Intersects(HorizontalCube1WinCollider.bounds))
        {
            foreach (var cube in horizontal1Cubes)
                cube.enabled = false;

            CursorController.Instance.currentMode.Exit();
            horizontalCubes1InPlace = true;

            StartCoroutine(MoveToLocalPosition(movingHorizontalCube1, movingHorizontalCube1FinalPosition));
            CheckIfWinPuzzle();
        }

        if (!horizontalCubes2InPlace && HorizontalCube2.bounds.Intersects(HorizontalCube2WinCollider.bounds))
        {
            foreach (var cube in horizontal2Cubes)
                cube.enabled = false;

            CursorController.Instance.currentMode.Exit();
            horizontalCubes2InPlace = true;

            StartCoroutine(MoveToLocalPosition(movingHorizontalCube2, movingHorizontalCube2FinalPosition));
            CheckIfWinPuzzle();
        }
    }

    public void ResetPuzzle()
    {
        StopAllCoroutines();
        winPuzzle = false;

        verticalCubeInPlace = false;
        verticalCube1InPlace = false;
        verticalCube2InPlace = false;
        horizontalCubesInPlace = false;
        horizontalCubes1InPlace = false;
        horizontalCubes2InPlace = false;

        movingVericalCube.transform.localPosition = verticalStartPos;
        movingVericalCube1.transform.localPosition = vertical1StartPos;
        movingVericalCube2.transform.localPosition = vertical2StartPos;

        movingHorizontalCube.transform.localPosition = horizontalStartPos;
        movingHorizontalCube1.transform.localPosition = horizontal1StartPos;
        movingHorizontalCube2.transform.localPosition = horizontal2StartPos;

        movingVericalCube.SetActive(true);
        movingVericalCube1.SetActive(true);
        movingVericalCube2.SetActive(true);

        movingHorizontalCube.SetActive(true);
        movingHorizontalCube1.SetActive(true);
        movingHorizontalCube2.SetActive(true);

        foreach (var cube in verticalCube) cube.enabled = true;
        foreach (var cube in vertical1Cube) cube.enabled = true;
        foreach (var cube in vertical2Cube) cube.enabled = true;
        foreach (var cube in horizontalCubes) cube.enabled = true;
        foreach (var cube in horizontal1Cubes) cube.enabled = true;
        foreach (var cube in horizontal2Cubes) cube.enabled = true;
    }


    private void CheckIfWinPuzzle()
    {
        if (verticalCubeInPlace && verticalCube1InPlace && verticalCube2InPlace &&
            horizontalCubesInPlace && horizontalCubes1InPlace && horizontalCubes2InPlace)
        {
            winPuzzle = true;
            StartCoroutine(RotateGearY());
            StartCoroutine(MovePillarOneToFinalPosition());
            boxCollider.enabled = false;
            Services.Audio.PlaySFX("SafeWinAkaPuzzleWin");
        }
    }

    private IEnumerator MovePillarOneToFinalPosition()
    {
        yield return new WaitForSeconds(2f);

        movingVericalCube.SetActive(false);
        movingVericalCube1.SetActive(false);
        movingVericalCube2.SetActive(false);

        VerticalCubeWinCollider.enabled = false;
        VerticalCube1WinCollider.enabled = false;
        VerticalCube2WinCollider.enabled = false;

        HorizontalCubeWinCollider.enabled = false;
        HorizontalCube1WinCollider.enabled = false;
        HorizontalCube2WinCollider.enabled = false;

        movingHorizontalCube.SetActive(false);
        movingHorizontalCube1.SetActive(false);
        movingHorizontalCube2.SetActive(false);

        yield return new WaitForSeconds(1f);

        rotatingPillarMovingBlock.ExitAfterWin();

        float elapsedTime = 0f;

        Vector3 startPos = movingPillar.transform.position;
        Vector3 endPos = new Vector3(
            startPos.x,
            movingPillarFinalPosition.y,
            startPos.z
        );

        while (elapsedTime < movingPillarDuration)
        {
            float t = elapsedTime / movingPillarDuration;
            movingPillar.transform.position = Vector3.Lerp(startPos, endPos, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        movingPillar.transform.position = endPos;

        foreach (var rotatingPillar in rotatingPillars)
        {
            rotatingPillar.canRotateMoveSphereOnePillarPuzzle = true;
        }

        moveSphereOnePillarPuzzle.ActivePuzzle = true;
    }

    private IEnumerator MoveToLocalPosition(GameObject obj, Vector3 target)
    {
        while (Vector3.Distance(obj.transform.localPosition, target) > 0.01f)
        {
            obj.transform.localPosition = Vector3.MoveTowards(
                obj.transform.localPosition,
                target,
                moveSpeed * Time.deltaTime
            );
            yield return null;
        }

        obj.transform.localPosition = target;
        obj.SetActive(false);
    }
    private IEnumerator RotateGearY()
    {
        Services.Audio.PlaySFX("MoveGearRoomTen");
        float duration = 6f;
        float elapsedTime = 0f;
        float rotationSpeed = 360f / duration;

        while (elapsedTime < duration)
        {
            gear.transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime, Space.World);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}