// Bullet.cs

using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Bullet : NetworkBehaviour
{
    public float life = 3f;
    public int bulletDamage = 10;
    public NetworkObject networkObject;
    public Gun parent;
    public bool isPowerBullet = false;

    private void Awake()
    {
        // Destroy(gameObject, life);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Obstacle"))
        {
            if (parent == null)
            {
                return;
            }

            parent.DespawnBulletsServerRpc();
        }
        else if (other.gameObject.CompareTag("Player"))
        {
            if (parent == null)
            {
                return;
            }

            if (isPowerBullet)
            {
                parent.DespawnBulletsServerRpc();
                other.gameObject.GetComponent<PlayerSettings>().TakeDamageServerRpc(666);
            }

            parent.DespawnBulletsServerRpc();
            other.gameObject.GetComponent<PlayerSettings>().TakeDamageServerRpc(bulletDamage);
        }
        else if (other.gameObject.CompareTag("Enemy")) // Tambahkan logika untuk musuh
        {
            if (parent == null)
            {
                return;
            }

            other.gameObject.GetComponent<Enemy>().StunEnemy(); // Panggil fungsi stun pada Enemy
            parent.DespawnBulletsServerRpc();
        }
    }

    private void DespawnObject()
    {
        networkObject.DontDestroyWithOwner = true;
        networkObject.Despawn();
    }

    public void SetPowerBullet()
    {
        isPowerBullet = true;
    }

    public void SetNormalBullet()
    {
        isPowerBullet = false;
    }
}