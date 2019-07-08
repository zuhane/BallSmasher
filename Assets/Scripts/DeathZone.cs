using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    public team team = team.red;
    private AudioSource audioSource;
    [SerializeField] private AudioClip[] clips;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Rigidbody2D rigidBody = collision.gameObject.GetComponent<Rigidbody2D>();

        if (rigidBody != null && collision.gameObject.tag == "Bouncy")
        {
            Destroy(collision.gameObject);

            audioSource.clip = clips[Random.Range(0, clips.GetLength(0))];
            audioSource.Play();

            GameObject.Find("GoalManager").GetComponent<GoalManager>().ScoreGoal(team);
        }
    }

}
