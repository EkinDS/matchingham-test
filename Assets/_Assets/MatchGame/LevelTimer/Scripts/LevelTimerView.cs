using TMPro;
using UnityEngine;

public class LevelTimerView : MonoBehaviour
{
   [SerializeField] private TextMeshProUGUI levelTimerText;
   
   public void SetTimerText(float timer)
   {
      int minutes = Mathf.FloorToInt(timer / 60f);
      int seconds = Mathf.FloorToInt(timer % 60f);
      levelTimerText.text = ($"{minutes:00}:{seconds:00}");
   }
}
