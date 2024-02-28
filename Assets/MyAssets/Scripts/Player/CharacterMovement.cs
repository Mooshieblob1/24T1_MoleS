using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

namespace MoleSurvivor
{
    public class CharacterMovement : MonoBehaviour
    {
        private bool isMoving;
        private Coroutine rotateDelayCoroutine;
        private float distance;

        #region Old
        //IEnumerator Move(Vector3 targetPos, Transform _targetTransform)
        //{
        //    isMoving = true;

        //    while (_targetTransform && (_targetTransform.position - targetPos).sqrMagnitude > Mathf.Epsilon)
        //    {

        //        //_targetTransform.position = Vector3.MoveTowards(_targetTransform.position, targetPos, moveSpeed * Time.deltaTime);

        //        // Calculate the distance to the target
        //        float distance = Vector3.Distance(_targetTransform.position, targetPos);

        //        // Calculate the duration it should take to move to the target at the given speed
        //        float duration = distance / moveSpeed;

        //        // Use DOTween to move to the target position over the calculated duration
        //        _targetTransform.DOMove(targetPos, duration);
        //        _targetTransform.DORotate(new Vector3(0, _inputR, 0), duration / 10);

        //        #region
        //        // Calculate the target rotation
        //        //Quaternion targetRotation = Quaternion.Euler(new Vector3(0, _inputR, 0));

        //        // Lerp between the current rotation and the target rotation
        //        //if (_targetTransform)
        //        //{
        //        //    _targetTransform.rotation = Quaternion.Lerp(_targetTransform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
        //        //}
        //        #endregion

        //        yield return null;
        //    }

        //    // Using Mathf.RoundToInt (rounds to the nearest integer)
        //    _targetTransform.position = targetPos;

        //    isMoving = false;
        //}
        #endregion

        #region Old2
        //private float speedDuration; // Speed Duration
        //private float rotateDuration; // Rotation Duration

        //public IEnumerator Move(Action checkBeforeMove, Action checkAfterMove, Transform _targetTransform, Transform _targetOrientation, Vector3 targetPos, Vector3 targetRotate, float moveSpeed, float rotationSpeed)
        //{
        //    checkBeforeMove?.Invoke();

        //    isMoving = true;

        //    while (_targetTransform && (_targetTransform.position - targetPos).sqrMagnitude > Mathf.Epsilon)
        //    {

        //        // Calculate the distance to the target
        //        float distance = Vector3.Distance(_targetTransform.position, targetPos);

        //        // Calculate the duration it should take to move to the target at the given speed
        //        speedDuration = distance / moveSpeed;
        //        rotateDuration = distance / Mathf.Clamp(rotationSpeed, 0.5f, rotationSpeed);

        //        // Use DOTween to move to the target position over the calculated duration
        //        _targetTransform.DOMove(targetPos, speedDuration);
        //        _targetOrientation.DORotate(targetRotate, rotateDuration);

        //        yield return null;
        //    }

        //    // Using Mathf.RoundToInt (rounds to the nearest integer)
        //    _targetTransform.position = targetPos;
        //    _targetOrientation.rotation = Quaternion.Euler(targetRotate);

        //    isMoving = false;

        //    checkAfterMove?.Invoke();
        //}
        #endregion

        public void Move( Action checkBeforeMove, Action checkAfterMove, Transform _targetTransform, Transform _targetOrientation, Vector3 targetPos, Vector3 targetRotate, float moveSpeed, float rotationSpeed, float delayRotation = 0, bool isAllowedToMove = true)
        {
            checkBeforeMove?.Invoke();

            isMoving = true;

            // Calculate the distance to the target
            distance = Vector3.Distance(_targetTransform.position, targetPos);

            // Calculate the duration it should take to move to the target at the given speed
            //float speedDuration = distance / moveSpeed;
            //float rotateDuration = distance / rotationSpeed;



            if (isAllowedToMove)
            {
                float speedDuration = distance / moveSpeed;
                float rotateDuration = distance / rotationSpeed;

                // Use DoTween for movement
                _targetTransform.DOMove(targetPos, speedDuration)
                    .OnComplete(() =>
                    {
                        // Once movement is complete, set the final position
                        _targetTransform.position = targetPos;
                        isMoving = false;
                        checkAfterMove?.Invoke();

                        // Check if not moving, then rotate orientation
                        // Start the delay coroutine for orientation reset
                        if (!isMoving && rotateDelayCoroutine != null) { StopCoroutine(rotateDelayCoroutine); }
                        if (!isMoving) { rotateDelayCoroutine = StartCoroutine(RotateDelayCoroutine(_targetOrientation, rotateDuration, delayRotation)); }

                    });

                // Use DoTween for rotation
                _targetOrientation.DORotate(targetRotate, rotateDuration)
                    .OnComplete(() =>
                    {
                    // Once rotation is complete, set the final rotation
                    _targetOrientation.rotation = Quaternion.Euler(targetRotate);
                    });
            }
            else
            {
                float speedDuration = distance / moveSpeed;
                // Set rotateDuration to the duration of rotation only
                float rotateDuration = Mathf.Abs(Vector3.Angle(_targetOrientation.eulerAngles, targetRotate)) / rotationSpeed;

                // Use DoTween for movement
                DOTween.Sequence()
                    .Append(_targetTransform.DOMove(_targetTransform.position, 0)) // Move to the same position (effectively no movement)
                    .AppendInterval(speedDuration) // Delay for the duration of movement
                    .OnComplete(() =>
                    {
                        // Once movement is complete, set the final position
                        _targetTransform.position = _targetTransform.position;
                        isMoving = false;
                        checkAfterMove?.Invoke();

                        // Check if not moving, then rotate orientation
                        // Start the delay coroutine for orientation reset
                        if (!isMoving && rotateDelayCoroutine != null) { StopCoroutine(rotateDelayCoroutine); }
                        if (!isMoving) { rotateDelayCoroutine = StartCoroutine(RotateDelayCoroutine(_targetOrientation, rotateDuration, delayRotation)); }

                    });

                // Use DoTween for rotation
                _targetOrientation.DORotate(targetRotate, rotateDuration)
                    .OnComplete(() =>
                    {
                    // Once rotation is complete, set the final rotation
                    _targetOrientation.rotation = Quaternion.Euler(targetRotate);
                    });
            }
        }

        private IEnumerator RotateDelayCoroutine(Transform targetOrientation, float rotateDuration, float rotationResetDelay)
        {
            yield return new WaitForSeconds(rotationResetDelay);
            targetOrientation.DORotate(Vector3.zero, rotateDuration);
        }

        public bool ReturnCheckIsMoving()
        {
            return isMoving;
        }
    }
}
