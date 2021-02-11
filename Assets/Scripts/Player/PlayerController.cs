using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        private PlayerMovement _playerMovement;
        
        // Start is called before the first frame update
        private void Start()
        {
            _playerMovement = GetComponent<PlayerMovement>();
        }

        // Update is called once per frame
        private void Update()
        {
            _playerMovement.UpdateMovement();
        }
    }
}
