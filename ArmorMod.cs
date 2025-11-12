using UnityEngine;
using TheForest.Utils;
using SonsSdk;
using Sons;
using Sons.Cutscenes;
using Sons.Multiplayer.Client;
using HarmonyLib;
using RedLoader;
using Endnight.Utilities;
using Sons.Crafting;
using Sons.Animation.PlayerControl;
using Sons.Gameplay;


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
    }

    public GameObject[] PlayerNets;

    protected override void OnGameStart()
    {
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
            OldSkin.transform.Find("tacti_hemlet1")?.gameObject.SetActive(false);
            OldSkin.transform.Find("tacti_jacket1")?.gameObject.SetActive(false);
            OldSkin.transform.Find("tacti_mask1")?.gameObject.SetActive(false);
            OldSkin.transform.Find("tacti_pants1")?.gameObject.SetActive(false);
            OldSkin.transform.Find("tacti_sunglasses1")?.gameObject.SetActive(false);
            RLog.Msg("Your PlayerLocation patch method worked, thank you GLaD0S, i love you <3");
        }
    }
        [HarmonyPatch(typeof(Cutscene), "Play")]
        private static class BeginCutscenePatch
        {
            private static void Postfix()
            {
                var Hips = LocalPlayer.Transform.Find("PlayerAnimator")?.transform.Find("Root")?.transform.Find("Hips");
                var Root = LocalPlayer.Transform.Find("PlayerAnimator")?.transform.Find("Root");


                Hips.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
                Root.transform.localScale = new Vector3(1, 1, 1);
              // RLog.Msg("Cutscene beginning, patch applied.");
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
            // RLog.Msg("Cutscene ended, patch applied.");


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
               // RLog.Msg("Cave entrance cutscene ending, patch applied.");
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
               // RLog.Msg("Inventory cutscene beginning, patch applied.");
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
               // RLog.Msg("Inventory closing, patch applied.");
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
            // RLog.Msg("Raft cutscene ending, patch applied.");
        }

    }
    [HarmonyPatch(typeof(RebreatherController), "RaiseMouthpiece")]
    private static class BeginDivingPatch
    {
        private static void Postfix()
        {
            var Hips = LocalPlayer.Transform.Find("PlayerAnimator")?.transform.Find("Root")?.transform.Find("Hips");
            var Root = LocalPlayer.Transform.Find("PlayerAnimator")?.transform.Find("Root");


            Hips.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
            Root.transform.localScale = new Vector3(1, 1, 1);
            // RLog.Msg("Rebreather mouthpiece raising, patch applied.");
        }
    }
    [HarmonyPatch(typeof(RebreatherController), "LowerMouthpiece")]
    private static class EndDivingPatch
    {
        private static void Postfix()
        {
            var Hips = LocalPlayer.Transform.Find("PlayerAnimator")?.transform.Find("Root")?.transform.Find("Hips");
            var Root = LocalPlayer.Transform.Find("PlayerAnimator")?.transform.Find("Root");


            Hips.transform.localScale = new Vector3(1, 1, 1);
            Root.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
            // RLog.Msg("Rebreather lowering, patch applied.");
        }
    }

}
    
  
