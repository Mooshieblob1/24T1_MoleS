using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using Sirenix.OdinInspector;

namespace MoleSurvivor
{
    public class InGameController : MonoBehaviour
    {
        public static InGameController Instance { get; private set; }

        private void Awake()
        {
            // Set Parent to Root
            transform.parent = null;

            //If there is more than one instance, destroy the extra else Set the static instance to this instance
            if (Instance != null && Instance != this) { Destroy(this.gameObject); } else { Instance = this; }
        }

        private bool setActiveUpdate;

        [BoxGroup("BOX LEVEL SANDWITCH", false)]
        [Title("BOX LEVEL SANDWITCH/LEVEL SANDWITCH")]
        public Transform inGameCamera;
        [BoxGroup("BOX LEVEL SANDWITCH")]
        public Transform levelGridParent;
        
        [Space]

        [BoxGroup("BOX LEVEL SANDWITCH")]
        public Transform delayText;
        [BoxGroup("BOX LEVEL SANDWITCH")]
        public int countdownStartValue = 3;
        [BoxGroup("BOX LEVEL SANDWITCH")]
        public float delayBetweenCountdown = 1f;

        [Space]

        [BoxGroup("BOX LEVEL SANDWITCH")]
        public int eachLayerSize = 30;
        [BoxGroup("BOX LEVEL SANDWITCH")]
        public float levelDuration;

        [Space]

        [BoxGroup("BOX LEVEL SANDWITCH")]
        public Transform[] levelSandwitch;

        Transform[] currentlevelSandwitch;
        int endGoal;

        [BoxGroup("BOX PLAYER", false)]
        [TitleGroup("BOX PLAYER/CHECK PLAYER")]
        public PlayerController 
        player1,
        player2,
        player3,
        player4;

        [TitleGroup("BOX PLAYER/PLAYER START POSITION", "Set all the players start position depending on how many players")]
        [FoldoutGroup("BOX PLAYER/PLAYER START POSITION/Collapse Folder")]
        [BoxGroup("BOX PLAYER/PLAYER START POSITION/Collapse Folder/Set start height all Player")]
        [HideLabel]
        [SerializeField]
        public int
        setAllPlayerHeight;


        [BoxGroup("BOX PLAYER/PLAYER START POSITION/Collapse Folder/Only have 1 Player")] 
        [HideLabel]
        [SerializeField]
        public int 
        only1Player;

        [BoxGroup("BOX PLAYER/PLAYER START POSITION/Collapse Folder/Only have 2 Player")]
        [HideLabel]
        [SerializeField]
        public int
        only2Player1,
        only2Player2;

        [BoxGroup("BOX PLAYER/PLAYER START POSITION/Collapse Folder/Only have 3 Player")]
        [HideLabel]
        [SerializeField]
        public int
        only3Player1,
        only3Player2,
        only3Player3;

        [BoxGroup("BOX PLAYER/PLAYER START POSITION/Collapse Folder/Only have 4 Player")]
        [HideLabel]
        [SerializeField]
        public int
        only4Player1,
        only4Player2,
        only4Player3,
        only4Player4;


        [TitleGroup("BOX PLAYER/START SPEED & ROTATION", "Set all the players the Speed and Rotation on the start")]
        [BoxGroup("BOX PLAYER")]
        public float 
        playerMoveSpeed,
        playerRotateSpeed,
        playerRotateDelay;

        //[TitleGroup("BOX PLAYER/CHECK PLAYER ACTIVE", "Set how many players in the game")]
        //[BoxGroup("BOX PLAYER")]
        [HideInInspector]
        public bool 
        p1Active,
        p2Active,
        p3Active,
        p4Active;

        [TitleGroup("BOX PLAYER/CHECK PLAYER ACTIVE", "Set how many players in the game")]
        [BoxGroup("BOX PLAYER")]
        public int playersActive;

        int P1Pos, P2Pos, P3Pos, P4Pos;

        [TitleGroup("BOX PLAYER/SET MODE", "Singleplayer / Multiplayer")]
        [BoxGroup("BOX PLAYER")]
        public bool soloMode;

        private void Start() 
        {
            // First, ensure currentlevelSandwitch is properly initialized.
            currentlevelSandwitch = new Transform[levelSandwitch.Length];

            endGoal = -((levelSandwitch.Length * eachLayerSize) - eachLayerSize);

            // Then, instantiate the objects.
            for (int i = 0; i < levelSandwitch.Length; i++)
            {
                currentlevelSandwitch[i] = Instantiate(levelSandwitch[i]);
                currentlevelSandwitch[i].parent = levelGridParent;
                currentlevelSandwitch[i].transform.localPosition = new Vector3(0, -(i * eachLayerSize), 0);
                currentlevelSandwitch[i].gameObject.SetActive(false);
            }

            if (player1 != null && p1Active) { player1.gameObject.SetActive(true); }
            if (player2 != null && p2Active) { player2.gameObject.SetActive(true); }
            if (player3 != null && p3Active) { player3.gameObject.SetActive(true); }
            if (player4 != null && p4Active) { player4.gameObject.SetActive(true); }

            StartCoroutine(CountdownRoutine(countdownStartValue, delayBetweenCountdown)); 
        }

