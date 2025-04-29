using UnityEngine;
using Unity.Netcode;
using UnityEngine.InputSystem;
using StarterAssets;

public class ClientPlayerMove : NetworkBehaviour
{
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private StarterAssetsInputs starterAssetsInputs;
    [SerializeField] private ThirdPersonController thirdPersonController;
    void Awake()
    {
        playerInput.enabled = false;
        starterAssetsInputs.enabled = false;
        thirdPersonController.enabled = false;
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (IsOwner) {
            playerInput.enabled = true;
            starterAssetsInputs.enabled = true;
        }

        if (IsServer)
        {
            thirdPersonController.enabled = true;
        }
    }

    [Rpc(SendTo.Server)]
    private void UpdateInputServerRpc(Vector2 move, Vector2 look, bool jump, bool sprint)
    {
        starterAssetsInputs.MoveInput(move);
        starterAssetsInputs.LookInput(look);
        starterAssetsInputs.JumpInput(jump);
        starterAssetsInputs.SprintInput(sprint);
    }

    private void LateUpdate()
    {
        if (!IsOwner)
            return;
        UpdateInputServerRpc(starterAssetsInputs.move, starterAssetsInputs.look, starterAssetsInputs.jump, starterAssetsInputs.sprint);
    }
}
