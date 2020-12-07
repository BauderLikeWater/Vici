using UnityEngine;

//[ExecuteInEditMode]
public class SpriteOutline : MonoBehaviour
{
    //color of outline
    public Color color = Color.white;

    //sets available range of outlineSize
    [Range(0, 32)]
    public int outlineSize = 1;

    private SpriteRenderer spriteRenderer;

    //enables
    void OnEnable()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        UpdateOutline(true);
    }

    //disables
    void OnDisable()
    {
        UpdateOutline(false);

    }

    void Update()
    {
        UpdateOutline(true);
    }

    void UpdateOutline(bool outline)
    {
        //changes material values to display outline or not - I'm not 100% this'll work as intended since materials are a little funky
        MaterialPropertyBlock mpb = new MaterialPropertyBlock();
        spriteRenderer.GetPropertyBlock(mpb);
        mpb.SetFloat("_Outline", outline ? 1f : 0);
        mpb.SetColor("_OutlineColor", color);
        mpb.SetFloat("_OutlineSize", outlineSize);
        spriteRenderer.SetPropertyBlock(mpb);
    }
}
