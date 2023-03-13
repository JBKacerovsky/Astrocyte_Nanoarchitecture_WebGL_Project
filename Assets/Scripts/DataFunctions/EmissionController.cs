using UnityEngine;

namespace JBK.AstrocyteViewer.DataFunctions
{
    public class EmissionController : MonoBehaviour
    {
        public bool emission;
        private Material _material;
        private bool _fire;

        private void Start()
        {
            _material = GetComponent<Renderer>().material;
            // Emission needs to be turned on by default so the emission component of the shader is compiled
            SetEmission(false); 
        }

        private void Update()
        {
            ConditionalResetAllEmmiters();
            ConditionalExpandShooting();
        }

        public void ToggleEmission()
        {
            emission = !emission;
            _material.SetInt("_emissionToggle", emission ? 1 : 0);
        }

        public void SetEmission(bool targetState)
        {
            emission = targetState;
            _material.SetInt("_emissionToggle", emission ? 1 : 0);
        }

        public void SetShootability(int shootablity)
        {
            if (shootablity == 1)
            {
                gameObject.layer = LayerMask.NameToLayer("shootable");
                transform.tag = "ShootableMesh";
                return;
            }

            gameObject.layer = LayerMask.NameToLayer("UnShootable");
            transform.tag = "UnShootableMesh";
        }

        private void ConditionalResetAllEmmiters()
        {
            if (Input.GetKey(KeyCode.A) & Input.GetKey(KeyCode.E))
            {
                SetEmission(false);
            }
        }

        private void ConditionalExpandShooting()
        {
            // the double conditional KeyDown and KeyUp seems a bit silly
            // but it ensures that the selection doesn't keep spreading
            // with only the first condition the next selected objects will also sometimes fire and select the next etc
            // presumably if their update is called slightly later in the frame they will already have emission == true and the key press is still active
            // this way only objects that had emission on when the key is pressed will pass their id to KeepShooting once the key is released (and since the emissions are only set after the key release, the next elements cannot have emission == true and keyDown)
            if (emission & Input.GetKeyDown(KeyCode.N))
            {
                _fire = true;
            }

            if (_fire & Input.GetKeyUp(KeyCode.N))
            {
                ShootManager.ShootInDirectConnections(transform.name);
                _fire = false;
            }
        }
    }
}
