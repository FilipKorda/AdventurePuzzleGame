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

    private void Start()
    {
        winPuzzle = false;
        verticalCubeInPlace = false;
        verticalCube1InPlace = false;
        verticalCube2InPlace = false;
        horizontalCubesInPlace = false;
        horizontalCubes1InPlace = false;
        horizontalCubes2InPlace = false;
    }

    private void Update()
    {
        if (winPuzzle)
            return;

        if (!verticalCubeInPlace && VerticalCube.bounds.Intersects(VerticalCubeWinCollider.bounds))
        {
            Debug.Log("VerticalCube reached its WinCollider!");
            foreach (var cube in verticalCube)
            {
                cube.enabled = false;
            }
            CursorController.Instance.currentMode.Exit();
            verticalCubeInPlace = true;
            CheckIfWinPuzzle();
        }

        if (!verticalCube1InPlace && VerticalCube1.bounds.Intersects(VerticalCube1WinCollider.bounds))
        {
            Debug.Log("VerticalCube1 reached its WinCollider!");
            foreach (var cube in vertical1Cube)
            {
                cube.enabled = false;
            }
            CursorController.Instance.currentMode.Exit();
            verticalCube1InPlace = true;
            CheckIfWinPuzzle();
        }

        if (!verticalCube2InPlace && VerticalCube2.bounds.Intersects(VerticalCube2WinCollider.bounds))
        {
            Debug.Log("VerticalCube2 reached its WinCollider!");
            foreach (var cube in vertical2Cube)
            {
                cube.enabled = false;
            }
            CursorController.Instance.currentMode.Exit();
            verticalCube2InPlace = true;
            CheckIfWinPuzzle();
        }

        if (!horizontalCubesInPlace && HorizontalCube.bounds.Intersects(HorizontalCubeWinCollider.bounds))
        {
            Debug.Log("HorizontalCube reached its WinCollider!");

            foreach (var cube in horizontalCubes)
            {
                cube.enabled = false;
            }
            CursorController.Instance.currentMode.Exit();
            horizontalCubesInPlace = true;
            CheckIfWinPuzzle();
        }

        if (!horizontalCubes1InPlace && HorizontalCube1.bounds.Intersects(HorizontalCube1WinCollider.bounds))
        {
            Debug.Log("HorizontalCube1 reached its WinCollider!");
            foreach (var cube in horizontal1Cubes)
            {
                cube.enabled = false;
            }
            CursorController.Instance.currentMode.Exit();
            horizontalCubes1InPlace = true;
            CheckIfWinPuzzle();
        }

        if (!horizontalCubes2InPlace && HorizontalCube2.bounds.Intersects(HorizontalCube2WinCollider.bounds))
        {
            Debug.Log("HorizontalCube2 reached its WinCollider!");
            foreach (var cube in horizontal2Cubes)
            {
                cube.enabled = false;
            }
            CursorController.Instance.currentMode.Exit();
            horizontalCubes2InPlace = true;
            CheckIfWinPuzzle();
        }
    }

    private void CheckIfWinPuzzle()
    {
        if (verticalCubeInPlace && verticalCube1InPlace && verticalCube2InPlace &&
            horizontalCubesInPlace && horizontalCubes1InPlace && horizontalCubes2InPlace)
        {
            winPuzzle = true;
            Debug.Log("WinPuzzle – wszystkie elementy na miejscu!");
        }
    }
}