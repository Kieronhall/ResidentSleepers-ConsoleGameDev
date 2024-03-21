using UnityEngine;
using TMPro;
public class Pistol_Player : MonoBehaviour
{
    public float timeBetweenShooting, timeBetweenShots, reloadTime;
    public int magazineSize, bulletsPerTap, extraMagazine;
    public int bulletsLeft, bulletsShot;

    public bool shooting, readyToShoot, reloading;

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
        text.SetText("" + bulletsLeft + ""); // + " / " + extraMagazine);
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

}
