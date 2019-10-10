using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MC.UI
{
    [ExecuteInEditMode]
    public class HPBar : MonoBehaviour
    {
        public Image BackGround;
        public Image CurrentFillGround;
        public Image LaterFillGround;

        public bool backHitMove = false;

        [Range(0, 1)] public float currentValue;
        [Range(0, 1)] public float laterValue;

        public float sizeX;
        public float sizeY;
        Vector2 size;

        public void Update()
        {
            ReSize();
            CurrentValue();
            LaterValue();
        }

        private void ReSize()
        {
            size = new Vector2(sizeX, sizeY);
            BackGround.rectTransform.sizeDelta = size;
            CurrentFillGround.rectTransform.sizeDelta = size;
            LaterFillGround.rectTransform.sizeDelta = size;
        }

        private void CurrentValue()
        {
            Vector3 deltaPos = new Vector3(CurrentFillGround.rectTransform.sizeDelta.x * 0.9f, 0, 0)
                * (-1 + currentValue);

            CurrentFillGround.transform.localPosition = deltaPos;
        }

        private void LaterValue()
        {
            Vector3 deltaPos = new Vector3(LaterFillGround.rectTransform.sizeDelta.x * 0.9f, 0, 0)
                * (-1 + laterValue);

            LaterFillGround.transform.localPosition = deltaPos;
        }

        public void HitBackFun()
        {
            backHitMove = true;
        }

    }
}