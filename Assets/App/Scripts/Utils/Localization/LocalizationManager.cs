using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Smartplank.Scripts.Configs;
using UnityEngine;
using Zenject;

namespace Smartplank.Localization
{
    /// <summary>
    /// Localization manager.
    /// </summary>
    public class LocalizationManager 
    {
        [Serializable]
        public struct LanguagePreference
        {
            public AvailableLanguages language;
        }

        private string localizationExtraPath = "";

        /// <summary>
        /// Fired when localization changed.
        /// </summary>
        public event Action LocalizationChanged = () => { };

        public readonly Dictionary<AvailableLanguages, Dictionary<string, string>> Dictionary =
            new Dictionary<AvailableLanguages, Dictionary<string, string>>();

        private AvailableLanguages _language = AvailableLanguages.Russian;

        private LocalCacheManager cacheManager;

        private Dictionary<string, AvailableLanguages> availableLanguages = new Dictionary<string, AvailableLanguages>()
        {
            {"Russian", AvailableLanguages.Russian},
            {"English", AvailableLanguages.English},
        };

        private Dictionary<AvailableLanguages, string> availableLanguagesShortCodes = new Dictionary<AvailableLanguages, string>()
        {
            {AvailableLanguages.Russian, "ru"},
            {AvailableLanguages.English, "en"},
        };

        private Dictionary<string, AvailableLanguages> availableShortCodeLanguages = new Dictionary<string, AvailableLanguages>()
        {
            {"ru", AvailableLanguages.Russian},
            {"en", AvailableLanguages.English},
        };

        public AvailableLanguages CurrentLanguage
        {
            get { return _language; }
            set
            {
                _language = value;
                LocalizationChanged();
            }
        }

        private LocalCacheManager _localCacheManager;
        private LocalizationTexts _localizationTexts;

        public static LocalizationManager Instance;

        [Inject]
        public LocalizationManager(LocalCacheManager localCacheManager, LocalizationTexts localizationTexts)
        {
            _localCacheManager = localCacheManager;
            _localizationTexts = localizationTexts;
            Init();
            Instance = this;
        }


