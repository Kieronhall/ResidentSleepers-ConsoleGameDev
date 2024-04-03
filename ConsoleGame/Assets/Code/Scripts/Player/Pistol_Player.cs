using UnityEngine;
using TMPro;
using UnityEngine.Rendering;
using Cinemachine;

public class Pistol_Player : MonoBehaviour
{
    public float timeBetweenShooting, timeBetweenShots, reloadTime;
    public int magazineSize, bulletsPerTap, extraMagazine;
    public int bulletsLeft, bulletsShot;

    public bool shooting, readyToShoot, reloading;

    //Damage
    public float damage = 100f;

    //VFX
    public GameObject muzzleFlash;
    public TextMeshProUGUI text;

    public CinemachineImpulseSource impulseSource;

    private void Start()
    {
        bulletsLeft = magazineSize;
        readyToShoot = true;
        impulseSource = GetComponent<CinemachineImpulseSource>();
    }


    private void Update()
    {
        text.SetText("" + bulletsLeft + ""); // + " / " + extraMagazine);
    }

    public void Shoot(Transform hitTransform)
    {
        CameraShake.Instance.CamShake(impulseSource);
        readyToShoot = false;
        bulletsLeft--;
        bulletsShot--;
        Invoke("ResetShot", timeBetweenShooting);


        if (bulletsShot > 0 && bulletsLeft > 0)
        {
            Invoke("Shoot", timeBetweenShots);
        }
    }
    private void ResetShot()
    {
        readyToShoot = true;
    }

    public void AddAmmo(int amount)
    {
        bulletsLeft += amount;
        bulletsLeft = Mathf.Min(bulletsLeft, magazineSize);
    }

}
