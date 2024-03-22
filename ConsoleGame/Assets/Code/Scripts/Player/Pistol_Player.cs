using Unity.VisualScripting;
using UnityEngine;
using TMPro;
public class Pistol_Player : MonoBehaviour
{
    //public int maxAmmo = 6;
    public float timeBetweenShooting, timeBetweenShots, reloadTime;
    public int magazineSize, bulletsPerTap;
    public int bulletsLeft, bulletsShot;

    bool shooting, readyToShoot, reloading;

    //Damage
    public float damage = 50f;

    // Target
    [SerializeField]
    private Target target;

    //VFX
    public GameObject muzzleFlash, Bullets;
    public TextMeshProUGUI text;

    private void Awake()
    {
        bulletsLeft = magazineSize;
        readyToShoot = true;
    }
    private void Update()
    {
        GunInput();
        text.SetText(bulletsLeft + " / " + magazineSize);

        if (bulletsLeft == 0)
        {
            Reload();
        }

    }
    private void GunInput()
    {
        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !reloading)
        {
            Reload();
        }
    }

    public void Shoot(Transform hitTransform)
    {
        readyToShoot = false;
        if (hitTransform != null)
        {
            Target target = hitTransform.GetComponent<Target>();
            if (target != null)
            {
                target.TakeDamage(damage);
            }
        }
        
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
    private void Reload()
    {
        reloading = true;
        Invoke("ReloadFinished", reloadTime);
    }

    private void ReloadFinished()
    {
        bulletsLeft = magazineSize;
        reloading = false;
    }

}
