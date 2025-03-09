using System.Globalization;
using Managers;
using TMPro;
using UnityEngine;
using Weapons;

namespace UI
{
    public class StatsViewer : MonoBehaviour
    {
        [SerializeField] private Obstacle[] obstacle;
        [SerializeField] private TextMeshProUGUI nameTmp;
        [SerializeField] private TextMeshProUGUI descriptionTmp;
        [SerializeField] private TextMeshProUGUI amountTmp;
        [SerializeField] private TextMeshProUGUI hpTmp;
        [SerializeField] private TextMeshProUGUI damageTmp;
        [SerializeField] private PlacementManager manager;

        /// <summary>
        /// Shows the selected weapon's stats in a Canvas.
        /// </summary>
        /// <param name="weapon"></param>
        public void OnSetStats(int weapon)
        {
            var amount = manager.obstacles[weapon].maxCount - manager.obstacles[weapon].currentCount -1;
            if (amount == -1)
            {
                amount = 0;
            }
            nameTmp.text = "Name: " + obstacle[weapon].name;
            descriptionTmp.text = "Description: " + obstacle[weapon].description;
            amountTmp.text = amount + " Left";
            hpTmp.text ="HP: " + obstacle[weapon].health.ToString(CultureInfo.InvariantCulture);
            damageTmp.text ="Damage: " + obstacle[weapon].damage;
        }
    }
}
