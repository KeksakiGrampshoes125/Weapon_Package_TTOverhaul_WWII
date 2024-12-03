datablock AudioProfile(SawnOffFireSound)
{
   filename    = "./Sounds/SawnOff_fire.wav";
   description = AudioDefault3d;
   preload = true;
};

AddDamageType("SawnOff",   '<bitmap:add-ons/Weapon_Package_TTOverhaul_WWII/Icons/CI_SawnOff> %1 Blasted themselves',    '%2 Pow\'d <bitmap:add-ons/Weapon_Package_TTOverhaul_WWII/Icons/CI_SawnOff> %1\'s',0.75,1);
datablock ProjectileData(SawnOffProjectile : TrenchShotgunProjectile)
{
   directDamage        = 8;
   directDamageType    = $DamageType::SawnOff;
   radiusDamageType    = $DamageType::SawnOff;
};

datablock ProjectileData(SawnOffBlastProjectile : ShotgunBlastProjectile)
{
   directDamage        = 102; //14
   directDamageType    = $DamageType::SawnOff;
   radiusDamageType    = $DamageType::SawnOff;
};

//////////
// item //
//////////
datablock ItemData(SawnOffItem)
{
	category = "Weapon";  // Mission editor category
	className = "Weapon"; // For inventory system
	 // Basic Item Properties
	shapeFile = "./Models/SawnOff.dts";
	rotate = false;
	mass = 1;
	density = 0.2;
	elasticity = 0.2;
	friction = 0.6;
	emap = true;
	//gui stuff
	uiName = "Sawn Off";
	iconName = "./Icons/SawnOff";
	doColorShift = true;
	colorShiftColor = "0.34 0.34 0.4 1.000";
	 // Dynamic properties defined by the scripts
	image = SawnOffImage;
	canDrop = true;

	TTO_ammoType = "Shotgun";
	TTO_reloads = true;
	TTO_maxAmmo = 2;
	TTO_alwaysReloadPref = "NonEx";
};

////////////////
//weapon image//
////////////////
datablock ShapeBaseImageData(SawnOffImage)
{
   // Basic Item properties
	shapeFile = "./Models/SawnOff.dts";
   emap = true;
   mountPoint = 0;
   offset = "0 0.03 -.02";
   eyeOffset = 0; //"0.7 1.2 -0.5";
   rotation = eulerToMatrix( "0 0 0" );
   correctMuzzleVector = true;
   className = "WeaponImage";
   item = SawnOffItem;
   ammo = " ";
   projectile = SawnOffProjectile;
   projectileType = Projectile;
   casing = TwelveGaugeShellDebris;
	armready = true;
   shellExitDir        = "1.0 0.1 1.0";
   shellExitOffset     = "0 0 0";
   shellExitVariance   = 10.0;	
   shellVelocity       = 5.0;
   doColorShift = true;
   colorShiftColor = SawnOffItem.colorShiftColor;

   altTriggerEnabled = true;

	stateName[0]                     = "Activate";
	stateTimeoutValue[0]             = 0.05;
	stateTransitionOnTimeout[0]       = "LoadCheckA";
	stateSound[0]					= weaponswitchSound;

	stateName[1]                     = "Ready";
	stateTransitionOnTriggerDown[1]  = "FireCheckA";
	stateTransitionOnNotLoaded[1]       = "ReloadStart";
	stateScript[1]                  = "onReady";

	stateName[2]                    = "Fire";
	stateTransitionOnTimeout[2]     = "Delay";
	stateTimeoutValue[2]            = 0.01;
	stateFire[2]                    = true;
	stateAllowImageChange[2]        = false;
	stateScript[2]                  = "onFire";
	//stateSound[2]					= DoubleShotgunFireSound;
	stateEmitter[2]			= TTOMuzzleFlashEmitter;
	stateEmitterTime[2]		= 0.05;
	stateEmitterNode[2]		= "muzzleNode";

	stateName[3]			= "Delay";
	stateEmitter[3]                  = gunSmokeEmitter;
	stateEmitterTime[3]              = 0.05;
	stateEmitterNode[3]                  = "muzzleNode";
	stateTransitionOnTriggerUp[3]     = "ReloadStart";
	stateTimeoutValue[3]            = 0.5;
	
	stateName[4]				= "LoadCheckA";
	stateScript[4]				= "TTO_onLoadCheck";
	stateTimeoutValue[4]			= 0.01;
	stateTransitionOnTimeout[4]		= "LoadCheckB";
	
	stateName[5]				= "LoadCheckB";
	stateTransitionOnLoaded[5]		= "Ready";
	stateTransitionOnNotLoaded[5]		= "Empty";

	stateName[6]				= "PreReload";
	stateEjectShell[6]              = true;
	stateTransitionOnAmmo[6]		= "Reload";
	stateTransitionOnNoAmmo[6]    = "Empty";

	stateName[7]				= "Reload";
	stateTimeoutValue[7]			= 0.35; // 0.3
	stateEjectShell[7]              = true;
	stateScript[7]				= "onReloadStart";
	stateSequence[7]                  = "reload_Open";
	stateTransitionOnTimeout[7]		= "DeathCheck";
	
	stateName[8]				= "ShellLoad";
	stateScript[8]				= "onShellLoad";
	stateTimeoutValue[8]				= 0.27;
	stateTransitionOnTimeout[8] 				= "Wait";
	
	stateName[9]				= "Wait";
	stateTimeoutValue[9]			= 0.3;
	stateSequence[9]                  = "reload_Close";
	stateScript[9]				= "onReloadWait";
	stateTransitionOnTimeout[9]		= "Reloaded";
	stateTransitionOnNoAmmo[9]       = "Empty";
	
	stateName[10]				= "FireLoadCheckA";
	stateScript[10]				= "TTO_onLoadCheck";
	stateTimeoutValue[10]			= 0.01;
	stateTransitionOnTimeout[10]		= "FireLoadCheckB";
	
	stateName[11]				= "FireLoadCheckB";
	stateTransitionOnLoaded[11]		= "Ready";
	stateTransitionOnAmmo[11]		= "PreReload";
	stateTransitionOnNoAmmo[11]    = "Empty";
	
	stateName[12]                   = "Empty";
	stateTransitionOnLoaded[12]     = "Ready";
	stateTransitionOnAmmo[12]  = "ReloadStart";
	stateTransitionOnTriggerDown[12]     = "FireCheckA";

	stateName[13]                   = "EmptyFire";
	stateScript[13]                 = "TTO_onEmptyFire";
	stateTransitionOnLoaded[13]      = "Ready";
	stateTransitionOnAmmo[13]       = "ReloadStart";
	stateTransitionOnTriggerUp[13]    = "Empty";
	
	stateName[14]				= "Reloaded";
	stateTimeoutValue[14]			= 0.04;
	stateScript[14]				= "onReloaded";
	stateTransitionOnTimeout[14]		= "Ready";

	stateName[15]                     = "ReloadStart";
	stateTransitionOnTimeout[15]  = "ReloadStartB";
	stateTimeoutValue[15]            = 0.1;
	stateAllowImageChange[15]         = false;

	stateName[16]                     = "ReloadStartB";
	stateTransitionOnTimeout[16]  = "FireLoadCheckA";
	stateTimeoutValue[16]            = 0.01;

	stateName[17]                    = "FireCheckA";
	stateScript[17]                  = "TTO_onFireCheck";
	stateTransitionOnTimeout[17]     = "FireCheckB";

	stateName[18]                    = "FireCheckB";
	stateTransitionOnLoaded[18]   = "Fire";
	stateTransitionOnNotLoaded[18]      = "EmptyFire";

	stateName[19]                   = "DeathCheck";
	stateScript[19]                 = "TTO_onDeathCheck";
	stateTimeoutValue[19]           = 0.032;
	stateTransitionOnTimeout[19]    = "ShellLoad";
};

