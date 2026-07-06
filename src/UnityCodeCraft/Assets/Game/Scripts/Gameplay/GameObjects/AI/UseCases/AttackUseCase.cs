using SampleGame;
using UnityEngine;

namespace Game
{
    public static class AttackUseCase
    {
        public static bool Attack(GameObject character, GameObject enemy, float attackDistance, float deltaTime)
        {
            Vector3 delta = enemy.transform.position - character.transform.position;
            delta.y = 0;

            if (delta.sqrMagnitude > attackDistance * attackDistance)
                return false;

            RotateTransformComponent rotateComponent = character.GetComponent<RotateTransformComponent>();
            rotateComponent.RotateTowards(enemy, deltaTime);

            character.GetComponent<AttackComponent>().Attack(enemy);

            return true;
        }
    }
}