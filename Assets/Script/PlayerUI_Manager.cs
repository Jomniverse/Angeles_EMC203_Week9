using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUI_Manager : MonoBehaviour
{
    public Image healthFill;
    public TMP_Text healthText;

    public void UpdateHealth(float currentHP, float maxHP)
    {
        healthFill.fillAmount = currentHP / maxHP;

        healthText.text =
            Mathf.CeilToInt(currentHP) + " / " +
            Mathf.CeilToInt(maxHP) + " HP";
    }
}
