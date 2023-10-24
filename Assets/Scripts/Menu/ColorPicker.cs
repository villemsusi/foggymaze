using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ColorPicker : MonoBehaviour
{
    RectTransform Rect;
    Texture2D ColorPickerTexture;

    public Image PickerPreview;

    private void Start()
    {
        Rect = GetComponent<RectTransform>();
        ColorPickerTexture = GetComponent<Image>().mainTexture as Texture2D;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && RectTransformUtility.RectangleContainsScreenPoint(Rect, Input.mousePosition))
        {
            PickColor();
        }

    }

    private void PickColor()
    {
        Vector2 delta;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(Rect, Input.mousePosition, null, out delta);

        float width = Rect.rect.width;
        float height = Rect.rect.height;
        delta += new Vector2(width * .5f, height * .5f);

        float x = Mathf.Clamp(delta.x / width, 0f, 1f);
        float y = Mathf.Clamp(delta.y / height, 0f, 1f);

        int texX = Mathf.RoundToInt(x * ColorPickerTexture.width);
        int texY = Mathf.RoundToInt(y * ColorPickerTexture.height);

        Color color = ColorPickerTexture.GetPixel(texX, texY);

        
        PickerPreview.color = color;

        if (transform.name == "ProjColorPicker")
            Events.SetProjectileColor(color);
        else if (transform.name == "AuraColorPicker")
        {
            Events.SetAuraColor(color);
            Debug.Log("AURA");
        }
            
    }
}
