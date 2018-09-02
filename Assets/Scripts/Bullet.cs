namespace PlayoVR {
    using UnityEngine;

    public class Bullet : MonoBehaviour {
        public AudioClip hitSolidSound;
        public AudioClip hitSoftSound;
        public float power;
        public GameObject explosionPrefab;

        private double timeCreated;
        private bool shouldDestroy;

        void Start() {
            // Add velocity to the bullet
            GetComponent<Rigidbody>().velocity = transform.forward * power;
            timeCreated = Time.time;
            shouldDestroy = false;
        }

        void Update() {
            if (shouldDestroy || Time.time - timeCreated > 1) {
                Destroy(gameObject);
            }
        }

        void OnCollisionEnter(Collision collision) {
            var hit = collision.gameObject;
            GameObject collisionParent = hit.transform.parent.gameObject;

            if (collisionParent.tag == "Player")
            {
                print("Player Hit");
                AudioSource.PlayClipAtPoint(hitSoftSound, transform.position, 1.0f);
                collisionParent.tag = "DeadPlayer";
            }

            // Very stupid check to see if we're hitting a gun
            if (hit.GetComponent<Gun>() != null) {
                return;
            }
            ContactPoint contact = collision.contacts[0];
            Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
            Vector3 pos = contact.point;
            Instantiate(explosionPrefab, pos, rot);

            AudioSource.PlayClipAtPoint(hitSolidSound, transform.position, 1.0f);
            shouldDestroy = true;
        }
    }
}
