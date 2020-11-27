using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FirstPersonController : MonoBehaviour
{
    //movement vars
    private float originalForwardSpeed = 6f;
    private float originalStrafeSpeed = 5f;
    private float currentStrafeSpeed;
    private float currentForwardSpeed;

    private float airboneStrafeModifier = 0.7f;


    private float jumpSpeed = 20f;
    private List<Ray> groundedRays = new List<Ray>();

    private CharacterController charCon;
    private PlayerAudioController audioController;

    private float verticalVelocity = 0;
    private Vector3 movementVec;
    private Camera camera;

    #region Camera Variables

    private float mouseSensitivity = 5.0f; //def 5
    private float upDownRange = 85f;
    private float verticalRotation = 0;

    #endregion

    #region Sprinting Vars

    private IEnumerator sprintintCoroutine;
    private float sprintModifier = 2.5f;
    private float originalFOV;
    private float FOVSprintModifier = 1.5f;
    private float FOVInAdjustTime = .5f;
    private float FOVOutAdjustTime = .2f;
    private bool shouldSprint = false;
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

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        foreach(Ray r in groundedRays)
        {
            Gizmos.DrawRay(r);
        }
    }

    #endregion

    //Look


    public FirstPersonController()
    {
    }

    public void Awake()
    {
        charCon = GetComponent<CharacterController>();
        camera = GetComponentInChildren<Camera>();
        audioController = GetComponentInChildren<PlayerAudioController>();
    }

    public void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        originalFOV = camera.fieldOfView;
        currentStrafeSpeed = originalStrafeSpeed;
        currentForwardSpeed = originalForwardSpeed;


        StartCoroutine(Look());
        StartCoroutine(Jump());
        StartCoroutine(Move());
    }

    public IEnumerator Move()
    {
        while (canMove && charCon != null)
        {
            float forwardInput = Input.GetAxis("Forward") * currentForwardSpeed;
            float strafeInput = Input.GetAxis("Sideways") * currentStrafeSpeed;
            shouldSprint = Input.GetKey(KeyCode.LeftShift);

            if (Input.GetAxisRaw("Forward") > 0) {
                //Sprint check
                if (shouldSprint && !isSprinting)
                {
                    isSprinting = true;
                    forwardInput = forwardInput * sprintModifier;
                    StartCameraFOVLerp(1);
                }
                else if(isSprinting)
                {
                    forwardInput = forwardInput * sprintModifier;
                }
            }
            else
            {
                isSprinting = false;
                StartCameraFOVLerp(-1);
            }

            movementVec = transform.rotation * new Vector3(strafeInput, verticalVelocity, forwardInput);
            charCon.Move(movementVec * Time.deltaTime);
            yield return null;
        }
    }

    private IEnumerator Jump()
    {
        while (canMove && charCon != null)
        {
            groundedRays = new List<Ray>() {
                new Ray(transform.position, -transform.up), //mid
                new Ray((transform.position + new Vector3(0,0,0.5f)), -transform.up), //top
                new Ray((transform.position + new Vector3(0,0,0.5f * -1f)), -transform.up), //bot
                new Ray((transform.position+ new Vector3(0.5f,0,0)), -transform.up), //left
                new Ray((transform.position+ new Vector3(0.5f *-1f,0,0)), -transform.up) //right
            };
            
            bool isGrounded = groundedRays.Any(r => {
                RaycastHit hit = new RaycastHit();
                return Physics.Raycast(r, out hit, charCon.height/2f + .15f, 1, QueryTriggerInteraction.Ignore);
            });

            //Gravity
            if (!isGrounded)
            {
                verticalVelocity -= (Mathf.Pow(Physics.gravity.y, 2) * Time.deltaTime) / 3f;
                currentStrafeSpeed = originalForwardSpeed * airboneStrafeModifier;
            }
            else if (isGrounded)
            {
                currentStrafeSpeed = originalStrafeSpeed;
                verticalVelocity = 0f;
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

    private IEnumerator Look()
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
}
