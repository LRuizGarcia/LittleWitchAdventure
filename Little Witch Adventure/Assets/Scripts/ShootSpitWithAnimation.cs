using UnityEngine;

public class ShootSpitWithAnimation : MonoBehaviour
{

    //After Toxic Frog does spit animation, this method is called
    //for synchronization of projectile and animation
    public void ShootSpit()
    {
        Spit spit = GetComponentInParent<Spit>();
        spit.ShootSpit();
    }

}
