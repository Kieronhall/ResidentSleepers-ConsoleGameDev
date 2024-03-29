using UnityEngine;
using TMPro;
using UnityEngine.Rendering;

public class Pistol_Player : MonoBehaviour
{
    public float timeBetweenShooting, timeBetweenShots, reloadTime;
    public int magazineSize, bulletsPerTap, extraMagazine;
    public int bulletsLeft, bulletsShot;

    public bool shooting, readyToShoot, reloading;

    //Damage
    public float damage = 100f;

    // Target
    [SerializeField]
    private Target target;

    //VFX
    public GameObject muzzleFlash;
    public TextMeshProUGUI text;

    private void Start()
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
