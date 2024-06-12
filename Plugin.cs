using BepInEx;
using System;
using System.Drawing;
using UnityEngine;
using Utilla;

namespace Teleport_Gun
{
    /// <summary>
    /// This is your mod's main class.
    /// </summary>

    /* This attribute tells Utilla to look for [ModdedGameJoin] and [ModdedGameLeave] */
    [ModdedGamemode]
    [BepInDependency("org.legoandmars.gorillatag.utilla", "1.5.0")]
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    public class Plugin : BaseUnityPlugin
    {
        bool inRoom;
        public static GameObject pointer;

        void Start()
        {
            /* A lot of Gorilla Tag systems will not be set up when start is called /*
			/* Put code in OnGameInitialized to avoid null references */

            Utilla.Events.GameInitialized += OnGameInitialized;
        }

        void OnEnable()
        {
            /* Set up your mod here */
            /* Code here runs at the start and whenever your mod is enabled*/

            HarmonyPatches.ApplyHarmonyPatches();
        }

        void OnDisable()
        {
            /* Undo mod setup here */
            /* This provides support for toggling mods with ComputerInterface, please implement it :) */
            /* Code here runs whenever your mod is disabled (including if it disabled on startup)*/

            HarmonyPatches.RemoveHarmonyPatches();
        }

        void OnGameInitialized(object sender, EventArgs e)
        {
            /* Code here runs after the game initializes (i.e. GorillaLocomotion.Player.Instance != null) */
        }

        void Update()
        {
            if (inRoom)
            {
                if (ControllerInputPoller.instance.rightGrab)
                {


                    Physics.Raycast(GorillaLocomotion.Player.Instance.rightControllerTransform.position, -GorillaLocomotion.Player.Instance.rightControllerTransform.up, out var hitInfo);
                    pointer = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    pointer.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                    pointer.GetComponent<Renderer>().material.color = new Color32(255, 255, 255, 0);
                    pointer.transform.position = hitInfo.point;
                    GameObject.Destroy(pointer.GetComponent<BoxCollider>());
                    GameObject.Destroy(pointer.GetComponent<Rigidbody>());
                    GameObject.Destroy(pointer.GetComponent<Collider>());
                    if (ControllerInputPoller.instance.rightControllerIndexFloat >= 0.1)
                    {
                        GameObject.Destroy(pointer, Time.deltaTime);
                        GorillaLocomotion.Player.Instance.transform.position = pointer.transform.position;
                        GorillaLocomotion.Player.Instance.GetComponent<Rigidbody>().velocity = pointer.transform.position;
                    }

                    if (pointer != null)
                    {
                        GameObject.Destroy(pointer, Time.deltaTime);
                    }

                }
            }
        }

        /* This attribute tells Utilla to call this method when a modded room is joined */
        [ModdedGamemodeJoin]
        public void OnJoin(string gamemode)
        {
            /* Activate your mod here */
            /* This code will run regardless of if the mod is enabled*/

            inRoom = true;
        }

        /* This attribute tells Utilla to call this method when a modded room is left */
        [ModdedGamemodeLeave]
        public void OnLeave(string gamemode)
        {
            /* Deactivate your mod here */
            /* This code will run regardless of if the mod is enabled*/

            inRoom = false;
        }
    }
}
