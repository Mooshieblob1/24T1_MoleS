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

        #region SANDWITCH LEVEL SETTING INSPECTOR
        [BoxGroup("BOX LEVEL SANDWITCH", false)]
        [Title("BOX LEVEL SANDWITCH/LEVEL SANDWITCH")]
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
        #endregion

        #region CAMERA & GRID SETTING INSPECTOR
        [BoxGroup("BOX CAMERA HOLDER", false)]
        [TitleGroup("BOX CAMERA HOLDER/CAMERA & GRID")]
        public Transform inGameCamera;
        [BoxGroup("BOX CAMERA HOLDER")]
        public Grid grid; // Reference to the isometric grid
        [BoxGroup("BOX CAMERA HOLDER")]
        public Vector2 _screenSpace;
        [BoxGroup("BOX CAMERA HOLDER")]
        public float vertical;
        [BoxGroup("BOX CAMERA HOLDER")]
        public float horizontal;
        #endregion

        #region PLAYER SETTING INSPECTOR
        [BoxGroup("BOX PLAYER", false)]
        [TitleGroup("BOX PLAYER/CHECK PLAYER")]
        public PlayerController 
        player1,
        player2,
        player3,
        player4;

        [BoxGroup("BOX PLAYER")]
        [TitleGroup("BOX PLAYER/PLAYER HUD")]
        public PlayerHud 
        player1Health,
        player2Health,
        player3Health,
        player4Health;

        //---------------------------------------------------------------------------------------------------------------------------------------

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

        //---------------------------------------------------------------------------------------------------------------------------------------

        //[TitleGroup("BOX PLAYER/PLAYER START POSITION", "Set all the players start position depending on how many players")]
        //[FoldoutGroup("BOX PLAYER/PLAYER START POSITION/Collapse Folder")]
        //[BoxGroup("BOX PLAYER/PLAYER START POSITION/Collapse Folder/Set start height all Player")]

        //[BoxGroup("BOX LEVEL SANDWITCH", false)]
        //[Title("BOX LEVEL SANDWITCH/LEVEL SANDWITCH")]

        [FoldoutGroup("BOX PLAYER/PLAYER RESPAWN POSITION/Collapse Folder")]
        [TitleGroup("BOX PLAYER/PLAYER RESPAWN POSITION", "Set all the players respawn position")]
        [BoxGroup("BOX PLAYER/PLAYER RESPAWN POSITION/Collapse Folder/Player Respawn Timer")]
        [HideLabel]
        [SerializeField]
        public float
        playerRespawnTimer;

        private float
        p1SpawnTimer,
        p2SpawnTimer,
        p3SpawnTimer,
        p4SpawnTimer;

        private bool
        p1Death,
        p2Death,
        p3Death,
        p4Death;

        [BoxGroup("BOX PLAYER/PLAYER RESPAWN POSITION/Collapse Folder/Player Height Respawn Location")]
        [HideLabel]
        [SerializeField]
        public float
        playerRespawnHeightLocation;

        [BoxGroup("BOX PLAYER/PLAYER RESPAWN POSITION/Collapse Folder/1 Player Respawn Location")]
        [HideLabel]
        [SerializeField]
        public float
        only1player1RespawnLocation;

        [BoxGroup("BOX PLAYER/PLAYER RESPAWN POSITION/Collapse Folder/1 Player Respawn Location")]
        [LabelText("Debug")]
        [SerializeField]
        public bool
        only1playersRespawnLocationDebug;

        [BoxGroup("BOX PLAYER/PLAYER RESPAWN POSITION/Collapse Folder/2 Player Respawn Location")]
        [HideLabel]
        [SerializeField]
        public float
        only2player1RespawnLocation,
        only2player2RespawnLocation;

        [BoxGroup("BOX PLAYER/PLAYER RESPAWN POSITION/Collapse Folder/2 Player Respawn Location")]
        [LabelText("Debug")]
        [SerializeField]
        public bool
        only2playersRespawnLocationDebug;

        [BoxGroup("BOX PLAYER/PLAYER RESPAWN POSITION/Collapse Folder/3 Player Respawn Location")]
        [HideLabel]
        [SerializeField]
        public float
        only3player1RespawnLocation,
        only3player2RespawnLocation,
        only3player3RespawnLocation;

        [BoxGroup("BOX PLAYER/PLAYER RESPAWN POSITION/Collapse Folder/3 Player Respawn Location")]
        [LabelText("Debug")]
        [SerializeField]
        public bool
        only3playersRespawnLocationDebug;

        [BoxGroup("BOX PLAYER/PLAYER RESPAWN POSITION/Collapse Folder/4 Player Respawn Location")]
        [HideLabel]
        [SerializeField]
        public float
        only4player1RespawnLocation,
        only4player2RespawnLocation,
        only4player3RespawnLocation,
        only4player4RespawnLocation;

        [BoxGroup("BOX PLAYER/PLAYER RESPAWN POSITION/Collapse Folder/4 Player Respawn Location")]
        [LabelText("Debug")]
        [SerializeField]
        public bool
        only4playersRespawnLocationDebug;

        [TitleGroup("BOX PLAYER/START SPEED & ROTATION", "Set all the players the Speed and Rotation on the start")]
        [BoxGroup("BOX PLAYER", false)]
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
        #endregion

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

            p1SpawnTimer = playerRespawnTimer;
            p2SpawnTimer = playerRespawnTimer;
            p3SpawnTimer = playerRespawnTimer;
            p4SpawnTimer = playerRespawnTimer;

            // Start the coroutine to move the camera
            //StartCoroutine(MoveCameraCoroutine());
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

        private void Update() 
        {
            SetUpdate(setActiveUpdate); 
        }

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

        void SnapToGrid(Transform player)
        {
            // Convert world position to cell position and back to snap to the grid
            player.position = grid.CellToWorld(grid.WorldToCell(player.position));
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

            //---------------------------------------------------------------------------------------
            
            Gizmos.color = Color.red;

            Vector3 cPosition = new Vector3(0, inGameCamera.position.y + playerRespawnHeightLocation, inGameCamera.position.z);
            Gizmos.DrawLine(cPosition + new Vector3(-horizontal, 0), cPosition + new Vector3(horizontal, 0));

            if (only1playersRespawnLocationDebug)
            {
                Gizmos.DrawWireSphere(cPosition + new Vector3(only1player1RespawnLocation, 0, 0), 0.5f);
            }

            if (only2playersRespawnLocationDebug)
            {
                Gizmos.DrawWireSphere(cPosition + new Vector3(only2player1RespawnLocation, 0, 0), 0.5f);
                Gizmos.DrawWireSphere(cPosition + new Vector3(only2player2RespawnLocation, 0, 0), 0.5f);
            }

            if (only3playersRespawnLocationDebug)
            {
                Gizmos.DrawWireSphere(cPosition + new Vector3(only3player1RespawnLocation, 0, 0), 0.5f);
                Gizmos.DrawWireSphere(cPosition + new Vector3(only3player2RespawnLocation, 0, 0), 0.5f);
                Gizmos.DrawWireSphere(cPosition + new Vector3(only3player3RespawnLocation, 0, 0), 0.5f);
            }

            if (only4playersRespawnLocationDebug)
            {
                Gizmos.DrawWireSphere(cPosition + new Vector3(only4player1RespawnLocation, 0, 0), 0.5f);
                Gizmos.DrawWireSphere(cPosition + new Vector3(only4player2RespawnLocation, 0, 0), 0.5f);
                Gizmos.DrawWireSphere(cPosition + new Vector3(only4player3RespawnLocation, 0, 0), 0.5f);
                Gizmos.DrawWireSphere(cPosition + new Vector3(only4player4RespawnLocation, 0, 0), 0.5f);
            }
        }

    }
}
