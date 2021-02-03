using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerUIController))]
[RequireComponent(typeof(PlayerAudioController))]
public class FirstPersonController : MonoBehaviour
{
    //movement vars
    private float originalForwardSpeed = 6f;
    private float originalStrafeSpeed = 5f;
    private float currentStrafeSpeed;
    private float currentForwardSpeed;

    private float airboneStrafeModifier = 0.7f;
    private float airbornTime = 0f;

    private float jumpSpeed = 20f;
    private List<Ray> groundedRays = new List<Ray>();
    private float rayRadius = 0.4f;

    private CharacterController charCon;
    private PlayerAudioController audioController;
    private PlayerUIController uiController;

    [HideInInspector]
    public float verticalVelocity = 0;
    [HideInInspector]
    public bool boostApplied = false;
    private float boostTimer = 0f;
    private Vector3 movementVec;
    private Camera camera;

    #region Camera Variables

    private float mouseSensitivity = 4.0f; //def 4
    private float upDownRange = 85f;
    private float verticalRotation = 0;

    #endregion

    #region Sprinting Vars

    private IEnumerator sprintintCoroutine;

    //timers / don't touchers
    private float sprintTimer = 0f;
    private float sprintChargeTimer = 0f;
    private float originalFOV;

    //comparers
    private float sprintTimeMax = 15;
    private float sprintTimeMin = 1;    
    private float sprintRechargeDelay = 1.6f;
    private float FOVInAdjustTime = .5f;
    private float FOVOutAdjustTime = .2f;

    //modifiers
    private float sprintSpeedModifier = 2.5f;
    private float sprintRechargeModifier = 2f;
    private float FOVSprintModifier = 1.2f;

    //inputs
    private bool sprintHit = false;
    private bool isSprinting = false;
    
    #endregion

    #region Public Accessors 

    //making these just in case we want validation on them later
    public bool CanLook { get { return canLook; } set { canLook = value; } }
    public bool CanMove { get { return canMove; } set { CanMove = value; } }

    #endregion

    #region Privates
    private bool canLook = true;
    private bool canMove = true;  

    float forwardInput = 0.0f;

    #endregion

    public FirstPersonController()
    {
    }

    public void Awake()
    {
        charCon = GetComponent<CharacterController>();
        audioController = GetComponent<PlayerAudioController>();
        uiController = GetComponent<PlayerUIController>();
        camera = GetComponentInChildren<Camera>();
    }

    public void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        originalFOV = camera.fieldOfView;
        currentStrafeSpeed = originalStrafeSpeed;
        currentForwardSpeed = originalForwardSpeed;


