using Backgammon.Localization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Smartplank.Localization
{
    /// <summary>
    /// Localize text component.
    /// </summary>
    //[RequireComponent(typeof(TextMeshProUGUI))]
    public class LocalizedText : MonoBehaviour
    {
        public string LocalizationKey;

        private LocalizationManager localizationManager;

        [Inject]
        private void Construct(LocalizationManager localizationManager)
        {
            this.localizationManager = localizationManager;
        }

        private void Awake()
        {
            localizationManager = LocalizationManager.Instance;
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

        public void ForceLocalize()
        {
            Localize();
        }

        private void Localize()
        {
            var text = GetComponent<TMP_Text>();
            if (text != null)
                text.text = localizationManager.Localize(LocalizationKey);
            else
            {
                var textUI = GetComponent<TextMeshProUGUI>();
                if(textUI != null)
                    textUI.text = localizationManager.Localize(LocalizationKey);
                Debug.Log($"{localizationManager.Localize(LocalizationKey)}");
            }
        }
    }
}