        void SetStart()
        {
            SetPlayerPositionActivation(playersActive);

            if (player1 != null && p1Active) { SetStartPlayer(player1, P1Pos, playerMoveSpeed, playerRotateSpeed, playerRotateDelay, soloMode); }
            if (player2 != null && p2Active) { SetStartPlayer(player2, P2Pos, playerMoveSpeed, playerRotateSpeed, playerRotateDelay, soloMode); }
            if (player3 != null && p3Active) { SetStartPlayer(player3, P3Pos, playerMoveSpeed, playerRotateSpeed, playerRotateDelay, soloMode); }
            if (player4 != null && p4Active) { SetStartPlayer(player4, P4Pos, playerMoveSpeed, playerRotateSpeed, playerRotateDelay, soloMode); }

            // Start the coroutine
            StartCoroutine(MoveCameraCoroutine());
        }

        public void SetPlayerPositionActivation(int numberOfPlayers)
        {
            // First, deactivate all players
            p1Active = p2Active = p3Active = p4Active = false;

            // Then, activate players based on the number of players
            switch (numberOfPlayers)
            {
                case 1:
                    // Set Player Position
                    P1Pos = only1Player;
                    // Optionally reset other player values
                    P2Pos = P3Pos = P4Pos = 0; // Or any default value

                    // Set Player Active
                    p1Active = true;
                    player1.AssignGamepad(0);
                    break;
                case 2:
                    // Set Player Position
                    P1Pos = only2Player1;
                    P2Pos = only2Player2;
                    // Optionally reset other player values
                    P3Pos = P4Pos = 0; // Or any default value

                    // Set Player Active
                    p1Active = p2Active = true;
                    player1.AssignGamepad(0);
                    player2.AssignGamepad(1);
                    break;
                case 3:
                    // Set Player Position
                    P1Pos = only3Player1;
                    P2Pos = only3Player2;
                    P3Pos = only3Player3;
                    // Optionally reset other player value
                    P4Pos = 0; // Or any default value

                    // Set Player Active
                    p1Active = p2Active = p3Active = true;
                    player1.AssignGamepad(0);
                    player2.AssignGamepad(1);
                    player3.AssignGamepad(2);
                    break;
                case 4:
                    // Set Player Position
                    P1Pos = only4Player1;
                    P2Pos = only4Player2;
                    P3Pos = only4Player3;
                    P4Pos = only4Player4;

                    // Set Player Active
                    p1Active = p2Active = p3Active = p4Active = true;
                    player1.AssignGamepad(0);
                    player2.AssignGamepad(1);
                    player3.AssignGamepad(2);
                    player4.AssignGamepad(3);
                    break;
                default:
                    Debug.LogError("Unsupported number of players: " + numberOfPlayers);

                    // Optionally reset all player Position
                    P1Pos = P2Pos = P3Pos = P4Pos = 0; // Or any default value
                    break;
            }

            // Update PlayerControllers' active status based on the active flags
            player1.gameObject.SetActive(p1Active);
            player2.gameObject.SetActive(p2Active);
            player3.gameObject.SetActive(p3Active);
            player4.gameObject.SetActive(p4Active);
        }

        private void Update() { SetUpdate(setActiveUpdate); }

        void SetUpdate(bool active)
        {
            if (active)
            {
                if (player1 != null && p1Active) player1.SetUpdate();
                if (player2 != null && p2Active) player2.SetUpdate();
                if (player3 != null && p3Active) player3.SetUpdate();
                if (player4 != null && p4Active) player4.SetUpdate();
            }

            // Set the Level to false
            for (int i = 0; i < currentlevelSandwitch.Length; i++)
            {
                OutsideBound(currentlevelSandwitch[i], eachLayerSize / 2);
            }
        }

        IEnumerator MoveCameraCoroutine()
        {
            float elapsedTime = 0f;
            Vector3 startingPosition = inGameCamera.transform.position;

            while (elapsedTime < levelDuration)
            {
                elapsedTime += Time.fixedDeltaTime;
                inGameCamera.transform.position = Vector3.Lerp(startingPosition, new Vector3(0, endGoal, startingPosition.z), elapsedTime / levelDuration);
                yield return new WaitForFixedUpdate();
            }

            // Ensure the camera reaches exactly the target position
            inGameCamera.transform.position = new Vector3(0, endGoal, startingPosition.z);
        }

