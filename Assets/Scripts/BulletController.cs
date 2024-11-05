using System.Collections;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [Header("Bullet Info")]
    [SerializeField] private float activetime;

    private float shootTime;

    private int damage;

    public int Damage {  get => damage; set => damage = value; }

    //When the gameobject SetActive = true

    private void OnEnable()
    {
        StartCoroutine(DeactiveAfterTime());
    }

    private IEnumerator DeactiveAfterTime()
    {
        yield return new WaitForSeconds(activetime);
        gameObject.SetActive(false);
    }

    //When the bullet collide with something
    private void OnTriggerEnter(Collider other)
    {
        //Deactivate the bullet
        gameObject.SetActive(false);

        //TODO Collision with enemy or player or wall or object
    }
}
