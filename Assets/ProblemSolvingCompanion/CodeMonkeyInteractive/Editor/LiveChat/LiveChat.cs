using CodeMonkey.Utils;
using System;
using UnityEditor;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.UIElements;

namespace CodeMonkey.CSharpCourse.Interactive {

    public class LiveChat : EditorWindow {


        [SerializeField] private VisualTreeAsset visualTreeAsset = default;



        private TextField inputTextField;
        private Label onlineLabel;
        private Label chatLabel;
        private FunctionTimer.FunctionTimerObject inputFocusFunctionTimerObject;
        private long lastTimestamp;
        private double nextGetMessagesTime;



        [MenuItem("Code Monkey/Live Chat", priority = 200)]
        public static void ShowWindow() {
            LiveChat liveChat = GetWindow<LiveChat>();
            liveChat.titleContent = new GUIContent("Code Monkey Live Chat");
        }

        public void CreateGUI() {
            // Each editor window contains a root VisualElement object
            VisualElement root = rootVisualElement;

            // Instantiate UXML
            VisualElement rootVisualTreeAsset = visualTreeAsset.Instantiate();
            root.Add(rootVisualTreeAsset);

            CodeMonkeyInteractiveSO codeMonkeyInteractiveSO = CodeMonkeyInteractiveSO.GetCodeMonkeyInteractiveSO();

            VisualElement loginContainer = rootVisualElement.Q<VisualElement>("loginContainer");
            VisualElement chatContainer = rootVisualElement.Q<VisualElement>("chatContainer");
            VisualElement fullVersionContainer = rootVisualTreeAsset.Q<VisualElement>("fullVersionContainer");

            loginContainer.style.display = DisplayStyle.None;
            chatContainer.style.display = DisplayStyle.None;
            fullVersionContainer.style.display = DisplayStyle.Flex;

            fullVersionContainer.Q<Button>("fullVersionButton").RegisterCallback((ClickEvent clickEvent) => {
                Application.OpenURL("https://cmonkey.co/problemsolvingcourse");
            });

            onlineLabel = rootVisualTreeAsset.Q<Label>("onlineLabel");
            onlineLabel.text = "---";
        }

        private void Update() {
            if (inputFocusFunctionTimerObject != null) {
                if (inputFocusFunctionTimerObject.Update()) {
                    inputFocusFunctionTimerObject = null;
                }
            }

            if (EditorApplication.timeSinceStartup > nextGetMessagesTime) {
                double nextGetMessagesTimeAdd = 3f;
                nextGetMessagesTime = EditorApplication.timeSinceStartup + nextGetMessagesTimeAdd;

                RefreshOnlineState();
            }
        }

        private void RefreshOnlineState() {
            if (!CodeMonkeyInteractiveSO.HasInternetConnection()) {
                return;
            }

            CodeMonkeyInteractiveSO.ContactWebsiteLiveChatGetCodeMonkeyState((bool isOnline, string offlineReason) => {
                // Success!
                if (isOnline) {
                    onlineLabel.text = "I'm <u>ONLINE</u> right now!\r\nNeed help with anything? Ask me!";
                    onlineLabel.style.color = Color.green;
                } else {
                    onlineLabel.text = "I'm <u>OFFLINE</u> right now! Be back when I can!\nAlso remember how you can post questions in the Lecture comments.\n(" + offlineReason + ")";
                    onlineLabel.style.color = Color.grey;
                }
            },
                (string other) => {
                    Debug.Log("OTHER: " + other);
                    AddText("OTHER: " + other);
                },
                (string error) => {
                    Debug.Log("ERROR: " + error);
                    AddText("ERROR: " + error);
                }
            );
        }


        private void AddText(string text) {
            chatLabel.text = text + "\n" + chatLabel.text;
        }

    }

}