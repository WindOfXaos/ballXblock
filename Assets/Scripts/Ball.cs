using UnityEngine;

public class Ball : MonoBehaviour
{
    //config param
    [SerializeField] Paddle paddle1;
    [Range(1f, 5f)] [SerializeField] float initForce = 100f;
    [SerializeField] float xPush = 2f;
    [SerializeField] float yPush = 15f;
    [SerializeField] AudioClip[] ballSounds;
    [Range(0f, 5f)] [SerializeField] float randomFactor;

    //state
    Vector2 paddleToBallVector;

    //Cached component refrences
    AudioSource myAudioSource;
    Rigidbody2D myRigidBody2D;

    //Flags
    bool launched = false;
    bool initSpeed = true;

    // Start is called before the first frame update
    void Start()
    {
        paddleToBallVector = transform.position - paddle1.transform.position;
        myAudioSource = GetComponent<AudioSource>();
        myRigidBody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

        if (!launched)
        {
            LockBallToPaddle();
            LaunchOnMouseClick();
        }
    }

    private void LaunchOnMouseClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            launched = true;
            myRigidBody2D.velocity = new Vector2(xPush * initForce, yPush * initForce);
        }
    }

    private void NormalizeSpeed()
    {
        myRigidBody2D.velocity = new Vector2(xPush, yPush);
        initSpeed = false;
    }

    private void LockBallToPaddle()
    {
        Vector2 paddlePos = new Vector2(paddle1.transform.position.x, paddle1.transform.position.y);
        transform.position = paddlePos + paddleToBallVector;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(initSpeed && launched) NormalizeSpeed();

        Vector2 velocityTweak = new Vector2
            (Random.Range(0, randomFactor),
             Random.Range(0, randomFactor));

        if (launched)
        {
            AudioClip clip = ballSounds[UnityEngine.Random.Range(0, ballSounds.Length)];
            myAudioSource.PlayOneShot(clip);
            myRigidBody2D.velocity += velocityTweak;
        }
    }
}
