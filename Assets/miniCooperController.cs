using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using System;

public class miniCooperController : MonoBehaviour
{
    public long frames = 0;
    [SerializeField] private Transform _transformFL;
    [SerializeField] private Transform _transformFR;
    [SerializeField] private Transform _transformBL;
    [SerializeField] private Transform _transformBR;
    //[SerializeField] private Transform _mainMenu;

    [SerializeField] private WheelCollider _colliderFL;
    [SerializeField] private WheelCollider _colliderFR;
    [SerializeField] private WheelCollider _colliderBL;
    [SerializeField] private WheelCollider _colliderBR;

    [SerializeField] private float _force;
    [SerializeField] private float _maxAngle;

    Rigidbody rb;

    [SerializeField] private float speed;
    [SerializeField] private float speedPerSecond;
    [SerializeField] private Vector3 oldPosition;

    // ====================
    // [SerializeField] private bool isGamePaused;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject miniMap;

    [SerializeField] private TMP_Text _textSpeed;


    float elapsed = 0f;
    float elapsedSpeed = 0f;
    Queue<int> queue;
    TimeSpan time;
    TimeSpan timeMax = new TimeSpan(0, 0, 30);

    // ==================================

    [SerializeField] private GameObject startWall;
    [SerializeField] private GameObject finishWall;
    private bool started;
    private bool finished;
    [SerializeField] private TMP_Text _textTimeRemain;
    [SerializeField] private TMP_Text _textGameOver;
    [SerializeField] private GameObject GameOver;
    [SerializeField] private TMP_Text _textTime;


    void Awake()
    {
       // DontDestroyOnLoad(transform.gameObject);
        Debug.Log("Awake");
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.mass = PlayerPrefs.GetFloat("mass");
        _force = PlayerPrefs.GetFloat("force");  
        rb.centerOfMass = new Vector3(0, 0, 0);

        //float positionX = PlayerPrefs.GetFloat("positionX", 25);
        //float positionY = PlayerPrefs.GetFloat("positionY", 17.5f);
        //float positionZ = PlayerPrefs.GetFloat("positionZ", -27);
        //transform.position = new Vector3(positionX, positionY, positionZ);
        // transform.position = new Vector3(6.6f, 22, -17.5f);
        Debug.Log("start");
        // isGamePaused = false;

        queue = new Queue<int>();
        for (int i = 0; i < 5; i++)
        {
            queue.Enqueue(0);
        }
        time = timeMax;
        _textTimeRemain.text = "Time: " + time;
        _textSpeed.text = "0 km/h";
        started = false;
        finished = false;
    }


    public void Restart()
    {
        Debug.Log("restart");
        // SceneManager.LoadScene(SceneManager.GetActiveScene().name); 
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); 
        Time.timeScale = 1f;
    }



    public void Pause()
    {
        // isGamePaused = true;
        mainMenu.SetActive(true); // show main menu
        miniMap.SetActive(false);
        Time.timeScale = 0;
        Debug.Log("inside pause");
    }
     

    public void Resume()
    {
        Debug.Log("Resume");
        // isGamePaused = false;
        mainMenu.SetActive(false);  // hide main menu
        miniMap.SetActive(true);
        Time.timeScale = 1f;
    }

    //void Update()
    //{
    //    speed = Vector3.Distance(oldPosition, transform.position); //This is the speed per frames
    //    speedPerSecond = Vector3.Distance(oldPosition, transform.position) / Time.deltaTime; //This is the speed per second
    //    oldPosition = transform.position;
    //    Debug.Log("Speed: " + speed.ToString("F2"));
    //    //Debug.Log("Start () = " + Time.time);
    //}


    private void OnTriggerEnter(Collider other)
    {       
        if (other.gameObject == startWall)
        {
            if (!started)
            {
                Debug.Log("Started");
                started = true;
            }
            else
            {
                Debug.Log("not Started");
                started = false;
            }
        }



        if (other.gameObject == finishWall && started)
        {
            Debug.Log("timeMax " + timeMax);
            Debug.Log("time " + time);
            _textTime.text = "Your time: " + (timeMax - time);
            GameOver.SetActive(true);
            started = false;
            _textGameOver.text = "You win";
        }



        //if (other.gameObject == finishWall)
        //{
        //    Debug.Log("timeMax " + timeMax);
        //    Debug.Log("time " + time);
        //    _textTime.text = "Your time: " +  (timeMax - time);
        //    GameOver.SetActive(true);
        //    started = false;
        //    _textGameOver.text = "You win";
            
        //}

    }
 

    private void FixedUpdate()
    {
        if (started)
        {
            elapsed += Time.deltaTime;
            if (elapsed >= 1f)
            {
                time -= new TimeSpan(0, 0, 1);               
                _textTimeRemain.text = "Time: " + time;
                elapsed = elapsed % 1f;
                if(time <= new TimeSpan(0, 0, 0))
                {
                    GameOver.SetActive(true);
                    _textTime.text = "Time is up!";
                    started = false;
                }
            }
        }



        ////  ========  SPEED =========
        elapsedSpeed += Time.deltaTime * 5;
        if (elapsedSpeed >= 1f)
        {
            elapsedSpeed = elapsedSpeed % 1f;
            queue.Dequeue();
            int speed = Mathf.RoundToInt(rb.velocity.magnitude * 2);
            queue.Enqueue(speed);
            float average = (float)queue.Average();
            _textSpeed.text = Mathf.RoundToInt(average) + " km/h";
            // _textSpeed.text = Mathf.RoundToInt(rb.velocity.magnitude * 10) + " km/h";
        }

        //Debug.Log(Mathf.RoundToInt(rb.velocity.magnitude * 10) + " km/h");

        if (Input.GetKey(KeyCode.Escape))
        {
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
            //oldPosition = transform.position;
            //PlayerPrefs.SetFloat("positionX", oldPosition.x);
            //PlayerPrefs.SetFloat("positionY", oldPosition.y+0.5f);
            //PlayerPrefs.SetFloat("positionZ", oldPosition.z);
            Debug.Log("Esc");
            Pause();
            // Restart();
            Debug.Log("after pause");
        }


        _colliderFL.motorTorque = Input.GetAxis("Vertical") * _force;
        _colliderFR.motorTorque = Input.GetAxis("Vertical") * _force;

        if (Input.GetKey(KeyCode.Space))
        {
            _colliderFL.brakeTorque = 3000f;
            _colliderFR.brakeTorque = 3000f;
            _colliderBL.brakeTorque = 3000f;
            _colliderBR.brakeTorque = 3000f;
        }
        else
        {
            _colliderFL.brakeTorque = 0f;
            _colliderFR.brakeTorque = 0f;
            _colliderBL.brakeTorque = 0f;
            _colliderBR.brakeTorque = 0f;
        }
        

        _colliderFL.steerAngle = _maxAngle * Input.GetAxis("Horizontal");
        _colliderFR.steerAngle = _maxAngle * Input.GetAxis("Horizontal");

        // Debug.Log("left right = " + Input.GetAxis("Horizontal"));

        RotateWheel(_colliderFL, _transformFL);
        RotateWheel(_colliderFR, _transformFR);
        RotateWheel(_colliderBL, _transformBL);
        RotateWheel(_colliderBR, _transformBR);
    }

    private void RotateWheel(WheelCollider collider, Transform transform)
    {
        Vector3 position;
        Quaternion rotation;

        collider.GetWorldPose(out position, out rotation);

        transform.rotation = rotation;
        transform.position = position;
    }
}
