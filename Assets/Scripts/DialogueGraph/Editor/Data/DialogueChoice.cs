using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

[System.Serializable]
public enum Affinities
{
    None,
    Fire,
    Freeze,
    Lying,
    Charismatic,
    Persuasion,
}

[System.Serializable]
public class DialogueChoice
{
    public string choiceText;
    public Affinities affinity = Affinities.None;
}

[CustomPropertyDrawer(typeof(DialogueChoice))]
public class DialogueChoiceDrawer : PropertyDrawer
{
    public override VisualElement CreatePropertyGUI(SerializedProperty property)
    {
        var container = new VisualElement();
        container.style.paddingLeft = 10;
        container.style.paddingRight = 10;
        container.style.paddingTop = 5;
        container.style.paddingBottom = 5;

        // Choice Text field
        var choiceTextProperty = property.FindPropertyRelative("choiceText");
        var choiceTextField = new TextField("Choice Text") { multiline = true };
        choiceTextField.style.whiteSpace = WhiteSpace.Normal;
        choiceTextField.BindProperty(choiceTextProperty);
        container.Add(choiceTextField);

        // Affinity enum field
        var affinityProperty = property.FindPropertyRelative("affinity");
        var affinityField = new PropertyField(affinityProperty, "Affinity");
        container.Add(affinityField);

        return container;
    }
}
