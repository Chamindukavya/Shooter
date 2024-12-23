using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int damage;
    public float fireRate;
    private float nextFire;

    public Camera camera1;

    [Header("VFX")]
    public GameObject hitVFX;

    // Update is called once per frame
    void Update()
    {

        if (nextFire > 0)
        {
            nextFire -= Time.deltaTime;
        }

        if (Input.GetButton("Fire1") && nextFire <= 0)
        {
            nextFire = 1/fireRate;
            fire();
        }
        else
        {
            nextFire -= Time.deltaTime;
        }
    }

    void fire(){
        Ray ray = new Ray(camera1.transform.position, camera1.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray.origin, ray.direction,out hit, 100f))
        {

            //adding fre effects
            PhotonNetwork.Instantiate(hitVFX.name, hit.point, Quaternion.identity);


            if(hit.transform.gameObject.GetComponent<Health>()){
                hit.transform.gameObject.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.All, damage);
            }
        }
    }
}
