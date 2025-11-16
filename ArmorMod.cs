using UnityEngine;
using UnityEngine.Animations;
using TheForest.Utils;
using SonsSdk;
using Sons.Cutscenes;
using HarmonyLib;
using RedLoader;
using Endnight.Utilities;
using Sons.Animation.PlayerControl;
using Sons.Ai.Vail;
using Sons.Ai.Vail.StimuliTypes;
using UnityEngine.SceneManagement;
using SUI;
using Sons.Wearable.Clothing;
using Sons.Wearable.Armour.Clothing;
using Sons.Wearable.Race;
using Sons.Multiplayer.Client;
using Sons.Multiplayer;
using TMPro;
using Sons.Multiplayer.Gui;
using UnityEngine.Playables;
using Il2CppInterop.Runtime.InteropTypes.Arrays;

// Commented code is either elevated logging or nonfunctional code left for later

namespace ArmorMod;

public class ArmorMod : SonsMod
{
    public ArmorMod()
    {
        HarmonyPatchAll = true;
    }

    protected override void OnInitializeMod()
    {

        Config.Init();
    }

    protected override void OnSdkInitialized()
    {
        SettingsRegistry.CreateSettings(this, null, typeof(Config));
    }

    public GameObject[] PlayerNets;
    public static Material HelmetMaterial;
    public static GameObject Hair;
    public static Transform Spine2Ref;
    public static Vector3 TranslationOffset = new Vector3(0.0f, 0.0736f, 0.0859f);
    public static Vector3 RotationOffset = new Vector3(60f, 0f, 0f);
    public static Vector3 TranslationRest = new Vector3(0f, -0.45f, 0.15f);
    public static Transform Jaw;
    public static Transform Hemlet;
    public static SkinnedMeshRenderer HemletRenderer;
    public static VailActor Robby;
    public static SkinnedMeshRenderer RobbyRenderer;
    public static List<Transform> HeadBones;
    public static List<Transform> HelmetBones;
    public static Transform PlayerHead;
    public static GameObject Backpack;
    protected override void OnGameStart()
    {
        UnityEngine.SceneManagement.Scene SonsStorySpots = SceneManager.GetSceneByName("SonsStorySpots");
        if (SonsStorySpots.IsValid() == false)
        {
            RLog.Msg("Failed to find SonsStorySpots Scene!");
            return;
        }
        Array SonsStorySpotsObjects = SonsStorySpots.GetRootGameObjects();

        foreach (GameObject PossibleStickPickup in SonsStorySpotsObjects)
        {
            if (PossibleStickPickup.name == "HelmetStickPickup")
            {
                RLog.Msg("Found helmet stick pickup object");
                MeshRenderer HelmetRenderer = PossibleStickPickup.GetComponentInChildren<MeshRenderer>();
                if (HelmetRenderer)
                {
                    RLog.Msg("Found mesh renderer");
                    HelmetMaterial = HelmetRenderer.sharedMaterial;
                }
                else
                {
                    RLog.Msg("Failed to find mesh renderer");
                }
                if (HelmetMaterial)
                {
                    RLog.Msg("Found Helmet Material");
                }
                else
                {
                    RLog.Msg("Failed to find Helmet Material");
                }
            }
            Robby = ActorTools.GetRobby();
            RobbyRenderer = Robby.transform.Find("VisualRoot").transform.Find("RobbyRig").transform.Find("GEO").transform.Find("TacticalArmorHeadHelmetMesh").gameObject.GetComponent<SkinnedMeshRenderer>();
            if (Config.enableRobbyHelmet.Value == true)
            {
                RobbyRenderer.sharedMaterial = HelmetMaterial;
                RobbyRenderer.gameObject.SetActive(true);
                Robby.transform.Find("VisualRoot/RobbyRig/GEO/RobbyHair").gameObject.SetActive(false);
                RLog.Msg("Robby helmet enabled");
            }
        }
    }
    [HarmonyPatch(typeof(PlayerLocation), "OnEnable")]
    private static class RemoteSetupPatch
    {
        private static void Postfix(PlayerLocation __instance)
        {
            var Hips = __instance.transform.Find("PlayerAnimator")?.transform.Find("Root")?.transform.Find("Hips");
            var Root = __instance.transform.Find("PlayerAnimator")?.transform.Find("Root");
            var OldSkin = __instance.transform.Find("PlayerAnimator")?.transform.Find("Root")?.transform.Find("OldSkin");
            var TactiBodyArmor = OldSkin.transform.Find("tacti_body_armor1").GetComponent<SkinnedMeshRenderer>();
            List<Transform> TactiBodyArmorBones = TactiBodyArmor.bones.ToList();
            TactiBodyArmorBones[0] = Hips;
            Il2CppReferenceArray<Transform> NewTactiBodyArmorBones = TactiBodyArmorBones.ToArray();
            TactiBodyArmor.bones = NewTactiBodyArmorBones;
            if (__instance.gameObject != LocalPlayer.GameObject)
            {
                var PlayerNameVar = __instance.transform.Find("PlayerName");
                var NameTagModel = PlayerNameVar.FindChild("NameTagModel");
                var NameTagConstraint = NameTagModel.GetComponent<ParentConstraint>();
                ConstraintSource NameTagConstraintSource = NameTagConstraint.GetSource(1);
                NameTagConstraint.RemoveSourceInternal(0);
                if (NameTagConstraintSource == null || NameTagConstraint.sourceCount == 0)
                {
                    NameTagConstraint.SetSource(0, new ConstraintSource());
                    NameTagConstraintSource = NameTagConstraint.GetSource(0);
                    NameTagConstraintSource.sourceTransform = Spine2Ref;
                    NameTagConstraintSource.m_SourceTransform = Spine2Ref;
                    NameTagConstraintSource.weight = 1;
                    NameTagConstraintSource.m_Weight = 1;
                }
                    NameTagConstraint.SetRotationOffset(0, RotationOffset);
                    NameTagConstraint.SetTranslationOffset(0, TranslationOffset);
            }

            //Hips.transform.localScale = new Vector3(1, 1, 1);
            //Root.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);

            OldSkin.gameObject.SetActive(true);
            OldSkin.transform.Find("LeftArmMesh1")?.gameObject.SetActive(false);
            OldSkin.transform.Find("RightArmTattooMesh1")?.gameObject.SetActive(false);
            OldSkin.transform.Find("tacti_boots1")?.gameObject.SetActive(false);
            OldSkin.transform.Find("tacti_eyes1")?.gameObject.SetActive(false);
            OldSkin.transform.Find("tacti_head1")?.gameObject.SetActive(false);
            OldSkin.transform.Find("tacti_hemlet1")?.gameObject.SetActive(true);
            OldSkin.transform.Find("tacti_jacket1")?.gameObject.SetActive(false);
            OldSkin.transform.Find("tacti_mask1")?.gameObject.SetActive(false);
            OldSkin.transform.Find("tacti_pants1")?.gameObject.SetActive(false);
            OldSkin.transform.Find("tacti_sunglasses1")?.gameObject.SetActive(false);
            RLog.Msg("Your PlayerLocation patch method worked, thank you GLaD0S, i love you <3");
            Spine2Ref = Hips.transform.Find("Spine").transform.Find("Spine1").transform.Find("Spine2").GetComponent<Transform>();

        }
    }

