using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    private AudioSource _source;
    private Animator _anim;
    [SerializeField]
    private ParticleSystem _smokeParticle, _muzzleFlash, _tracer;
    [SerializeField]
    private bool _canFire = true;
    [SerializeField]
    private GameObject _bloodSplat;
    
    // Start is called before the first frame update
    void Start()
    {
        _anim = GetComponentInChildren<Animator>();
        _source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //if left click
        if (Input.GetMouseButtonDown(0) && _canFire == true)
        {
            _canFire = false;

            _anim.SetTrigger("Fire");
            _source.Play();
            _smokeParticle.Emit(5);
            _tracer.Emit(10);
            _muzzleFlash.Emit(1);
            StartCoroutine(WeaponCoolDownRoutine());

            //cast ray from center of the screen through the radicule 
            Ray rayOrigin = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f));
            RaycastHit hitInfo; //Store information about the object we hit

            //cast a ray
            if (Physics.Raycast(rayOrigin, out hitInfo))
            {
                EnemyAI enemy = hitInfo.collider.GetComponent<EnemyAI>();

                if (enemy != null)
                {
                    GameObject blood = Instantiate(_bloodSplat, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
                    Destroy(blood, 5.0f);
                    enemy.Damage(20);
                }
            }
        }
    }

    IEnumerator WeaponCoolDownRoutine()
    {
        yield return new WaitForSeconds(1.5f);
        _canFire = true;
    }
}
