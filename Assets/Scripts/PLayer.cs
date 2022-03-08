using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PLayer : Character
{
    // Start is called before the first frame update

    [SerializeField]
    float moveSpeed = 5f;

    [SerializeField]
    float jumpForce;
    
    [SerializeField]
    float jumpForceShort;

    [SerializeField]
    float raydistance = 5f;

    [SerializeField]
    Color raycolor = Color.red;

    [SerializeField]
    LayerMask groundlayer;
    SpriteRenderer spr;
    Animator anim;

    [SerializeField]
    Vector3 rayOrigin;

    GameInputs gameInputs;

    Rigidbody2D rb2D;

    void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        spr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        gameInputs = new GameInputs();
    }
    void OnEnable()
    {
        gameInputs.Enable();
    }
    
    void OnDisable()
    {
        gameInputs.Disable();
    }


    void Start()
    {
        //gameInputs.Gameplay.Jump.performed += _=> Jump();       
    }

    public void OnJump(InputAction.CallbackContext context){
        switch(context.phase){
            case InputActionPhase.Canceled:
                Jump();
                Debug.Log("Largo");
                break;
            case InputActionPhase.Performed:
                JumpShort();
                Debug.Log("corto");
                break;
        }
    }

    void FixedUpdate()
    {
        rb2D.position += Vector2.right * Horizontal * moveSpeed * Time.fixedDeltaTime;
    }
    
    void Update()
    {
        //transform.Translate(Vector2.right * Axis.x * moveSpeed * Time.deltaTime);
        spr.flipX = FlipSpriteX;
        /*if(JumpButton && IsGrounding )
        {
            rb2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            anim.SetTrigger("jump");
        }*/

    }

    void LateUpdate()
    {
        anim.SetFloat("AxisX", Mathf.Abs(Horizontal));
        anim.SetBool("ground", IsGrounding);
    }

    
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("coin"))
        {
            Coin coin = other.GetComponent<Coin>();
            GameManager.instance.GetScore.AddPoints(coin.GetPoints);
            Destroy(other.gameObject);
        }
    }


    void OnDrawGizmosSelected()
    {
        Gizmos.color = raycolor;
        Gizmos.DrawRay(transform.position + rayOrigin,Vector2.down*raydistance);
        
    }

    void Jump()
    {
        if(IsGrounding)
        {
            rb2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            anim.SetTrigger("jump");
        }

    }

    void JumpShort(){
        if(IsGrounding)
        {
            rb2D.AddForce(Vector2.up * jumpForceShort, ForceMode2D.Impulse);
            anim.SetTrigger("jump");
        }
    }


    float Horizontal => gameInputs.Gameplay.Horizontal.ReadValue<float>();

    //Vector2 Axis => new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
   // bool JumpButton => Input.GetButtonDown("Jump");

    bool IsGrounding => Physics2D.Raycast(transform.position + rayOrigin, Vector2.down, raydistance, groundlayer);

    bool FlipSpriteX => Horizontal > 0f ? false : Horizontal < 0f ? true : spr.flipX;


}
