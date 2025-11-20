using TMPro;
using UnityEngine;

public class LevelTimerView : MonoBehaviour
{
   [SerializeField] private TextMeshProUGUI levelTimerText;
   [SerializeField] private TextMeshProUGUI levelText;
   
   public void SetTimerText(float timer)
   {
      int minutes = Mathf.FloorToInt(timer / 60f);
      int seconds = Mathf.FloorToInt(timer % 60f);
      levelTimerText.text = ($"{minutes:00}:{seconds:00}");
   }

   public void SetLevelText(int level)
   {
      levelText.text = "Level " + level;
   }
}
