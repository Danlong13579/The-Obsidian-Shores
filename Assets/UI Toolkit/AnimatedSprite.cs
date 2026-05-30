using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[UxmlElement]
public partial class AnimatedSprite : VisualElement
{
    [UxmlAttribute]
    float animationDuration = 1.0f;

    [UxmlAttribute]
    AnimationCurve curveTime;

    [UxmlAttribute]
    public List<Sprite> frames = new();
}
