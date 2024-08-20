using PlayerController.Abilities;
using UnityEngine;

public class UIInstantiateTest : MonoBehaviour
{
    public FireUltimateAttack fireUltimatePrefab;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Instantiate(fireUltimatePrefab, Vector3.zero, Quaternion.identity);
        }
    }
}
