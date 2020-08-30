using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UIElements;


// [ -> https://github.com/Verasl/BoatAttack/blob/master/Packages/com.verasl.water-system/Scripts/Rendering/WaterSystemFeature.cs ]
public class WaterCaustics : ScriptableRendererFeature
{
    class WaterCausticsPass : ScriptableRenderPass
    {
        private const string k_RenderWaterCausticsTag = "Render Water Caustics";

        private ProfilingSampler _waterCausticsProfile = new ProfilingSampler(k_RenderWaterCausticsTag);
        public Material WaterCausticsMaterial;
        private static Mesh _mesh;
        // This method is called before executing the render pass.
        // It can be used to configure render targets and their clear state. Also to create temporary render target textures.
        // When empty this render pass will render to the active camera render target.
        // You should never call CommandBuffer.SetRenderTarget. Instead call <c>ConfigureTarget</c> and <c>ConfigureClear</c>.
        // The render pipeline will ensure target setup and clearing happens in an performance manner.
        public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
        {

        }

        // Here you can implement the rendering logic.
        // Use <c>ScriptableRenderContext</c> to issue drawing commands or execute command buffers
        // https://docs.unity3d.com/ScriptReference/Rendering.ScriptableRenderContext.html
        // You don't have to call ScriptableRenderContext.submit, the render pipeline will call it at specific points in the pipeline.
        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            var cam = renderingData.cameraData.camera;
            if (cam.cameraType == CameraType.Preview) return; // no rendering in preview

            // setup command buffer, draw mesh
            CommandBuffer cmd = CommandBufferPool.Get(k_RenderWaterCausticsTag);

            using (new ProfilingScope(cmd, _waterCausticsProfile))
            {
                if (!_mesh) _mesh = GenerateCausticsMesh(100f);


                // matrix pos for caustics mesh
                Vector3 pos = cam.transform.position;
                pos.y = 0; // <- TODO THIS NEEDS TO BE WATER LEVEL
                Matrix4x4 matrix = Matrix4x4.TRS(pos, Quaternion.identity, Vector3.one); // <- this also needs to read water properties

                cmd.DrawMesh(_mesh, matrix, WaterCausticsMaterial, 0, 0);

                Debug.Log("Drew mesh");
            }

              

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);


        }

        /// Cleanup any allocated resources that were created during the execution of this render pass.
        public override void FrameCleanup(CommandBuffer cmd)
        {
        }
    }


    private WaterCausticsPass _waterCausticsPass;

    public WaterSystemSettings settings = new WaterSystemSettings();

    [HideInInspector] [SerializeField] private Shader _causticShader;
    [HideInInspector] [SerializeField] private Texture2D _causticTexture;

    private Material _causticMaterial;

    private static readonly int SrcBlend = Shader.PropertyToID("_SrcBlend");
    private static readonly int DstBlend = Shader.PropertyToID("_DstBlend");
    private static readonly int Size = Shader.PropertyToID("_Size");
    private static readonly int CausticTexture = Shader.PropertyToID("_CausticMap");

    public override void Create()
    {
        _waterCausticsPass = new WaterCausticsPass();

        // Configures where the render pass should be injected.
        _waterCausticsPass.renderPassEvent = RenderPassEvent.AfterRenderingOpaques;

        _causticShader = _causticShader ? _causticShader : Shader.Find("Caustics_BA_Shader");
        if (_causticShader == null) return;
        
        if (_causticMaterial) DestroyImmediate(_causticMaterial); //destroy if exists, to setup new

        _causticMaterial = CoreUtils.CreateEngineMaterial(_causticShader);
        _causticMaterial.SetFloat("_BlendDistance", settings.causticBlendDistance); // TODO SET VALUE VAR HERE 

        if (_causticTexture == null) Debug.Log("Caustics Texture is missing");

        _causticMaterial.SetTexture("_CausticTexture", _causticTexture);

        _causticMaterial.SetFloat("Size", settings.causticScale); // // TODO SET VALUE VAR HERE 
        _waterCausticsPass.WaterCausticsMaterial = _causticMaterial; // set mat to pass

        Debug.Log("Set mat");
    }

    // Here you can inject one or multiple render passes in the renderer.
    // This method is called when setting up the renderer once per-camera.
    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        renderer.EnqueuePass(_waterCausticsPass);
    }



    /// <summary>
    /// This function Generates a flat quad for use with the caustics pass.
    /// </summary>
    /// <param name="size">The length of the quad.</param>
    /// <returns></returns>
    private static Mesh GenerateCausticsMesh(float size)
    {
        var m = new Mesh();
        size *= 0.5f;

        var verts = new[]
        {
                new Vector3(-size, 0f, -size),
                new Vector3(size, 0f, -size),
                new Vector3(-size, 0f, size),
                new Vector3(size, 0f, size)
            };
        m.vertices = verts;

        var tris = new[]
        {
                0, 2, 1,
                2, 3, 1
            };
        m.triangles = tris;

        return m;
    }

    [System.Serializable]
    public class WaterSystemSettings
    {
        [Header("Caustics Settings")]
        [Range(0.1f, 1f)]
        public float causticScale = 0.25f;

        public float causticBlendDistance = 3f;

        [Header("Advanced Settings")] public DebugMode debug = DebugMode.Disabled;

        public enum DebugMode
        {
            Disabled,
            WaterEffects,
            Caustics
        }
    }
}