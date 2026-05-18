using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class FrameworkCheckboxWindow : EditorWindow {

    [SerializeField] private VisualTreeAsset m_VisualTreeAsset = default;


    //[MenuItem("Code Monkey/Framework Checkbox Window")]
    public static void ShowExample() {
        FrameworkCheckboxWindow wnd = GetWindow<FrameworkCheckboxWindow>();
        wnd.titleContent = new GUIContent("Framework Checkbox Window");
    }

    public void CreateGUI() {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        // Instantiate UXML
        VisualElement labelFromUXML = m_VisualTreeAsset.Instantiate();
        root.Add(labelFromUXML);
    }

}
