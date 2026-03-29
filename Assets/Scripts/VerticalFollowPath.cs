using System.Collections;
using UnityEngine;

public class VerticalFollowPath : MonoBehaviour
{
    public Transform leftObject;
    public ParticleSystem leftObjectPS;

    public Transform rightObject;
    public ParticleSystem rightObjectPS;

    public Transform[] leftPoints;
    public Transform[] rightPoints;

    public float speed = 0.5f;
    public float stopTime = 2f;

    public PathMover mover;

    void Start()
    {
        StartCoroutine(Sequence());
    }

    IEnumerator MoveBothAndWait(int leftIndex, int rightIndex)
    {
        bool leftDone = false;
        bool rightDone = false;

        leftObjectPS.Stop();
        rightObjectPS.Stop();

        StartCoroutine(
            mover.MoveToPoint(
                leftObject,
                leftPoints[leftIndex],
                speed,
                () => leftDone = true
            )
        );

        StartCoroutine(
            mover.MoveToPoint(
                rightObject,
                rightPoints[rightIndex],
                speed,
                () => rightDone = true
            )
        );

        while (!leftDone || !rightDone)
            yield return null;

        leftObjectPS.Play();
        rightObjectPS.Play();

        yield return new WaitForSeconds(stopTime);
    }

    IEnumerator MoveEntireLineAndWait(bool leftUp, bool rightUp)
    {
        leftObjectPS.Stop();
        rightObjectPS.Stop();

        int leftCount = leftPoints.Length;
        int rightCount = rightPoints.Length;

        Transform leftStartPoint = leftUp ? leftPoints[0] : leftPoints[leftCount - 1];
        Transform rightStartPoint = rightUp ? rightPoints[0] : rightPoints[rightCount - 1];

        bool leftStartDone = false;
        bool rightStartDone = false;

        StartCoroutine(
            mover.MoveToPoint(leftObject, leftStartPoint, speed, () => leftStartDone = true)
        );

        StartCoroutine(
            mover.MoveToPoint(rightObject, rightStartPoint, speed, () => rightStartDone = true)
        );

        while (!leftStartDone || !rightStartDone)
            yield return null;

        yield return new WaitForSeconds(0.1f);

        leftObjectPS.Play();
        rightObjectPS.Play();

        int leftStart = leftUp ? 0 : leftCount - 1;
        int leftEnd = leftUp ? leftCount : -1;
        int leftStep = leftUp ? 1 : -1;

        int rightStart = rightUp ? 0 : rightCount - 1;
        int rightEnd = rightUp ? rightCount : -1;
        int rightStep = rightUp ? 1 : -1;

        int leftIndex = leftStart;
        int rightIndex = rightStart;

        while ((leftUp && leftIndex < leftEnd) || (!leftUp && leftIndex > leftEnd) ||
               (rightUp && rightIndex < rightEnd) || (!rightUp && rightIndex > rightEnd))
        {
            bool leftDone = false;
            bool rightDone = false;

            if ((leftUp && leftIndex < leftEnd) || (!leftUp && leftIndex > leftEnd))
            {
                StartCoroutine(
                    mover.MoveToPoint(leftObject, leftPoints[leftIndex], speed, () => leftDone = true)
                );
            }
            else
            {
                leftDone = true;
            }

            if ((rightUp && rightIndex < rightEnd) || (!rightUp && rightIndex > rightEnd))
            {
                StartCoroutine(
                    mover.MoveToPoint(rightObject, rightPoints[rightIndex], speed, () => rightDone = true)
                );
            }
            else
            {
                rightDone = true;
            }

            while (!leftDone || !rightDone)
                yield return null;

            if ((leftUp && leftIndex < leftEnd) || (!leftUp && leftIndex > leftEnd))
                leftIndex += leftStep;

            if ((rightUp && rightIndex < rightEnd) || (!rightUp && rightIndex > rightEnd))
                rightIndex += rightStep;
        }

        leftObjectPS.Stop();
        rightObjectPS.Stop();

        yield return new WaitForSeconds(stopTime);
    }

    IEnumerator Sequence()
    {
        yield return MoveBothAndWait(3, 2);
        yield return MoveBothAndWait(1, 0);
        yield return MoveBothAndWait(0, 3);
        yield return MoveEntireLineAndWait(true, false);
        yield return MoveEntireLineAndWait(false, true);
        yield return MoveBothAndWait(2, 1);
        yield return MoveEntireLineAndWait(false, true);
        yield return MoveEntireLineAndWait(true, false);
    }
}
