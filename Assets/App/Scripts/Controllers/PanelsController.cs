using UnityEngine;
using Zenject;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using RSG;

namespace Smartplank.Scripts.Presenters.Panels
{
    public class PanelsController : MonoBehaviour
    {
        [SerializeField] private string resourcesPath;
        [SerializeField] private Transform panelsParent;

        private List<PanelBase> panelsStack = new List<PanelBase>();
        private Dictionary<string, PanelBase> panelsDict = new Dictionary<string, PanelBase>();

        private Queue<OpenPanelOptions> panelsToOpen = new Queue<OpenPanelOptions>();

        private void Start()
        {
            OpenPanel<MainMenuPanel>();
        }

        private void Update()
        {
            while (panelsToOpen.Count > 0)
            {
                string name = panelsToOpen.Dequeue().type.Name;
                Debug.Log($"opening panel: {name}");
                if (!panelsDict.ContainsKey(name))
                    panelsDict.Add(name, CreatePanel(name));
                var panel = panelsDict[name];
                OpenPanel(panel);
            }
        }

        public Promise<T> OpenPanel<T>() where T : class, IPanel
        {
            return StackingShow<T>(new PopupStackItem()
            {
                type = typeof(T)
            }) ;
        }

        public Promise<T> OpenPanel<T, D>(D initData) where T : class, IPanel<D>
        {
            return StackingShow<T>(new PopupStackItem<T, D>(initData));
        }

        private Promise<T> StackingShow<T>(PopupStackItem stackItem) where T : IPanel
        {

            var result = new Promise<T>();

            var popup = GetOrCreatePopupInstance(stackItem.type);

            stackItem.initAction?.Invoke(popup);
            OpenPanel(popup);

            return result;
        }

        private PanelBase GetOrCreatePopupInstance(Type type)
        {
            string name = type.Name;
            Debug.Log($"opening panel: {name}");
            if (!panelsDict.ContainsKey(name))
                panelsDict.Add(name, CreatePanel(name));
            var panel = panelsDict[name];
            return panel;
        }

        public void CloseLastPanel()
        {
            Debug.Log($"going back, stack size: { panelsStack.Count }");
            if (panelsStack.Count <= 1)
                return;

            ClosePanel(panelsStack[panelsStack.Count - 1]);
                ShowPanel(panelsStack[panelsStack.Count - 1]);
        }

        public void OpenMainMenu()
        {
            for (int i = 0; i < panelsStack.Count; i++)
            {
                CloseLastPanel(false);
            }

            OpenPanel<MainMenuPanel>();
        }

        public void CloseLastPanel(bool showNew)
        {
            Debug.Log($"going back, stack size: { panelsStack.Count }");
            if (panelsStack.Count <= 1)
                return;

            ClosePanel(panelsStack[panelsStack.Count - 1]);
            if(showNew)
             ShowPanel(panelsStack[panelsStack.Count - 1]);
        }

        private void SwitchPanel(Type type)
        {
            var panelOptions = new OpenPanelOptions()
            {
                type = type,
            };
            panelsToOpen.Enqueue(panelOptions);
        }

        private PanelBase CreatePanel(string name)
        {
            var path = $"{resourcesPath}/{name}";
            var prefab = Resources.Load(path);
            var instantiated = Instantiate(prefab, panelsParent) as GameObject;
            instantiated.SetActive(true);
            var panel = instantiated.GetComponent<PanelBase>();
            return panel;
        }

        private void OpenPanel(PanelBase panel)
        {
            if (panelsStack.Count > 0)
            {
                bool isTheSamePanel = ReferenceEquals(panel, panelsStack[panelsStack.Count - 1]);
                if (isTheSamePanel)
                {
                    ShowPanel(panel);
                    return;
                }
                var panelToHide = panelsStack[panelsStack.Count - 1];
                HidePanel(panelToHide);
            }

            panelsStack.Add(panel);
            ShowPanel(panel);
        }

        private void ClosePanel(PanelBase panel)
        {
            int index = GetPanelIndex(panel);
            panelsStack.RemoveAt(index);
            HidePanel(panel);
        }

        private void ShowPanel(PanelBase panel)
        {
            panel.gameObject.SetActive(true);
            panel.Show();
        }

        private void HidePanel(PanelBase panel)
        {
            panel.Hide().Done(() => panel.gameObject.SetActive(false));
        }

        public void ClearPanels()
        {
            foreach (var item in panelsStack)
            {
                item.gameObject.SetActive(false);
            }
            panelsStack.Clear();
        }

        private int GetPanelIndex(PanelBase panel)
        {
            int panelIndex = -1;
            for (int i = 0; i < panelsStack.Count; i++)
            {
                if (ReferenceEquals(panelsStack[i], panel))
                {
                    panelIndex = i;
                    break;
                }
            }
            return panelIndex;
        }

        private struct OpenPanelOptions
        {
            public Type type;
        }
        private class PopupStackItem
        {
            public Type type { get;set; }
            public Action<IPanel> initAction { get; protected set; }
        }

        private class PopupStackItem<T, D> : PopupStackItem where T : class, IPanel<D>
        {
            public PopupStackItem(D initData)
            {
                type = typeof(T);
                initAction = bw =>
                {
                    (bw as T).Init(initData);
                    (bw as T).SetUp(initData);
                };
            }
        }
    }
}