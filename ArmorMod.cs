using UnityEngine;
using Unity;
using UnityEngine.AddressableAssets;
using TheForest.Utils;
using SonsSdk;
using Sons.Cutscenes;
using HarmonyLib;
using RedLoader;
using Endnight.Utilities;
using Sons.Animation.PlayerControl;
using Sons.Ai.Vail;
using UnityEngine.SceneManagement;
using SUI;
using Sons.Wearable.Clothing;
using Sons.Wearable.Armour.Clothing;

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

            Hips.transform.localScale = new Vector3(1, 1, 1);
            Root.transform.localScale = new Vector3((float)0.4, (float)0.4, (float)0.4);
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
        }
    }
    [HarmonyPatch(typeof(PlayerLocation), "OnEnable")]
    private static class HelmetPatch
    {
        private static void Postfix(PlayerLocation __instance)
        {
            var RobbyRenderer = ActorTools.GetPrefab(VailActorTypeId.Robby).gameObject.transform.Find("VisualRoot").transform.Find("RobbyRig").transform.Find("GEO").transform.Find("TacticalArmorHeadHelmetMesh").gameObject.GetComponent<SkinnedMeshRenderer>();
            // var Hair = __instance.transform.FindDeepChild("Hair");
            
            var OldSkin = __instance.transform.Find("PlayerAnimator")?.transform.Find("Root")?.transform.Find("OldSkin");
            var Hemlet = __instance.transform.Find("PlayerAnimator")?.transform.Find("Root")?.transform.Find("OldSkin").transform.Find("tacti_hemlet1");
            var HemletRenderer = Hemlet.GetComponent<SkinnedMeshRenderer>();
                
                if (Config.enableHelmet.Value == true)
                {
                    HemletRenderer.enabled = true;
                    HemletRenderer.castShadows = true;
                    /* if (Hair != null)
                     {
                         Hair.gameObject.SetActive(false);
                     }
                     else 
                     {
                         RLog.Msg("Hair object not found!");
                     } 
                    */
                    if (Config.cutsceneHelmet.Value == true)
                    {
                        HemletRenderer.sharedMesh = RobbyRenderer.sharedMesh;
                        HemletRenderer.sharedMaterial = HelmetMaterial;
                    }
                }
            
        } 
    }
    /* Public Static GameObject Backpack;
    [HarmonyPatch(typeof(PlayerClothingSystem), "OnEnable")]
        private static class BackpackPatch
    {
        private static void Postfix(PlayerClothingSystem __instance)
        {           
            GameObject ClothingSystem = __instance.transform.Find("ClothingSystem")?.gameObject;
            if (ClothingSystem != null)
            {
                RLog.Msg("Found clothing system!");
            }
            else
            {
                RLog.Msg("Failed to find clothing system");
            }
            Array EquippedClothing = ClothingSystem.GetComponentsInChildren<SkinnedMeshRenderer>();

            if (EquippedClothing != null)
            {
                foreach (SkinnedMeshRenderer PossibleBackpacks in EquippedClothing)
                {
                    if (PossibleBackpacks.sharedMesh.name == "Backpack")
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

                if (Config.hideBackpack.Value == true && ClothingSystem != null && __instance.gameObject != LocalPlayer.GameObject && Backpack != null)
                {

                    RLog.Msg("Hid player backpack");
                }
                else
                {
                    RLog.Msg("Failed to hide backpack!");
                }
                    
            }
    

        } 
    } */
    [HarmonyPatch(typeof(Cutscene), "Play")]
        private static class BeginCutscenePatch
        {
            private static void Postfix()
            {
                var Hips = LocalPlayer.Transform.Find("PlayerAnimator")?.transform.Find("Root")?.transform.Find("Hips");
                var Root = LocalPlayer.Transform.Find("PlayerAnimator")?.transform.Find("Root");


                Hips.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
                Root.transform.localScale = new Vector3(1, 1, 1);
              //RLog.Msg("Cutscene playing, patch applied.");
            }
        }

    [HarmonyPatch(typeof(CutsceneManager), "OnCutsceneEnded")]

    private static class CompleteCutscenePatch
    {
        private static void Postfix()
        {
            var Hips = LocalPlayer.Transform.Find("PlayerAnimator")?.transform.Find("Root")?.transform.Find("Hips");
            var Root = LocalPlayer.Transform.Find("PlayerAnimator")?.transform.Find("Root");

            Hips.transform.localScale = new Vector3(1, 1, 1);
            Root.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
            //RLog.Msg("Cutscene ended, patch applied.");


        }
    } 
        [HarmonyPatch(typeof(CaveEntranceCutscene), "FinalizeSequence")]

        private static class CompleteCutscenePatch2
        {
            private static void Postfix()
            {
                var Hips = LocalPlayer.Transform.Find("PlayerAnimator")?.transform.Find("Root")?.transform.Find("Hips");
                var Root = LocalPlayer.Transform.Find("PlayerAnimator")?.transform.Find("Root");

                Hips.transform.localScale = new Vector3(1, 1, 1);
                Root.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
               //RLog.Msg("Cave entrance cutscene ending, patch applied.");
            }
        }

        [HarmonyPatch(typeof(InventoryCutscene), "Play")]
        private static class BeginCraftingPatch
        {
            private static void Postfix()
            {
                var Hips = LocalPlayer.Transform.Find("PlayerAnimator")?.transform.Find("Root")?.transform.Find("Hips");
                var Root = LocalPlayer.Transform.Find("PlayerAnimator")?.transform.Find("Root");


                Hips.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
                Root.transform.localScale = new Vector3(1, 1, 1);
               //RLog.Msg("Inventory cutscene beginning, patch applied.");
            }
        }
        [HarmonyPatch(typeof(InventoryCutscene), "Cleanup")]

        private static class CompleteCraftingPatch
        {
            private static void Postfix()
            {
                var Hips = LocalPlayer.Transform.Find("PlayerAnimator")?.transform.Find("Root")?.transform.Find("Hips");
                var Root = LocalPlayer.Transform.Find("PlayerAnimator")?.transform.Find("Root");

                Hips.transform.localScale = new Vector3(1, 1, 1);
                Root.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
               //RLog.Msg("Inventory closing, patch applied.");
            }

        }
        [HarmonyPatch(typeof(RaftTrigger), "FinalizeCutscene")]

        private static class CompleteRaftPatch
    {
        private static void Postfix()
        {
            var Hips = LocalPlayer.Transform.Find("PlayerAnimator")?.transform.Find("Root")?.transform.Find("Hips");
            var Root = LocalPlayer.Transform.Find("PlayerAnimator")?.transform.Find("Root");

            Hips.transform.localScale = new Vector3(1, 1, 1);
            Root.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
            //RLog.Msg("Raft cutscene ending, patch applied.");
        }

    }
    [HarmonyPatch(typeof(PlayerAnimatorControl), "SetIsSwimming")]
    private static class BeginSwimmingPatch
    {
        private static void Postfix()
        {
            var Hips = LocalPlayer.Transform.Find("PlayerAnimator")?.transform.Find("Root")?.transform.Find("Hips");
            var Root = LocalPlayer.Transform.Find("PlayerAnimator")?.transform.Find("Root");


            Hips.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
            Root.transform.localScale = new Vector3(1, 1, 1);
            //RLog.Msg("Started swimming, patch applied.");
        }
    }
    [HarmonyPatch(typeof(PlayerAnimatorControl), "SwimmingUnstashItems")]
    private static class EndDivingPatch
    {
        private static void Postfix()
        {
            var Hips = LocalPlayer.Transform.Find("PlayerAnimator")?.transform.Find("Root")?.transform.Find("Hips");
            var Root = LocalPlayer.Transform.Find("PlayerAnimator")?.transform.Find("Root");


            Hips.transform.localScale = new Vector3(1, 1, 1);
            Root.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
            //RLog.Msg("Unstashing items from swimming, patch applied.");
        }
    }
    [HarmonyPatch(typeof(PlayerAnimatorControl), "enterClimbMode")]
    private static class StartClimbingPatch
    {
        private static void Postfix()
        {
            var Hips = LocalPlayer.Transform.Find("PlayerAnimator")?.transform.Find("Root")?.transform.Find("Hips");
            var Root = LocalPlayer.Transform.Find("PlayerAnimator")?.transform.Find("Root");


            Hips.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
            Root.transform.localScale = new Vector3(1, 1, 1);
            //RLog.Msg("Started climbing, patch applied.");
        }
    }
    [HarmonyPatch(typeof(PlayerAnimatorControl), "exitClimbMode")]
    private static class EndClimbingPatch
    { 
        private static void Postfix()
        {
            var Hips = LocalPlayer.Transform.Find("PlayerAnimator")?.transform.Find("Root")?.transform.Find("Hips");
            var Root = LocalPlayer.Transform.Find("PlayerAnimator")?.transform.Find("Root");


            Hips.transform.localScale = new Vector3(1, 1, 1);
            Root.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
            //RLog.Msg("Stopped climbing, patch applied.");
        }
    }
    [HarmonyPatch(typeof(PlayerAnimatorControl), "OnStartSlide")]
    private static class BeginSlidingPatch
    {
        private static void Postfix()
        {
            var Hips = LocalPlayer.Transform.Find("PlayerAnimator")?.transform.Find("Root")?.transform.Find("Hips");
            var Root = LocalPlayer.Transform.Find("PlayerAnimator")?.transform.Find("Root");


            Hips.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
            Root.transform.localScale = new Vector3(1, 1, 1);
            //RLog.Msg("Started sliding, patch applied.");
        }
    }
    [HarmonyPatch(typeof(PlayerAnimatorControl), "OnStopSlide")]
    private static class EndSlidingPatch
    {
        private static void Postfix()
        {
            var Hips = LocalPlayer.Transform.Find("PlayerAnimator")?.transform.Find("Root")?.transform.Find("Hips");
            var Root = LocalPlayer.Transform.Find("PlayerAnimator")?.transform.Find("Root");


            Hips.transform.localScale = new Vector3(1, 1, 1);
            Root.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
            //RLog.Msg("Stopped sliding, patch applied.");
        }
    }
    [HarmonyPatch(typeof(PlayerAnimatorControl), "UnlockFromCutscene")]
    private static class UnlockFromCutscenePatch
    {
        private static void Postfix()
        {
            var Hips = LocalPlayer.Transform.Find("PlayerAnimator")?.transform.Find("Root")?.transform.Find("Hips");
            var Root = LocalPlayer.Transform.Find("PlayerAnimator")?.transform.Find("Root");


            Hips.transform.localScale = new Vector3(1, 1, 1);
            Root.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
            //RLog.Msg("Unlocked from cutscene, patch applied.");
        }
    }
    [HarmonyPatch(typeof(PlayerAnimatorControl), "LockForCutscene")]
    private static class LockForCutscenePatch
    {
        private static void Postfix()
        {
            var Hips = LocalPlayer.Transform.Find("PlayerAnimator")?.transform.Find("Root")?.transform.Find("Hips");
            var Root = LocalPlayer.Transform.Find("PlayerAnimator")?.transform.Find("Root");


            Hips.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
            Root.transform.localScale = new Vector3(1, 1, 1);
            //RLog.Msg("Locked for cutscene, patch applied.");
        }
    }
    [HarmonyPatch(typeof(PlayerAnimatorControl), "LockForHiddenCutscene")]
    private static class LockForHiddenCutscenePatch
    {
        private static void Postfix()
        {
            var Hips = LocalPlayer.Transform.Find("PlayerAnimator")?.transform.Find("Root")?.transform.Find("Hips");
            var Root = LocalPlayer.Transform.Find("PlayerAnimator")?.transform.Find("Root");


            Hips.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
            Root.transform.localScale = new Vector3(1, 1, 1);
            //RLog.Msg("Locked for hidden cutscene, patch applied.");
        }
    }
    [HarmonyPatch(typeof(PlayerAnimatorControl), "SpawnStandUpProps")]
    private static class StandUpFromCrashRoutinePatch
    {
        private static void Postfix()
        {
            var Hips = LocalPlayer.Transform.Find("PlayerAnimator")?.transform.Find("Root")?.transform.Find("Hips");
            var Root = LocalPlayer.Transform.Find("PlayerAnimator")?.transform.Find("Root");


            Hips.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
            Root.transform.localScale = new Vector3(1, 1, 1);
            //RLog.Msg("Spawned stand up props, patch applied.");
        }
    }
  // Placing animation fix causes obviously visible vest inflation 
    /*
    [HarmonyPatch(typeof(PlayerAnimationData), "EnterAnimation")]
    private static class EnterPlaceAnimationPatch
    {
        private static void Postfix()
        {
            var Hips = LocalPlayer.Transform.Find("PlayerAnimator")?.transform.Find("Root")?.transform.Find("Hips");
            var Root = LocalPlayer.Transform.Find("PlayerAnimator")?.transform.Find("Root");


            Hips.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
            Root.transform.localScale = new Vector3(1, 1, 1);
            //RLog.Msg("Started placing animation, patch applied.");
        }
    }
    [HarmonyPatch(typeof(PlayerAnimationData), "ExitAnimation")]
    private static class ExitPlaceAnimationPatch
    {
        private static void Postfix()
        {
            var Hips = LocalPlayer.Transform.Find("PlayerAnimator")?.transform.Find("Root")?.transform.Find("Hips");
            var Root = LocalPlayer.Transform.Find("PlayerAnimator")?.transform.Find("Root");


            Hips.transform.localScale = new Vector3(1, 1, 1);
            Root.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
            //RLog.Msg("Exiting place animation, patch applied.");
        }
    }
   */
    [HarmonyPatch(typeof(PlayerAnimatorControl), "DestroyStandUpProps")]
    private static class DestroyStandUpPropsPatch
    {
        private static void Postfix()
        {
            var Hips = LocalPlayer.Transform.Find("PlayerAnimator")?.transform.Find("Root")?.transform.Find("Hips");
            var Root = LocalPlayer.Transform.Find("PlayerAnimator")?.transform.Find("Root");


            Hips.transform.localScale = new Vector3(1, 1, 1);
            Root.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
            //RLog.Msg("Destroying stand up props, patch applied.");
        }
    }
}
    
  
