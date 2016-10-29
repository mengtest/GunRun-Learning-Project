using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public int damagePerShot = 20;
    public float timeBetweenBullets = 0.15f;
    public float range = 100f;


    float timer;
    Ray shootRay;
    RaycastHit shootHit;
    int shootableMask;
    ParticleSystem gunParticles;
    LineRenderer gunLine;
    AudioSource gunAudio;
    Light gunLight;
    float effectsDisplayTime = 0.2f;


    void Awake ()
    {
        shootableMask = LayerMask.GetMask ("Shootable");
        gunParticles = GetComponent<ParticleSystem> ();
        gunLine = GetComponent <LineRenderer> ();
        gunAudio = GetComponent<AudioSource> ();
        gunLight = GetComponent<Light> ();
    }


    void Update ()
    {
        timer += Time.deltaTime;

		if(Input.GetButton ("Fire1") && timer >= timeBetweenBullets && Time.timeScale != 0)
        {
            Shoot ();
        }

        if(timer >= timeBetweenBullets * effectsDisplayTime)
        {
            DisableEffects ();
        }
    }


    public void DisableEffects ()
    {
        gunLine.enabled = false;
        gunLight.enabled = false;
    }


    void Shoot ()
    {
        timer = 0f;

        gunAudio.Play ();

        gunLight.enabled = true;

        gunParticles.Stop ();
        gunParticles.Play ();

        gunLine.enabled = true;
        gunLine.SetPosition (0, transform.position);

        shootRay.origin = transform.position;
        shootRay.direction = transform.forward;

        //si pega con algo
        if(Physics.Raycast (shootRay, out shootHit, range, shootableMask))
        {
            //consigue la vida
            EnemyHealth enemyHealth = shootHit.collider.GetComponent <EnemyHealth> ();
            //si le arrojó null significa que es un muro o algo que puede bloquear el disparo pero NO es un enemigo.
            if(enemyHealth != null)
            {
                //Si entro significa que es un enemigo y allí aplica el daño.
                enemyHealth.TakeDamage (damagePerShot, shootHit.point);
            }
            //sin embargo igual si encuentra un enemigo o no, que lanze el rayo igual.
            gunLine.SetPosition (1, shootHit.point);
        }
        else
        {
            //Si no encontró nada igual queremos que lanze el rayo al aire como una bala normal, es un juego.
            gunLine.SetPosition (1, shootRay.origin + shootRay.direction * range);
        }
    }
}
