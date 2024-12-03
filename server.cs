%error = ForceRequiredAddOn("Weapon_Package_TTOverhaul_BASE");

if(%error == $Error::AddOn_NotFound)
{
   error("ERROR: Weapon_Package_TTOverhaul_WWII - required add-on Weapon_Package_TTOverhaul_BASE not found");
}
else
{

   //PISTOLS
   exec("./Weapon_VeteranPistol.cs");
   //exec("./Weapon_LugerPistole.cs"); model and sound done
   //exec/"./Weapon_BroomhandlePistol.cs"); model and sound done
   //exec("./Weapon_TokarevPistolet.cs"); sound done

   //SHOTGUNS
   exec("./Weapon_TrenchShotgun.cs");
   exec("./Weapon_RepeatingShotgun.cs");
   exec("./Weapon_SawnOff.cs");

   //RIFLES
   //exec("./Weapon_InfantryRifle.cs"); model and sounds done
   //exec("./Weapon_MosinVintovka.cs"); sound done
   //exec("./Weapon_KarabinerGewehr.cs"); model and sound done
   //exec("./Weapon_ArisakaRaifuru.cs"); sound done

   //SUBMACHINE GUNS
   exec("./Weapon_WWIIThompsonSMG.cs");
   exec("./Weapon_GreaseSMG.cs");
   exec("./Weapon_StenlingSMG.cs");
   //exec("./Weapon_MachinenPistole.cs"); sound done
   exec("./Weapon_HyakuJu.cs");
   //exec("./Weapon_PistoletPumelet.cs"); sound done

   //ASSAULT RIFLES
   exec("./Weapon_BAR.cs"); 
   exec("./Weapon_SturmGewehr.cs"); 

   //MACHINE GUNS
   //exec("./Weapon_LewisLMG.cs"); sound done
   //exec("./Weapon_BrensGun.cs"); sound done
   exec("./Weapon_StaatmeistersBuzzsaw.cs");

   //MAGNUMS
   //exec("./Weapon_PeacemakerMagnum.cs");

   //EXPLOSIVE WEAPONS
   //exec("./Weapon_WW2Bazooka.cs"); sound done
   //exec("./Weapon_Panzerogre.cs");
}