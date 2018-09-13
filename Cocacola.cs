using System;
using System.IO;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using SharedLib;
using Model.Session;

public class GG : MonoBehaviour
{
    GUISkin skin;
    GUIStyle espColor = new GUIStyle();

    Rect baseRect = new Rect(20, 20, 200, 20);
    Rect windowRect = new Rect(200, 100, 240, 90);

    Boolean g_gui = false;
    Boolean g_esp = false;
    Boolean g_chams = false;
    
    internal static Material silBumpedDiffuseMat;
    internal static Material bumpedDiffuseMat;

    public static string AssemblyDirectory
    {
        get
        {
            string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            string path = Uri.UnescapeDataString(uri.Path);
            return Path.GetDirectoryName(path);
        }
    }

    public static object GetPropValue(object src, string propName)
    {
        return src.GetType().GetProperty(propName, BindingFlags.Instance | BindingFlags.NonPublic).GetValue(src, null);
    }

    public static object GetFieldValue(object src, string fieldName)
    {
        return src.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic).GetValue(src);
    }

    public static void SetFieldValue(object src, string fieldName, object value)
    {
        src.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic).SetValue(src, value);
    }

    void Start()
    {
        espColor.normal.textColor = Color.red;

        AssetBundle bundle = AssetBundle.LoadFromFile(Path.Combine(AssemblyDirectory, "shaders"));
        bundle.LoadAllAssets<Shader>();
        silBumpedDiffuseMat = bundle.LoadAsset<Material>("SBD");
        bumpedDiffuseMat = bundle.LoadAsset<Material>("BD");
        bundle = AssetBundle.LoadFromFile(Path.Combine(AssemblyDirectory, "metalgui"));
        this.skin = bundle.LoadAsset<GUISkin>("MetalGUISkin");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Delete))
        {
            g_gui = !g_gui;
        }
    }

    void MainWindow(int windowID)
    {
        GUI.DragWindow(new Rect(0, 0, 10000, 20));

        g_esp = GUI.Toggle(
            new Rect(baseRect.xMin, baseRect.yMin + 10, baseRect.width, baseRect.height),
            g_esp, "Esp");

        g_chams = GUI.Toggle(
            new Rect(baseRect.xMin, baseRect.yMin + 30, baseRect.width, baseRect.height),
            g_chams, "Chams");
    }

    void OnGUI()
    {
        GUI.skin = this.skin;

        if (g_gui)
        {
            windowRect = GUI.Window(5000, windowRect, MainWindow, "encode P.A.S. unknowncheats.me");
        }

        if(g_esp && World.Instance.HasCharacter)
        {
            foreach (MatchStats.Player player in GetPlayers())
            {
                if (!(player.Name == Session.UserProfile.name))
                {
                    if (!IsInMyTeam(player.Team))
                    {
                        RemoteCharacter entity = World.Instance.GetEntity<RemoteCharacter>(player.EntityId);
                        if(!entity.Dead)
                        {
                            String name = player.Name;
                            float health = entity.NetworkCharacter.Health.GetValue();
                            float maxHealth = entity.NetworkCharacter.MaxHealth.GetValue();
                            Vector3 head = entity.GetHeadBone().position;
                            Vector3 headPos = Camera.main.WorldToScreenPoint(head);
                            Vector3 overHead = entity.GetHeadBone().position + Vector3.up * 0.3f;
                            Vector3 overHeadPos = Camera.main.WorldToScreenPoint(overHead);
                            Vector3 feet = entity.BoneMap[RemoteCharacter.ROOT_BONE_NAME].position;
                            Vector3 feetPos = Camera.main.WorldToScreenPoint(feet);
                            float distance = Vector3.Distance(Camera.main.transform.position, head)/2;
                            if (headPos.z > 0f & headPos.y < (float)(Screen.width - 2) && distance < 401)
                            {
                                headPos.y = (float)Screen.height - (headPos.y + 1f);
                                overHeadPos.y = (float)Screen.height - (overHeadPos.y + 1f);
                                feetPos.y = (float)Screen.height - (feetPos.y + 1f);

                                Cocacola.Render.BoxRect(new Rect(headPos.x - 1, headPos.y - 1, 2, 2), Color.red);
                                float height = (feetPos.y - overHeadPos.y);
                                float width = height / 2;
                                Cocacola.Render.DrawBox(new Vector2(overHeadPos.x - (width / 2), overHeadPos.y), new Vector2(width, height), 1.5f, Color.red);

                                if (distance < 151)
                                {
                                    Cocacola.Render.DrawBorderedString(new Vector2(overHeadPos.x, overHeadPos.y - 70), name, Color.red, Color.black, true);
                                    Cocacola.Render.DrawBorderedString(new Vector2(overHeadPos.x, overHeadPos.y - 50), string.Format("[HP: {0:0}/", health) + string.Format("{0:0}]", maxHealth), Color.red, Color.black, true);
                                    Cocacola.Render.DrawBorderedString(new Vector2(overHeadPos.x, overHeadPos.y - 30), string.Format("[DS: {0:0}m]", distance), Color.red, Color.black, true);
                                }
                                if (!entity.gameObject.HasComponent<Cocacola.Chams>())
                                {
                                    entity.gameObject.AddComponent<Cocacola.Chams>();
                                    Cocacola.Chams chams = entity.gameObject.GetComponent<Cocacola.Chams>();
                                    chams.AttachEvent();

                                }
                                if (g_chams)
                                {
                                    Cocacola.Chams chams =  entity.gameObject.GetComponent<Cocacola.Chams>();
                                    if(!chams.chamsEnabled)
                                    {
                                        chams.EnableChams();
                                    }
                                }
                                else
                                {
                                    Cocacola.Chams chams = entity.gameObject.GetComponent<Cocacola.Chams>();
                                    if (chams.chamsEnabled)
                                    {
                                        chams.DisableChams();
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (entity.gameObject.HasComponent<LineRenderer>())
                            {
                                Cocacola.Render.Disable3DLine(entity.gameObject);
                            }
                        }
                    }
                }
            }
        }
    }

    private List<MatchStats.Player> GetPlayers()
    {
        return World.Instance.ClientMatchStats.Value.Players;
    }

    private Boolean IsInMyTeam(byte team)
    {
        return team == World.Instance.Team;
    }
}

public static class hasComponent
{
    public static bool HasComponent<T>(this GameObject flag) where T : Component
    {
        return flag.GetComponent<T>() != null;
    }
}