        void SetStartPlayer(PlayerController player, int pPosition, float pMoveSpeed, float pRotateSpeed, float pRotateDelay, bool solo)
        {
            player.transform.position = new Vector3(pPosition, setAllPlayerHeight, player.transform.position.z);

            player.moveSpeed = pMoveSpeed;
            player.rotateDuration = pRotateSpeed;
            player.rotateDelay = pRotateDelay;

            player.singleMovement = solo;
            player.SetStart();
        }

        IEnumerator CountdownRoutine(int startValue, float delay)
        {
            TextMeshProUGUI countdownText = delayText.GetComponent<TextMeshProUGUI>();

            for (int i = startValue; i > 0; i--)
            {
                // Set text to current countdown value
                countdownText.text = i.ToString();

                // Resize using DoTween
                countdownText.rectTransform.DOScale(1.5f, 0.5f).SetEase(Ease.OutBounce).OnComplete(() =>
                {
                    // Restore original size
                    countdownText.rectTransform.DOScale(1f, 0.2f);
                });

                // Delay between countdown numbers
                yield return new WaitForSeconds(delay);
            }

            // Display "Go!" or any other message
            countdownText.text = "Go!";
            countdownText.rectTransform.DOScale(1.5f, 0.5f).SetEase(Ease.OutBounce).OnComplete(() =>
            {
                // Restore original size
                countdownText.rectTransform.DOScale(1f, 0.2f);
                countdownText.gameObject.SetActive(false);
                SetStart();
                setActiveUpdate = true;
            });
        }

        public void SetDestroy()
        {
            Destroy(this.gameObject);
        }

        public Vector2 _screenSpace;
        public float vertical;
        public float horizontal;

        public void OutsideBound(Transform objectBound, float objectBoundPositionOffset)
        {
            // Assuming inGameCamera, horizontal, vertical, and _screenSpace are already defined

            // Center position based on the inGameCamera
            Vector3 centerPosition = inGameCamera.position;

            // Inner boundaries
            float innerLeft = centerPosition.x - horizontal;
            float innerRight = centerPosition.x + horizontal;
            float innerBottom = centerPosition.y - vertical;
            float innerTop = centerPosition.y + vertical;

            // Outer boundaries (expanded by _screenSpace)
            float outerLeft = innerLeft - _screenSpace.x;
            float outerRight = innerRight + _screenSpace.x;
            float outerBottom = innerBottom - _screenSpace.y;
            float outerTop = innerTop + _screenSpace.y;

            Vector3 pointToCheckTop = objectBound.position + new Vector3(0, -objectBoundPositionOffset, 0); // Example point
            Vector3 pointToCheckDown = objectBound.position + new Vector3(0, objectBoundPositionOffset, 0); // Example point

            if (pointToCheckTop.y > outerTop)
            {
                // The point the outer boundaries
                objectBound.gameObject.SetActive(false);
            }
            if (pointToCheckDown.y > outerBottom && pointToCheckTop.y < outerTop)
            {
                // The point the outer boundaries
                objectBound.gameObject.SetActive(true);
            }
        }

        private void OnDrawGizmos()
        {
            if (inGameCamera == null)
            {
                Debug.LogWarning("inGameCamera Transform is not set.");
                return;
            }

            Vector3 centerPosition = inGameCamera.position;

            // Adjusted positions to center around inGameCamera
            Vector3 bottomLeft = centerPosition + new Vector3(-horizontal, -vertical);
            Vector3 bottomRight = centerPosition + new Vector3(horizontal, -vertical);
            Vector3 topLeft = centerPosition + new Vector3(-horizontal, vertical);
            Vector3 topRight = centerPosition + new Vector3(horizontal, vertical);

            // Drawing inner borders
            Gizmos.DrawLine(bottomLeft, bottomRight);
            Gizmos.DrawLine(topLeft, topRight);
            Gizmos.DrawLine(bottomLeft, topLeft);
            Gizmos.DrawLine(bottomRight, topRight);

            Gizmos.color = Color.yellow;

            // Adjusted positions for outer borders using _screenSpace
            Vector3 outerBottomLeft = bottomLeft + new Vector3(-_screenSpace.x, -_screenSpace.y);
            Vector3 outerBottomRight = bottomRight + new Vector3(_screenSpace.x, -_screenSpace.y);
            Vector3 outerTopLeft = topLeft + new Vector3(-_screenSpace.x, _screenSpace.y);
            Vector3 outerTopRight = topRight + new Vector3(_screenSpace.x, _screenSpace.y);

            // Drawing outer borders
            Gizmos.DrawLine(outerBottomLeft, outerBottomRight);
            Gizmos.DrawLine(outerTopLeft, outerTopRight);
            Gizmos.DrawLine(outerBottomLeft, outerTopLeft);
            Gizmos.DrawLine(outerBottomRight, outerTopRight);
        }

    }
}
