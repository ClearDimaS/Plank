using Backgammon.Localization;
using TMPro;
using UnityEngine;

namespace Smartplank.Localization
{
    /// <summary>
    /// Localize dropdown component.
    /// </summary>
    [RequireComponent(typeof(TMP_Dropdown))]
    public class LocalizedDropdown : MonoBehaviour
    {
        public string[] LocalizationKeys;
        private LocalizationManager localizationManager;

        public void Construct(LocalizationManager localizationManager)
        {
            this.localizationManager = localizationManager;
        }

        public void Start()
        {
            Localize();
            localizationManager.LocalizationChanged += Localize;
        }

        public void OnDestroy()
        {
            localizationManager.LocalizationChanged -= Localize;
        }

        private void Localize()
        {
            var dropdown = GetComponent<TMP_Dropdown>();

            for (var i = 0; i < LocalizationKeys.Length; i++)
            {
                dropdown.options[i].text = localizationManager.Localize(LocalizationKeys[i]);
            }

            if (dropdown.value < LocalizationKeys.Length)
            {
                dropdown.captionText.text = localizationManager.Localize(LocalizationKeys[dropdown.value]);
            }
        }
    }
}
