using UnityEngine;
using UnityEngine.Animations;
using TheForest.Utils;
using SonsSdk;
using Sons.Cutscenes;
using HarmonyLib;
using RedLoader;
using Endnight.Utilities;
using Sons.Ai.Vail;
using SUI;
using Sons.Wearable.Race;
using Sons.Multiplayer.Client;
using TMPro;
using Sons.Multiplayer;
using Sons.Multiplayer.Gui;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Sons.Multiplayer.Utilities;
using Endnight.Animation;
using static RedLoader.RLog;
using Sons.Wearable.Clothing;
using Sons.Animation.PlayerControl;
using Sons.Inventory;
using Sons.Items;
using Il2CppSystem;
using UnityEngine.AddressableAssets;
using Sons.Utils;
using Il2CppInterop.Runtime;


// Commented code is either elevated logging or nonfunctional code left for later

namespace ArmorMod;

public class ArmorMod : SonsMod
{
    public ArmorMod()
    {
        //OnUpdateCallback = OnUpdateMethod;
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

    public static bool ModInitialized = false;
    public GameObject[] PlayerNets;
    public static Material HelmetMaterial;
    public static Vector3 TranslationOffset = new Vector3(0.0f, 0.0736f, 0.0859f);
    public static Vector3 RotationOffset = new Vector3(60f, 0f, 0f);
    public static Vector3 TranslationRest = new Vector3(0f, -0.45f, 0.15f);
    public static VailActor Robby;
    public static SkinnedMeshRenderer RobbyHelmetRenderer;
    public static GameObject RobbyGloves;
    public static GameObject RobbyJacketRenderer;
    public static OpeningCutscene OpeningCutsceneRef;
    public static Transform TreeCrashTacticoolRef;
    public static Transform AssetManager;
    public static Transform PlayerTactiRef;
    public static Transform TacticalSoldierRef;
    public static SkinnedMeshRenderer TacticoolBalaclavaRef;
    public static SkinnedMeshRenderer HelmetRendererRef;
    public static bool RobbyInitialized = false;


    public static void ApplyGloves(Transform Object)
    {
        var OldSkin = Object.transform.FindDeepChild("OldSkin");
        var RaceSystem = Object.Find("RaceSystem").GetComponent<PlayerRaceSystem>();
        var LeftHand = RaceSystem.GetLeftArm().GetComponent<SkinnedMeshRenderer>();
        var RightHand = RaceSystem.GetRightArm().GetComponent<SkinnedMeshRenderer>();
        var Spine2Ref = Object.FindDeepChild("Spine2");
        var GloveRef = TacticalSoldierRef.FindDeepChild("gloves").GetComponent<SkinnedMeshRenderer>();
        var LeftHandBones = LeftHand.bones.ToList();
        var RightHandBones = RightHand.bones.ToList();
        var GloveBones = new List<Transform>(88);
        GloveBones.Add(Spine2Ref.FindDeepChild("LeftForeArmTwistNew3"));
        GloveBones.Add(Spine2Ref.FindDeepChild("LeftForeArmTwistNew4"));
        GloveBones.Add(Spine2Ref.FindDeepChild("LeftHand"));
        GloveBones.Add(Spine2Ref.FindDeepChild("LeftHand1"));
        GloveBones.Add(Spine2Ref.FindDeepChild("LeftHandThumb1"));
        GloveBones.Add(Spine2Ref.FindDeepChild("LeftHandThumb2"));
        GloveBones.Add(Spine2Ref.FindDeepChild("LeftHandThumb3"));
        GloveBones.Add(Spine2Ref.FindDeepChild("LeftHandThumb4"));
        GloveBones.Add(Spine2Ref.FindDeepChild("LeftHandThumbHelper2"));
        GloveBones.Add(Spine2Ref.FindDeepChild("LeftHandWeapon"));
        GloveBones.Add(Spine2Ref.FindDeepChild("LeftHandIndex1"));
        GloveBones.Add(Spine2Ref.FindDeepChild("LeftHandIndex2"));
        GloveBones.Add(Spine2Ref.FindDeepChild("LeftHandIndex3"));
        GloveBones.Add(Spine2Ref.FindDeepChild("LeftHandIndex4"));
        GloveBones.Add(Spine2Ref.FindDeepChild("LeftHandIndexHelper6"));
        GloveBones.Add(Spine2Ref.FindDeepChild("LeftHandIndexHelper5"));
        GloveBones.Add(Spine2Ref.FindDeepChild("LeftHandIndexHelper4"));
        GloveBones.Add(Spine2Ref.FindDeepChild("LeftHandMiddle1"));
        GloveBones.Add(Spine2Ref.FindDeepChild("LeftHandMiddle2"));
        GloveBones.Add(Spine2Ref.FindDeepChild("LeftHandMiddle3"));
        GloveBones.Add(Spine2Ref.FindDeepChild("LeftHandMiddle4"));
        GloveBones.Add(Spine2Ref.FindDeepChild("LeftHandMiddleHelper6"));
        GloveBones.Add(Spine2Ref.FindDeepChild("LeftHandMiddleHelper5"));
        GloveBones.Add(Spine2Ref.FindDeepChild("LeftHandMiddleHelper4"));
        GloveBones.Add(Spine2Ref.FindDeepChild("LeftHandRing1"));
        GloveBones.Add(Spine2Ref.FindDeepChild("LeftHandRing2"));
        GloveBones.Add(Spine2Ref.FindDeepChild("LeftHandRing3"));
        GloveBones.Add(Spine2Ref.FindDeepChild("LeftHandRing4"));
        GloveBones.Add(Spine2Ref.FindDeepChild("LeftHandRingHelper6"));
        GloveBones.Add(Spine2Ref.FindDeepChild("LeftHandRingHelper5"));
        GloveBones.Add(Spine2Ref.FindDeepChild("LeftHandRingHelper4"));
        GloveBones.Add(Spine2Ref.FindDeepChild("LeftHandPinky1"));
        GloveBones.Add(Spine2Ref.FindDeepChild("LeftHandPinky2"));
        GloveBones.Add(Spine2Ref.FindDeepChild("LeftHandPinky3"));
        GloveBones.Add(Spine2Ref.FindDeepChild("LeftHandPinky4"));
        GloveBones.Add(Spine2Ref.FindDeepChild("LeftHandPinkyHelper6"));
        GloveBones.Add(Spine2Ref.FindDeepChild("LeftHandPinkyHelper5"));
        GloveBones.Add(Spine2Ref.FindDeepChild("LeftHandPinkyHelper4"));
        GloveBones.Add(Spine2Ref.FindDeepChild("LeftHandIndexPalm1"));
        GloveBones.Add(Spine2Ref.FindDeepChild("LeftHandMiddlePalm1"));
        GloveBones.Add(Spine2Ref.FindDeepChild("LeftHandRingPalm1"));
        GloveBones.Add(Spine2Ref.FindDeepChild("LeftHandPinkyPalm1"));
        GloveBones.Add(Spine2Ref.FindDeepChild("LeftHandHelper1"));
        GloveBones.Add(Spine2Ref.FindDeepChild("RightForeArmTwistNew2"));
        GloveBones.Add(Spine2Ref.FindDeepChild("RightForeArmTwistNew3"));
        GloveBones.Add(Spine2Ref.FindDeepChild("RightForeArmTwistNew4"));
        GloveBones.Add(Spine2Ref.FindDeepChild("RightHand"));
        GloveBones.Add(Spine2Ref.FindDeepChild("RightHand1"));
        GloveBones.Add(Spine2Ref.FindDeepChild("RightHandThumb1"));
        GloveBones.Add(Spine2Ref.FindDeepChild("RightHandThumb2"));
        GloveBones.Add(Spine2Ref.FindDeepChild("RightHandThumb3"));
        GloveBones.Add(Spine2Ref.FindDeepChild("RightHandThumb4"));
        GloveBones.Add(Spine2Ref.FindDeepChild("RightHandThumbHelper2"));
        GloveBones.Add(Spine2Ref.FindDeepChild("RightHandWeapon"));
        GloveBones.Add(Spine2Ref.FindDeepChild("RightHandIndex1"));
        GloveBones.Add(Spine2Ref.FindDeepChild("RightHandIndex2"));
        GloveBones.Add(Spine2Ref.FindDeepChild("RightHandIndex3"));
        GloveBones.Add(Spine2Ref.FindDeepChild("RightHandIndex4"));
        GloveBones.Add(Spine2Ref.FindDeepChild("RightHandIndexHelper6"));
        GloveBones.Add(Spine2Ref.FindDeepChild("RightHandIndexHelper5"));
        GloveBones.Add(Spine2Ref.FindDeepChild("RightHandIndexHelper4"));
        GloveBones.Add(Spine2Ref.FindDeepChild("RightHandMiddle1"));
        GloveBones.Add(Spine2Ref.FindDeepChild("RightHandMiddle2"));
        GloveBones.Add(Spine2Ref.FindDeepChild("RightHandMiddle3"));
        GloveBones.Add(Spine2Ref.FindDeepChild("RightHandMiddle4"));
        GloveBones.Add(Spine2Ref.FindDeepChild("RightHandMiddleHelper6"));
        GloveBones.Add(Spine2Ref.FindDeepChild("RightHandMiddleHelper5"));
        GloveBones.Add(Spine2Ref.FindDeepChild("RightHandMiddleHelper4"));
        GloveBones.Add(Spine2Ref.FindDeepChild("RightHandRing1"));
        GloveBones.Add(Spine2Ref.FindDeepChild("RightHandRing2"));
        GloveBones.Add(Spine2Ref.FindDeepChild("RightHandRing3"));
        GloveBones.Add(Spine2Ref.FindDeepChild("RightHandRing4"));
        GloveBones.Add(Spine2Ref.FindDeepChild("RightHandRingHelper6"));
        GloveBones.Add(Spine2Ref.FindDeepChild("RightHandRingHelper5"));
        GloveBones.Add(Spine2Ref.FindDeepChild("RightHandRingHelper4"));
        GloveBones.Add(Spine2Ref.FindDeepChild("RightHandPinky1"));
        GloveBones.Add(Spine2Ref.FindDeepChild("RightHandPinky2"));
        GloveBones.Add(Spine2Ref.FindDeepChild("RightHandPinky3"));
        GloveBones.Add(Spine2Ref.FindDeepChild("RightHandPinky4"));
        GloveBones.Add(Spine2Ref.FindDeepChild("RightHandPinkyHelper6"));
        GloveBones.Add(Spine2Ref.FindDeepChild("RightHandPinkyHelper5"));
        GloveBones.Add(Spine2Ref.FindDeepChild("RightHandPinkyHelper4"));
        GloveBones.Add(Spine2Ref.FindDeepChild("RightHandIndexPalm1"));
        GloveBones.Add(Spine2Ref.FindDeepChild("RightHandMiddlePalm1"));
        GloveBones.Add(Spine2Ref.FindDeepChild("RightHandRingPalm1"));
        GloveBones.Add(Spine2Ref.FindDeepChild("RightHandPinkyPalm1"));
        GloveBones.Add(Spine2Ref.FindDeepChild("RightHandHelper1"));
        GloveBones.Add(Spine2Ref.FindDeepChild("RightForeArmSleeve1"));

        Il2CppReferenceArray<Transform> GloveBonesFinal = GloveBones.ToArray();
        var PlayerGlovesRenderer = GameObject.Instantiate(GloveRef.gameObject, OldSkin).GetComponent<SkinnedMeshRenderer>();
        PlayerGlovesRenderer.rootBone = Spine2Ref.FindDeepChild("RightForeArmTwistNew2");
        PlayerGlovesRenderer.bones = GloveBonesFinal;
        PlayerGlovesRenderer.gameObject.SetActive(true);
        PlayerGlovesRenderer.gameObject.layer = LayerMask.NameToLayer("Player");
    }
    public static void ApplyMask(Transform Object)
    {
        var Root = Object.FindDeepChild("Root");
        var OldSkin = Object.transform.FindDeepChild("OldSkin");
        var Spine2Ref = Object.FindDeepChild("Spine2");
        var MaskRef = TreeCrashTacticoolRef.Find("GEO/HeadVariants/CaucasianMask/CaucasianHead/WhiteHeadBalaclavaBloody_addressableInstance/TacticoolBalaclava");
        var PlayerMask = GameObject.Instantiate(MaskRef, OldSkin);
        SkinnedMeshBoneRemapCache.RetargetBones(PlayerMask.GetComponent<SkinnedMeshBoneRemapCache>(), Root);
        PlayerMask.GetComponent<SkinnedMeshRenderer>().sharedMaterial = TacticoolBalaclavaRef.sharedMaterial;

    }


    protected override void OnGameStart()
    {
        
            
                AssetManager = new GameObject().transform;
                AssetManager.gameObject.name = "ArmorMod Asset Manager";
                AssetManager.gameObject.SetActive(true);
                var LocalPlayerRef = LocalPlayer.GameObject;

                OpeningCutsceneRef = CutsceneManager._instance._openingCutscene.GetComponent<OpeningCutscene>();

                if (OpeningCutsceneRef)
                {
                    PlayerTactiRef = GameObject.Instantiate(OpeningCutsceneRef.GetComponent<OpeningCutscene>()._playerAnimator.transform.Find("GEO"), AssetManager);
                    PlayerTactiRef.gameObject.SetActive(false);
                    TacticalSoldierRef = GameObject.Instantiate(OpeningCutsceneRef.GetComponent<OpeningCutscene>()._tacti1Animator.transform.Find("GEO"), AssetManager);
                    TacticalSoldierRef.gameObject.SetActive(true);
                    TacticoolBalaclavaRef = TacticalSoldierRef.FindDeepChild("mask").GetComponent<SkinnedMeshRenderer>();
                    TreeCrashTacticoolRef = GameObject.Instantiate(OpeningCutsceneRef._helicopterCrashCutscenes[0].GetComponent<HelicopterTreeCrashCutscene>()._crashedHelicopterGo.transform.FindDeepChild("TreeCrashTacticool"), AssetManager);
                    TreeCrashTacticoolRef.gameObject.SetActive(true);
                    TreeCrashTacticoolRef.Find("GEO").gameObject.SetActive(true);
                    HelmetRendererRef = TacticalSoldierRef.Find("TacticalArmorHeadHelmetMesh").GetComponent<SkinnedMeshRenderer>();

                    RLog.Msg("Opening cutscene assets initialized");
                }

                var Hips = LocalPlayerRef.transform.Find("PlayerAnimator")?.transform.Find("Root")?.transform.Find("Hips");
                var Root = LocalPlayerRef.transform.Find("PlayerAnimator")?.transform.Find("Root");
                var OldSkin = LocalPlayerRef.transform.Find("PlayerAnimator")?.transform.Find("Root")?.transform.Find("OldSkin");
                var ClothingSystem = LocalPlayerRef.transform.Find("ClothingSystem").GetComponent<PlayerClothingSystem>();
                var RaceSystem = LocalPlayerRef.transform.Find("RaceSystem").GetComponent<PlayerRaceSystem>();
                var LeftHand = RaceSystem.GetLeftArm();
                var RightHand = RaceSystem.GetRightArm();
                var PlayerHead = RaceSystem.GetHead().transform;
                var OldJacket = OldSkin.Find("tacti_jacket1").GetComponent<SkinnedMeshRenderer>();
                var NewJacket = ClothingSystem.transform.Find("TacticalJacket").GetComponent<SkinnedMeshRenderer>();
                var Spine2Ref = Hips.FindDeepChild("Spine2");
                var TactiBodyArmor = OldSkin.transform.Find("tacti_body_armor1").GetComponent<SkinnedMeshRenderer>();


                var OldJacketBones = OldJacket.bones.ToList();
                OldJacketBones[0] = Hips;
                var OldJacketBonesNew = OldJacketBones.ToArray();
                OldJacket.bones = OldJacketBonesNew;

                var TactiBodyArmorBones = TactiBodyArmor.bones.ToList();
                TactiBodyArmorBones[0] = Hips;
                var TactiBodyArmorBonesNew = TactiBodyArmorBones.ToArray();
                TactiBodyArmor.bones = TactiBodyArmorBonesNew;

                OldSkin.gameObject.SetActive(true);

                var OldSkinChildren = OldSkin.GetChildren();
                foreach (var child in OldSkinChildren)
                {
                    child.GetComponent<SkinnedMeshRenderer>().enabled = true;
                    child.GetComponent<SkinnedMeshRenderer>().castShadows = true;
                    child.gameObject.SetActive(false);
                }
                if (Config.enableBodyArmor.Value == true)
                {
                    TactiBodyArmor.gameObject.SetActive(true);
                }
                if (Config.useOldJacket.Value == true)
                {
                    NewJacket.sharedMesh = OldJacket.sharedMesh;
                    NewJacket.sharedMaterial = OldJacket.sharedMaterial;
                    NewJacket.bones = OldJacketBonesNew;
                }
        if (Config.hideBackpack.Value == true)
        {
            if (ClothingSystem._defaultClothing._size == 4)
            {
                ClothingSystem._defaultClothing.RemoveAt(3);
            }
            if (ClothingSystem._allClothing._size == 16)
            {
                ClothingSystem._allClothing.RemoveAt(11);
            }
            ClothingSystem.transform.Find("Backpack")?.gameObject.SetActive(false);

            if (Config.useGloves.Value == true)
            {
                ApplyGloves(LocalPlayer.Transform);
                RaceSystem.HideArms(true);
            }
            if (ActorTools.GetRobby())
            {
                Robby = ActorTools.GetRobby();
                RobbyHelmetRenderer = Robby.transform.Find("VisualRoot").transform.Find("RobbyRig").transform.Find("GEO").transform.Find("TacticalArmorHeadHelmetMesh").gameObject.GetComponent<SkinnedMeshRenderer>();
                HelmetMaterial = HelmetRendererRef.sharedMaterial;
                RobbyHelmetRenderer.sharedMaterial = HelmetMaterial;
                RobbyGloves = Robby.transform.Find("VisualRoot/RobbyRig/GEO/gloves").gameObject;
                if (Config.enableRobbyHelmet.Value == true && RobbyHelmetRenderer.gameObject.active == false)
                {
                    RobbyHelmetRenderer.gameObject.SetActive(true);
                    Robby.transform.Find("VisualRoot/RobbyRig/GEO/RobbyHair").gameObject.SetActive(false);
                    RLog.Msg("Robby helmet enabled");
                }
                RobbyInitialized = true;
            }
        }
    }

    [HarmonyPatch(typeof(InventoryCutscene), "PostStartHook")]
    private static class InventoryCutsceneCheck
    {
        private static void Postfix(InventoryCutscene __instance)
        {
            if (Config.useGloves.Value == true)
            {
                __instance._playerAnimator.transform.root.Find("RaceSystem").GetComponent<PlayerRaceSystem>().HideArms(true);
            }
        }
    }



    [HarmonyPatch(typeof(CrashCutsceneBase), "ShowBadGuyActor")]
    private static class NewGamePatch
    {
        private static void Postfix()
        {
            if (ActorTools.GetRobby() && RobbyInitialized == false)
            {
                Robby = ActorTools.GetRobby();
                RobbyHelmetRenderer = Robby.transform.Find("VisualRoot").transform.Find("RobbyRig").transform.Find("GEO").transform.Find("TacticalArmorHeadHelmetMesh").gameObject.GetComponent<SkinnedMeshRenderer>();
                HelmetMaterial = HelmetRendererRef.sharedMaterial;
                RobbyHelmetRenderer.sharedMaterial = HelmetMaterial;
                RobbyGloves = Robby.transform.Find("VisualRoot/RobbyRig/GEO/gloves").gameObject;
                if (Config.enableRobbyHelmet.Value == true && RobbyHelmetRenderer.gameObject.active == false)
                {
                    RobbyHelmetRenderer.gameObject.SetActive(true);
                    Robby.transform.Find("VisualRoot/RobbyRig/GEO/RobbyHair").gameObject.SetActive(false);
                    RLog.Msg("Robby helmet enabled");
                }
                RobbyInitialized = true;
            }
            if (Config.useGloves.Value == true && LocalPlayer.RaceSystem)
            {
                LocalPlayer.RaceSystem?.HideArms(true);
            }
        }
    }


[HarmonyPatch(typeof(PlayerClothingSystem), "TryWearClothingPiece")]

private static class JacketChangePatch
{
    private static void Postfix(PlayerClothingSystem __instance)
    {
            if (!CutsceneManager._instance?._activeCutscene && !CutsceneManager._instance?._openingCutsceneInstance && RobbyInitialized == true)
            {
                RLog.Msg("Clothing change detected");
                Thread.Sleep(100);
                if (__instance.transform.Find("TacticalJacket"))
                {
                    if (Config.useOldJacket.Value == true)
                    {
                        var OldJacket = __instance._animationRoot.transform.FindDeepChild("tacti_jacket1").GetComponent<SkinnedMeshRenderer>();
                        var NewJacket = __instance.transform.Find("TacticalJacket").GetComponent<SkinnedMeshRenderer>();
                        NewJacket.sharedMesh = OldJacket.sharedMesh;
                        NewJacket.sharedMaterial = OldJacket.sharedMaterial;
                        NewJacket.bones = OldJacket.bones;
                        RLog.Msg("Jacket change reapplied");
                    }
                    if (Config.useGloves.Value == true)
                    {
                        __instance.transform.Find("RaceSystem").GetComponent<PlayerRaceSystem>().HideArms(true);
                    }
                }
            }
    }
}



[HarmonyPatch(typeof(PlayerLocation), "OnEnable")]
    private static class PlayerLocationSetupPatch
    {
        private static void Postfix(PlayerLocation __instance)
        {
            


                    RLog.Msg("Your PlayerLocation patch method worked, thank you GLaD0S, i love you <3");

                    var Hips = __instance.transform.FindDeepChild("Hips");
                    var OldSkin = __instance.transform.FindDeepChild("OldSkin");
                    var OldJacket = OldSkin.Find("tacti_jacket1").GetComponent<SkinnedMeshRenderer>();
                    var TactiBodyArmor = OldSkin.transform.Find("tacti_body_armor1").GetComponent<SkinnedMeshRenderer>();

                    var OldJacketBones = OldJacket.bones.ToList();
                    OldJacketBones[0] = Hips;
                    var OldJacketBonesNew = OldJacketBones.ToArray();
                    OldJacket.bones = OldJacketBonesNew;

                    var TactiBodyArmorBones = TactiBodyArmor.bones.ToList();
                    TactiBodyArmorBones[0] = Hips;
                    var TactiBodyArmorBonesNew = TactiBodyArmorBones.ToArray();
                    TactiBodyArmor.bones = TactiBodyArmorBonesNew;


                
            
        }
    }


    [HarmonyPatch(typeof(CoopPlayerRemoteSetup), "UpdatePlayerView")]
    private static class SystemsPatches
    {
        private static void Postfix(CoopPlayerRemoteSetup __instance)
        {
            RLog.Msg("UpdatePlayerView called and patched");
            var Hips = __instance.transform.FindDeepChild("Hips");
            var Root = __instance.transform.FindDeepChild("Root");
            var OldSkin = __instance.transform.FindDeepChild("OldSkin");
            var Jaw = __instance.transform.FindDeepChild("Jaw1_Caucasian");
            var ClothingSystem = __instance.transform.Find("ClothingSystem").GetComponent<PlayerClothingSystem>();
            var RaceSystem = __instance.transform.Find("RaceSystem").GetComponent<PlayerRaceSystem>();
            var Hemlet = OldSkin.transform.Find("tacti_hemlet1");
            var HemletRenderer = Hemlet.GetComponent<SkinnedMeshRenderer>();
            var LeftHand = RaceSystem.GetLeftArm();
            var RightHand = RaceSystem.GetRightArm();
            var PlayerHead = RaceSystem.GetHead().transform;
            var HeadBones = PlayerHead?.GetComponent<SkinnedMeshRenderer>().bones.ToList();
            var HelmetBones = HemletRenderer.bones.ToList();
            var OldJacket = OldSkin.Find("tacti_jacket1").GetComponent<SkinnedMeshRenderer>();
            var NewJacket = ClothingSystem.transform.Find("TacticalJacket").GetComponent<SkinnedMeshRenderer>();
            var Spine2Ref = Hips.FindDeepChild("Spine2");
            var TactiBodyArmor = OldSkin.transform.Find("tacti_body_armor1").GetComponent<SkinnedMeshRenderer>();

            var OldJacketBones = OldJacket.bones.ToList();
            OldJacketBones[0] = Hips;
            var OldJacketBonesNew = OldJacketBones.ToArray();
            OldJacket.bones = OldJacketBonesNew;

            var TactiBodyArmorBones = TactiBodyArmor.bones.ToList();
            TactiBodyArmorBones[0] = Hips;
            var TactiBodyArmorBonesNew = TactiBodyArmorBones.ToArray();
            TactiBodyArmor.bones = TactiBodyArmorBonesNew;
            if (Config.useMasks.Value == true)
            {
                if (RaceSystem._race != PlayerRace.Race.White && RaceSystem._race != PlayerRace.Race.Latin)
                {
                    RaceSystem.ApplyRace(PlayerRace.Race.Latin);
                    RaceSystem.HideArms(true);                    
                }

            }

            OldSkin.gameObject.SetActive(true);

            var OldSkinChildren = OldSkin.GetChildren();
            foreach (var child in OldSkinChildren)
            {
                child.GetComponent<SkinnedMeshRenderer>().enabled = true;
                child.GetComponent<SkinnedMeshRenderer>().castShadows = true;
                child.gameObject.SetActive(false);
            }

            if (Config.useNameTags.Value == true)
            {
                ConstraintSource NameTagConstraintSource;
                var PlayerNameVar = __instance.transform.Find("PlayerName");
                var NameTagModel = PlayerNameVar.FindChild("NameTagModel");
                var NameTagConstraint = NameTagModel.GetComponent<ParentConstraint>();
                if (NameTagConstraint.sourceCount == 2)
                {
                    NameTagConstraintSource = NameTagConstraint.GetSource(1);
                    NameTagConstraint.RemoveSourceInternal(0);
                }
                else
                {
                    NameTagConstraintSource = NameTagConstraint.GetSource(0);
                }

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
                PlayerNameTagLabelText.GetComponent<TextMeshProUGUI>().ForceMeshUpdate(true, true);
            }

            if (ClothingSystem)
            {
                RLog.Msg("Found clothing system!");
            }
            else
            {
                RLog.Msg("Failed to find clothing system");

            }
            if (Config.enableBodyArmor.Value == true)
            {
                TactiBodyArmor.gameObject.SetActive(true);
            }
            if (Config.enableHelmet.Value == true)
            {
                Hemlet.gameObject.SetActive(true);
            }
            List<Transform> EquippedClothing = ClothingSystem?.gameObject.GetChildren();


            if (Config.hideBackpack.Value == true)
            {
                if (ClothingSystem._defaultClothing._size == 4)
                {
                    ClothingSystem._defaultClothing.RemoveAt(3);
                }
                if (ClothingSystem._allClothing._size == 16)
                {
                    ClothingSystem._allClothing.RemoveAt(11);
                }
                    ClothingSystem.transform.Find("Backpack")?.gameObject.SetActive(false);
            }

            if (Config.enableHelmet.Value == true)
                {
                    Hemlet.gameObject.SetActive(true);
                    //RaceSystem.GetHead().transform.Find("Hair").gameObject.SetActive(false);
            }
                if (Config.cutsceneHelmet.Value == true)
                {
                    HemletRenderer.sharedMesh = RobbyHelmetRenderer.sharedMesh;
                    HemletRenderer.sharedMaterial = HelmetMaterial;
                    HelmetBones[1] = Jaw.transform;
                    Il2CppReferenceArray<Transform> NewHelmetBones = HelmetBones.ToArray();
                    HemletRenderer.bones = NewHelmetBones;
                }
            if (Config.useOldJacket.Value == true)
            {                
                NewJacket.sharedMesh = OldJacket.sharedMesh;
                NewJacket.sharedMaterial = OldJacket.sharedMaterial;
                NewJacket.bones = OldJacketBonesNew;
                RaceSystem.HideArms(true);
                ApplyGloves(__instance.transform);
                OldJacket.gameObject.SetActive(false);
            }
            if (Config.useOldJacket.Value == false && Config.useGloves.Value == true)
            {
                RaceSystem.HideArms(true);
                ApplyGloves(__instance.transform);
            }
            if (Config.useMasks.Value == true)
            {
                ApplyMask(__instance.transform);
                //RaceSystem.GetHead().transform.Find("Hair").gameObject.SetActive(false);               
            }

            if (Config.useGlasses.Value == true)
            {
                OldSkin.Find("tacti_sunglasses1").gameObject.SetActive(true);
            }


            

        }
    }
}


