using System;
using UnityEngine.UI;
using UnityEngine;
using Image_U3D = UnityEngine.UI.Image;

namespace DIY.UI.Image
{
    public static class ImageUtil
    {
        public static Image_U3D Bind(Transform _imageTrans, bool _openRay=false) {
            Image_U3D image = _imageTrans.GetComponent<Image_U3D>();
            image.raycastTarget = _openRay;
            return image;
        }
    }
}
