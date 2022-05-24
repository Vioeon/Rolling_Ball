using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerCtrl : MonoBehaviour
{
    public GameObject mainCam;
    public GameObject player;
    public GameObject Cursor;
    public GameObject over; // Game Over SetActive
    public Text text;
    Rigidbody rigid;

    public GameObject clear; // 클리어 SetActive
    public GameObject playtime; // 플레이시간 SetActive
    public GameObject deadcount; // 죽은 횟수 SetActive
    public Text PT; // 플레이시간 텍스트
    public Text DC; // 죽은 횟수 텍스트
    public GameObject goMain;

    private float count = 0f;

    private float move = 10.0f;  // 화면 기울이면 좌/우 이동
    private float positionX = 0.0f;

    private float runSpeed = 8.0f;
    private float Maxspeed = 8.0f;
    private float jumpPower = 6.0f;

    private Vector3 initPlayerPosition; // 재시작 위치

    bool isJump = false; // 점프상태

    private int deadCount = 0; // 죽은 획수
    private float playTime = 0; // 플레이 시간
    private int _Min;

    public ParticleSystem particle1;
    public ParticleSystem particle2;
    public ParticleSystem particle3;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }
    void Start()
    {
        initPlayerPosition = player.transform.position;
    }

    void Update()
    {
        Timer();

        if (!isJump)
        {
            if (Input.GetMouseButtonDown(0))
            {
                isJump = true;
                rigid.AddForce(new Vector3(0, jumpPower, 0), ForceMode.Impulse);
                AudioSource jump = GetComponent<AudioSource>();
                jump.Play();
            }
        }

        if (player.gameObject.transform.position.y <= -10) // 낙사 시 재시작
        {
            deadCount++;
            Debug.Log(deadCount);

            StartCoroutine(Restart());
        }

    }

    private void FixedUpdate()
    {

        // 고개 기울이면 좌우 이동
        //positionX = this.transform.position.x - mainCam.transform.rotation.z * move / 90.0f;
        //this.transform.position = new Vector3(positionX, this.transform.position.y, this.transform.position.z);

        if (rigid.velocity.z < Maxspeed)
        {
            rigid.AddForce(mainCam.transform.forward * runSpeed); // 메인캠 정면으로 이동
            Debug.Log(rigid.velocity);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        isJump = false;

        if(other.gameObject.tag == "Goal")
        {
            initPlayerPosition = player.transform.position;

            StartCoroutine(Clear());
 
            ParticleSystem fire1 = Instantiate(particle1, particle1.transform.position, particle1.transform.rotation);
            ParticleSystem fire2 = Instantiate(particle2, particle2.transform.position, particle2.transform.rotation);
            ParticleSystem fire3 = Instantiate(particle3, particle3.transform.position, particle3.transform.rotation);
            fire1.Play();
            fire2.Play();
            fire3.Play();
            /*particle1.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            particle2.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            particle3.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);*/
            // 일정 시간 뒤  결과 씬으로 SceneManager.LoadScene("");
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "JumpTile") // High Jump
        {
            rigid.AddForce(new Vector3(0, jumpPower * 2.5f, 0), ForceMode.Impulse);
            AudioSource jump = GetComponent<AudioSource>();
            jump.Play();
        }
        if (other.gameObject.tag == "SpeedTile") // Fast Speed
        {
            rigid.AddForce(mainCam.transform.forward * runSpeed * 3, ForceMode.Impulse);
            AudioSource speed = other.GetComponent<AudioSource>();
            speed.Play();
        }
        if (other.gameObject.tag == "SlowTile") // Slow Speed
        {
            rigid.AddForce(mainCam.transform.forward * -15, ForceMode.Impulse);
            AudioSource slow = other.GetComponent<AudioSource>();
            slow.Play();
        }
    }

    void Timer()
    {
        playTime += Time.deltaTime;

        if((int)playTime > 59)
        {
            playTime = 0;
            _Min++;
        }
    }

    IEnumerator Restart()
    {
        over.gameObject.SetActive(true);
        text.text = "Game Over";

        yield return new WaitForSeconds(1.5f);
        
        over.gameObject.SetActive(false);
        player.transform.position = initPlayerPosition;
        rigid.velocity = Vector3.zero;
        rigid.angularVelocity = Vector3.zero;

    }

    IEnumerator Clear()
    {
        runSpeed = 0;
        jumpPower = 0;
        rigid.velocity = Vector3.zero;
        rigid.angularVelocity = Vector3.zero;

        Cursor.SetActive(true);
        clear.SetActive(true);
        
        yield return new WaitForSeconds(3.0f);
        /*string timeStr;
        timeStr = "" + playTime.ToString("00.00");
        timeStr = timeStr.Replace(".", ":");*/

        playtime.SetActive(true);
        PT.text = "  Play Time : " + string.Format("{0:D2}:{1:D2}", _Min, (int)playTime);


        deadcount.SetActive(true);
        DC.text = "   죽은 횟수 : " + deadCount/90;

        goMain.SetActive(true);
        //SceneManager.LoadScene("StartScene");
    }
}