        StartCoroutine(TrackLook());
        StartCoroutine(TrackJump());
        StartCoroutine(TrackMove());
    }

    private void LateUpdate()
    {
        if (boostApplied)
        {
            boostTimer += Time.deltaTime;
            if (boostTimer >= 0.1f)
            {
                //insurance for boosts getting applied multiple times o
                boostTimer = 0f;
                boostApplied = false;
            }
        }
        charCon.Move(transform.rotation * movementVec * Time.deltaTime);
    }

    public void ApplyBoost(Vector3 boostVec) {
        verticalVelocity = boostVec.y;
        boostApplied = true;
    }

    public IEnumerator TrackMove()
    {
        while (canMove && charCon != null)
        {   
            #region  raycast cluster fuck 凸ಠ益ಠ)凸
            groundedRays = new List<Ray>() {
                new Ray(transform.position, -transform.up), //mid
                new Ray(transform.position + new Vector3(0,0,rayRadius), -transform.up), //N
                new Ray(transform.position + new Vector3(rayRadius * Mathf.Cos(Mathf.PI/4),0,rayRadius * Mathf.Sin(Mathf.PI/4)), -transform.up), //NE
                new Ray(transform.position + new Vector3(-rayRadius * Mathf.Cos(Mathf.PI/4),0,rayRadius * Mathf.Sin(Mathf.PI/4)), -transform.up), //NW
                new Ray(transform.position + new Vector3(0,0,rayRadius * -1f), -transform.up), //S
                new Ray(transform.position + new Vector3(rayRadius * Mathf.Cos(Mathf.PI/4),0,-rayRadius * Mathf.Sin(Mathf.PI/4)), -transform.up), //SE
                new Ray(transform.position + new Vector3(-rayRadius * Mathf.Cos(Mathf.PI/4),0,-rayRadius * Mathf.Sin(Mathf.PI/4)), -transform.up), //SW
                new Ray(transform.position + new Vector3(rayRadius,0,0), -transform.up), //W
                new Ray(transform.position + new Vector3(rayRadius *-1f,0,0), -transform.up) //E
            };
            Vector3 floorAngle = new Vector3();

            for(int i = 0; i < groundedRays.Count(); i++){
                var r = groundedRays[i];

                RaycastHit hit = new RaycastHit();
                if (Physics.Raycast(r, out hit, charCon.height/2f + .15f, 1, QueryTriggerInteraction.Ignore)) {
                    floorAngle += hit.normal;
                }
            }
            #endregion

            floorAngle = floorAngle.normalized * -1;
            float floorLerp = 
            (Vector3.Angle(floorAngle, transform.forward)/90.0f) * 3.0f; 
            
            floorLerp = (floorLerp <= 0) ? 1.0f : floorLerp;
            
            //Debug.Log(floorLerp.ToString());

            if (floorLerp <= 3.0f) {
               forwardInput = Input.GetAxis("Vertical") * currentForwardSpeed * floorLerp;      
            } else {
                float current_forward = Input.GetAxis("Vertical") * currentForwardSpeed * floorLerp;
                forwardInput = Mathf.Max(forwardInput, current_forward);
            }
           

            float strafeInput = Input.GetAxis("Horizontal") * currentStrafeSpeed;
            sprintHit = Input.GetKeyDown(KeyCode.LeftShift);

            #region Sprint Logic

            if (Input.GetAxisRaw("Vertical") > 0)
            {                
                if (!isSprinting && sprintHit)
                {
                    //begin sprint
                    isSprinting = true;
                    sprintChargeTimer = 0f;
                    StartCameraFOVLerp(1);
                }
                else if (isSprinting && sprintHit)
                {
                    //stop sprint
                    sprintChargeTimer = 0f;
                    isSprinting = false;
                }
            }
            else
            {
                isSprinting = false;
            }

            if (isSprinting && sprintTimer <= sprintTimeMax)
            {
                //handle sprinting
                isSprinting = true;
                sprintTimer += Time.deltaTime;
                sprintChargeTimer = 0f;
                forwardInput *= sprintSpeedModifier;
            }
            if (!isSprinting || sprintTimer > sprintTimeMax)
            {
                //handle not sprinting
                isSprinting = false;
                sprintChargeTimer += Time.deltaTime;
                if (sprintChargeTimer >= sprintRechargeDelay)
                {
                    sprintTimer = Mathf.Max(0, sprintTimer - (Time.deltaTime * sprintRechargeModifier));
                }
                StartCameraFOVLerp(-1);
            }

            //sprint circle
            uiController.SetSprintCircle(1 - (sprintTimer / sprintTimeMax));

            #endregion

            movementVec = new Vector3(strafeInput, verticalVelocity, forwardInput);
            
            //charCon.Move(movementVec * Time.deltaTime);
            yield return null;
        }
    }

    private IEnumerator TrackJump()
    {
        while (canMove && charCon != null)
        {
            groundedRays = new List<Ray>() {
                new Ray(transform.position, -transform.up), //mid
                new Ray(transform.position + new Vector3(0,0,rayRadius), -transform.up), //N
                new Ray(transform.position + new Vector3(rayRadius * Mathf.Cos(Mathf.PI/4),0,rayRadius * Mathf.Sin(Mathf.PI/4)), -transform.up), //NE
                new Ray(transform.position + new Vector3(-rayRadius * Mathf.Cos(Mathf.PI/4),0,rayRadius * Mathf.Sin(Mathf.PI/4)), -transform.up), //NW
                new Ray(transform.position + new Vector3(0,0,rayRadius * -1f), -transform.up), //S
                new Ray(transform.position + new Vector3(rayRadius * Mathf.Cos(Mathf.PI/4),0,-rayRadius * Mathf.Sin(Mathf.PI/4)), -transform.up), //SE
                new Ray(transform.position + new Vector3(-rayRadius * Mathf.Cos(Mathf.PI/4),0,-rayRadius * Mathf.Sin(Mathf.PI/4)), -transform.up), //SW
                new Ray(transform.position + new Vector3(rayRadius,0,0), -transform.up), //W
                new Ray(transform.position + new Vector3(rayRadius *-1f,0,0), -transform.up) //E
            };
            
            bool isGrounded = groundedRays.Any(r => {
                RaycastHit hit = new RaycastHit();
                return Physics.Raycast(r, out hit, charCon.height/2f + .15f, 1, QueryTriggerInteraction.Ignore);
            });

            //Gravity            
            if (!isGrounded)
            {
                airbornTime = Time.deltaTime;
                verticalVelocity += Mathf.Min(Physics.gravity.y * airbornTime * 6f, 60f); //terminal velocity
                currentStrafeSpeed = originalStrafeSpeed * airboneStrafeModifier;
            }
            else if (isGrounded)
            {
                currentStrafeSpeed = originalStrafeSpeed;
                airbornTime = 0f;
                //Jump Check                
                if (Input.GetButton("Jump"))
                {
                    verticalVelocity = jumpSpeed;
                    audioController.PlayPlayerClip(PlayerAudioClips.Jump);
                }
            }
            yield return null;
        }
    }

    private IEnumerator TrackLook()
    {
        while (canLook && camera != null)
        {
            float sideToSideRotation = Input.GetAxis("Mouse X") * mouseSensitivity;
            transform.Rotate(0, sideToSideRotation, 0);

            verticalRotation -= Input.GetAxis("Mouse Y") * mouseSensitivity;
            verticalRotation = Mathf.Clamp(verticalRotation, -upDownRange, upDownRange);
            camera.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);

            yield return null;
        }
    }

    //Camera FOV Methods
    private void StartCameraFOVLerp(int zoomDirection)
    {
        //ensures only 1 instance is running
        if(sprintintCoroutine != null)
            StopCoroutine(sprintintCoroutine);

        sprintintCoroutine = LerpCameraFOV(zoomDirection);
        StartCoroutine(sprintintCoroutine);
    }
    private IEnumerator LerpCameraFOV(int zoomDirection)
    {
        float startFOV, finishFOV, time;
        switch (zoomDirection)
        {
            case 1:
                startFOV = originalFOV;
                finishFOV = originalFOV * FOVSprintModifier;
                time = FOVInAdjustTime;
                break;
            case -1:
                finishFOV = originalFOV;
                startFOV = camera.fieldOfView;
                time = FOVOutAdjustTime;
                break;
            default:
                //shouldn't hit this.
                Debug.LogError("LerpCameraFOV called with value other than {1,-1}");
                startFOV = originalFOV;
                finishFOV = originalFOV;
                time = FOVInAdjustTime;
                break;
        }

        float elapsedTime = 0;
        while (elapsedTime < time)
        {
            camera.fieldOfView = Mathf.Lerp(startFOV, finishFOV, (elapsedTime / time));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    public Vector3 GetMoveVector(){
        return movementVec;
    }

    void OnGUI()
    {
        //GUILayout.Label(sprintChargeTimer.ToString());
        //GUILayout.Label(sprintChargeTimer.ToString());
    }

    void OnDrawGizmos()
    {
        //ground check rays
        Gizmos.color = Color.yellow;
        foreach (Ray r in groundedRays)
        {
            Gizmos.DrawRay(r);
        }
    }

}