function SawnOffImage::onFire(%this,%obj,%slot)
{

	%obj.stopAudio(1);
	%obj.playAudio(1, SawnOffFireSound);

	TTO_knockback(%obj, -2, -2, -2);
	%obj.playThread(2, shiftRight);
	%obj.playThread(3, shiftLeft);

	%this.TTO_decrementAmmo(%obj);
	%this.TTO_displayAmmo(%obj);

	if($Pref::Server::TTO::Recoil)
		%obj.spawnExplosion(TTOBigRecoilProjectile,"1 1 1");

	//shotgun blast projectile: only effective at point blank, sends targets flying off into the distance
	//
	//more or less represents the concussion blast. i can only assume such a thing exists because
	// i've never stood infront of a fucking shotgun before
	///////////////////////////////////////////////////////////

	TTO_createProjectile(%this, %obj, %slot, SawnOffBlastProjectile, 1);

	%projectile = %this.projectile;
	%spread = 0.0045;
	%shellCount = 9;

	return TTO_createProjectile(%this, %obj, %slot, %projectile, %shellCount, %spread);
}

function SawnOffImage::onReloadStart(%this,%obj,%slot)
{
	if($Pref::Server::TTO::DeathStopAnims && %obj.getDamagePercent() >= 1.0)
		return;
	%obj.playThread(2, plant);
	serverPlay3D(BreakActionOpenSound,%obj.getPosition());
	%this.TTO_displayAmmo(%obj);
}

function SawnOffImage::onShellLoad(%this,%obj,%slot)
{
	if($Pref::Server::TTO::DeathStopAnims && %obj.getDamagePercent() >= 1.0)
		return;
	%obj.playThread(0, plant);
	%obj.playThread(2, leftRecoil);
	%obj.playThread(3, shiftLeft);
	serverPlay3D(ReloadClick4Sound,%obj.getPosition());
	%this.TTO_displayAmmo(%obj);
}

function SawnOffImage::onReloadWait(%this,%obj,%slot)
{
	if($Pref::Server::TTO::DeathStopAnims && %obj.getDamagePercent() >= 1.0)
		return;
	%obj.playThread(2, shiftAway);
	serverPlay3D(BreakActionCloseSound,%obj.getPosition());
	%this.TTO_displayAmmo(%obj);
}

function SawnOffImage::onReloaded(%this,%obj,%slot)
{
	if($Pref::Server::TTO::DeathStopAnims && %obj.getDamagePercent() >= 1.0)
		return %obj.setImageLoaded(%slot, 1);
	%this.TTO_reload(%obj, %slot);
	%this.TTO_displayAmmo(%obj);
}