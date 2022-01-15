using UnityEngine;

namespace XAsset
{
    public sealed class InitializeVersions : Operation
    {
        public ManifestAsset asset;
        public string file { get; private set; }

        public override void Start()
        {
            base.Start();
            var settings = Resources.Load<PlayerSettings>(nameof(PlayerSettings));
            if (settings == null) settings = ScriptableObject.CreateInstance<PlayerSettings>();
            file = nameof(Manifest);
            Versions.builtinAssets.AddRange(settings.assets);
            Versions.OfflineMode = settings.offlineMode;
            asset = ManifestAsset.LoadAsync(file, true);
        }


        protected override void Update()
        {
            if (status == OperationStatus.Processing)
            {
                progress = asset.progress;
                if (!asset.isDone) return;

                if (string.IsNullOrEmpty(asset.error))
                {
                    asset.Override();
                    asset.Release();
                }

                Finish(asset.error);
            }
        }
    }
}