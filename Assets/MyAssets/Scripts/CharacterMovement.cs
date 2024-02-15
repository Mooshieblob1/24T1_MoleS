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

        public IEnumerator Move(Action checkBeforeMove, Action checkAfterMove, Transform _targetTransform, Vector3 targetPos, Vector3 targetRotate, float moveSpeed, float rotationSpeed)
        {
            checkBeforeMove?.Invoke();

            isMoving = true;

            while (_targetTransform && (_targetTransform.position - targetPos).sqrMagnitude > Mathf.Epsilon)
            {

                // Calculate the distance to the target
                float distance = Vector3.Distance(_targetTransform.position, targetPos);

                // Calculate the duration it should take to move to the target at the given speed
                float speedDuration = distance / moveSpeed;
                float rotateDuration = distance / rotationSpeed;

                // Use DOTween to move to the target position over the calculated duration
                _targetTransform.DOMove(targetPos, speedDuration);
                _targetTransform.DORotate(targetRotate, rotateDuration / 10);

                yield return null;
            }

            // Using Mathf.RoundToInt (rounds to the nearest integer)
            _targetTransform.position = targetPos;

            isMoving = false;

            checkAfterMove?.Invoke();
        }

        public bool ReturnCheckIsMoving()
        {
            return isMoving;
        }
    }
}
