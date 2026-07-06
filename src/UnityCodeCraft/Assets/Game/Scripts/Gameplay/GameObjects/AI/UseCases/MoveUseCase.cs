using SampleGame;
using UnityEngine;

namespace Game
{
    public static class MoveUseCase
    {
        public static bool Move(GameObject character, Vector3 targetPosition, float stoppingDistance, float deltaTime)
        {
            Vector3 delta = targetPosition - character.transform.position;
            delta.y = 0;

            if (delta.sqrMagnitude < stoppingDistance * stoppingDistance)
                return false;

            character.GetComponent<MoveComponent>().MoveStep(delta.normalized, deltaTime);
            return true;
        }
    }
}