using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Turning;

public class SetTurnTypeFromPlayerPref : MonoBehaviour
{
    public SnapTurnProvider snapTurn;
    public ContinuousTurnProvider continuousTurn;

    void Start()
    {
        ApplyPlayerPref();
    }

    public void ApplyPlayerPref()
    {
        int value = PlayerPrefs.GetInt("turn", 0); // 默认值为 Snap Turn

        if (value == 0) // Snap
        {
            snapTurn.enabled = true;
            continuousTurn.enabled = false;
        }
        else if (value == 1) // Continuous
        {
            snapTurn.enabled = false;
            continuousTurn.enabled = true;
        }
    }
}
