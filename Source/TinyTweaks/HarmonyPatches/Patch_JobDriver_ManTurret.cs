﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.AI.Group;
using RimWorld;
using HarmonyLib;

namespace TinyTweaks
{

    public static class Patch_JobDriver_ManTurret
    {

        [HarmonyPatch(typeof(JobDriver_ManTurret), nameof(JobDriver_ManTurret.FindAmmoForTurret))]
        public static class FindAmmoForTurret
        {

            public static bool Prefix(Pawn pawn, Building_TurretGun gun, ref Thing __result)
            {
                // Prevent raiders from putting stone chunks into their cannons of death if applicable
                if (TinyTweaksSettings.siegeFix)
                {
                    var allowedShellsSettings = gun.gun.TryGetComp<CompChangeableProjectile>().allowedShellsSettings;
                    Predicate<Thing> validator = (Thing t) => !t.IsForbidden(pawn) && pawn.CanReserve(t, 10, 1, null, false) && allowedShellsSettings.AllowedToAccept(t);
                    __result = GenClosest.ClosestThingReachable(gun.Position, gun.Map, ThingRequest.ForGroup(ThingRequestGroup.Shell), PathEndMode.OnCell, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false), 40f, validator, null, 0, -1, false, RegionType.Set_Passable, false);
                    return false;
                }
                return true;
            }

        }

    }

}
