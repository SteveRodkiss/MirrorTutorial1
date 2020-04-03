using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class LaserGun : NetworkBehaviour
{
    public Transform laserTransform;
    public LineRenderer line;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isLocalPlayer && Input.GetMouseButtonDown(0))
        {
            //we want to shoot!
            CmdShoot();
        }
    }

    [Command]
    public void CmdShoot()
    {
        //do a raycast to see where we hit
        Ray ray = new Ray(laserTransform.position, laserTransform.forward);
        if(Physics.Raycast(ray, out RaycastHit hit, 100f))
        {
            //we hit something- draw the line
            RpcDrawLaser(laserTransform.position, hit.point);
        }
        else
        {
            RpcDrawLaser(laserTransform.position, laserTransform.position + laserTransform.forward * 100f);
        }
    }

    [ClientRpc]
    void RpcDrawLaser(Vector3 start, Vector3 end)
    {
        //do the laser flas enumerator function
        StartCoroutine(LaserFlash(start, end));
    }

    IEnumerator LaserFlash(Vector3 start, Vector3 end)
    {
        line.SetPosition(0, start);
        line.SetPosition(1, end);
        yield return new WaitForSeconds(0.3f);
        line.SetPosition(0, Vector3.zero);
        line.SetPosition(1, Vector3.zero);
    }


}
