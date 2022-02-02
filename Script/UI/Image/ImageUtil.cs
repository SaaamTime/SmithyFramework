using System;
using UnityEngine.UI;
using UnityEngine;

namespace DIY.UI
{
    public class ImageUtil: DIY.Base.SingleClassBase<ImageUtil>
    {
        public Image Bind(Transform _imageTrans, bool _openRay=false) {
            Image image = _imageTrans.GetComponent<Image>();
            image.raycastTarget = _openRay;
            return image;
        }
    }
}
