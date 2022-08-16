using Game.Scripts.Enemy;
using UnityEngine;

public class SphereTrap : MonoBehaviour
{
    // Start is called before the first frame update
    public Rigidbody rigidbody;
    public AudioSource audio;
    void Start()
    {
        audio.volume = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (rigidbody.velocity.magnitude >= 1)
        {
            if (!audio.isPlaying)
            {
                audio.Play();
            }
            audio.volume = rigidbody.velocity.magnitude / 30f;

            //Play sound here if you have a rigidbody component and if your movement is rigidbody.AddForce
        }
        else
        {
            if (audio.isPlaying)
            {
                audio.Pause();
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (rigidbody.velocity.magnitude < 1) return;
        if (!collision.gameObject.CompareTag("Enemy")) return;
        
        collision.gameObject.GetComponent<EnemyController>().GetDamage();  
    }
}
