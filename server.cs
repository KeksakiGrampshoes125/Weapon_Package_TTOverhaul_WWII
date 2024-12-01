%error = ForceRequiredAddOn("Weapon_Package_TTOverhaul_BASE");

if(%error == $Error::AddOn_NotFound)
{
   error("ERROR: Weapon_Package_TTOverhaul_WWII - required add-on Weapon_Package_TTOverhaul_BASE not found");
}
else
{

   //PISTOLS
   exec("./Weapon_VeteranPistol.cs");
   //exec("./Weapon_LugerPistole.cs"); //Model done
   //exec/"./Weapon_BroomhandlePistol.cs"); /Model done
   //exec("./Weapon_NambuPistoru.cs");
   //exec("./Weapon_TokarevPistolet.cs");

   //SHOTGUNS
   exec("./Weapon_TrenchShotgun.cs");
   //exec("./Weapon_RepeatingShotgun.cs"); model done
   //exec("./Weapon_SawnOff.cs"); edi- i mean model done

   //RIFLES
   //exec("./Weapon_InfantryRifle.cs");
   //exec("./Weapon_MosinVintovka.cs");
   //exec("./Weapon_KarabinerGewehr.cs");
   //exec("./Weapon_ArisakaRaifuru.cs");

   //SUBMACHINE GUNS
   exec("./Weapon_WWIIThompsonSMG.cs");
   //exec("./Weapon_GreaseSMG.cs");
   //exec("./Weapon_StenlingSMG.cs"); //model done
   //exec("./Weapon_MachinenPistole.cs");
   //exec("./Weapon_HyakuJu.cs");
   //exec("./Weapon_PistoletPumelet.cs");

   //ASSAULT RIFLES
   //exec("./Weapon_BAR.cs");
   //exec("./Weapon_SturmGewehr.cs"); model done

   //MACHINE GUNS
   //exec("./Weapon_LewisLMG.cs");
   //exec("./Weapon_BrensGun.cs");
   //exec("./Weapon_StaatmeistersBuzzsaw.cs"); //model done

   //MAGNUMS
   //exec("./Weapon_WebleySnubnose.cs");
   //exec("./Weapon_NagantRevolver.cs");
   //exec("./Weapon_ServiceMagnum.cs");

   //EXPLOSIVE WEAPONS
   //exec("./Weapon_WW2Bazooka.cs");
   //exec("./Weapon_Panzerogre.cs");
}