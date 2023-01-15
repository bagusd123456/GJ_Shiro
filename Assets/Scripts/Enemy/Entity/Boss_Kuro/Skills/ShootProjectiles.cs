using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootProjectiles : MonoBehaviour
{
    public ProjectilesForward projectile;
    public List<ProjectilesForward> projectiles = new List<ProjectilesForward>();
    public float projectileSpeed = 1f;
    public int maxOrb = 1;
    public float targetDistance = 3;
    public Vector3 offset;

    public int spawnCount;
    // Update is called once per frame
    void Update()
    {
        foreach (var item in projectiles)
        {
            item.projectileSpeed = projectileSpeed;
            item.targetDistance = targetDistance;
            if (!item.GetComponent<ProjectilesForward>().canMove)
                item.SetPosition();
            //item.targetDistance = Mathf.Lerp(1f, targetDistance, Mathf.PingPong(Time.time, 1));
        }
    }

    [ContextMenu("Launch")]
    //Launch Projectiles Simultaneously
    public void Launch()
    {
        for (int i = projectiles.Count - 1; i >= 0; i--)
        {
            ProjectilesForward prj = projectiles[i].GetComponent<ProjectilesForward>();
            prj.canMove = true;
            if (prj.canMove)
            {
                projectiles.RemoveAt(i);
            }
        }
    }

    [ContextMenu("Launch2")]
    public void DebugLaunch2()
    {
        StartCoroutine(Launch2());
    }

    [ContextMenu("Spawn1")]
    public void DebugSpawn1()
    {
        SpawnProjectile(spawnCount);
    }

    [ContextMenu("Spawn2")]
    public void DebugSpawn2()
    {
        StartCoroutine(Spawn2());
    }

    //spawn Projectiles Recursively every 0.1f seconds
    public IEnumerator Spawn2()
    {
        int j = 0;
        for (int i = projectiles.Count; i < spawnCount; i++)
        {
            yield return new WaitForSeconds(0.1f);
            var prj = Instantiate(projectile, transform);
            prj.center = transform;
            prj.transform.position = transform.position + Vector3.forward;
            prj.angle = 6.29f / spawnCount * j;
            j++;

            projectiles.Add(prj);
        }
        //Launch Projectiles
        StartCoroutine(Launch2());
    }

    [ContextMenu("Spawn3")]
    public void Spawn3()
    {
        int j = 0;
        for (int i = projectiles.Count; i < spawnCount; i++)
        {
            var prj = Instantiate(projectile, transform);
            prj.center = transform;
            prj.angle = 6.29f / spawnCount * j;
            j++;

            projectiles.Add(prj);
        }
        StartCoroutine(Launch2());
    }

    public IEnumerator Launch2()
    {
        for (int i = 0; i < projectiles.Count;)
        {
            var unit = projectiles[i].GetComponent<ProjectilesForward>();
            yield return new WaitForSeconds(0.1f);
            unit.canMove = true;

            if (unit.canMove)
            {
                projectiles.RemoveAt(i);
            }
            else
            {
                i++;
            }
        }

        /*for (int i = projectiles.Count - 1; i >= 0; i--)
        {
            ProjectilesForward prj = projectiles[i].GetComponent<ProjectilesForward>();
            yield return new WaitForSeconds(0.2f);
            prj.canMove = true;
            if (prj.canMove)
            {
                projectiles.RemoveAt(i);
            }
        }*/
    }

    public void SpawnProjectile(int spawnCount)
    {
        for (int i = projectiles.Count; i < spawnCount; i++)
        {
            var prj = Instantiate(projectile, transform);
            prj.center = transform;
            prj.transform.position = transform.position + Vector3.forward;
            projectiles.Add(prj);

            if (projectiles.Count > 0)
            {
                int j = 0;
                foreach (var item in projectiles)
                {

                    item.angle = 6.29f / projectiles.Count * j;
                    j++;
                }
            }
        }

        Invoke("Launch",0.2f);
    }

    public void ListChecker()
    {
        if (projectiles.Count < maxOrb)
        {
            var prj = Instantiate(projectile,transform);
            prj.center = transform;
            prj.transform.position = transform.position + Vector3.forward;
            projectiles.Add(prj);

            if (projectiles.Count > 0)
            {
                int i = 0;
                foreach (var item in projectiles)
                {
                    
                    item.angle = 6.29f / projectiles.Count * i;
                    i++;
                }
            }
        }

        else if( projectiles.Count > maxOrb)
        {
            var prj = projectiles[projectiles.Count - 1];
            projectiles.Remove(prj);
            Destroy(prj.gameObject);

            if (projectiles.Count > 0)
            {
                int i = 0;
                foreach (var item in projectiles)
                {

                    item.angle = 6.29f / projectiles.Count * i;
                    i++;
                }
            }
        }
    }
}