        public void Init()
        {
            cacheManager = _localCacheManager;
            Debug.Log($"Setting language in localisation manager");
            if (_localCacheManager.FileExists<LanguagePreference>() == true)
            {
                CurrentLanguage = _localCacheManager.Load<LanguagePreference>().language;
            }
            else
            {
                switch (Application.systemLanguage)
                {
                    case (SystemLanguage.Russian):
                        CurrentLanguage = AvailableLanguages.Russian;
                        break;
                    default:
                        CurrentLanguage = AvailableLanguages.Russian;
                        break;
                }
            }

            var textAssets = _localizationTexts.TextAssets;

            foreach (var textAsset in textAssets)
            {
                var text = ReplaceMarkers(textAsset.text).Replace("\"\"", "[quotes]");
                MatchCollection matches = Regex.Matches(text, "\"[\\s\\S]+?\"");

                foreach (System.Text.RegularExpressions.Match match in matches)
                {
                    text = text.Replace(match.Value,
                        match.Value.Replace("\"", null).Replace(",", "[comma]").Replace("\n", "[newline]"));
                }

                var lines = text.Split(new[] {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries);
                List<string> languageNames = lines[0].Split(',').Select(i => i.Trim()).ToList();
                List<AvailableLanguages> languages = new List<AvailableLanguages>();
                foreach (string langName in languageNames)
                {
                    if (langName == "Key")
                    {
                        continue;
                    }

                    if (availableLanguages.ContainsKey(langName) == false)
                    {
                        Debug.LogError($"l10n: {availableLanguages.Keys} doesn't cointain {langName}");
                    }
                    else
                    {
                        languages.Add(availableLanguages[langName]);
                    }
                }

                for (var i = 0; i < languages.Count; i++)
                {
                    if (!Dictionary.ContainsKey(languages[i]))
                    {
                        Dictionary.Add(languages[i], new Dictionary<string, string>());
                    }
                }

                for (var i = 1; i < lines.Length; i++)
                {
                    var columns = lines[i].Split(',').Select(j => j.Trim()).Select(j =>
                        j.Replace("[comma]", ",").Replace("[newline]", "\n").Replace("[quotes]", "\"")).ToList();
                    var key = columns[0];

                    if (key == "") continue;

                    for (int j = 0; j < languages.Count; j++)
                    {
                        AvailableLanguages language = languages[j];
                        Dictionary[language].Add(key, columns[j + 1]);
                    }
                }
                Debug.Log($"file: {textAsset.name} is ok!");
            }
        }

        public string GetLanguageShortCode(AvailableLanguages currentLanguage)
        {
            var langCode = "";

            if (availableLanguagesShortCodes.ContainsKey(currentLanguage))
                langCode = availableLanguagesShortCodes[currentLanguage];
            else
                Debug.LogError($"{currentLanguage} short code for server not found!");

            return langCode;
        }

        public void SelectLanguage(string languageShortCode)
        {
            AvailableLanguages currentLanguage = AvailableLanguages.Russian;
            if (availableShortCodeLanguages.ContainsKey(languageShortCode))
                currentLanguage = availableShortCodeLanguages[languageShortCode];
            else
                Debug.LogError($"{languageShortCode} short code for server not found!");

            SelectLanguage(currentLanguage);
        }

        public void SelectLanguage(AvailableLanguages newSelectedLanguage)
        {
            if (CurrentLanguage == newSelectedLanguage)
            {
                return;
            }

            CurrentLanguage = newSelectedLanguage;

            //Save selected language preference
            LanguagePreference languagePreference = new LanguagePreference() {language = CurrentLanguage};

            cacheManager.Save<LanguagePreference>(languagePreference, true);
        }

        public bool HasKey(string localizationKey)
        {
            return Dictionary[CurrentLanguage].ContainsKey(localizationKey);
        }

        /// <summary>
        /// Get localized value by localization key.
        /// </summary>
        public string Localize(string localizationKey)
        {
            if (Dictionary.Count == 0)
            {
                Debug.LogError("trying to localize without initialized LocalizationManager!");
                return null;
            }

            if (localizationKey == null)
                return null;

            if (Dictionary[CurrentLanguage].ContainsKey(localizationKey))
            {
                return Dictionary[CurrentLanguage][localizationKey];
            }
            
            if (Dictionary[AvailableLanguages.Russian].ContainsKey(localizationKey))
            {
                return Dictionary[AvailableLanguages.Russian][localizationKey];
            }

            Debug.LogWarning($"Translation not found: {localizationKey} ({CurrentLanguage}).");
            return null;

        }

        /// <summary>
        /// Get localized value by localization key.
        /// </summary>
        public string Localize(string localizationKey, params object[] args)
        {
            var pattern = Localize(localizationKey);

            return string.Format(pattern, args);
        }

        public string GetChars()
        {
            var asset = Resources.Load<TextAsset>("Localization/Common");

            if (asset == null) return "";

            var chars = new List<char>();

            foreach (var s in
                "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZАаБбВвГгДдЕеЁёЖжЗзИиЙйКкЛлМмНнОоПпСсТтУуФфХхЦцЧчШшЩщЪъЫыЬьЭэЮюЯя!\"#$%&'()*+,-./:;<=>?@[\\]^_`{|}~Ξ ")
                if (!chars.Contains(s))
                    chars.Add(s);
            foreach (var s in asset.text)
            {
                if (!chars.Contains(char.ToLower(s))) chars.Add(char.ToLower(s));
                if (!chars.Contains(char.ToUpper(s))) chars.Add(char.ToUpper(s));
            }

            chars.Sort();

            var text = new System.Text.StringBuilder();

            foreach (var s in chars) text.Append(s);

            return text.ToString();
        }

        private string ReplaceMarkers(string text)
        {
            return text.Replace("[Newline]", "\n");
        }

        private SystemLanguage AvailableLanguageToSystem(AvailableLanguages language)
        {
            switch (language)
            {
                case AvailableLanguages.Russian:
                    return SystemLanguage.Russian;
                default:
                    Debug.LogWarning("Maybe you wanna handle current localization.");
                    break;
            }

            return SystemLanguage.Russian;
        }

    }
}