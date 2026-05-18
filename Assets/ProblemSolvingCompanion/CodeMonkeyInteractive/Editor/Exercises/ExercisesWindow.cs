using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace CodeMonkey.CSharpCourse.Interactive {

    public class ExercisesWindow : EditorWindow {


        [SerializeField] private VisualTreeAsset visualTreeAsset;
        [SerializeField] private VisualTreeAsset exerciseSingleVisualTreeAsset;
        [SerializeField] private LectureSO defaultLectureSO;

        [SerializeField] private VisualTreeAsset textTemplateVisualTreeAsset;
        [SerializeField] private VisualTreeAsset codeTemplateVisualTreeAsset;
        [SerializeField] private VisualTreeAsset videoTemplateVisualTreeAsset;


        private ExerciseSO exerciseSO;
        private VisualElement exerciseSingleContainerVisualElement;
        private VisualElement overVisualElement;
        private Label titleLabel;
        private Label topMessageLabel;
        private Label completedLabel;
        private ScrollView exerciseListScrollView;
        private Button startStopButton;
        private Button showHintButton;
        private Button showSolutionButton;
        private Button videoWalkthroughButton;
        private Button backButton;
        private Button markCompletedButton;
        private VisualElement frameworkCheckboxTemplateVisualElement;
        private VisualElement liveChatContainer;
        private VisualElement onlyInFullVersionContainer;



        [MenuItem("Code Monkey/Exercises", priority = 103)]
        public static ExercisesWindow ShowWindow() {
            ExercisesWindow exercisesWindow = GetWindow<ExercisesWindow>();
            exercisesWindow.titleContent = new GUIContent("Exercises");
            return exercisesWindow;
        }

        public void OnDestroy() {
            CodeMonkeyInteractiveSO.GetCodeMonkeyInteractiveSO().OnStateChanged -= CodeMonkeyInteractiveSO_OnStateChanged;
        }

        private void CodeMonkeyInteractiveSO_OnStateChanged(object sender, EventArgs e) {
            ShowExercise();
        }

        public void CreateGUI() {
            // Each editor window contains a root VisualElement object
            VisualElement root = rootVisualElement;

            CodeMonkeyInteractiveSO.GetCodeMonkeyInteractiveSO().OnStateChanged -= CodeMonkeyInteractiveSO_OnStateChanged;
            CodeMonkeyInteractiveSO.GetCodeMonkeyInteractiveSO().OnStateChanged += CodeMonkeyInteractiveSO_OnStateChanged;

            // Instantiate UXML
            VisualElement rootVisualTreeAsset = visualTreeAsset.Instantiate();
            rootVisualTreeAsset.style.flexGrow = 1f;
            root.Add(rootVisualTreeAsset);

            liveChatContainer = root.Q<VisualElement>("liveChatContainer");
            liveChatContainer.style.display = DisplayStyle.None;
            liveChatContainer.RegisterCallback((ClickEvent clickEvent) => {
                LiveChat.ShowWindow();
            });

            onlyInFullVersionContainer = root.Q<VisualElement>("onlyInFullVersion");
            onlyInFullVersionContainer.style.display = DisplayStyle.None;

            onlyInFullVersionContainer.Q<Button>("fullVersionButton").RegisterCallback((ClickEvent clickEvent) => {
                Application.OpenURL("https://cmonkey.co/problemsolvingcourse");
            });

            exerciseListScrollView = root.Q<ScrollView>();

            ObjectField objectField = rootVisualElement.Q<ObjectField>("scriptableObjectField");
            if (objectField.value == null) {
                if (CodeMonkeyInteractiveSO.GetLastSelectedLectureSO() != null) {
                    objectField.value = CodeMonkeyInteractiveSO.GetLastSelectedLectureSO();
                } else {
                    objectField.value = defaultLectureSO;
                }
            }
            objectField.RegisterValueChangedCallback((ChangeEvent<UnityEngine.Object> evt) => {
                ShowExerciseList();
            });


            exerciseSingleContainerVisualElement = root.Q<VisualElement>("exerciseSingleContainer");
            overVisualElement = root.Q<VisualElement>("over");
            titleLabel = root.Q<Label>("titleLabel");
            topMessageLabel = root.Q<Label>("topMessageLabel");
            completedLabel = root.Q<Label>("completedLabel");

            startStopButton = exerciseSingleContainerVisualElement.Q<Button>("startStopButton");
            startStopButton.RegisterCallback<ClickEvent>((ClickEvent clickEvent) => {
                EditorApplication.isPlaying = false;
                exerciseSO.StartStopCompleteExercise();
            });

            showHintButton = exerciseSingleContainerVisualElement.Q<Button>("showHintButton");
            showHintButton.RegisterCallback((ClickEvent clickEvent) => {
                ShowHint(exerciseSO);
            });

            showSolutionButton = exerciseSingleContainerVisualElement.Q<Button>("showSolutionButton");
            showSolutionButton.RegisterCallback((ClickEvent clickEvent) => {
                ShowSolution(exerciseSO);
            });

            videoWalkthroughButton = exerciseSingleContainerVisualElement.Q<Button>("videoWalkthroughButton");
            videoWalkthroughButton.RegisterCallback((ClickEvent clickEvent) => {
                Application.OpenURL(exerciseSO.videoWalkthroughUrl);
            });

            Button overCloseButton = exerciseSingleContainerVisualElement.Q<Button>("overCloseButton");
            overCloseButton.RegisterCallback((ClickEvent clickEvent) => {
                overVisualElement.style.display = DisplayStyle.None;
            });

            backButton = exerciseSingleContainerVisualElement.Q<Button>("backButton");
            backButton.RegisterCallback((ClickEvent clickEvent) => {
                ShowExerciseList();
            });

            markCompletedButton = exerciseSingleContainerVisualElement.Q<Button>("markCompletedButton");
            markCompletedButton.RegisterCallback((ClickEvent clickEvent) => {
                ToggleCompleted(exerciseSO);
            });
            
            frameworkCheckboxTemplateVisualElement = exerciseSingleContainerVisualElement.Q<VisualElement>("frameworkCheckboxTemplate");
            frameworkCheckboxTemplateVisualElement.style.display = DisplayStyle.None;

            CodeMonkeyInteractiveSO.ProblemSolvingFrameworkCheckboxState problemSolvingFrameworkCheckboxState =
                CodeMonkeyInteractiveSO.GetProblemSolvingFramworkCheckboxState();

            frameworkCheckboxTemplateVisualElement.Q<Toggle>("step1Toggle").value = problemSolvingFrameworkCheckboxState.step1;
            frameworkCheckboxTemplateVisualElement.Q<Toggle>("step2Toggle").value = problemSolvingFrameworkCheckboxState.step2;
            frameworkCheckboxTemplateVisualElement.Q<Toggle>("step3Toggle").value = problemSolvingFrameworkCheckboxState.step3;

            EventCallback<ChangeEvent<bool>> toggleChangeEvent = (ChangeEvent<bool> changeEvent) => {
                CodeMonkeyInteractiveSO.SetProblemSolvingFrameworkCheckboxState(new CodeMonkeyInteractiveSO.ProblemSolvingFrameworkCheckboxState {
                    step1 = frameworkCheckboxTemplateVisualElement.Q<Toggle>("step1Toggle").value,
                    step2 = frameworkCheckboxTemplateVisualElement.Q<Toggle>("step2Toggle").value,
                    step3 = frameworkCheckboxTemplateVisualElement.Q<Toggle>("step3Toggle").value,
                });
            };
            frameworkCheckboxTemplateVisualElement.Q<Toggle>("step1Toggle").RegisterCallback(toggleChangeEvent);
            frameworkCheckboxTemplateVisualElement.Q<Toggle>("step2Toggle").RegisterCallback(toggleChangeEvent);
            frameworkCheckboxTemplateVisualElement.Q<Toggle>("step3Toggle").RegisterCallback(toggleChangeEvent);

            ShowExerciseList();
        }

        public void SetLectureSO(LectureSO lectureSO) {
            ObjectField objectField = rootVisualElement.Q<ObjectField>("scriptableObjectField");
            objectField.value = lectureSO;
        }

        private void ShowExerciseList() {
            ObjectField objectField = rootVisualElement.Q<ObjectField>("scriptableObjectField");
            if (objectField.value != null) {
                LectureSO lectureSO = objectField.value as LectureSO;
                ShowExerciseList(lectureSO);
            }

            CheckLiveChatOnline();
        }

        private void CheckLiveChatOnline() {
            if (!CodeMonkeyInteractiveSO.HasInternetConnection()) {
                return;
            }
            CodeMonkeyInteractiveSO.ContactWebsiteLiveChatGetCodeMonkeyState((bool isOnline, string offlineReason) => {
                // Success!
                if (isOnline) {
                    liveChatContainer.style.display = DisplayStyle.Flex;
                } else {
                    liveChatContainer.style.display = DisplayStyle.None;
                }
            },
                (string other) => {
                    Debug.Log("OTHER: " + other);
                },
                (string error) => {
                    Debug.Log("ERROR: " + error);
                }
            );
        }

        private void ShowExerciseList(LectureSO lectureSO) {
            exerciseListScrollView.style.display = DisplayStyle.Flex;
            exerciseSingleContainerVisualElement.style.display = DisplayStyle.None;
            overVisualElement.style.display = DisplayStyle.None;

            // Remove old elements
            MainWindow.DestroyChildren(exerciseListScrollView);

            titleLabel.text = "EXERCISES (" + lectureSO.lectureTitle + ")";

            // Spawn exercises
            foreach (ExerciseSO exerciseSO in lectureSO.exerciseListSO.exerciseSOList) {
                VisualElement exerciseSingle = exerciseSingleVisualTreeAsset.Instantiate();
                exerciseSingle.Q<Button>("button").text = "<b>" + exerciseSO.exerciseNumber + ":</b> " + exerciseSO.exerciseTitle;
                exerciseSingle.RegisterCallback<ClickEvent>((ClickEvent clickEvent) => {
                    ShowExercise(exerciseSO);
                });

                exerciseSingle.Q<VisualElement>("done").style.display =
                    (CodeMonkeyInteractiveSO.GetState(exerciseSO) == CodeMonkeyInteractiveSO.State.Completed) ?
                        DisplayStyle.Flex : DisplayStyle.None;

                exerciseListScrollView.Add(exerciseSingle);
            }

            HideTopMessage();

            ExerciseSO alreadyStartedExerciseSO = CodeMonkeyInteractiveSO.GetActiveExerciseSO();
            if (alreadyStartedExerciseSO != null) {
                // Has an exercise currently active
                ShowExercise(alreadyStartedExerciseSO);
            }
        }

        private void ShowExercise() {
            if (exerciseSO != null) {
                ShowExercise(exerciseSO);
            }

            CheckLiveChatOnline();
        }

        private void ShowExercise(ExerciseSO exerciseSO) {
            this.exerciseSO = exerciseSO;

            exerciseListScrollView.style.display = DisplayStyle.None;
            exerciseSingleContainerVisualElement.style.display = DisplayStyle.Flex;
            overVisualElement.style.display = DisplayStyle.None;

            if (!string.IsNullOrEmpty(exerciseSO.videoWalkthroughUrl)) {
                videoWalkthroughButton.style.display = DisplayStyle.Flex;
            } else {
                videoWalkthroughButton.style.display = DisplayStyle.None;
            }

            Label exerciseNameLabel = exerciseSingleContainerVisualElement.Q<Label>("exerciseNameLabel");
            Label exerciseTextLabel = exerciseSingleContainerVisualElement.Q<Label>("exerciseTextLabel");

            exerciseNameLabel.text = "<b>" + exerciseSO.exerciseNumber + ":</b> " + exerciseSO.exerciseTitle;
            exerciseTextLabel.text = exerciseSO.exerciseText;

            if (exerciseSO.IsExerciseActive()) {
                // This exercise is already active
                frameworkCheckboxTemplateVisualElement.style.display = DisplayStyle.Flex;

                CodeMonkeyInteractiveSO.ProblemSolvingFrameworkCheckboxState problemSolvingFrameworkCheckboxState =
                    CodeMonkeyInteractiveSO.GetProblemSolvingFramworkCheckboxState();

                frameworkCheckboxTemplateVisualElement.Q<Toggle>("step1Toggle").value = problemSolvingFrameworkCheckboxState.step1;
                frameworkCheckboxTemplateVisualElement.Q<Toggle>("step2Toggle").value = problemSolvingFrameworkCheckboxState.step2;
                frameworkCheckboxTemplateVisualElement.Q<Toggle>("step3Toggle").value = problemSolvingFrameworkCheckboxState.step3;


                startStopButton.text = "COMPLETE EXERCISE";
                showHintButton.style.display = DisplayStyle.Flex;
                showSolutionButton.style.display = DisplayStyle.Flex;
                completedLabel.style.display = DisplayStyle.None;

                backButton.text = "Complete Exercise to go Back";

                if (CodeMonkeyInteractiveSO.GetState(exerciseSO) == CodeMonkeyInteractiveSO.State.Completed) {
                    // Exercise Active but already completed
                    startStopButton.text = "COMPLETE EXERCISE";
                    string[] congratsMessageArray = new string[] { 
                        "CONGRATS!",
                        "GOOD JOB!",
                        "GOOD WORK!",
                        "AWESOME!"
                    };
                }
            } else {
                // Exercise not active
                frameworkCheckboxTemplateVisualElement.style.display = DisplayStyle.None;

                startStopButton.text = "START EXERCISE";
                showHintButton.style.display = DisplayStyle.None;
                showSolutionButton.style.display = DisplayStyle.None;
                completedLabel.style.display = DisplayStyle.None;
                backButton.text = "Back";
            }

            if (exerciseSO.IsCompleted()) {
                markCompletedButton.text = "[Debug] Mark as UnCompleted";
            } else {
                markCompletedButton.text = "[Debug] Mark as Completed";
            }

            switch (CodeMonkeyInteractiveSO.GetState(exerciseSO)) {
                case CodeMonkeyInteractiveSO.State.None:
                    HideTopMessage();
                    break;
                case CodeMonkeyInteractiveSO.State.Started:
                    ShowTopMessage("Exercise in progress...", Color.yellow);
                    break;
                case CodeMonkeyInteractiveSO.State.Completed:
                    ShowTopMessage("Exercise Completed!", Color.green);
                    break;
            }


            if (exerciseSO.isFree) {
                onlyInFullVersionContainer.style.display = DisplayStyle.None;
                startStopButton.style.display = DisplayStyle.Flex;
            } else {
                onlyInFullVersionContainer.style.display = DisplayStyle.Flex;
                startStopButton.style.display = DisplayStyle.None;
            }
        }

        private void ShowHint(ExerciseSO exerciseSO) {
            overVisualElement.style.display = DisplayStyle.Flex;

            Label overTitleLabel = exerciseSingleContainerVisualElement.Q<Label>("overTitleLabel");
            overTitleLabel.text = "HINT";
            Label overTextLabel = exerciseSingleContainerVisualElement.Q<Label>("overTextLabel");
            overTextLabel.text = exerciseSO.hintText;
        }

        private void ShowSolution(ExerciseSO exerciseSO) {
            overVisualElement.style.display = DisplayStyle.Flex;

            Label overTitleLabel = exerciseSingleContainerVisualElement.Q<Label>("overTitleLabel");
            overTitleLabel.text = "SOLUTION";
            Label overTextLabel = exerciseSingleContainerVisualElement.Q<Label>("overTextLabel");
            overTextLabel.text = exerciseSO.solutionText;
        }

        private void ToggleCompleted(ExerciseSO exerciseSO) {
            if (exerciseSO.IsCompleted()) {
                exerciseSO.SetUnCompleted();
                ShowExerciseList();
            } else {
                exerciseSO.SetCompleted();
                ShowExerciseList();
            }
        }

        private void HideTopMessage() {
            topMessageLabel.style.display = DisplayStyle.None;
        }

        private void ShowTopMessage(string topMessage, Color color) {
            topMessageLabel.style.display = DisplayStyle.Flex;
            topMessageLabel.text = topMessage;

            topMessageLabel.style.backgroundColor = color;
        }

    }

}