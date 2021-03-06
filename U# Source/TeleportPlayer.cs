
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace UdonToolboxV2
{
    /// <summary>
    /// TeleportPlayer
    /// Teleports player when triggered
    /// Created by Hitori Ou
    /// Last edit: 26-10-2020 Version 2.3
    /// </summary>
    public class TeleportPlayer : UdonSharpBehaviour
    {
        ushort count_mem = 0;
        float time_mem = 0;

        [Tooltip("Add multiple to cycle use one at a time")]
        public Transform[] Teleport_to = new Transform[1];
        [Tooltip("Select to have destination picked at random from Teleport_To list")]
        public bool Random = false;
        [Tooltip("Prevents spamming or infinite looping")]
        public float Cooldown = 1;

        #region Events
        [Header("Events")]
        public bool Detect_Player = true;
        public bool Detect_Object = false;

        public bool EventInteract = true;
        //public bool Event_OnCollisionEnter = false;
        //public bool Event_OnCollisionExit = false;
        public bool Event_OnTriggerEnter = false;
        public bool Event_OnTriggerExit = false;

        public override void Interact() { if (EventInteract) { SendCustomEvent("Teleport"); } }
        //void OnCollisionEnter(Collision other) { if (Event_OnCollisionEnter) { SendCustomEvent("Teleport"); } }
        //void OnCollisionExit(Collision other) { if (Event_OnCollisionExit) { SendCustomEvent("Teleport"); } }
        void OnTriggerEnter(Collider other) { if (Detect_Object && Event_OnTriggerEnter) { SendCustomEvent("Teleport"); } }
        void OnTriggerExit(Collider other) { if (Detect_Object && Event_OnTriggerExit) { SendCustomEvent("Teleport"); } }

        //public void OnPlayerCollisionEnter(VRCPlayerApi player) { if (Event_OnCollisionEnter) { SendCustomEvent("Teleport"); } }
        //public void OnPlayerCollisionExit(VRCPlayerApi player) { if (Event_OnCollisionExit) { SendCustomEvent("Teleport"); } }
        public override void OnPlayerTriggerEnter(VRCPlayerApi player) { if (Detect_Player && Event_OnTriggerEnter && player.isLocal) { SendCustomEvent("Teleport"); } }
        public override void OnPlayerTriggerExit(VRCPlayerApi player) { if (Detect_Player && Event_OnTriggerExit && player.isLocal) { SendCustomEvent("Teleport"); } }
        #endregion

        public void Teleport()
        {
            if (Teleport_to.Length != 0 && Teleport_to[0] != null && time_mem < Time.time)
            {
                time_mem = Time.time + Cooldown;
                if (Random)
                {
                    int rand = (int)UnityEngine.Random.Range((int)0, (int)Teleport_to.Length);/*rand min/max are different compare*/
                    Networking.LocalPlayer.TeleportTo(Teleport_to[rand].position, Teleport_to[rand].rotation);
                }
                else
                {
                    Networking.LocalPlayer.TeleportTo(Teleport_to[count_mem].position, Teleport_to[count_mem].rotation);
                    if (count_mem + 1 >= Teleport_to.Length)
                    { count_mem = 0; }
                    else
                    { count_mem++; }
                }
            }
        }
    }
}