    [HarmonyPatch(typeof(PlayerLocation), "OnEnable")]
    private static class HelmetPatch
    {
        private static void Postfix(PlayerLocation __instance)
        {
            var OldSkin = __instance.transform.Find("PlayerAnimator")?.transform.Find("Root")?.transform.Find("OldSkin");
            Hemlet = __instance.transform.Find("PlayerAnimator")?.transform.Find("Root")?.transform.Find("OldSkin").transform.Find("tacti_hemlet1");
            HemletRenderer = Hemlet.GetComponent<SkinnedMeshRenderer>();
            if (Robby.gameObject != null)
            {
                RLog.Msg("Found Robby");
            }
            else
            {
                RLog.Msg("Robby is " + Robby.ToString());
            }   

            if (Config.enableHelmet.Value == true && __instance.gameObject != LocalPlayer.GameObject)
            {
                HemletRenderer.enabled = true;
                HemletRenderer.castShadows = true;
            }
            if (Config.cutsceneHelmet.Value == true)
            {
                HemletRenderer.sharedMesh = RobbyRenderer.sharedMesh;
                HemletRenderer.sharedMaterial = HelmetMaterial;
            }
        }
    }

    [HarmonyPatch(typeof(CoopPlayerRemoteSetup), "UpdatePlayerView")]
    private static class SystemsPatches
    {
        private static void Postfix(CoopPlayerRemoteSetup __instance)
        {
            RLog.Msg("UpdatePlayerView called and patched");
            var ClothingSystem = __instance.transform.Find("ClothingSystem");
            List<Transform> RaceChildren = __instance.transform.Find("RaceSystem")?.GetChildren();
            foreach (Transform RaceChild in RaceChildren) { 
            if (RaceChild.name.Contains("Head"))
                {
                    PlayerHead = RaceChild;
                }
            else
                {
                    RLog.Msg("Unable to find player head, attempting RaceSystem get");
                    PlayerHead = __instance.transform.Find("RaceSystem").GetComponent<PlayerRaceSystem>().GetHead().transform;
                }
            }
            HeadBones = PlayerHead?.GetComponent<SkinnedMeshRenderer>().bones.ToList();
            HelmetBones = HemletRenderer.bones.ToList();
            if (ClothingSystem != null)
            {
                RLog.Msg("Found clothing system!");
            }
            else
            {
                RLog.Msg("Failed to find clothing system");

            }
            List<Transform> EquippedClothing = ClothingSystem?.gameObject.GetChildren();

            if (EquippedClothing != null)
            {

                foreach (Transform PossibleBackpacks in EquippedClothing)
                {
                    if (PossibleBackpacks.gameObject.name == ("Backpack"))
                    {
                        Backpack = PossibleBackpacks.gameObject;
                    }
                    else
                    {
                        Backpack = null;
                    }

                    if (Backpack != null)
                    {
                        RLog.Msg("Found backpack!");
                    }
                    else if (Backpack == null)
                    {
                        RLog.Msg("Failed to find backpack");

                    }
                }

                if (Config.hideBackpack.Value == true && __instance.gameObject != LocalPlayer.GameObject && Backpack.gameObject.name == "Backpack")
                {
                    Backpack.gameObject.SetActive(false);
                    RLog.Msg("Hid player backpack!");
                }
                else
                {
                    RLog.Msg("Failed to hide backpack!");

                }
            }

            if (Config.cutsceneHelmet.Value == true)
            {
                Jaw = __instance.transform.Find("PlayerAnimator/Root/Hips/Spine/Spine1/Spine2/Neck/Neck1/Head/Head1/Jaw1_Caucasian");

                HelmetBones[1] = Jaw.transform;
                Il2CppReferenceArray<Transform> NewHelmetBones = HelmetBones.ToArray();
                HemletRenderer.bones = NewHelmetBones;
            }    
                GameObject Head;

                foreach (Transform PossibleHead in RaceChildren)
                {
                    if (PossibleHead.gameObject.name.Contains("Head") && Config.enableHelmet.Value == true && __instance.gameObject != LocalPlayer.GameObject)
                    {
                        Head = PossibleHead.gameObject;
                        RLog.Msg("Head found!");
                        Head.transform.Find("Hair")?.gameObject.SetActive(false);
                    }
                    else
                    {
                        RLog.Msg("Head not found, or helmet disabled.");

                    }
                }
                if (__instance.gameObject != LocalPlayer.GameObject)
                {
                    var PlayerNameVar = __instance.transform.Find("PlayerName");
                    var NameTagModel = PlayerNameVar.FindChild("NameTagModel");
                    var NameTagConstraint = NameTagModel.GetComponent<ParentConstraint>();
                    //var NameTagConstraintSource = NameTagConstraint.GetSource(1);
                    /* if (NameTagConstraint.sourceCount == 2)
                    {
                        NameTagConstraint.RemoveSource(1);
                    }
                    //NameTagConstraint.translationAtRest = TranslationRest;
                    //NameTagConstraint.rotationAtRest = RotationOffset;
                    //NameTagConstraint.RemoveSource(0);
                    //NameTagConstraint.SetSource(0, new ConstraintSource());
                    /*if (NameTagConstraint.sourceCount == 0)
                    {
                        NameTagConstraint.SetSource(0, new ConstraintSource());  
                    } */
                    /*    NameTagConstraintSource.sourceTransform = Spine2Ref;
                        NameTagConstraintSource.weight = 1;
                        NameTagConstraintSource.m_SourceTransform = Spine2Ref;
                        NameTagConstraintSource.m_Weight = 1;   */
                    //NameTagConstraint.SetSource(0, NameTagConstraintSource);
                    //NameTagConstraint.GetSource(0).sourceTransform = Spine2Ref;
                    //NameTagConstraint.GetSource(0).m_SourceTransform = Spine2Ref;

                    //NameTagConstraint.SetRotationOffset(1, RotationOffset);
                    //NameTagConstraint.SetTranslationOffset(1, TranslationOffset);
                    var PlayerNameLink = __instance.GetComponent<CoopPlayerRemoteSetup>();
                    var RemotePlayerUsername = PlayerNameLink._cachedPlayerName;
                    var RemotePlayerUsername2 = PlayerNameVar.GetComponent<PlayerNameUiLink>()._playerName;
                    NameTagModel.gameObject.SetActive(true);
                    var PlayerNameTagLabelText = NameTagModel.transform.Find("NameTagCanvas").transform.Find("NameTagLabel").GetComponent<TextMeshProUGUI>();
                    PlayerNameTagLabelText.SetText(RemotePlayerUsername);
                    if (PlayerNameTagLabelText.text == null)
                    {
                        RLog.Msg("Name set 1 failed");
                        PlayerNameTagLabelText.SetText(RemotePlayerUsername2);
                    }
                    if (PlayerNameTagLabelText.text != null)
                    {
                        PlayerNameTagLabelText.GetComponent<TextMeshProUGUI>().ForceMeshUpdate(true, true);
                    }
                    else return;
                }
            
        }

    }
}
    
  
