using UnityEngine;

public interface IPlayerState
{
    void Enter();       // Called ONCE when switching TO this state (Initialization/Setup)
    void Update();      // Called EVERY FRAME while in this state (Math/Inputs)
    void FixedUpdate(); // Called EVERY PHYSICS STEP (if using Rigidbody/Physics checks)
    void Exit();        // Called ONCE when leaving this state (Cleanup)
}
