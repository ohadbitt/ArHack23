using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(AudioSource))]
public class RaycastGun : MonoBehaviour
{
    public Camera playerCamera;
    public Transform laserOrigin;
    public GameObject explosion;
    public UnityEvent<GameObject> kill;
    public float gunRange = 50f;
    public float fireRate = 0.2f;
    public float laserDuration = 0.05f;
    //public float clipLength = 1f;

    AudioSource sound;
    LineRenderer laserLine;
    float fireTimer;

    void Awake()
    {
        laserLine = GetComponent<LineRenderer>();
        sound = GetComponent<AudioSource>();
    }

    void Update()
    {
        fireTimer += Time.deltaTime;
        if (Input.touchCount > 0 && fireTimer > fireRate)
        {
            fireTimer = 0;
            laserLine.SetPosition(0, laserOrigin.position);
            Vector3 rayOrigin = playerCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hit;
            GameObject explode = null;
            if (Physics.Raycast(rayOrigin, playerCamera.transform.forward, out hit, gunRange))
            {
                laserLine.SetPosition(1, hit.point);
                explode = Instantiate(explosion, hit.point, Quaternion.identity);
                kill.Invoke(hit.transform.gameObject);
                //Destroy(explode);
            }
            else
            {
                laserLine.SetPosition(1, rayOrigin + (playerCamera.transform.forward * gunRange));
            }
            StartCoroutine(ShootLaser(explode));
        }
    }

    IEnumerator ShootLaser(GameObject explode)
    {
        laserLine.enabled = true;
        yield return new WaitForSeconds(laserDuration);
        laserLine.enabled = false;
        sound.Play();
        if (explode != null)
        {
            Destroy(explode);
        }
    }


}