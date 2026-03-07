using UnityEngine;
using UnityEngine.Rendering;

public class Skidmarks : MonoBehaviour {
    const int MAX_MARKS = 1024;
    const float MARK_WIDTH = 0.25f;
    const float GROUND_OFFSET = 0.02f;
    const float MIN_DISTANCE = 0.25f;
    const float MIN_SQR_DISTANCE = 0.0625f;
    const float MAX_OPACITY = 1f;
    public static Skidmarks Instance;
    [SerializeField] Material skidmarksMaterial;
    Color32 black = Color.black;
    Color32[] colors;
    bool haveSetBounds;
    int markIndex;
    Mesh marksMesh;
    bool meshUpdated;
    MeshFilter mf;
    MeshRenderer mr;
    Vector3[] normals;
    MarkSection[] skidmarks;
    Vector4[] tangents;
    int[] triangles;
    Vector2[] uvs;
    Vector3[] vertices;

    protected void Awake() {
        Instance = this;
        if (transform.position != Vector3.zero) {
            transform.position = Vector3.zero;
            transform.rotation = Quaternion.identity;
        }
    }

    protected void Start() {
        skidmarks = new MarkSection[1024];
        for (var i = 0; i < 1024; i++) {
            skidmarks[i] = new MarkSection();
        }

        mf = GetComponent<MeshFilter>();
        mr = GetComponent<MeshRenderer>();
        if (mr == null) {
            mr = gameObject.AddComponent<MeshRenderer>();
        }

        marksMesh = new Mesh();
        marksMesh.MarkDynamic();
        if (mf == null) {
            mf = gameObject.AddComponent<MeshFilter>();
        }

        mf.sharedMesh = marksMesh;
        vertices = new Vector3[4096];
        normals = new Vector3[4096];
        tangents = new Vector4[4096];
        colors = new Color32[4096];
        uvs = new Vector2[4096];
        triangles = new int[6144];
        mr.shadowCastingMode = ShadowCastingMode.Off;
        mr.receiveShadows = false;
        mr.material = skidmarksMaterial;
        mr.lightProbeUsage = LightProbeUsage.Off;
    }

    protected void LateUpdate() {
        if (!meshUpdated) {
            return;
        }

        meshUpdated = false;
        marksMesh.vertices = vertices;
        marksMesh.normals = normals;
        marksMesh.tangents = tangents;
        marksMesh.triangles = triangles;
        marksMesh.colors32 = colors;
        marksMesh.uv = uvs;
        if (!haveSetBounds) {
            marksMesh.bounds = new Bounds(new Vector3(0f, 0f, 0f), new Vector3(10000f, 10000f, 10000f));
            haveSetBounds = true;
        }

        mf.sharedMesh = marksMesh;
    }

    public int AddSkidMark(Vector3 pos, Vector3 normal, float opacity, int lastIndex) {
        if (opacity > 1f) {
            opacity = 1f;
        }
        else if (opacity < 0f) {
            return -1;
        }

        black.a = (byte)(opacity * 255f);
        return AddSkidMark(pos, normal, black, lastIndex);
    }

    public int AddSkidMark(Vector3 pos, Vector3 normal, Color32 colour, int lastIndex) {
        if (colour.a == 0) {
            return -1;
        }

        MarkSection markSection = null;
        var lhs = Vector3.zero;
        var vector = pos + normal * 0.02f;
        if (lastIndex != -1) {
            markSection = skidmarks[lastIndex];
            lhs = vector - markSection.Pos;
            if (lhs.sqrMagnitude < 0.0625f) {
                return lastIndex;
            }

            if (lhs.sqrMagnitude > 0.625f) {
                lastIndex = -1;
                markSection = null;
            }
        }

        colour.a = (byte)(colour.a * 1f);
        var markSection2 = skidmarks[markIndex];
        markSection2.Pos = vector;
        markSection2.Normal = normal;
        markSection2.Colour = colour;
        markSection2.LastIndex = lastIndex;
        if (markSection != null) {
            var normalized = Vector3.Cross(lhs, normal).normalized;
            markSection2.Posl = markSection2.Pos + normalized * 0.25f * 0.5f;
            markSection2.Posr = markSection2.Pos - normalized * 0.25f * 0.5f;
            markSection2.Tangent = new Vector4(normalized.x, normalized.y, normalized.z, 1f);
            if (markSection.LastIndex == -1) {
                markSection.Tangent = markSection2.Tangent;
                markSection.Posl = markSection2.Pos + normalized * 0.25f * 0.5f;
                markSection.Posr = markSection2.Pos - normalized * 0.25f * 0.5f;
            }
        }

        UpdateSkidmarksMesh();
        var result = markIndex;
        var num = markIndex + 1;
        markIndex = num;
        markIndex = num % 1024;
        return result;
    }

    void UpdateSkidmarksMesh() {
        var markSection = skidmarks[markIndex];
        if (markSection.LastIndex == -1) {
            return;
        }

        var markSection2 = skidmarks[markSection.LastIndex];
        vertices[markIndex * 4] = markSection2.Posl;
        vertices[markIndex * 4 + 1] = markSection2.Posr;
        vertices[markIndex * 4 + 2] = markSection.Posl;
        vertices[markIndex * 4 + 3] = markSection.Posr;
        normals[markIndex * 4] = markSection2.Normal;
        normals[markIndex * 4 + 1] = markSection2.Normal;
        normals[markIndex * 4 + 2] = markSection.Normal;
        normals[markIndex * 4 + 3] = markSection.Normal;
        tangents[markIndex * 4] = markSection2.Tangent;
        tangents[markIndex * 4 + 1] = markSection2.Tangent;
        tangents[markIndex * 4 + 2] = markSection.Tangent;
        tangents[markIndex * 4 + 3] = markSection.Tangent;
        colors[markIndex * 4] = markSection2.Colour;
        colors[markIndex * 4 + 1] = markSection2.Colour;
        colors[markIndex * 4 + 2] = markSection.Colour;
        colors[markIndex * 4 + 3] = markSection.Colour;
        uvs[markIndex * 4] = new Vector2(0f, 0f);
        uvs[markIndex * 4 + 1] = new Vector2(1f, 0f);
        uvs[markIndex * 4 + 2] = new Vector2(0f, 1f);
        uvs[markIndex * 4 + 3] = new Vector2(1f, 1f);
        triangles[markIndex * 6] = markIndex * 4;
        triangles[markIndex * 6 + 2] = markIndex * 4 + 1;
        triangles[markIndex * 6 + 1] = markIndex * 4 + 2;
        triangles[markIndex * 6 + 3] = markIndex * 4 + 2;
        triangles[markIndex * 6 + 5] = markIndex * 4 + 1;
        triangles[markIndex * 6 + 4] = markIndex * 4 + 3;
        meshUpdated = true;
    }

    class MarkSection {
        public Color32 Colour;
        public int LastIndex;
        public Vector3 Normal = Vector3.zero;
        public Vector3 Pos = Vector3.zero;
        public Vector3 Posl = Vector3.zero;
        public Vector3 Posr = Vector3.zero;
        public Vector4 Tangent = Vector4.zero;
    }
}