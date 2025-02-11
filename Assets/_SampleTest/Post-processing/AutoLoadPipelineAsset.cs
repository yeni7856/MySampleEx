using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace MySampleEx
{
    [ExecuteAlways]
    public class AutoLoadPipelineAsset : MonoBehaviour
    {
      public UniversalRenderPipelineAsset pipelineAsset;

        private void OnEnable()
        {
            if(pipelineAsset != null)
            {
                GraphicsSettings.defaultRenderPipeline = pipelineAsset;
                QualitySettings.renderPipeline = pipelineAsset;
            }
        }
    }
}
