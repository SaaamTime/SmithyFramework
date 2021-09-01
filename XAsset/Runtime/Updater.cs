using System;
using UnityEngine;

namespace XAsset
{
    public sealed class Updater : MonoBehaviour
    {
        public static Action updated;
        public float maxUpdateTimeSlice = 0.01f;
        public float time { get; private set; }
        public bool busy => Time.realtimeSinceStartup - time >= maxUpdateTimeSlice;

        public static Updater Instance { get; private set; }

        private void Update()
        {
            time = Time.realtimeSinceStartup;
            Loadable.UpdateAll();
            Operation.UpdateAll();
            Download.UpdateAll();
            if (updated != null) updated.Invoke();
        }

        private void OnDestroy()
        {
            Download.ClearAllDownloads();
        }

        public static void InitializeOnLoad(Transform _targetTrans)
        {
             Instance = _targetTrans.gameObject.AddComponent<Updater>();
        }
    }
}