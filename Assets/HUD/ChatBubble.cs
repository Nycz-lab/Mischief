using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChatBubble : MonoBehaviour
{
    private SpriteRenderer backgroundSpriteRenderer;
    private TextMeshPro textMeshPro;

    public static Transform Create(Transform parent, Vector3 localPos, string Text)
    {
        Transform chatBubbleTransform = Instantiate(GameAssets.i.pfChatBubble, parent);
        chatBubbleTransform.localPosition = localPos;

        chatBubbleTransform.GetComponent<ChatBubble>().Setup(Text);

        Destroy(chatBubbleTransform.gameObject, 6.0f);

        return chatBubbleTransform;
    }

    private void Awake()
    {
        backgroundSpriteRenderer = transform.Find("Background").GetComponent<SpriteRenderer>();
        textMeshPro = transform.Find("Text").GetComponent<TextMeshPro>();
    }


    private void Setup(string text)
    {
        textMeshPro.SetText(text);
        textMeshPro.ForceMeshUpdate();
        Vector2 textSize = textMeshPro.GetRenderedValues(false);

        Vector2 padding = new Vector2(2f, 4f);
        backgroundSpriteRenderer.size = new Vector2(textSize.y, textSize.x) + padding;
    }
}
