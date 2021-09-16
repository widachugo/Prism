using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Movement values
    [Header("Valeurs de base")]
    public float moveSpeed;
    [Tooltip("Angle Y de base du personnage")]
    public float yBaseOrientation = 90f;
    
    /*[Tooltip("Pied gauche du personnage (avec collider)")]
    public GameObject leftFoot;
    [Tooltip("Pied droit du personnage (avec collider)")]
    public GameObject rightFoot;*/

    // Collision checks
    [Header("Collisions")]
    public LayerMask groundLayer;
    public bool onGround = false;
    [Tooltip("Origine des raycasts vérifiant si le joueur est au sol")]
    public GameObject anklePosition;
    [Tooltip("Longueur du raycast vérifiant si le joueur est au sol")]
    public float groundRaycastLength = 0.6f;
    [Tooltip("Ecartement entre les deux raycasts vérifiants si le perso est au sol")]
    public Vector3 groundCheckerOffset;
    private Rigidbody rb;

    // Jump values
    [Header("Valeurs de saut")]
    public float jumpVelocity = 5f;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;
    [Tooltip("Ajoute un délai au saut pour ne pas pouvoir le spammer")]
    public float jumpDelay;
    private float jumpTimer = 0;
    
    // Ghost Jump values: Ajoute un petit délai pour pouvoir sauter après avoir chuté d'une plateforme
    /* [Tooltip("Valeur de la tolérance du ghost jump (en seconde)")]
    public float ghostJumpAmount = 0.1f;
    private float ghostJumpCurrent; */

    [Header("Animations")]
    [Tooltip("Animator du personnage")]
    public Animator anim;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        onGround = Physics.Raycast(anklePosition.transform.position + groundCheckerOffset, Vector3.down, groundRaycastLength, groundLayer) || Physics.Raycast(anklePosition.transform.position - groundCheckerOffset, Vector3.down, groundRaycastLength, groundLayer);

        if (onGround)
        {
            anim.SetBool("IsGrounded", true);
            anim.SetBool("IsJumping", false);
        }

        // Déplacements du joueur sur l'axe horizontal
        rb.velocity = new Vector3(Input.GetAxis("Horizontal") * moveSpeed, rb.velocity.y, 0);

        if (rb.velocity.x > 0.05f)
        {
            gameObject.transform.rotation = Quaternion.Euler(0f, yBaseOrientation, 0f);
            anim.SetBool("Move", true);
        }
        else if (rb.velocity.x < -0.05f)
        {
            gameObject.transform.rotation = Quaternion.Euler(0f, yBaseOrientation + 180, 0f);
            anim.SetBool("Move", true);
        }else
        {
            anim.SetBool("Move", false);
        }

        // Saut du joueur, si il est en contact avec le sol
        if (Input.GetButtonDown("Jump"))
        {
            jumpTimer = Time.time + jumpDelay;
            //Debug.Log(jumpTimer);
            
            if (jumpTimer > Time.time && onGround)
            {
                anim.SetBool("IsJumping", true);
                anim.SetBool("IsGrounded", false);
                rb.velocity += Vector3.up * jumpVelocity;
                jumpTimer = 0f;
            }
        }

        // Sauts à hauteur variable en fonction de la durée d'appui sur la touche
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rb.velocity.y > 0)
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
        //ghostJumpCurrent = 0f;
    }


    // Debug si le joueur touche le sol
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(anklePosition.transform.position + groundCheckerOffset, anklePosition.transform.position + Vector3.down * groundRaycastLength);
        Gizmos.DrawLine(anklePosition.transform.position - groundCheckerOffset, anklePosition.transform.position + Vector3.down * groundRaycastLength);
    }
}
