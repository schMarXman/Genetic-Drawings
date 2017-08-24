using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(VerticalLayoutGroup)), ExecuteInEditMode]
public class ScrollViewContentWrap : MonoBehaviour
{
    public bool ResizeOnStart = true;

    private VerticalLayoutGroup mVLG;

    private RectTransform mRectT;

    private int mChildCount;

    void Start()
    {
        mVLG = GetComponent<VerticalLayoutGroup>();
        mRectT = GetComponent<RectTransform>();
    }

    void Update()
    {
        if (transform.childCount != mChildCount || ResizeOnStart)
        {
            float height = 0;

            for (int i = 0; i < transform.childCount; i++)
            {
                height += transform.GetChild(i).GetComponent<RectTransform>().sizeDelta.y;
                height += mVLG.spacing;
            }

            mRectT.sizeDelta = new Vector2(mRectT.sizeDelta.x, height);

            mChildCount = transform.childCount;

            ResizeOnStart = false;
        }
    }
}
