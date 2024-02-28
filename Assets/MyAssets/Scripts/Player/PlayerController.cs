using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace MoleSurvivor
{
    [RequireComponent(typeof(CharacterMovement))]
    public class PlayerController : MonoBehaviour
    {
        public Transform orientation;

        [Header("MOVEMENT & ROTATION")]
        public float moveSpeed = 5f;
        public float rotateSpeed = 10f;
        public float rotateDelay = 0.5f;

        public bool singleMovement;

        [HideInInspector] public Vector3 targetPos;
        [HideInInspector] public Vector2 _inputM;
        private float _inputR;

        private CharacterMovement characterMovement;
        [HideInInspector] public bool isAllowedToMove = true;

        public void SetStart()
        {
            characterMovement = GetComponent<CharacterMovement>();
            targetPos = transform.position;
            IsCheckTile(targetPos);
        }

        #region InputFunction
        public KeyCode? key1 = null, key2 = null, key3 = null, key4 = null;

        public void InputLeftDownUpdate(KeyCode? key = null) { key1 = key; }
        public void InputLeftUpUpdate(KeyCode? key = null) { key2 = key; }
        public void InputRightDownUpdate(KeyCode? key = null) { key3 = key; }
        public void InputRightUpUpdate(KeyCode? key = null) { key4 = key; }

        void InputUpdate()
        {
            // Check if key is not null and then if the key is being pressed down.

            // public void InputLeftDownUpdate(KeyCode? key = null)
            if (key1 != null)
            {
                if (Input.GetKeyDown(key1.Value))
                {
                    _inputM = new Vector2(-1, -1);
                    _inputR = 90f;
                }
            }

            // public void InputLeftUpUpdate(KeyCode? key = null)
            if (key2 != null)
            {
                if (Input.GetKeyDown(key2.Value))
                {
                    _inputM = new Vector2(-1, 1);
                    _inputR = 90f;
                }
            }

            // public void InputRightDownUpdate(KeyCode? key = null)
            if (key3 != null)
            {
                if (Input.GetKeyDown(key3.Value))
                {
                    _inputM = new Vector2(1, -1);
                    _inputR = -90f;
                }
            }

            // public void InputRightUpUpdate(KeyCode? key = null)
            if (key4 != null)
            {
                if (Input.GetKeyDown(key4.Value))
                {
                    _inputM = new Vector2(1, 1);
                    _inputR = -90f;
                }
            }
        }
        #endregion

        public void SetUpdate()
        {

            // Single Movement or Continous Movement
            _inputM = (singleMovement) ? Vector2.zero : _inputM;

            InputUpdate();

            if (isAllowedToMove)
            {
                #region Move
                if (!characterMovement.ReturnCheckIsMoving())
                {
                    if (_inputM != Vector2.zero) // I only put this so it could stop when pause
                    {
                        // Calculate the direction based on the current rotation
                        Vector3 direction = _inputM;

                        // Update targetPos based on the direction
                        targetPos = transform.position + direction;
                        targetPos = new Vector3Int(Mathf.RoundToInt(targetPos.x), Mathf.RoundToInt(targetPos.y), Mathf.RoundToInt(targetPos.z));

                        CheckBeforeMove();

                        // Check Before Move
                        IsCheckTile(targetPos);

                        // Move
                        characterMovement.Move(null, CheckAfterMove, transform, orientation, targetPos, new Vector3(0, _inputR, 0), moveSpeed, rotateSpeed, rotateDelay, isAllowedToMove);
                    }
                }
                #endregion
            }
        }

        void CheckBeforeMove()
        {
            //// Check Before Move
            Debug.Log("Check before Move");

            IsCheckTargetPos(targetPos, false);
        }

        void CheckAfterMove()
        {
            // Check After Move
            //InGameController.Instance.tileTypeDestroy.DestroyTile(targetPos);

            DetectColliders();

            IsCheckTargetPos(targetPos, true);

            //Debug.Log("Check after Move");
        }

        void IsCheckTargetPos(Vector3 targetPos, bool cBeforeOrAfter)
        {
            Collider[] colliders = Physics.OverlapSphere(targetPos, 0.3f);

            foreach (Collider c in colliders)
            {
                if (c.GetComponent<Trap>() != null)
                {
                    Trap coll = c.GetComponent<Trap>();
                    coll.SetStart(transform, cBeforeOrAfter);
                }
            }
        }

        void IsCheckTile(Vector3 targetPos)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(targetPos, 0.3f);

            foreach (Collider2D c in colliders)
            {
                if (c.GetComponent<TileDestroyer>() != null)
                {
                    TileDestroyer coll = c.GetComponent<TileDestroyer>();
                    coll.DestroyTile(targetPos);
                }
            }
        }

        void DetectColliders()
        {
            float radius = 0.3f;
            Vector3[] offsets = new Vector3[]
            {
            new Vector3(1, 1, 0),
            new Vector3(1, -1, 0),
            new Vector3(-1, 1, 0),
            new Vector3(-1, -1, 0)
            };

            foreach (Vector3 offset in offsets)
            {
                Collider[] colliders = Physics.OverlapSphere(transform.position + offset, radius);
                foreach (Collider collider in colliders)
                {
                    // Here you can check the specific offset and collider
                    ProcessCollision(offset, collider);
                }
            }
        }

        void ProcessCollision(Vector3 offset, Collider collider)
        {
            //Debug.Log($"Collision at offset {offset} with {collider.name}");

            // Example of specific actions based on offset
            if (offset == new Vector3(1, 1, 0))
            {

            }
            else if (offset == new Vector3(1, -1, 0))
            {

            }
            else if (offset == new Vector3(-1, 1, 0))
            { 

            }
            else if (offset == new Vector3(-1, -1, 0))
            {

            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;

            if (Application.isPlaying) { Gizmos.DrawWireSphere(targetPos, 0.3f); }
            
            Gizmos.DrawWireCube(transform.position, new Vector3(1f, 1f, 1f));
            Gizmos.DrawWireCube(transform.position + new Vector3(1, 1, 0), new Vector3(1f, 1f, 1f));
            Gizmos.DrawWireCube(transform.position + new Vector3(1, -1, 0), new Vector3(1f, 1f, 1f));
            Gizmos.DrawWireCube(transform.position + new Vector3(-1, 1, 0), new Vector3(1f, 1f, 1f));
            Gizmos.DrawWireCube(transform.position + new Vector3(-1, -1, 0), new Vector3(1f, 1f, 1f));

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position + new Vector3(1, 1, 0), 0.2f);
            Gizmos.DrawWireSphere(transform.position + new Vector3(1, -1, 0), 0.2f);
            Gizmos.DrawWireSphere(transform.position + new Vector3(-1, 1, 0), 0.2f);
            Gizmos.DrawWireSphere(transform.position + new Vector3(-1, -1, 0), 0.2f);
        }
    }

}
