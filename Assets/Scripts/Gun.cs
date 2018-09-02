namespace PlayoVR {
    using UnityEngine;
    using VRTK;

    public class Gun : Photon.MonoBehaviour {
        public GameObject bulletPrefab;
        public Transform bulletSpawn;
        public AudioClip fireGunSound;
        public Animation fireAnimation;
        public GameObject particle;
        public GameObject bolt;

        public float OneHandStrayFactor;
        public float TwoHandStrayFactor;

        private bool fired;
        private float strayFactor;



        // Use this for initialization
        void Awake() {
            GetComponent<VRTK_InteractableObject>().InteractableObjectUsed += new InteractableObjectEventHandler(DoFireGun);
            
        }

        void DoFireGun(object sender, InteractableObjectEventArgs e) {
            fired = true;
        }

        // Update is called once per frame
        void Update() {
            // Handle firing
            if (fired) {
                CmdFire();
                fired = false;
            }
            VRTK_InteractableObject interactableObject = GetComponent<VRTK_InteractableObject>();
            if (interactableObject.GetSecondaryGrabbingObject() && strayFactor != TwoHandStrayFactor)
            {
                print("SecondHand Grabbed");
                strayFactor = TwoHandStrayFactor;
            }
            else if(!interactableObject.GetSecondaryGrabbingObject() && strayFactor != OneHandStrayFactor)
            {
                print("OneHand Grabbed");
                strayFactor = OneHandStrayFactor;
            }
        }

        void CmdFire() {
            // Now create the bullet and play sound/animation locally and on all other clients
            photonView.RPC("NetFire", PhotonTargets.All, bulletSpawn.position, bulletSpawn.rotation);
        }

        [PunRPC]
        void NetFire(Vector3 position, Quaternion rotation) {
            var randomNumberX = Random.Range(-strayFactor, strayFactor);
            var randomNumberY = Random.Range(-strayFactor, strayFactor);
            var randomNumberZ = Random.Range(-strayFactor, strayFactor);
            // Create the Bullet from the Bullet Prefab
            var bullet = Instantiate(
                bulletPrefab,
                position,
                rotation);
            bullet.transform.Rotate(randomNumberX, randomNumberY, randomNumberZ);
            // create Particle
            GameObject smokeInstance = Instantiate(particle, position, rotation);
            Destroy(smokeInstance, 0.7f);
            // Play sound of gun shooting
            AudioSource.PlayClipAtPoint(fireGunSound, transform.position, 1.0f);
            // Play animation of gun shooting
            fireAnimation.Play();
        }
    }
}
